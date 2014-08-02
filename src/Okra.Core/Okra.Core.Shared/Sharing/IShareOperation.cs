using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public interface IShareOperation
    {
        // *** Properties ***

        ISharePackageView Data { get; }

        // *** Methods ***

        void ReportCompleted();
        void ReportDataRetrieved();
        void ReportError(string value);
        void ReportStarted();
        void ReportSubmittedBackgroundTask();
    }
}
