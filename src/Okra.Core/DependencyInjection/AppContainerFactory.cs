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
    public class AppContainerFactory : IAppContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AppContainerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAppContainer CreateAppContainer()
        {
            var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var parentAppContainer = _serviceProvider.GetService<IAppContainer>() as AppContainer;
            
            var appContainer = new AppContainer(serviceScopeFactory, parentAppContainer);

            return appContainer;
        }

        // *** Private Sub-classes ***

        private class AppContainer : IAppContainer
        {
            private readonly IServiceScope _serviceScope;
            private readonly AppContainer _parent;
            private readonly List<IAppContainer> _childContainers = new List<IAppContainer>();
            private readonly ILifetimeManagerSource _lifetimeManagerSource = new LifetimeManagerSource();

            public AppContainer(IServiceScopeFactory serviceScopeFactory, AppContainer parent)
            {
                _serviceScope = serviceScopeFactory.CreateScope();
                _parent = parent;

                if (_parent != null)
                    _parent._childContainers.Add(this);

                Services.InjectService<IAppContainer>(this);
                Services.InjectService<ILifetimeManager>(LifetimeManager);
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
                get
                {
                    return _serviceScope.ServiceProvider;
                }
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

            public async Task Deactivate()
            {
                var childDeactivations = _childContainers.Select(c => c.Deactivate());
                await Task.WhenAll(childDeactivations);

                await _lifetimeManagerSource.Deactivate();
            }

            public void Dispose()
            {
                if (Parent == null)
                    throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotDisposeRootAppContainer"));

                _parent._childContainers.Remove(this);
                _serviceScope.Dispose();
            }
        }
    }
}
