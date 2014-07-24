using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DataTransfer
{
    public delegate Task<T> AsyncDataProvider<T>(string formatId, DateTimeOffset deadline);

    public static class DataPackageEx
    {
        // *** Static Methods ***

        public static void SetAsyncDataProvider(this DataPackage dataPackage, string formatId, AsyncDataProvider<object> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncApplicationLink(this DataPackage dataPackage, AsyncDataProvider<Uri> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncBitmap(this DataPackage dataPackage, AsyncDataProvider<RandomAccessStreamReference> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncHtmlFormat(this DataPackage dataPackage, AsyncDataProvider<string> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncRtf(this DataPackage dataPackage, AsyncDataProvider<string> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncStorageItems(this DataPackage dataPackage, AsyncDataProvider<IEnumerable<IStorageItem>> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncText(this DataPackage dataPackage, AsyncDataProvider<string> delayRenderer)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncWebLink(this DataPackage dataPackage, AsyncDataProvider<Uri> delayRenderer)
        {
            throw new NotImplementedException();
        }
    }
}
