using System;
using System.Linq;
using System.Windows;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.Model;
using GitSubmodules.Mvvm.ViewModel;

namespace GitSubmodules.Mvvm.View
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    internal sealed partial class MainView
    {
        #region Public Properties

        /// <summary>
        /// The view model for the this view that contains all logic and the model
        /// </summary>
        public MainViewModel ViewModel { get; private set; }

        #endregion Public Properties

        #region Internal Constructor

        /// <summary>
        /// Constructor for the <see cref="MainView"/>
        /// </summary>
        /// <param name="viewModel"></param>
        internal MainView(MainViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        #endregion Internal Constructor

        #region Private Methods

        /// <summary>
        /// Event method for open a folder of a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleOpenFolder(object sender, EventArgs e)
        {
            var frameworkContentElement = sender as FrameworkContentElement;
            if(frameworkContentElement != null)
            {
                ViewModel.DoOpenFolder(frameworkContentElement.Tag as Submodule);
                return;
            }

            ViewModel.DoOpenFolder(SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for init a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleInit(object sender, EventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleCommand.OneInit, SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for deinit a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleDeinit(object sender, EventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleCommand.OneDeinit, SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for force deinit a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleDeinitForce(object sender, EventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleCommand.OneDeinitForce, SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for update a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleUpdate(object sender, EventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleCommand.OneUpdate, SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for force update a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleUpdateForce(object sender, EventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleCommand.OneUpdateForce, SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for pull from origin master for a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmodulePullOriginMaster(object sender, EventArgs e)
        {
            ViewModel.DoPullOriginMaster(SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        /// <summary>
        /// Event method for copy the <see cref="Submodule.CompleteId"/> of the submodule to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyCompleteIdToClipboard(object sender, EventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if(submodule == null)
            {
                return;
            }

            TryToSetTextToClipboard(submodule.CompleteId);
        }

        /// <summary>
        /// Event method for copy the <see cref="Submodule.ShortId"/>  id of the submodule to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyShortIdToClipboard(object sender, EventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if(submodule == null)
            {
                return;
            }

            TryToSetTextToClipboard(submodule.ShortId);
        }

        /// <summary>
        /// Event method for copy the <see cref="Submodule.CompleteTag"/> of the submodule to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyCompleteTagToClipboard(object sender, EventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if(submodule == null)
            {
                return;
            }

            TryToSetTextToClipboard(submodule.CompleteTag);
        }

        /// <summary>
        /// Event method for copy the <see cref="Submodule.CurrentBranch"/> of the submodule to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyCurrentBranchToClipboard(object sender, EventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if(submodule == null)
            {
                return;
            }

            TryToSetTextToClipboard(submodule.CurrentBranch);
        }

        /// <summary>
        /// Event method for copy the <see cref="Submodule.ListOfBranches"/> of the submodule to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyBranchListToClipboard(object sender, EventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if((submodule == null) || (submodule.ListOfBranches == null))
            {
                return;
            }

            TryToSetTextToClipboard(submodule.ListOfBranches.Aggregate(string.Empty, (current, next) => current + "\n" + next));
        }

        /// <summary>
        /// Try to set a <see cref="string"/> to the <see cref="Clipboard"/>
        /// </summary>
        /// <param name="textForClipboard">The <see cref="string"/> for the <see cref="Clipboard"/></param>
        private static void TryToSetTextToClipboard(string textForClipboard)
        {
            if(string.IsNullOrEmpty(textForClipboard))
            {
                return;
            }

            try
            {
                Clipboard.SetText(textForClipboard);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Event method to change the information visibility for each module,
        /// show full information on mouse enter and show slim information on mouse leave
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void ExpandOneSubmodule(object sender, EventArgs e)
        {

            ViewModel.ExpandOneSubmodule(SubmoduleHelper.TryToGetSubmoduleFromTag(sender));
        }

        #endregion Private Methods
    }
}
