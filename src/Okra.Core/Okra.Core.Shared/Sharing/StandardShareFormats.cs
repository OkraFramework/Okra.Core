using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.Sharing
{
    public static class StandardShareFormats
    {
        public static string ApplicationLink => StandardDataFormats.ApplicationLink;
        public static string Html => StandardDataFormats.Html;
        public static string Rtf => StandardDataFormats.Rtf;
        public static string Text => StandardDataFormats.Text;
        public static string WebLink => StandardDataFormats.WebLink;
    }
}
