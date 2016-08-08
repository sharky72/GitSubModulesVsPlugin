using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EnvDTE80;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.Model;
using GitSubmodules.Mvvm.View;
using GitSubmodules.Other;

namespace GitSubmodules.Mvvm.ViewModel
{
    [Guid(GuidList.GuidToolWindowPersistenceString)]
    public sealed class MainViewModel : Microsoft.VisualStudio.Shell.ToolWindowPane
    {
        #region Public Properties

        public MainModel Model { get; private set; }

        #endregion Public Properties

        #region Internal Constructor

        public MainViewModel() : base(null)
        {
            Caption          = "Git Submodules";
            BitmapResourceID = 301;
            BitmapIndex      = 1;

            Model = new MainModel
            {
                ListOfSubmodules   = new Collection<Submodule>(),
                CanExecuteCommand = true
            };

            Content = new MainView(this);
        }

        #endregion Internal Constructor

        #region Commmands

        public ICommand CommandAllStatus
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.AllStatus),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllRegister
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.AllRegister),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllDeRegister
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.AllDeRegister),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllDeinitForce
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.AllDeRegisterForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllUpdate
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.AllUpdate),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllLatest
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.AllGetLatest),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneRegister
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.OneRegister),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneDeRegister
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.OneDeRegister),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneDeRegisterForce
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.OneDeRegisterForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneUpdate
        {
            get
            {
                return new RelayCommand(param => StartGit(param as Submodule, SubModuleCommand.OneUpdate),
                                        param => Model.CanExecuteCommand);
            }
        }

        #endregion Commands

        #region Internal Methods

        /// <summary>
        /// Update the solution full name
        /// </summary>
        /// <param name="dte2">The main Visual Studio object</param>
        internal void UpdateSolutionFullName(DTE2 dte2)
        {
            if(dte2 == null)
            {
                return;
            }

            if(Model.OutputPane == null)
            {
                Model.OutputPane = dte2.ToolWindows.OutputWindow.OutputWindowPanes.Add("Git submodules");
            }

            if(dte2.Solution == null)
            {
                return;
            }

            if(Model.CurrentSolutionFullName == dte2.Solution.FullName)
            {
                return;
            }

            Model.ListOfSubmodules        = null;
            Model.CanExecuteCommand       = false;
            Model.CurrentSolutionFullName = dte2.Solution.FullName;
            Model.GitCounter++;

            if(string.IsNullOrEmpty(Model.CurrentSolutionFullName))
            {
                WriteToOutputWindow(Category.Debug, "No solution opend");
                return;
            }

            try
            {
                Model.CurrentSolutionPath = Path.GetDirectoryName(Model.CurrentSolutionFullName);
            }
            catch(Exception exception)
            {
                WriteToOutputWindow(Category.Error, "Get diretory name from solution path");
                WriteToOutputWindow(Category.Error, exception.ToString());
            }

            StartGit(null, SubModuleCommand.AllStatus);
        }

        /// <summary>
        /// Check if the current open solution is a Git repository
        /// </summary>
        /// <returns><c>true</c> the curernt open solution is a Git repository, otherwise <c>false</c></returns>
        internal bool CheckSolutionIsAGitRepository()
        {
            if(string.IsNullOrEmpty(Model.CurrentSolutionPath) || !Directory.Exists(Model.CurrentSolutionPath))
            {
                WriteToOutputWindow(Category.Error, "Can't find solution path");
                return false;
            }

            WriteToOutputWindow(Category.Debug, string.Format("Set solution path to {0}", Model.CurrentSolutionPath));

            try
            {
                Directory.SetCurrentDirectory(Model.CurrentSolutionPath);
            }
            catch(Exception exception)
            {
                WriteToOutputWindow(Category.Error, "Can't switch to solution path");
                WriteToOutputWindow(Category.Error, exception.ToString());
                return false;
            }

            if(!Directory.Exists(".git"))
            {
                WriteToOutputWindow(Category.Debug, "Solution is not Git repository");
                return false;
            }

            WriteToOutputWindow(Category.Debug,"Solution is a Git repository");
            return true;
        }

        /// <summary>
        /// Start git.exe with the given arguments
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for this argument,
        /// use <c>null</c> for all submodules</param>
        /// <param name="submoduleCommand">The <see cref="SubModuleCommand"/> for this argument</param>
        internal void StartGit(Submodule submodule, SubModuleCommand submoduleCommand)
        {
            Task.Run(() =>
            {
                Model.GitCounter++;
                Model.CanExecuteCommand = false;

                if(!CheckSolutionIsAGitRepository())
                {
                    return;
                }

                var gitStartInfo = new ProcessStartInfo("git.exe")
                {
                    Arguments              = GitHelper.GetGitArguments(submodule, submoduleCommand),
                    CreateNoWindow         = true,
                    RedirectStandardError  = true,
                    RedirectStandardOutput = true,
                    UseShellExecute        = false
                };

                WriteToOutputWindow(Category.Debug, string.Format("Start Git with the follow arguments: {0}", gitStartInfo.Arguments));

                string consoleOutput;

                using(var process = Process.Start(gitStartInfo))
                {
                    if(process == null)
                    {
                        WriteToOutputWindow(Category.Error, "Can't start Git process");
                        Model.CanExecuteCommand = true;
                        return;
                    }

                    using(var reader = process.StandardOutput)
                    {
                        consoleOutput = reader.ReadToEnd().TrimEnd();
                    }

                    if(process.ExitCode != 0)
                    {
                        WriteToOutputWindow(Category.Error, string.Format("Error on Git process with command: {0}", gitStartInfo.Arguments));
                        WriteToOutputWindow(Category.Error, string.Format("Git process end with errorcode: {0}", process.ExitCode));

                        using(var reader = process.StandardError)
                        {
                            WriteToOutputWindow(Category.Error, reader.ReadToEnd().TrimEnd());
                            Model.CanExecuteCommand = true;
                            return;
                        }
                    }
                }

                if(submoduleCommand != SubModuleCommand.AllStatus)
                {
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error\n");
                    StartGit(null, SubModuleCommand.AllStatus);
                    Model.CanExecuteCommand = true;
                    return;
                }

                if(string.IsNullOrEmpty(consoleOutput))
                {
                    WriteToOutputWindow(Category.Debug, "No submodules found\n");
                    Model.CanExecuteCommand = true;
                    return;
                }

                var tempList = new List<Submodule>();

                var splitedAnswer = consoleOutput.Split('\n').Where(found => !string.IsNullOrEmpty(found)).ToList();

                WriteToOutputWindow(Category.Debug, "Console output from Git process:");
                WriteToOutputWindow(Category.Debug, string.Empty.PadRight(40, '-'));

                foreach(var line in splitedAnswer)
                {
                    WriteToOutputWindow(Category.Debug, line);
                }

                WriteToOutputWindow(Category.Debug, string.Empty.PadRight(40, '-'));

                WriteToOutputWindow(Category.Debug, string.Format("Git console output have {0} lines, that should be {0} submodules",
                                                                  splitedAnswer.Count));

                tempList.AddRange(splitedAnswer.Select(found => new Submodule(Model.CurrentSolutionPath, found)));

                Model.ListOfSubmodules = tempList;

                if(splitedAnswer.Count == Model.ListOfSubmodules.Count())
                {
                    WriteToOutputWindow(Category.Debug, string.Format("Count of Submodules: {0}\n", Model.ListOfSubmodules.Count()));
                }
                else
                {
                    WriteToOutputWindow(Category.Error, string.Format("Count of Submodules {0} are not identical with count of lines {1}\n",
                                                               Model.ListOfSubmodules.Count(), splitedAnswer.Count));
                }

                Model.CanExecuteCommand = true;
            });
        }

        internal void WriteToOutputWindow(Category category, string message)
        {
            if((Model == null) || (Model.OutputPane == null))
            {
                return;
            }

            Model.OutputPane.OutputString(string.Format("{0:HH:mm:ss} - {1:00} - {2} : {3}\n", DateTime.Now, Model.GitCounter, category, message));
        }

        #endregion Internal Methods
    }
}
