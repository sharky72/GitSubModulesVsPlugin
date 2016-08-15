using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
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

        /// <summary>
        /// The <see cref="Version"/> of the current installed git version
        /// </summary>
        public string GitVersion
        {
            get { return _gitVersion; }
            internal set
            {
                _gitVersion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The main forground color for all text elements
        /// </summary>
        public Brush Foreground
        {
            get { return _foreground; }
            internal set
            {
                _foreground = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicate that the <see cref="ProgressBar"/> for the git process is shown
        /// </summary>
        public bool ShowWatingIndicator
        {
            get { return _showWatingIndicator; }
            set
            {
                _showWatingIndicator = value;
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

        /// <summary>
        /// <see cref="AutoResetEvent"/> to wait for a finished of git process
        /// </summary>
        internal AutoResetEvent WaitingTimer;

        /// <summary>
        /// Indicate that Git is present
        /// </summary>
        internal bool GitIsPresent;

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

        /// <summary>
        /// The Backing-field for <see cref="GitVersion"/>
        /// </summary>
        private string _gitVersion;

        /// <summary>
        /// The Backing-field for <see cref="Foreground"/>
        /// </summary>
        private Brush _foreground;

        /// <summary>
        /// The Backing-field for <see cref="ShowWatingIndicator"/>
        /// </summary>
        private bool _showWatingIndicator;

        #endregion Private Backing-Fields
    }
}
