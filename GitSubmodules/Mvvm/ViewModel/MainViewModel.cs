using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
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
        /// <param name="solutionFullName">The full name of the current opend solution</param>
        internal void UpdateSolutionFullName(string solutionFullName)
        {
            Model.CanExecuteCommand  = false;
            Model.ConsoleOutput       = string.Empty;
            Model.CurrentSolutionPath = string.Empty;

            if(string.IsNullOrEmpty(solutionFullName))
            {
                Model.CurrentSolutionPath = "No solution opend";
                Model.ConsoleOutput       = "No solution opend";
                return;
            }

            try
            {
                Model.CurrentSolutionPath = Path.GetDirectoryName(solutionFullName);
            }
            catch(Exception exception)
            {
                Model.ConsoleOutput = exception.ToString();
                return;
            }

            if(!CheckSolutionIsAGitRepository())
            {
                return;
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
                Model.ConsoleOutput = "Error on switch to solution path";
                return false;
            }

            try
            {
                Directory.SetCurrentDirectory(Model.CurrentSolutionPath);
            }
            catch(Exception exception)
            {
                Model.ConsoleOutput = exception.ToString();
                return false;
            }

            if(!Directory.Exists(".git"))
            {
                Model.ConsoleOutput = "Solution is not Git repository";
                return false;
            }

            Model.CanExecuteCommand = true;
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
                Model.CanExecuteCommand = false;

                if(submoduleCommand != SubModuleCommand.AllStatus)
                {
                    Model.ConsoleOutput = string.Empty;

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

                    using(var process = Process.Start(gitStartInfo))
                    {
                        if(process == null)
                        {
                            return;
                        }

                        using(var reader = process.StandardOutput)
                        {
                            Model.ConsoleOutput = reader.ReadToEnd();
                        }

                        if(process.ExitCode == 0)
                        {
                            return;
                        }

                        using(var reader = process.StandardError)
                        {
                            Model.ConsoleOutput = string.Format("Error on git command: {0}\n\n{1}",
                                submoduleCommand, reader.ReadToEnd());
                        }
                    }

                    return;
                }

                var tempList = new List<Submodule>();

                tempList.AddRange(Model.ConsoleOutput.Split('\n')
                                                     .Where(found => !string.IsNullOrEmpty(found))
                                                     .Select(found => new Submodule(Model.CurrentSolutionPath, found)));

                Model.ListOfSubmodules = tempList;

                Model.CanExecuteCommand = true;
            });
        }

        #endregion Internal Methods
    }
}
