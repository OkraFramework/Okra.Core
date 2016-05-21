using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Search
{
    public interface ISearchPage
    {
        // *** Methods ***

        void PerformQuery(string queryText, string language);
    }
}
