using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Okra.Core
{
    public class DelegateCommand<T> : ICommand
    {
        // *** Fields ***

        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        // *** Events ***

        public event EventHandler CanExecuteChanged;

        // *** Constructors ***

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            // Validate arguments

            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));

            // Store parameters

            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<T> execute)
            : this(execute, _ => true)
        {
        }

        // *** ICommand Methods ***

        public bool CanExecute(object parameter)
        {
            // If the parameter is of an unexpected type then return false
            // NB: We also compare to default(T) so that nullable types accept 'null' parameters but non-nullable types do not

            if (!(parameter is T) && parameter != (object)default(T))
                return false;

            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        // *** Methods ***

        public void NotifyCanExecuteChanged()
        {
            EventHandler eventHandler = CanExecuteChanged;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }
    }
}
