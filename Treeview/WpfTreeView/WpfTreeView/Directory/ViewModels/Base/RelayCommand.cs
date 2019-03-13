using System;
using System.Windows.Input;

namespace WpfTreeView
{
    /// <summary>
    /// a basic command that runs an action 
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Private Members

        //An action is a function with no parameters and no return values
        private Action mAction;

        #endregion

        #region Public events

        /// <summary>
        /// The event that is fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructors

        public RelayCommand(Action action)
        {
            mAction = action;        
        }

        #endregion

        #region Command methods

        /// <summary>
        /// a relay command can always execute, i.e.: the button is always enabled, not greyed out
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute (object parameter) => true; 
        
        /// <summary>
        /// Executes the action that is passed in the constructor
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            mAction();
        }

        #endregion
    }
}
