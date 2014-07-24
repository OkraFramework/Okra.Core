using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Core
{
    public class DelegateCommand : DelegateCommand<object>
    {
        // *** Constructors ***

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : base(null, null)
        {
            throw new NotImplementedException();
        }

        public DelegateCommand(Action execute)
            : this(null, null)
        {
        }
    }
}
