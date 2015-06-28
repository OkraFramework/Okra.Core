using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public static class SharePackageViewEx
    {
        // *** Static Methods - GetXxx ***

        public static Task<Uri> GetApplicationLinkAsync(this ISharePackageView sharePackageView)
        {
            if (sharePackageView == null)
                throw new ArgumentNullException(nameof(sharePackageView));

            return GetApplicationLinkAsyncInternal(sharePackageView);
        }

        private async static Task<Uri> GetApplicationLinkAsyncInternal(this ISharePackageView sharePackageView)
        {
            return (Uri)await sharePackageView.GetDataAsync<Uri>(StandardShareFormats.ApplicationLink);
        }

        public static Task<string> GetHtmlFormatAsync(this ISharePackageView sharePackageView)
        {
            if (sharePackageView == null)
                throw new ArgumentNullException(nameof(sharePackageView));

            return GetHtmlFormatAsyncInternal(sharePackageView);
        }

        private async static Task<string> GetHtmlFormatAsyncInternal(this ISharePackageView sharePackageView)
        {
            return (string)await sharePackageView.GetDataAsync<string>(StandardShareFormats.Html);
        }

        public static Task<string> GetRtfAsync(this ISharePackageView sharePackageView)
        {
            if (sharePackageView == null)
                throw new ArgumentNullException(nameof(sharePackageView));

            return GetRtfAsyncInternal(sharePackageView);
        }

        private async static Task<string> GetRtfAsyncInternal(this ISharePackageView sharePackageView)
        {
            return (string)await sharePackageView.GetDataAsync<string>(StandardShareFormats.Rtf);
        }

        public static Task<string> GetTextAsync(this ISharePackageView sharePackageView)
        {
            if (sharePackageView == null)
                throw new ArgumentNullException(nameof(sharePackageView));

            return GetTextAsyncInternal(sharePackageView);
        }

        private async static Task<string> GetTextAsyncInternal(this ISharePackageView sharePackageView)
        {
            return (string)await sharePackageView.GetDataAsync<string>(StandardShareFormats.Text);
        }

        public static Task<Uri> GetWebLinkAsync(this ISharePackageView sharePackageView)
        {
            if (sharePackageView == null)
                throw new ArgumentNullException(nameof(sharePackageView));

            return GetWebLinkAsyncInternal(sharePackageView);
        }

        private async static Task<Uri> GetWebLinkAsyncInternal(this ISharePackageView sharePackageView)
        {
            return (Uri)await sharePackageView.GetDataAsync<Uri>(StandardShareFormats.WebLink);
        }
    }
}
