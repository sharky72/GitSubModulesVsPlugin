using System.Windows.Input;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.Model;

namespace GitSubmodules.Mvvm.ViewModel
{
    /// <summary>
    /// Partial class that contains all <see cref="ICommand"/>s for the <see cref="MainViewModel"/>
    /// </summary>
    public sealed partial class MainViewModel
    {
        /// <summary>
        /// Command for fetch all submodules
        /// </summary>
        public ICommand CommandAllFetch
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.AllFetch),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for init all submodules
        /// </summary>
        public ICommand CommandAllInit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.AllInit),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for deinit all submodules
        /// </summary>
        public ICommand CommandAllDeinit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.AllDeinit),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for force deinit all submodules
        /// </summary>
        public ICommand CommandAllDeinitForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.AllDeinitForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for upgrade all submodules
        /// </summary>
        public ICommand CommandAllUpdate
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.AllUpdate),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for force upgrade all submodules
        /// </summary>
        public ICommand CommandAllUpdateForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.AllUpdateForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for pull from origin master for all submodules
        /// </summary>
        public ICommand CommandAllPullOriginMaster
        {
            get
            {
                return new RelayCommand(param => DoPullOriginMaster(null),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for init one submodules
        /// </summary>
        public ICommand CommandOneInit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.OneInit, param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for deinit one submodules
        /// </summary>
        public ICommand CommandOneDeinit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.OneDeinit, param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for force deinit one submodules
        /// </summary>
        public ICommand CommandOneDeinitForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.OneDeinitForce, param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for update one submodules
        /// </summary>
        public ICommand CommandOneUpdate
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.OneUpdate, param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for force update one submodules
        /// </summary>
        public ICommand CommandOneUpdateForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(SubmoduleCommand.OneUpdateForce, param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }

        /// <summary>
        /// Command for pull from origin master for one submodules
        /// </summary>
        public ICommand CommandOnePullOriginMaster
        {
            get
            {
                return new RelayCommand(param => DoPullOriginMaster(param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }
    }
}
