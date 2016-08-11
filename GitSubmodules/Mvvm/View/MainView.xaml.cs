using System.Windows;
using GitSubmodules.Enumerations;
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

        public MainViewModel ViewModel { get; private set; }

        #endregion Public Properties

        #region Internal Constructor

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
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoOpenFolder(frameworkElement.Tag as Submodule);
        }

        /// <summary>
        /// Event method for init a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleInit(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OneInit);
        }

        /// <summary>
        /// Event method for deinit a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleDeinit(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OneDeinit);
        }

        /// <summary>
        /// Event method for force deinit a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleDeinitForce(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OneDeinitForce);
        }

        /// <summary>
        /// Event method for update a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleUpdate(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OneUpdate);
        }

        /// <summary>
        /// Event method for force update a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmoduleUpdateForce(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OneUpdateForce);
        }

        /// <summary>
        /// Event method for pull from origin master for a given <see cref="Submodule"/> in the system file-explorer
        /// </summary>
        /// <param name="sender">The sender that contains the <see cref="Submodule"/> information</param>
        /// <param name="e">The arguments for this event</param>
        private void SubmodulePullOriginMaster(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if(frameworkElement == null)
            {
                return;
            }

            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OnePullOriginMaster);
            ViewModel.DoStartGit(frameworkElement.Tag as Submodule, SubModuleCommand.OneStatus);
        }

        #endregion Private Methods
    }
}
