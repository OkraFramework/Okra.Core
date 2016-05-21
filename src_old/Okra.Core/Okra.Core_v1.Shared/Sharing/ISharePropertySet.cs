using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Okra.Sharing
{
    public interface ISharePropertySet
    {
        string Description { get; set; }
        string Title { get; set; }
    }
}
