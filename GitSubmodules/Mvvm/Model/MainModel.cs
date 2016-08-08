using System.Collections.Generic;
using EnvDTE;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.ViewModel;

namespace GitSubmodules.Mvvm.Model
{
    /// <summary>
    /// The main model that contains all data for the <see cref="MainViewModel"/>
    /// </summary>
    public sealed class MainModel : ModelBase
    {
        #region Public Properties

        /// <summary>
        /// A list with all found submodules
        /// </summary>
        public IEnumerable<Submodule> ListOfSubmodules
        {
            get { return _listOfSubmodules; }
            internal set
            {
                _listOfSubmodules = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The path to the current opend solution
        /// </summary>
        public string CurrentSolutionPath
        {
            get { return _currentSolutionPath; }
            internal set
            {
                _currentSolutionPath= value;
                OnPropertyChanged();
            }
        }

        #endregion Public Properties

        #region Internal Fields

        /// <summary>
        /// Indicate that a command can execute
        /// </summary>
        internal bool CanExecuteCommand;

        /// <summary>
        /// The separate <see cref="OutputWindowPane"/> for logging debug and error informations
        /// </summary>
        internal OutputWindowPane OutputPane;

        /// <summary>
        /// The complete path to the current opend solution (inclusive filename and file extension)
        /// </summary>
        internal string CurrentSolutionFullName;

        /// <summary>
        /// Counter of git calls with arguments
        /// </summary>
        internal int GitCounter;

        #endregion Internal Fields

        #region Private Backing-Fields

        /// <summary>
        /// The Backing-field for <see cref="ListOfSubmodules"/>
        /// </summary>
        private IEnumerable<Submodule> _listOfSubmodules;

        /// <summary>
        /// The Backing-field for <see cref="CurrentSolutionPath"/>
        /// </summary>
        private string _currentSolutionPath;

        #endregion Private Backing-Fields
    }
}
