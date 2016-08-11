using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
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
            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if(mcs == null)
            {
                return;
            }

            mcs.AddCommand(new MenuCommand(ShowToolWindow, new CommandID(GuidList.GuidVsPackage3CmdSet,
                                                                         Convert.ToInt32(PkgCmdIdList.CmdidMyTool))));

            var window = FindToolWindow(typeof(MainViewModel), 0, true) as MainViewModel;
            if(window == null)
            {
                return;
            }

            var dte = GetGlobalService(typeof(DTE)) as DTE2;
            if(dte == null)
            {
                return;
            }

            dte.Events.WindowEvents.WindowActivated += delegate { window.UpdateDte2(dte); };

            dte.Events.SolutionEvents.BeforeClosing += () => window.UpdateDte2(dte);
            dte.Events.SolutionEvents.Opened        += () => window.UpdateDte2(dte);
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
            var window = FindToolWindow(typeof(MainViewModel), 0, false) as MainViewModel;
            if((window == null) || (window.Frame == null))
            {
                return;
            }

            var vsWindowFrame = window.Frame as IVsWindowFrame;
            if(vsWindowFrame == null)
            {
                return;
            }

            ErrorHandler.ThrowOnFailure(vsWindowFrame.Show());
        }

        #endregion Private Methods
    }
}
