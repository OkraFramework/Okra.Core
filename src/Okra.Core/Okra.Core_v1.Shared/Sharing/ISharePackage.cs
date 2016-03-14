using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public delegate Task<T> AsyncDataProvider<T>(string formatId);

    public interface ISharePackage
    {
        // *** Properties ***

        ISharePropertySet Properties { get; }

        // *** Methods ***

        void SetData<T>(string formatId, T value);
        void SetAsyncData<T>(string formatId, AsyncDataProvider<T> dataProvider);
    }
}
