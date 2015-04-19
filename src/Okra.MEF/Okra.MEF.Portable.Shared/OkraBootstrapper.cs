using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;
using System.Linq;
using Okra.Navigation;
using Okra.Services;
using System;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Okra.Sharing;
using Okra.Helpers;
#else
using Xamarin.Forms;
#endif

#if WINDOWS_APP
using Okra.Search;
#endif

namespace Okra
{
    public abstract class OkraBootstrapper
    {
        // *** Fields ***

        private bool isActivated;

        // *** Imported Properties ***

        [Import]
        public IActivationManager ActivationManager { get; set; }

        [Import]
        public INavigationManager NavigationManager { get; set; }
        
        [Import]
        public ILaunchActivationHandler LaunchActivationHandler { get; set; }

#if !NETFX_CORE
        [Import]
        public ILifetimeManager LifetimeManager { get; set; }

        //[Export]
        //public PCLStorage.IFileSystem FileSystem
        //{
        //    get { return PCLStorage.FileSystem.Current; }
        //}
#endif        

        // *** Public Methods ***

#if NETFX_CORE
        public void Activate(IActivatedEventArgs args)
#else
        public async void Activate(IActivatedEventArgs args)
#endif
        {
            if (args == null)
                throw new ArgumentNullException("args");

            // Setup services if this is the first activation

            if (!isActivated)
            {
                SetupServices();
                isActivated = true;
            }

            // Call the activation manager
            // NB: Since this is async and we do not await it, we need to inject any exceptions into the Application.UnhandledException event

            Task activateTask = ActivationManager.Activate(args);
#if NETFX_CORE
            ExceptionHelper.InjectAsyncExceptions(activateTask);
#else
            await activateTask;
#endif
        }

        public virtual void Initialize()
        {
            Initialize(true);
        }

        public void Initialize(bool registerForActivation)
        {
            // Initialize MEF and compose bootstrapper

            ContainerConfiguration containerConfiguration = GetContainerConfiguration();
            CompositionHost compositionHost = containerConfiguration.CreateContainer();
            compositionHost.SatisfyImports(this);

#if NETFX_CORE
            // Attach to the CoreApplicationView for activation events

            if (registerForActivation)
            {
                CoreApplicationView coreApplicationView = CoreApplication.GetCurrentView();
                coreApplicationView.Activated += OnActivated;
            }
#endif
        }

        // *** Protected Methods ***

        protected ContainerConfiguration GetOkraContainerConfiguration()
        {
            // Create a basic container configuration with,
            //    - The Okra assembly (via the INavigationManager interface)
            //    - The Okra.MEF assembly (via the OkraBootstrapper class)

            ConventionBuilder okraConventionBuilder = new ConventionBuilder();

            okraConventionBuilder.ForType<NavigationManager>().Export<INavigationManager>()
                                                                .Shared()
                                                                .SelectConstructor(ctors => ctors.First(), (info, builder) =>
                                                                {
                                                                    if (info.ParameterType == typeof(INavigationTarget))
                                                                        builder.AllowDefault();
                                                                });
            
#if NETFX_CORE
            okraConventionBuilder.ForType<ShareSourceManager>().Export<IShareSourceManager>().Shared();
            okraConventionBuilder.ForType<ShareTargetManager>().Export<IShareTargetManager>().Shared();
#endif
            okraConventionBuilder.ForType<ActivationManager>().Export<IActivationManager>().Shared();
            okraConventionBuilder.ForType<LifetimeManager>().Export<ILifetimeManager>().Shared();
            okraConventionBuilder.ForType<StorageManager>().Export<IStorageManager>().Shared();
            okraConventionBuilder.ForType<LaunchActivationHandler>().Export<ILaunchActivationHandler>().Shared();

#if WINDOWS_APP
            okraConventionBuilder.ForType<SettingsPaneManager>().Export<ISettingsPaneManager>()
                                                                .Shared()
                                                                .SelectConstructor(ctors => ctors.First());

            okraConventionBuilder.ForType<SearchManager>().Export<ISearchManager>().Shared();
#endif

            return new ContainerConfiguration()
                        .WithAssembly(typeof(INavigationManager).GetTypeInfo().Assembly, okraConventionBuilder)
                        .WithAssembly(typeof(OkraBootstrapper).GetTypeInfo().Assembly);
        }

        protected virtual ContainerConfiguration GetContainerConfiguration()
        {
            // Create a basic container configuration with,
            //    - The application's main assembly (i.e. that defines the current Application subclass)
            //    - The Okra assembly (via the INavigationManager interface)
            //    - The Okra.MEF assembly (via the OkraBootstrapper class)

            return GetOkraContainerConfiguration()
                        .WithAssembly(Application.Current.GetType().GetTypeInfo().Assembly);
        }

        protected virtual void SetupServices()
        {
        }

#if NETFX_CORE
        protected virtual void OnActivated(CoreApplicationView sender, IActivatedEventArgs args)
        {
            Activate(args);
        }
#endif
    }
}
