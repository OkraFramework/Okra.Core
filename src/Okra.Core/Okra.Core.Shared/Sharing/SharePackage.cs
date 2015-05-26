using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.Sharing
{
    public class SharePackage : ISharePackage
    {
        // *** Fields ***

        private readonly DataPackage _dataPackage;
        private readonly SharePropertySet _properties;

        // *** Constructors ***

        public SharePackage(DataPackage dataPackage)
        {
            if (dataPackage == null)
                throw new ArgumentNullException("dataPackage");

            _dataPackage = dataPackage;
            _properties = new SharePropertySet(dataPackage.Properties);
        }

        // *** Properties ***

        public ISharePropertySet Properties
        {
            get
            {
                return _properties;
            }
        }

        // *** Methods ***

        public void SetData<T>(string formatId, T value)
        {
            if (string.IsNullOrEmpty(formatId))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "formatId");

            _dataPackage.SetData(formatId, value);
        }

        public void SetAsyncData<T>(string formatId, AsyncDataProvider<T> dataProvider)
        {
            if (string.IsNullOrEmpty(formatId))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "formatId");

            if (dataProvider == null)
                throw new ArgumentNullException("dataProvider");

            _dataPackage.SetDataProvider(formatId, (DataProviderRequest request) => DataProviderRequestHandler<T>(request, dataProvider));
        }

        // *** Private Methods ***

        private async void DataProviderRequestHandler<T>(DataProviderRequest request, AsyncDataProvider<T> delayRenderer)
        {
            // Get a deferral for the duration of the request

            DataProviderDeferral deferral = request.GetDeferral();

            // Get the data to return from the data provider

            object data = await delayRenderer(request.FormatId);
            request.SetData(data);

            // Complete the deferral

            deferral.Complete();
        }

        // *** Private sub-classes ***

        private class SharePropertySet : ISharePropertySet
        {
            // *** Fields ***

            private readonly DataPackagePropertySet _propertySet;

            // *** Constructors ***

            public SharePropertySet(DataPackagePropertySet propertySet)
            {
                _propertySet = propertySet;
            }

            // *** Properties ***

            public string Title
            {
                get
                {
                    return _propertySet.Title;
                }
                set
                {
                    _propertySet.Title = value;
                }
            }

            public string Description
            {
                get
                {
                    return _propertySet.Description;
                }
                set
                {
                    _propertySet.Description = value;
                }
            }
        }
    }
}
