using System;
using Microsoft.Extensions.DependencyInjection;

namespace FleckTest
{
    /// <summary>
    /// Dependency Injection wrapper.
    /// </summary>
    static class ServiceManager
    {
        #region Internal vars
        static IServiceCollection _services;
        static IServiceProvider _provider;
        #endregion

        #region Constructor
        static ServiceManager()
        {
            ServiceManager._services = new ServiceCollection();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Register a new instantiable service.
        /// </summary>
        /// <typeparam name="TService">Service interface.</typeparam>
        /// <typeparam name="TImplementation">Implementation to register.</typeparam>
        public static void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            ServiceManager._services.AddTransient<TService, TImplementation>();
        }

        /// <summary>
        /// Register a new singleton service.
        /// </summary>
        /// <typeparam name="TService">Service interface.</typeparam>
        /// <typeparam name="TImplementation">Implementation to register.</typeparam>
        public static void RegisterSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            ServiceManager._services.AddSingleton<TService, TImplementation>();
        }

        /// <summary>
        /// Initialize all registered services to allow get instances of them.
        /// </summary>
        public static void InitializeServices()
        {
            ServiceManager._provider = ServiceManager._services.BuildServiceProvider();
        }

        /// <summary>
        /// Get a new instance of the requested service.
        /// </summary>
        /// <typeparam name="T">Service interface to request.</typeparam>
        /// <returns>Returns the new instance of the service.</returns>
        public static T GetService<T>()
        {
            return ServiceManager._provider.GetService<T>();
        } 
        #endregion
    }
}
