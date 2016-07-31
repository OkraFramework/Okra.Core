using System;

namespace Okra
{
    public interface IOkraBootstrapper
    {
        IServiceProvider ApplicationServices { get; }
        void Initialize();
    }
}
