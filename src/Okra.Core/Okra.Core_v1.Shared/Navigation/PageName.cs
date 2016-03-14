using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public static class PageName
    {
        public static string FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.FullName;
        }
    }
}
