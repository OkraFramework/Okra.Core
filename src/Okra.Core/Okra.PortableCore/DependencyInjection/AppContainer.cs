using Microsoft.Extensions.DependencyInjection;
using Okra.Helpers;
using Okra.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public class AppContainer : IAppContainer
    {
        private readonly IServiceScope _serviceScope;
        private readonly AppContainer _parent;
        private readonly ILifetimeManagerSource _lifetimeManagerSource;
        private readonly List<IAppContainer> _childContainers = new List<IAppContainer>();

        private AppContainer(IServiceScope serviceScope, AppContainer parent)
            : this(serviceScope.ServiceProvider)
        {
            _serviceScope = serviceScope;
            _parent = parent;
        }

        public AppContainer(IServiceProvider serviceProvider)
        {
            this.Services = serviceProvider;
            this._lifetimeManagerSource = new LifetimeManagerSource();

            serviceProvider.InjectService<IAppContainer>(this);
            serviceProvider.InjectService<ILifetimeManager>(this.LifetimeManager);
        }

        public IAppContainer Parent
        {
            get
            {
                return _parent;
            }
        }

        public IServiceProvider Services
        {
            get;
        }

        public ILifetimeManager LifetimeManager
        {
            get
            {
                return _lifetimeManagerSource.LifetimeManager;
            }
        }

        public async Task Activate()
        {
            await _lifetimeManagerSource.Activate();

            var childActivations = _childContainers.Select(c => c.Activate());
            await Task.WhenAll(childActivations);
        }

        public IAppContainer CreateChildContainer()
        {
            // TODO : Enable removal of child containers
            // TODO : Make sure child services are disposed! 
            // TODO : Remove all legacy files related to lifetime management

            var serviceScopeFactory = Services.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();

            var appContainer = new AppContainer(serviceScope, this);
            _childContainers.Add(appContainer);

            return appContainer;
        }

        public async Task Deactivate()
        {
            var childDeactivations = _childContainers.Select(c => c.Deactivate());
            await Task.WhenAll(childDeactivations);

            await _lifetimeManagerSource.Deactivate();
        }

        public void Dispose()
        {
            if (_parent == null)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotDisposeRootAppContainer"));

            _parent._childContainers.Remove(this);
            _serviceScope.Dispose();
        }
    }
}
