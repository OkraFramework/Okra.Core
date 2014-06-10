using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Okra.Search
{
    public interface ISearchManager
    {
        // *** Properties ***

        string SearchPageName { get; set; }
    }
}
