using System;

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
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public DelegateCommand(Action execute)
            : this(execute, () => true)
        {
        }
    }
}
