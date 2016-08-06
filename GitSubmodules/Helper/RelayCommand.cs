using System;
using System.Windows.Input;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier handle with <see cref="ICommand"/>s
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        #region Private Fields

        /// <summary>
        /// The excute <see cref="Action{T}"/> of this command
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// The <see cref="Predicate{T}"/> of this command
        /// </summary>
        private readonly Predicate<object> _canExecute;

        #endregion Private Fields

        #region Public Constructor

        /// <summary>
        /// Constructor for <see cref="RelayCommand"/>,
        /// generate a new <see cref="ICommand"/> with the given <see cref="Action{T}"/> and <see cref="Predicate{T}"/>
        /// </summary>
        /// <param name="execute">The excute <see cref="Action{T}"/> of this command</param>
        /// <param name="canExecute">The <see cref="Predicate{T}"/> of this command</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute   = execute;
            _canExecute = canExecute;
        }

        #endregion Public Constructor

        #region ICommand Member

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state
        /// </summary>
        /// <param name="parameter">Data used by the command.
        /// If the command does not require data to be passed, this object can be set to null</param>
        /// <returns><c>true</c> if this command can be executed, otherwise <c>false</c></returns>
        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) || _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
               add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked
        /// </summary>
        /// <param name="parameter">Data used by the command.
        /// If the command does not require data to be passed, this object can be set to null</param>
        public void Execute(object parameter)
        {
            if(_execute == null)
            {
                return;
            }

            _execute(parameter);
        }

        #endregion ICommand Member
    }
}
