using System.Windows.Input;
using GitSubmodules.Enumerations;
using GitSubmodules.Helper;
using GitSubmodules.Mvvm.Model;

namespace GitSubmodules.Mvvm.ViewModel
{
    public sealed partial class MainViewModel
    {
        public ICommand CommandAllStatus
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllStatus),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllInit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllInit),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllDeinit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllDeinit),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllDeinitForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllDeinitForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllUpdate
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllUpdate),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllUpdateForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.AllUpdateForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandAllPullOriginMaster
        {
            get
            {
                return new RelayCommand(param => DoAllPullOriginMaster(null),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneInit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneInit),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneDeinit
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneDeinit),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneDeinitForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneDeinitForce),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneUpdate
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneUpdate),
                                        param => Model.CanExecuteCommand);
            }
        }

        public ICommand CommandOneUpdateForce
        {
            get
            {
                return new RelayCommand(param => DoStartGit(param as Submodule, SubModuleCommand.OneUpdateForce),
                                        param => Model.CanExecuteCommand);
            }
        }

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
