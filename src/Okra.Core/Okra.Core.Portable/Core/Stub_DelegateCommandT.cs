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

#pragma warning disable 0067 // Ignore CS0067 "The event 'CanExecuteChanged' is never used" (since this is only a stub)
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067
        
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
