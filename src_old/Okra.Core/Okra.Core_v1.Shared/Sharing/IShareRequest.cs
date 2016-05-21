using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Sharing
{
    public interface IShareRequest
    {
        // *** Properties ***

        ISharePackage Data { get; }

        // *** Methods ***

        void FailWithDisplayText(string displayText);
    }
}
