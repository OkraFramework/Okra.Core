using $safeprojectname$.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Pages.Home
{
    public class HomeDesignData : HomeViewModel
    {
        public HomeDesignData()
        {
            this.Items = DesignDataSource.CreateGroups();
        }
    }
}
