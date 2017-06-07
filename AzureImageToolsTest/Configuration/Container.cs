using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AzureImageToolsTest.Domain;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Serilog;

namespace AzureImageToolsTest.Configuration
{
    public class Container
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterInstance(typeof(ILogger), GetLogger());
            container.RegisterType<IFileStore, BlobFileStore>();

            // For blob storage
            container.RegisterInstance(
                typeof(StorageConnectionString), 
                new StorageConnectionString(ConfigurationManager.AppSettings["StorageConnectionString"]));
            
            // For disk storage.
            // container.RegisterType<IStoreStreamCommand, DiskFileStore>();
            // var filesPath  = $"{HttpRuntime.AppDomainAppPath}Files{Path.DirectorySeparatorChar}";
            // container.RegisterInstance(typeof(DiskFileStoreRoot), new DiskFileStoreRoot(filesPath));

            return container;
        }

        private static ILogger GetLogger()
        {
            var logPath =
                $"{HttpRuntime.AppDomainAppPath}" +
                $"{Path.DirectorySeparatorChar}Logs" +
                $"{Path.DirectorySeparatorChar}log.txt";

            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile(logPath)
                .CreateLogger();
        }
    }
}