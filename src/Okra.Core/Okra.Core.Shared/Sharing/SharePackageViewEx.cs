using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public static class SharePackageViewEx
    {
        // *** Static Methods - GetXxx ***

        public async static Task<Uri> GetApplicationLinkAsync(this ISharePackageView sharePackageView)
        {
            return (Uri)await sharePackageView.GetDataAsync<Uri>(StandardShareFormats.ApplicationLink);
        }

        public async static Task<string> GetHtmlFormatAsync(this ISharePackageView sharePackageView)
        {
            return (string)await sharePackageView.GetDataAsync<string>(StandardShareFormats.Html);
        }

        public async static Task<string> GetRtfAsync(this ISharePackageView sharePackageView)
        {
            return (string)await sharePackageView.GetDataAsync<string>(StandardShareFormats.Rtf);
        }

        public async static Task<string> GetTextAsync(this ISharePackageView sharePackageView)
        {
            return (string)await sharePackageView.GetDataAsync<string>(StandardShareFormats.Text);
        }

        public async static Task<Uri> GetWebLinkAsync(this ISharePackageView sharePackageView)
        {
            return (Uri)await sharePackageView.GetDataAsync<Uri>(StandardShareFormats.WebLink);
        }
    }
}
