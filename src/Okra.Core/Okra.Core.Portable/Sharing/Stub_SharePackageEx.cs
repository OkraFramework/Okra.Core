using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public static class SharePackageEx
    {
        // *** Static Methods - SetXxx ***

        public static void SetApplicationLink(this ISharePackage sharePackage, Uri value)
        {
            throw new NotImplementedException();
        }

        public static void SetHtmlFormat(this ISharePackage sharePackage, string value)
        {
            throw new NotImplementedException();
        }

        public static void SetRtf(this ISharePackage sharePackage, string value)
        {
            throw new NotImplementedException();
        }

        public static void SetText(this ISharePackage sharePackage, string value)
        {
            throw new NotImplementedException();
        }

        public static void SetWebLink(this ISharePackage sharePackage, Uri value)
        {
            throw new NotImplementedException();
        }

        // *** Static Methods - SetAsyncXxx ***

        public static void SetAsyncApplicationLink(this ISharePackage sharePackage, AsyncDataProvider<Uri> dataProvider)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncHtmlFormat(this ISharePackage sharePackage, AsyncDataProvider<string> dataProvider)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncRtf(this ISharePackage sharePackage, AsyncDataProvider<string> dataProvider)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncText(this ISharePackage sharePackage, AsyncDataProvider<string> dataProvider)
        {
            throw new NotImplementedException();
        }

        public static void SetAsyncWebLink(this ISharePackage sharePackage, AsyncDataProvider<Uri> dataProvider)
        {
            throw new NotImplementedException();
        }
    }
}
