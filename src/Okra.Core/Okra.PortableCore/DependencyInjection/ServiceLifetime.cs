using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    /// <summary> 
    /// Specifies the lifetime of a service in an <see cref="IServiceCollection"/>. 
    /// </summary> 
    public enum ServiceLifetime
    {
        /// <summary> 
        /// Specifies that a single instance of the service will be created. 
        /// </summary> 
        Singleton,
        /// <summary> 
        /// Specifies that a new instance of the service will be created for each scope. 
        /// </summary> 
        /// <remarks> 
        /// In ASP.NET applications a scope is created around each server request. 
        /// </remarks> 
        Scoped,
        /// <summary> 
        /// Specifies that a new instance of the service will be created every time it is requested. 
        /// </summary> 
        Transient
    }

}
