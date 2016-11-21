using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EnvDTE80;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.Model;
using GitSubmodules.Mvvm.View;
using GitSubmodules.Other;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitSubmodules.Mvvm.ViewModel
{
    [Guid(GuidList.GuidToolWindowPersistenceString)]
    public sealed partial class MainViewModel : Microsoft.VisualStudio.Shell.ToolWindowPane
    {
        #region Public Properties

        /// <summary>
        /// The model that contains all used data of the view-model
        /// </summary>
        public MainModel Model { get; private set; }

        #endregion Public Properties

        #region Public Constructor

        /// <summary>
        /// Constructor of <see cref="MainViewModel"/>
        /// </summary>
        public MainViewModel()
        {
            Caption          = "Git Submodules";
            BitmapResourceID = 301;
            BitmapIndex      = 1;

            Model = new MainModel
            {
                ListOfSubmodules = new Collection<Submodule>(),
                WaitingTimer     = new AutoResetEvent(false),
                GitVersion       = "Git is not present, please install",
                Foreground       = ThemeHelper.GetWindowTextColor()
            };

            if(!Model.GitIsPresent)
            {
                DoStartGit(SubmoduleCommand.OtherGitVersion);
            }

            Content = new MainView(this);
        }

        #endregion Internal Constructor

        #region Internal Command Methods

        /// <summary>
        /// Start git.exe with the given arguments
        /// </summary>
        /// <param name="submoduleCommand">The <see cref="SubmoduleCommand"/> for this argument</param>
        /// <param name="submodule">[Optional] The <see cref="Submodule"/> for this argument,
        /// use <c>null</c> for all submodules</param>
        internal void DoStartGit(SubmoduleCommand submoduleCommand, Submodule submodule = null)
        {
            Task.Run(() =>
            {
                Model.GitCounter++;
                CanExecuteCommand(false);

                WriteToOutputWindow(Category.EmptyLine, null);

                SetPathForGitProcess((submoduleCommand == SubmoduleCommand.OnePullOriginMaster)
                                  || (submoduleCommand == SubmoduleCommand.OneBranchList)
                                         ? submodule
                                         : null);

                var gitStartInfo = GitHelper.GetProcessStartInfo(submodule, submoduleCommand);

                WriteToOutputWindow(Category.Debug, string.Format("Start Git with the follow arguments: {0}", gitStartInfo.Arguments));

                using(var process = Process.Start(gitStartInfo))
                {
                    if(process == null)
                    {
                        WriteToOutputWindow(Category.Error, "Can't start Git process");
                        CanExecuteCommand(true);
                        Model.WaitingTimer.Set();
                        return;
                    }

                    string consoleOutput;

                    using(var reader = process.StandardOutput)
                    {
                        consoleOutput = reader.ReadToEnd().TrimEnd();
                    }

                    if(process.ExitCode == 0)
                    {
                        AnalyzeConsoleOutput(consoleOutput, submoduleCommand);
                        return;
                    }

                    WriteToOutputWindow(Category.Error, string.Format("Error on Git process with command: {0}", gitStartInfo.Arguments));
                    WriteToOutputWindow(Category.Error, string.Format("Git process end with errorcode: {0}", process.ExitCode));

                    using(var reader = process.StandardError)
                    {
                        WriteToOutputWindow(Category.Error, reader.ReadToEnd().TrimEnd());
                        CanExecuteCommand(true);
                        Model.WaitingTimer.Set();
                    }

                    if(submodule != null)
                    {
                        submodule.ChangeHealthStatus(HealthStatus.Error);
                        return;
                    }

                    if(Model.ListOfSubmodules == null)
                    {
                        return;
                    }

                    foreach(var module in Model.ListOfSubmodules)
                    {
                        module.ChangeHealthStatus(HealthStatus.Error);
                    }
                }
            });
        }

        /// <summary>
        /// Pull the origin master for one or all <see cref="Submodule"/>s
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for the pull, or <c>null</c> for all modules</param>
        internal void DoPullOriginMaster(Submodule submodule)
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
                        DoStartGit(SubmoduleCommand.OnePullOriginMaster, submoduleEntry);
                        Model.WaitingTimer.Reset();
                        Model.WaitingTimer.WaitOne(10000);
                    }

                    DoStartGit(SubmoduleCommand.AllStatus);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    DoStartGit(SubmoduleCommand.OnePullOriginMaster, submodule);
                    Model.WaitingTimer.Reset();
                    Model.WaitingTimer.WaitOne(10000);
                    DoStartGit(SubmoduleCommand.AllStatus);
                });
            }
        }

        /// <summary>
        /// Collect all branches of all <see cref="Submodule"/>s
        /// </summary>
        internal void DoCollectBranchList()
        {
            Task.Run(() =>
            {
                foreach(var submoduleEntry in Model.ListOfSubmodules)
                {
                    DoStartGit(SubmoduleCommand.OneBranchList, submoduleEntry);
                    Model.WaitingTimer.Reset();
                    Model.WaitingTimer.WaitOne(10000);
                }
            });
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

        #endregion Internal Command Methods

        #region Internal Methods

        /// <summary>
        /// Update the used Visual Studio object
        /// </summary>
        /// <param name="dte2">The Visual Studio object</param>
        /// <param name="iVsUiShell2">The <see cref="IVsUIShell2"/> for surface handling inside Visual Studio</param>
        internal void UpdateDte2(DTE2 dte2, IVsUIShell2 iVsUiShell2)
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

            Model.Foreground = ThemeHelper.GetWindowTextColor(iVsUiShell2);

            if((dte2.Solution == null) || (Model.CurrentSolutionFullName == dte2.Solution.FullName))
            {
                return;
            }

            Model.GitCounter++;
            Model.ListOfSubmodules        = null;
            Model.CanExecuteCommand       = false;
            Model.CurrentSolutionFullName = dte2.Solution.FullName;
            Model.CurrentSolutionPath     = string.Empty;

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

            DoStartGit(SubmoduleCommand.OtherBranchList);
            DoStartGit(SubmoduleCommand.AllFetch);
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

        /// <summary>
        /// Analyze the console output, based on the given <see cref="SubmoduleCommand"/>
        /// </summary>
        /// <param name="consoleOutput">The output of the console to analyze</param>
        /// <param name="submoduleCommand">The <see cref="SubmoduleCommand"/> for the analyze</param>
        internal void AnalyzeConsoleOutput(string consoleOutput, SubmoduleCommand submoduleCommand)
        {
            // TODO: cleanup this switch case

            switch(submoduleCommand)
            {
                case SubmoduleCommand.OtherGitVersion:
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

                    Model.GitVersion   = versionNumberString;
                    Model.GitIsPresent = true;

                    Model.WaitingTimer.Set();
                    CanExecuteCommand(true);
                    return;

                case SubmoduleCommand.OtherBranchList:
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");

                    Model.ListOfBranches  = new Collection<string>();
                    Model.CountOfBranches = "Branch (of 0)";
                    Model.CurrentBranch   = string.Empty;

                    if(string.IsNullOrEmpty(consoleOutput))
                    {
                        WriteToOutputWindow(Category.Error, "Can't get branch name");
                        return;
                    }

                    Model.ListOfBranches = consoleOutput.Split('\n').Select(found => found.TrimStart('*', ' '));
                    Model.CountOfBranches = string.Format("Branch (of {0})", Model.ListOfBranches.Count());

                    var branch = consoleOutput.Split('\n').FirstOrDefault(found => found.StartsWith("*", StringComparison.Ordinal));
                    if(string.IsNullOrEmpty(branch))
                    {
                        WriteToOutputWindow(Category.Error, "Can't parse branch name");
                        return;
                    }

                    Model.CurrentBranch = branch.TrimStart('*', ' ');
                    Model.WaitingTimer.Set();
                    return;

                case SubmoduleCommand.OneBranchList:
                    var currentDirectory = Directory.GetCurrentDirectory();
                    var submodule = Model.ListOfSubmodules.FirstOrDefault(found => currentDirectory.EndsWith(found.Name, StringComparison.Ordinal));
                    if(submodule == null)
                    {
                        WriteToOutputWindow(Category.Error, "Can't found submodule for branch list");
                        Model.WaitingTimer.Set();
                        return;
                    }

                    if(string.IsNullOrEmpty(consoleOutput))
                    {
                        WriteToOutputWindow(Category.Error, "Can't get branch list for submodule");
                        return;
                    }

                    submodule.ListOfBranches = consoleOutput.Split('\n').Select(found => found.TrimStart('*', ' '));
                    submodule.CountOfBranches = string.Format("Branch (of {0})", submodule.ListOfBranches.Count());

                    var submoduleBranch = consoleOutput.Split('\n').FirstOrDefault(found => found.StartsWith("*", StringComparison.Ordinal));
                    if(string.IsNullOrEmpty(submoduleBranch))
                    {
                        WriteToOutputWindow(Category.Error, "Can't parse branch name");
                        return;
                    }

                    submodule.CurrentBranch = submoduleBranch.TrimStart('*', ' ', '(').TrimEnd(')');

                    Model.WaitingTimer.Set();
                    CanExecuteCommand(true);
                    return;

                case SubmoduleCommand.OnePullOriginMaster:
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");
                    Model.WaitingTimer.Set();
                    return;

                case SubmoduleCommand.AllDeinit:
                case SubmoduleCommand.AllDeinitForce:
                case SubmoduleCommand.AllFetch:
                case SubmoduleCommand.AllInit:
                case SubmoduleCommand.AllPullOriginMaster:
                case SubmoduleCommand.AllUpdate:
                case SubmoduleCommand.AllUpdateForce:
                case SubmoduleCommand.OneDeinit:
                case SubmoduleCommand.OneDeinitForce:
                case SubmoduleCommand.OneInit:
                case SubmoduleCommand.OneStatus:
                case SubmoduleCommand.OneUpdate:
                case SubmoduleCommand.OneUpdateForce:
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");
                    DoStartGit(SubmoduleCommand.AllStatus);
                    return;

                default:
                    WriteToOutputWindow(Category.Debug, "Finished Git process with no error");
                    DoStartGit(SubmoduleCommand.AllStatus);
                    return;

                case SubmoduleCommand.AllStatus:
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

                    WriteToOutputWindow(Category.Debug,
                                        string.Format("Git console output have {0} lines, that should be {0} submodules",
                                                      splitedAnswer.Count));

                    tempList.AddRange(splitedAnswer.Select(found => new Submodule(Model.CurrentSolutionPath, found)));

                    Model.ListOfSubmodules = tempList;

                    if(splitedAnswer.Count == Model.ListOfSubmodules.Count())
                    {
                        WriteToOutputWindow(Category.Debug,
                                            string.Format("Count of Submodules: {0}", Model.ListOfSubmodules.Count()));
                    }
                    else
                    {
                        WriteToOutputWindow(Category.Error,
                                            string.Format("Count of Submodules {0} are not identical with count of lines {1}",
                                                          Model.ListOfSubmodules.Count(),
                                                          splitedAnswer.Count));
                    }

                    CanExecuteCommand(true);

                    DoCollectBranchList();

                    break;
            }

            // TODO: cleanup this switch case
        }

        /// <summary>
        /// Expand the view of aone<see cref="Submodule"/> from the list of submodules
        /// </summary>
        /// <param name="submoduleToExpand">The submodule to expand</param>
        internal void ExpandOneSubmodule(Submodule submoduleToExpand)
        {
            if((submoduleToExpand == null) || (Model == null) || (Model.ListOfSubmodules == null))
            {
                return;
            }

            foreach(var submodule in Model.ListOfSubmodules)
            {
                if(submodule == submoduleToExpand)
                {
                    submodule.ShowSlimInformations     = false;
                    submodule.ShowExtendedInformations = true;
                }
                else
                {
                    submodule.ShowSlimInformations     = true;
                    submodule.ShowExtendedInformations = false;
                }
            }
        }

        /// <summary>
        /// Try to set a <see cref="string"/> to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="textForClipboard">The <see cref="string"/> for the <see cref="Clipboard"/></param>
        internal void TryToSetTextToClipboard(string textForClipboard)
        {
            if(string.IsNullOrEmpty(textForClipboard))
            {
                return;
            }

            try
            {
                Clipboard.SetText(textForClipboard);
            }
            catch(Exception exception)
            {
                WriteToOutputWindow(Category.Error, exception.ToString());
            }
        }

        #endregion Internal Methods
    }
}
