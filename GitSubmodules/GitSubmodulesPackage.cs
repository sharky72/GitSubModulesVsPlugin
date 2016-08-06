using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using EnvDTE;
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
        private void ShowToolWindow(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));

            var window = FindToolWindow(typeof(MainViewModel), 0, true) as MainViewModel;
            if((window == null) || (window.Frame == null))
            {
                throw new NotSupportedException("Can not create tool window");
            }

            var dte = GetGlobalService(typeof(DTE)) as DTE;

            if(dte != null)
            {
                dte.Events.SolutionEvents.Opened        += () => window.UpdateSolutionFullName(dte.Solution.FullName);
                dte.Events.SolutionEvents.BeforeClosing += () => window.UpdateSolutionFullName(dte.Solution.FullName);
                dte.Events.WindowEvents.WindowActivated += delegate { window.UpdateSolutionFullName(dte.Solution.FullName); };
            }

            var vsWindowFrame = window.Frame as IVsWindowFrame;

            if(vsWindowFrame == null)
            {
                return;
            }

            ErrorHandler.ThrowOnFailure(vsWindowFrame.Show());
        }

        #region Package Members

        /// <summary>
        /// Called when the VSPackage is loaded by Visual Studio
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", ToString()));

            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if(mcs == null)
            {
                return;
            }

            mcs.AddCommand(new MenuCommand(ShowToolWindow, new CommandID(GuidList.GuidVsPackage3CmdSet,
                                                                         Convert.ToInt32(PkgCmdIdList.CmdidMyTool))));
        }

        #endregion
    }
}
