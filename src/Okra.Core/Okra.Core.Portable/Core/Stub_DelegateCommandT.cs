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
        // *** Events ***

        public event EventHandler CanExecuteChanged;

        // *** Constructors ***

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            throw new NotImplementedException();
        }

        public DelegateCommand(Action<T> execute)
            : this(null, null)
        {
        }

        // *** ICommand Methods ***

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        // *** Methods ***

        public void NotifyCanExecuteChanged()
        {
            throw new NotImplementedException();
        }
    }
}
