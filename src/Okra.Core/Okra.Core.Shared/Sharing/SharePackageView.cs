using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.Sharing
{
    public class SharePackageView : ISharePackageView
    {
        // *** Fields ***

        private readonly DataPackageView dataPackageView;
        private readonly SharePropertySet properties;

        // *** Constructors ***

        public SharePackageView(DataPackageView dataPackageView)
        {
            if (dataPackageView == null)
                throw new ArgumentNullException("dataPackageView");

            this.dataPackageView = dataPackageView;
            this.properties = new SharePropertySet(dataPackageView.Properties);
        }

        // *** Properties ***

        public IReadOnlyList<string> AvailableFormats
        {
            get
            {
                return dataPackageView.AvailableFormats;
            }
        }

        public ISharePropertySet Properties
        {
            get
            {
                return properties;
            }
        }

        // *** Methods ***

        public bool Contains(string formatId)
        {
            if (string.IsNullOrEmpty(formatId))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "formatId");

            return dataPackageView.Contains(formatId);
        }

        public Task<T> GetDataAsync<T>(string formatId)
        {
            if (string.IsNullOrEmpty(formatId))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "formatId");

            return GetDataAsyncInternal<T>(formatId);
        }

        private async Task<T> GetDataAsyncInternal<T>(string formatId)
        {
            object data = await dataPackageView.GetDataAsync(formatId);
            return (T)data;
        }

        // *** Private sub-classes ***

        private class SharePropertySet : ISharePropertySet
        {
            // *** Fields ***

            private readonly DataPackagePropertySetView propertySetView;

            // *** Constructors ***

            public SharePropertySet(DataPackagePropertySetView propertySetView)
            {
                this.propertySetView = propertySetView;
            }

            // *** Properties ***

            public string Title
            {
                get
                {
                    return propertySetView.Title;
                }
                set
                {
                    throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotModifyShareProperties"));
                }
            }

            public string Description
            {
                get
                {
                    return propertySetView.Description;
                }
                set
                {
                    throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotModifyShareProperties"));
                }
            }
        }
    }
}
