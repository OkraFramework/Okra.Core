using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace Okra.DataTransfer
{
    public interface IShareOperation
    {
        // *** Properties ***

        DataPackageView Data { get; }
        string QuickLinkId { get; }

        // *** Methods ***

        void RemoveThisQuickLink();
        void ReportCompleted();
        void ReportCompleted(QuickLink quicklink);
        void ReportDataRetrieved();
        void ReportError(string value);
        void ReportStarted();
        void ReportSubmittedBackgroundTask();
    }
}
