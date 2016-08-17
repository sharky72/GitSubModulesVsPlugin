using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE80;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.Model;
using GitSubmodules.Mvvm.View;
using GitSubmodules.Other;
using Microsoft.VisualStudio.PlatformUI;

namespace GitSubmodules.Mvvm.ViewModel
{
    [Guid(GuidList.GuidToolWindowPersistenceString)]
    public sealed partial class MainViewModel : Microsoft.VisualStudio.Shell.ToolWindowPane
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
                ListOfSubmodules  = new Collection<Submodule>(),
                WaitingTimer      = new AutoResetEvent(false),
                GitVersion        = "Git is not present, please install",
                Foreground        = ColorHelper.GetThemedBrush(EnvironmentColors.ToolWindowTextColorKey)
            };

            if(!Model.GitIsPresent)
            {
                DoStartGit(null, SubModuleCommand.OtherGitVersion);
            }

            VSColorTheme.ThemeChanged += delegate
            {
                Model.Foreground = ColorHelper.GetThemedBrush(EnvironmentColors.ToolWindowTextColorKey);
            };

            Content = new MainView(this);
        }

        #endregion Internal Constructor

        #region Command Methods

        /// <summary>
        /// Start git.exe with the given arguments
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for this argument,
        /// use <c>null</c> for all submodules</param>
        /// <param name="submoduleCommand">The <see cref="SubModuleCommand"/> for this argument</param>
        internal void DoStartGit(Submodule submodule, SubModuleCommand submoduleCommand)
        {
            Task.Run(() =>
            {
                Model.GitCounter++;
                CanExecuteCommand(false);

                WriteToOutputWindow(Category.EmptyLine, null);

                SetPathForGitProcess(submoduleCommand == SubModuleCommand.OnePullOriginMaster ? submodule : null);

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
                        CanExecuteCommand(true);
                        Model.WaitingTimer.Set();
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
                            CanExecuteCommand(true);
                            Model.WaitingTimer.Set();
                        }

                        submodule.ChangeHealthStatus(HealthStatus.Error);
                        return;
                    }
                }

                if(submoduleCommand == SubModuleCommand.OtherGitVersion)
                {
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");

                    if(string.IsNullOrEmpty(consoleOutput))
                    {
                        WriteToOutputWindow(Category.Error, "Can't get version number from git");
                        return;
                    }

                    var versionNumberString = consoleOutput.Split(' ').LastOrDefault();
                    if(string.IsNullOrEmpty(versionNumberString))
                    {
                        WriteToOutputWindow(Category.Error, "Can't parse version number from git");
                        return;
                    }

                    Model.GitVersion = versionNumberString;
                    Model.GitIsPresent = true;
                    Model.WaitingTimer.Set();
                }

                if(submoduleCommand == SubModuleCommand.OnePullOriginMaster)
                {
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");
                    Model.WaitingTimer.Set();
                    return;
                }

                if(submoduleCommand != SubModuleCommand.AllStatus)
                {
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");
                    DoStartGit(null, SubModuleCommand.AllStatus);
                    return;
                }

                if(string.IsNullOrEmpty(consoleOutput))
                {
                    WriteToOutputWindow(Category.Debug, "No submodules found");
                    CanExecuteCommand(true);
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
                    WriteToOutputWindow(Category.Debug, string.Format("Count of Submodules: {0}", Model.ListOfSubmodules.Count()));
                }
                else
                {
                    WriteToOutputWindow(Category.Error, string.Format("Count of Submodules {0} are not identical with count of lines {1}",
                                                               Model.ListOfSubmodules.Count(), splitedAnswer.Count));
                }

                CanExecuteCommand(true);
            });
        }

        /// <summary>
        /// Pull the origin master for one or all submodules
        /// </summary>
        /// <param name="submodule">The submodule for the pull, or <c>null</c> for all modules</param>
        internal void DoAllPullOriginMaster(Submodule submodule)
        {
            if((Model.ListOfSubmodules == null) || (Model.WaitingTimer == null))
            {
                return;
            }

            if(submodule == null)
            {
                Task.Run(() =>
                {
                    foreach(var submoduleEntry in Model.ListOfSubmodules)
                    {
                        DoStartGit(submoduleEntry, SubModuleCommand.OnePullOriginMaster);
                        Model.WaitingTimer.Reset();
                        Model.WaitingTimer.WaitOne(10000);
                    }

                    DoStartGit(null, SubModuleCommand.AllStatus);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    DoStartGit(submodule, SubModuleCommand.OnePullOriginMaster);
                    Model.WaitingTimer.Reset();
                    Model.WaitingTimer.WaitOne(10000);
                    DoStartGit(null, SubModuleCommand.AllStatus);
                });
            }
        }

        /// <summary>
        /// Open a folder of a <see cref="Submodule"/> with the system file-explorer
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> these folder should be open</param>
        internal void DoOpenFolder(Submodule submodule)
        {
            if(string.IsNullOrEmpty(Model.CurrentSolutionPath))
            {
                return;
            }

            var folderToOpen = (submodule != null) && !string.IsNullOrEmpty(submodule.Name)
                ? Path.Combine(Model.CurrentSolutionPath, submodule.Name)
                : Model.CurrentSolutionPath;

            if(!Directory.Exists(folderToOpen))
            {
                WriteToOutputWindow(Category.Error, string.Format("Folder not found {0}", folderToOpen));
                return;
            }

            try
            {
                Process.Start("explorer", folderToOpen);
            }
            catch(Exception exception)
            {
                WriteToOutputWindow(Category.Error, string.Format("Can't open explorer on the given path {0}", folderToOpen));
                WriteToOutputWindow(Category.Error, exception.ToString());
            }
        }

        #endregion Command Methods

        #region Internal Methods

        /// <summary>
        /// Update the used Visual Studio object
        /// </summary>
        /// <param name="dte2">The Visual Studio object</param>
        internal void UpdateDte2(DTE2 dte2)
        {
            if(dte2 == null)
            {
                return;
            }

            if(Model.OutputPane == null)
            {
                Model.OutputPane = dte2.ToolWindows.OutputWindow.OutputWindowPanes.Add("Git submodules");
                Model.OutputPane.Activate();
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
            Model.CurrentSolutionPath     = string.Empty;
            Model.GitCounter++;

            if(string.IsNullOrEmpty(Model.CurrentSolutionFullName))
            {
                Model.CurrentSolutionPath = "No solution opend";
                WriteToOutputWindow(Category.Debug, "No solution opend");
                return;
            }

            if(!CheckSolutionIsAGitRepository() || !Model.GitIsPresent)
            {
                return;
            }

            DoStartGit(null, SubModuleCommand.AllFetch);
        }

        /// <summary>
        /// Set the path for the Git process
        /// </summary>
        /// <param name="submodule">The submodule for the git process, or <c>null</c> for all submodules</param>
        internal void SetPathForGitProcess(Submodule submodule)
        {
            var submoduleFolder = submodule == null
                ? Model.CurrentSolutionPath
                : Path.Combine(Model.CurrentSolutionPath, submodule.Name);

            if(!Directory.Exists(submoduleFolder))
            {
                WriteToOutputWindow(Category.Error, string.Format("Directory of {0} not found", submoduleFolder));
                return;
            }

            Directory.SetCurrentDirectory(submoduleFolder);
            WriteToOutputWindow(Category.Debug, string.Format("Set path to {0} ", submoduleFolder));
        }

        /// <summary>
        /// Check if the current open solution is a Git repository
        /// </summary>
        /// <returns><c>true</c> the curernt open solution is a Git repository, otherwise <c>false</c></returns>
        internal bool CheckSolutionIsAGitRepository()
        {
            try
            {
                Model.CurrentSolutionPath = Path.GetDirectoryName(Model.CurrentSolutionFullName);
            }
            catch(Exception exception)
            {
                WriteToOutputWindow(Category.Error, "Get diretory name from solution path");
                WriteToOutputWindow(Category.Error, exception.ToString());
            }

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
                WriteToOutputWindow(Category.Debug, "Solution is not a Git repository");
                return false;
            }

            WriteToOutputWindow(Category.Debug,"Solution is a Git repository");
            return true;
        }

        /// <summary>
        /// Write a message under a <see cref="category"/> with timestamp and counted number to a output window
        /// </summary>
        /// <param name="category">The category for this message</param>
        /// <param name="message">The message to write</param>
        internal void WriteToOutputWindow(Category category, string message)
        {
            if((Model == null) || (Model.OutputPane == null))
            {
                return;
            }

            Model.OutputPane.OutputString(category != Category.EmptyLine
                ? string.Format("{0:HH:mm:ss} - {1:00} - {2} : {3}\n", DateTime.Now, Model.GitCounter, category, message)
                : "\n");

            if(category != Category.Error)
            {
                return;
            }

            Model.OutputPane.Activate();
        }

        /// <summary>
        /// Change the execution state of the functions of this extension
        /// </summary>
        /// <param name="status">The new execution state</param>
        internal void CanExecuteCommand(bool status)
        {
            Model.CanExecuteCommand   = status;
            Model.ShowWatingIndicator = !status;
        }

        #endregion Internal Methods
    }
}
