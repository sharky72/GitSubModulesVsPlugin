using System.Windows;
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
    }
}
