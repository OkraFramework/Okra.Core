using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.DataTransfer
{
    public interface IDataRequest
    {
        // *** Properties ***

        DataPackage Data { get; set; }
        DateTimeOffset Deadline { get; }

        // *** Methods ***

        void FailWithDisplayText(string displayText);
        DataRequestDeferral GetDeferral();
    }
}
