using System.Collections.Generic;
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
        /// Indicate that a command can execute
        /// </summary>
        public bool CanExecuteCommand
        {
            get { return _canExecuteCommand; }
            internal set
            {
                _canExecuteCommand = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The console output for informations and errorsa
        /// </summary>
        public string ConsoleOutput
        {
            get { return _consoleOutput; }
            internal set
            {
                _consoleOutput = value;
                OnPropertyChanged();
            }
        }

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

        #region Private Backing-Fields

        /// <summary>
        /// The Backing-field for <see cref="ListOfSubmodules"/>
        /// </summary>
        private IEnumerable<Submodule> _listOfSubmodules;

        /// <summary>
        /// The Backing-field for <see cref="CanExecuteCommand"/>
        /// </summary>
        private bool _canExecuteCommand;

        /// <summary>
        /// The Backing-field for <see cref="ConsoleOutput"/>
        /// </summary>
        private string _consoleOutput;

        /// <summary>
        /// The Backing-field for <see cref="CurrentSolutionPath"/>
        /// </summary>
        private string _currentSolutionPath;

        #endregion Private Backing-Fields
    }
}
