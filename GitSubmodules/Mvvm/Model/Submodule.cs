using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;

namespace GitSubmodules.Mvvm.Model
{
    /// <summary>
    /// Model that contains all data of a git submodule
    /// </summary>
    public sealed class Submodule : ModelBase
    {
        #region Public Properties

        /// <summary>
        /// The name of the submodule
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The id of the submodule (SHA1)
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The commit id of the submodule (SHA1)
        /// </summary>
        public string CommitId { get; private set; }

        /// <summary>
        /// The background color for this module that indicate the crrent status of it
        /// </summary>
        public SolidColorBrush BackgroundColor { get; private set; }

        #endregion Public Properties

        #region Private Backing-Fields

        /// <summary>
        /// The current status of this module
        /// </summary>
        private SubModuleStatus _status;

        #endregion Private Backing-Fields

        #region Internal Constructor

        /// <summary>
        /// Creates a new <see cref="Submodule"/> with the given information
        /// </summary>
        /// <param name="solutionPath">The path to the current opend solution</param>
        /// <param name="subModuleInformation">The <see cref="string"/>
        /// that contains informations of the submodule</param>
        internal Submodule(string solutionPath, string subModuleInformation)
        {
            if(string.IsNullOrEmpty(subModuleInformation))
            {
                return;
            }

            var lineSplit = subModuleInformation.TrimStart().Split(' ');

            Id       = lineSplit.FirstOrDefault();
            Name     = lineSplit.ElementAtOrDefault(1) ?? "???";
            CommitId = lineSplit.ElementAtOrDefault(2) ?? "???";

            Id = !string.IsNullOrEmpty(Id) ? Id = Id.Substring(1, Id.Length - 1) : "???";

            SetSubModuleStatus(solutionPath, subModuleInformation);
            SetBackgroundColor();
        }

        #endregion Internal Constructor

        #region Internal Methods

        /// <summary>
        /// Set the background color for thsi module based on the <see cref="_status"/>
        /// </summary>
        internal void SetBackgroundColor()
        {
            switch(_status)
            {
                case SubModuleStatus.Unknown:
                    BackgroundColor = Brushes.LightGray;
                    break;

                case SubModuleStatus.NotRegistered:
                    BackgroundColor = Brushes.LightCoral;
                    break;

                case SubModuleStatus.Registered:
                    BackgroundColor = Brushes.LightYellow;
                    break;

                case SubModuleStatus.MergeConflict:
                    BackgroundColor = Brushes.LightSteelBlue;
                    break;

                case SubModuleStatus.Current:
                    BackgroundColor = Brushes.YellowGreen;
                    break;

                case SubModuleStatus.NotCurrent:
                    BackgroundColor = Brushes.LightGreen;
                    break;

                default:
                    BackgroundColor = Brushes.White;
                    break;
            }
        }

        /// <summary>
        /// Set the <see cref="_status"/> of this module, based on the given information
        /// </summary>
        /// <param name="solutionPath">The path to the current opend solution</param>
        /// <param name="subModuleInformation">The <see cref="string"/>
        /// that contains informations of the submodule</param>
        internal void SetSubModuleStatus(string solutionPath, string subModuleInformation)
        {
            if(string.IsNullOrEmpty(subModuleInformation))
            {
                return;
            }

            switch(subModuleInformation.FirstOrDefault())
            {
                case ' ':
                    _status = SubModuleStatus.Current;
                    break;

                case 'U':
                    _status = SubModuleStatus.MergeConflict;
                    break;

                case '+':
                    _status = SubModuleStatus.NotCurrent;
                    break;

                case '-':
                    SetModuleRegistrationStatus(solutionPath);
                    break;

                default:
                    _status = SubModuleStatus.Unknown;
                    break;
            }
        }

        /// <summary>
        /// Set the registration status of a submodule (loking into the /.git/config)
        /// </summary>
        /// <param name="solutionPath">The path to the current opend solution</param>
        internal void SetModuleRegistrationStatus(string solutionPath)
        {
            if(string.IsNullOrEmpty(solutionPath))
            {
                return;
            }

            var gitConfigFilePath = Path.Combine(solutionPath, ".git", "config");

            try
            {
                using(var fileStream = File.Open(gitConfigFilePath, FileMode.Open, FileAccess.Read))
                {
                    using(var streamReader = new StreamReader(fileStream))
                    {
                        if(streamReader.ReadToEnd().Contains("[submodule \"" + Name + "\"]"))
                        {
                            _status  = SubModuleStatus.Registered;
                            CommitId = "Submodule is registered";
                            return;
                        }
                    }

                    _status  = SubModuleStatus.NotRegistered;
                    CommitId = "Submodule is not registered";
                }
            }
            catch(Exception exception)
            {
                CommitId = exception.Message;
                _status  = SubModuleStatus.NotRegistered;
            }
        }

        #endregion Internal Methods
    }
}
