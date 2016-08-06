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
    }
}
