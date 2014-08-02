using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Sharing
{
    public static class SharePackageEx
    {
        // *** Static Methods - SetXxx ***

        public static void SetApplicationLink(this ISharePackage sharePackage, Uri value)
        {
            sharePackage.SetData(StandardShareFormats.ApplicationLink, value);
        }

        public static void SetHtmlFormat(this ISharePackage sharePackage, string value)
        {
            sharePackage.SetData(StandardShareFormats.Html, value);
        }

        public static void SetRtf(this ISharePackage sharePackage, string value)
        {
            sharePackage.SetData(StandardShareFormats.Rtf, value);
        }

        public static void SetText(this ISharePackage sharePackage, string value)
        {
            sharePackage.SetData(StandardShareFormats.Text, value);
        }

        public static void SetWebLink(this ISharePackage sharePackage, Uri value)
        {
            sharePackage.SetData(StandardShareFormats.WebLink, value);
        }

        // *** Static Methods - SetAsyncXxx ***

        public static void SetAsyncApplicationLink(this ISharePackage sharePackage, AsyncDataProvider<Uri> dataProvider)
        {
            sharePackage.SetAsyncData(StandardShareFormats.ApplicationLink, dataProvider);
        }

        public static void SetAsyncHtmlFormat(this ISharePackage sharePackage, AsyncDataProvider<string> dataProvider)
        {
            sharePackage.SetAsyncData(StandardShareFormats.Html, dataProvider);
        }

        public static void SetAsyncRtf(this ISharePackage sharePackage, AsyncDataProvider<string> dataProvider)
        {
            sharePackage.SetAsyncData(StandardShareFormats.Rtf, dataProvider);
        }

        public static void SetAsyncText(this ISharePackage sharePackage, AsyncDataProvider<string> dataProvider)
        {
            sharePackage.SetAsyncData(StandardShareFormats.Text, dataProvider);
        }

        public static void SetAsyncWebLink(this ISharePackage sharePackage, AsyncDataProvider<Uri> dataProvider)
        {
            sharePackage.SetAsyncData(StandardShareFormats.WebLink, dataProvider);
        }
    }
}
