using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.ViewModel;
using GitSubmodules.Other;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitSubmodules
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MainViewModel))]
    [Guid(GuidList.GuidVsPackage3PkgString)]
    public sealed class GitSubmodulesPackage : Package
    {
        #region Package Members

        /// <summary>
        /// Called when the VSPackage is loaded by Visual Studio
        /// </summary>
        protected override void Initialize()
        {
            LogHelper.Log("Start: base.Initialize()");
            base.Initialize();
            LogHelper.Log("Leave: base.Initialize()");

            LogHelper.Log("Start: GetService(typeof(IMenuCommandService)) as OleMenuCommandService");
            var oleMenuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            LogHelper.Log("Leave: GetService(typeof(IMenuCommandService)) as OleMenuCommandService");

            if(oleMenuCommandService == null)
            {
                LogHelper.Log("Leave method - no OleMenuCommandService");
                return;
            }

            LogHelper.Log("Start: var commandId = new CommandID(GuidList.GuidVsPackage3CmdSet, Convert.ToInt32(PkgCmdIdList.CmdidMyTool))");
            var commandId = new CommandID(GuidList.GuidVsPackage3CmdSet, Convert.ToInt32(PkgCmdIdList.CmdidMyTool));
            LogHelper.Log("Leave: var commandId = new CommandID(GuidList.GuidVsPackage3CmdSet, Convert.ToInt32(PkgCmdIdList.CmdidMyTool))");

            LogHelper.Log("Start: var menueCommand = new MenuCommand(ShowToolWindow, commandId)");
            var menueCommand = new MenuCommand(ShowToolWindow, commandId);
            LogHelper.Log("Leave: var menueCommand = new MenuCommand(ShowToolWindow, commandId)");

            LogHelper.Log("Start: oleMenuCommandService.AddCommand(menueCommand)");
            oleMenuCommandService.AddCommand(menueCommand);
            LogHelper.Log("Leave: oleMenuCommandService.AddCommand(menueCommand)");

            LogHelper.Log("Start: FindToolWindow(typeof(MainViewModel), 0, true) as MainViewModel");
            var mainViewModel = FindToolWindow(typeof(MainViewModel), 0, true) as MainViewModel;
            LogHelper.Log("Leave: FindToolWindow(typeof(MainViewModel), 0, true) as MainViewModel");

            if(mainViewModel == null)
            {
                LogHelper.Log("Leave method - no mainViewModel");
                return;
            }

            LogHelper.Log("Start: GetGlobalService(typeof(DTE)) as DTE2");
            var dte2 = GetGlobalService(typeof(DTE)) as DTE2;
            LogHelper.Log("Leave: GetGlobalService(typeof(DTE)) as DTE2");

            LogHelper.Log("Start: GetService(typeof(SVsUIShell)) as IVsUIShell2");
            var iVsUiShell2 = GetService(typeof(SVsUIShell)) as IVsUIShell2;
            LogHelper.Log("Leave: GetService(typeof(SVsUIShell)) as IVsUIShell2");

            if((dte2 == null) || (iVsUiShell2 == null))
            {
                LogHelper.Log("Leave method - no dte2 or iVsUiShell2");
                return;
            }

            LogHelper.Log("Start: dte2.Events.WindowEvents.WindowActivated += delegate { mainViewModel.UpdateDte2(dte2, iVsUiShell2); }");
            dte2.Events.WindowEvents.WindowActivated += delegate { mainViewModel.UpdateDte2(dte2, iVsUiShell2); };
            LogHelper.Log("Leave: dte2.Events.WindowEvents.WindowActivated += delegate { mainViewModel.UpdateDte2(dte2, iVsUiShell2); }");

            LogHelper.Log("Start: dte2.Events.SolutionEvents.BeforeClosing += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2)");
            dte2.Events.SolutionEvents.BeforeClosing += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2);
            LogHelper.Log("Leave: dte2.Events.SolutionEvents.BeforeClosing += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2)");

            LogHelper.Log("Start: dte2.Events.SolutionEvents.Opened += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2)");
            dte2.Events.SolutionEvents.Opened += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2);
            LogHelper.Log("Leave: dte2.Events.SolutionEvents.Opened += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2)");
        }

        #endregion Package Members

        #region Private Methods

        /// <summary>
        /// Show the <see cref="ToolWindowPane"/> of this <see cref="Package"/>
        /// </summary>
        /// <param name="sender">The sender of this event (typical the <see cref="ToolWindowPane"/></param>
        /// <param name="e">The arguments for this event</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            LogHelper.Log("Start: FindToolWindow(typeof(MainViewModel), 0, false) as MainViewModel");
            var window = FindToolWindow(typeof(MainViewModel), 0, false) as MainViewModel;
            LogHelper.Log("Leave: FindToolWindow(typeof(MainViewModel), 0, false) as MainViewModel");

            if((window == null) || (window.Frame == null))
            {
                LogHelper.Log("Leave method - no window or no window.Frame");
                return;
            }

            LogHelper.Log("window.Frame as IVsWindowFrame");
            var vsWindowFrame = window.Frame as IVsWindowFrame;
            if(vsWindowFrame == null)
            {
                LogHelper.Log("Leave method - no vsWindowFrame");
                return;
            }

            LogHelper.Log("Start: ErrorHandler.ThrowOnFailure(vsWindowFrame.Show())");
            ErrorHandler.ThrowOnFailure(vsWindowFrame.Show());
            LogHelper.Log("Leave: ErrorHandler.ThrowOnFailure(vsWindowFrame.Show())");
        }

        #endregion Private Methods
    }
}
