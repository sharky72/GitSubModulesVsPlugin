using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using GitSubmodules.Mvvm.View;
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
        #region Overrides of Package

        /// <summary>
        /// Called when the VSPackage is loaded by Visual Studio
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var oleMenuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if(oleMenuCommandService == null)
            {
                return;
            }

            var commandId    = new CommandID(GuidList.GuidVsPackage3CmdSet, Convert.ToInt32(PkgCmdIdList.CmdidMyTool));
            var menueCommand = new MenuCommand(ShowToolWindow, commandId);
            oleMenuCommandService.AddCommand(menueCommand);

            var mainViewModel = FindToolWindow(typeof(MainViewModel), 0, true) as MainViewModel;
            if(mainViewModel == null)
            {
                return;
            }

            var dte2        = GetGlobalService(typeof(DTE)) as DTE2;
            var iVsUiShell2 = GetService(typeof(SVsUIShell)) as IVsUIShell2;
            if((dte2 == null) || (iVsUiShell2 == null))
            {
                return;
            }

            dte2.Events.WindowEvents.WindowActivated += delegate { mainViewModel.UpdateDte2(dte2, iVsUiShell2); };
            dte2.Events.SolutionEvents.BeforeClosing += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2);
            dte2.Events.SolutionEvents.Opened        += () => mainViewModel.UpdateDte2(dte2, iVsUiShell2);
        }

        /// <summary>
        /// Called when Visual Studio shutdown the <see cref="Package"/>
        /// </summary>
        /// <param name="canClose">Indicate that the <see cref="Package"/> can shutdown (or not)</param>
        /// <returns>Return state from the <see cref="Package"/> shutdown</returns>
        protected override int QueryClose(out bool canClose)
        {
            var dte2 = GetGlobalService(typeof(DTE)) as DTE2;
            if(dte2 == null)
            {
                return base.QueryClose(out canClose);
            }

            if((dte2.Version == "14.0") || (dte2.Version == "12.0") || (dte2.Version == "11.0"))
            {
                return base.QueryClose(out canClose);
            }

            var vsWindowFrame = TryToGetToolWindowFrame();
            if(vsWindowFrame == null)
            {
                return base.QueryClose(out canClose);
            }

            // INFO: DON'T REMOVE THE NEXT LINES!
            // Auto close the frame to avoid partial WPF crash
            // inside Visual Studio 2010 on restore the ToolWindow on startup
            vsWindowFrame.Hide();
            vsWindowFrame.CloseFrame(Convert.ToUInt32(__FRAMECLOSE.FRAMECLOSE_NoSave));

            return base.QueryClose(out canClose);
        }

        #endregion Overrides of Package

        #region Private Methods

        /// <summary>
        /// Show the <see cref="ToolWindowPane"/> of this <see cref="Package"/>
        /// </summary>
        /// <param name="sender">The sender of this event (typical the <see cref="ToolWindowPane"/></param>
        /// <param name="e">The arguments for this event</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            var vsWindowFrame = TryToGetToolWindowFrame();
            if(vsWindowFrame == null)
            {
                return;
            }

            ErrorHandler.ThrowOnFailure(vsWindowFrame.Show());

            // for testing
            new AddSubmodule().Show();
        }

        /// <summary>
        /// Return the <see cref="IVsWindowFrame"/> of the <see cref="MainViewModel"/>
        /// </summary>
        /// <returns>The <see cref="IVsWindowFrame"/> when found, otherwise <c>null</c></returns>
        private IVsWindowFrame TryToGetToolWindowFrame()
        {
            var window = FindToolWindow(typeof(MainViewModel), 0, false) as MainViewModel;

            return window?.Frame as IVsWindowFrame;
        }

        #endregion Private Methods
    }
}
