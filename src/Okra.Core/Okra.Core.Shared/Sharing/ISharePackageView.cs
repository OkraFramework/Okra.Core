using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public interface ISharePackageView
    {
        // *** Properties ***

        IReadOnlyList<string> AvailableFormats { get; }
        ISharePropertySet Properties { get; }

        // *** Methods ***

        bool Contains(string formatId);
        Task<T> GetDataAsync<T>(string formatId);
    }
}
