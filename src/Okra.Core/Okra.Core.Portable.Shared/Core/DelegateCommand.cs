using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Okra.Core
{
    public class DelegateCommand : DelegateCommand<object>
    {
        // *** Constructors ***

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : base(_ => execute(), _ => canExecute())
        {
            // Validate arguments
            // NB: Have to do it here as well as base class as we wrap the arguments in our own delegates

            if (execute == null)
                throw new ArgumentNullException("execute");

            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
        }

        public DelegateCommand(Action execute)
            : this(execute, () => true)
        {
        }
    }
}
