using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$
{
    public class $fileinputname$DesignData : $fileinputname$ViewModel
    {
        public $fileinputname$DesignData()
        {
            // Initialize design-time data here

            this.QueryText = "\u201cDesign-Time Query\u201d";

            this.Filters = new[]
            {
                new Filter(this, "First Filter", 10) { Active = true },
                new Filter(this, "Second Filter", 20),
                new Filter(this, "Third Filter", 30)
            };
            this.ShowFilters = true;
        }
    }
}
