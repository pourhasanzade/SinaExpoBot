using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using SinaExpoBot.Service;
using SinaExpoBot.Service.Interface;
using Unity.WebApi;

namespace Shop
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IMessengerService, MessengerService>();
            container.RegisterType<IKeypadService, KeypadService>();
            container.RegisterType<IConfigService, ConfigService>();
            container.RegisterType<IUserDataService, UserDataService>();
            container.RegisterType<ICacheService, CacheService>();
            container.RegisterType<IButtonService, ButtonService>();
            container.RegisterType<IExceptionLogService, ExceptionLogService>();
            container.RegisterType<IApiService, ApiService>();            
            container.RegisterType<IProvinceService, ProvinceService>();
            container.RegisterType<IFestivalService, FestivalService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IOrderService, OrderService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }


        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        /// 
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            //container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<ManageController>(new InjectionConstructor());

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            //container.RegisterType<IUnitOfWork, UnitOfWork>();
            //container.RegisterType<IProductRepository, ProductRepository>();



        }
    }
}