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

        private readonly DataPackageView _dataPackageView;

        // *** Constructors ***

        public SharePackageView(DataPackageView dataPackageView)
        {
            if (dataPackageView == null)
                throw new ArgumentNullException(nameof(dataPackageView));

            _dataPackageView = dataPackageView;
            Properties = new SharePropertySet(dataPackageView.Properties);
        }

        // *** Properties ***

        public IReadOnlyList<string> AvailableFormats
        {
            get
            {
                return _dataPackageView.AvailableFormats;
            }
        }

        public ISharePropertySet Properties { get; }

        // *** Methods ***

        public bool Contains(string formatId)
        {
            if (string.IsNullOrEmpty(formatId))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(formatId));

            return _dataPackageView.Contains(formatId);
        }

        public Task<T> GetDataAsync<T>(string formatId)
        {
            if (string.IsNullOrEmpty(formatId))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(formatId));

            return GetDataAsyncInternal<T>(formatId);
        }

        private async Task<T> GetDataAsyncInternal<T>(string formatId)
        {
            object data = await _dataPackageView.GetDataAsync(formatId);
            return (T)data;
        }

        // *** Private sub-classes ***

        private class SharePropertySet : ISharePropertySet
        {
            // *** Fields ***

            private readonly DataPackagePropertySetView _propertySetView;

            // *** Constructors ***

            public SharePropertySet(DataPackagePropertySetView propertySetView)
            {
                _propertySetView = propertySetView;
            }

            // *** Properties ***

            public string Title
            {
                get
                {
                    return _propertySetView.Title;
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
                    return _propertySetView.Description;
                }
                set
                {
                    throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotModifyShareProperties"));
                }
            }
        }
    }
}
