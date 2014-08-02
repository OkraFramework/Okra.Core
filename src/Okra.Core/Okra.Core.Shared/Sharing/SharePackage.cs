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

        private readonly DataPackage dataPackage;
        private readonly SharePropertySet properties;

        // *** Constructors ***

        public SharePackage(DataPackage dataPackage)
        {
            this.dataPackage = dataPackage;
            this.properties = new SharePropertySet(dataPackage.Properties);
        }

        // *** Properties ***

        public ISharePropertySet Properties
        {
            get
            {
                return properties;
            }
        }

        // *** Methods ***

        public void SetData(string formatId, object value)
        {
            dataPackage.SetData(formatId, value);
        }

        public void SetAsyncData<T>(string formatId, AsyncDataProvider<T> dataProvider)
        {
            dataPackage.SetDataProvider(formatId, (DataProviderRequest request) => DataProviderRequestHandler<T>(request, dataProvider));
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

            private readonly DataPackagePropertySet propertySet;

            // *** Constructors ***

            public SharePropertySet(DataPackagePropertySet propertySet)
            {
                this.propertySet = propertySet;
            }

            // *** Properties ***

            public string Title
            {
                get
                {
                    return propertySet.Title;
                }
                set
                {
                    propertySet.Title = value;
                }
            }

            public string Description
            {
                get
                {
                    return propertySet.Description;
                }
                set
                {
                    propertySet.Description = value;
                }
            }
        }
    }
}
