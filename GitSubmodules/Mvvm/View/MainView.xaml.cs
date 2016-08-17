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
        private void SubmoduleOpenFolder(object sender, RoutedEventArgs e)
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
        private void SubmoduleInit(object sender, RoutedEventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OneInit);
        }

        /// <summary>
        /// Event method for deinit a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleDeinit(object sender, RoutedEventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OneDeinit);
        }

        /// <summary>
        /// Event method for force deinit a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleDeinitForce(object sender, RoutedEventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OneDeinitForce);
        }

        /// <summary>
        /// Event method for update a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleUpdate(object sender, RoutedEventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OneUpdate);
        }

        /// <summary>
        /// Event method for force update a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleUpdateForce(object sender, RoutedEventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OneUpdateForce);
        }

        /// <summary>
        /// Event method for pull from origin master for a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmodulePullOriginMaster(object sender, RoutedEventArgs e)
        {
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OnePullOriginMaster);
            ViewModel.DoStartGit(SubmoduleHelper.TryToGetSubmoduleFromTag(sender), SubModuleCommand.OneStatus);
        }

        /// <summary>
        /// Event method for copy the id of the submodule to the clipboard
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyIdToClipboard(object sender, RoutedEventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if(submodule == null)
            {
                return;
            }

            Clipboard.SetText(submodule.Id);
        }

        /// <summary>
        /// Event method for copy the complete tag of the submodule to the clipboard
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void CopyTagToClipboard(object sender, RoutedEventArgs e)
        {
            var submodule = SubmoduleHelper.TryToGetSubmoduleFromTag(sender);
            if(submodule == null)
            {
                return;
            }

            Clipboard.SetText(submodule.CompleteTag);
        }

        #endregion Private Methods
    }
}
