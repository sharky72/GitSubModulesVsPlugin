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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllFetch),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllInit),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllDeinit),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllDeinitForce),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllUpdate),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllUpdateForce),
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
                return new RelayCommand(param => DoAllPullOriginMaster(null),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneInit),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneDeinit),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneDeinitForce),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneUpdate),
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
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneUpdateForce),
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
                return new RelayCommand(param => DoAllPullOriginMaster(param as Submodule),
                                        param => Model.CanExecuteCommand);
            }
        }
    }
}
