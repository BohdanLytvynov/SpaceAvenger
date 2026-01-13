using Domain.Utilities;
using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Attributes.PageManager;
using System.Reflection;

namespace SpaceAvenger.Extensions.Services
{
    /// <summary>
    /// Extension that configures Services for DI
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// This method will find all the ViewModels Type Infos and Register them to the Services Container as Singletone
        /// </summary>
        /// <param name="services"></param>
        public static void AddPageViewModelsAsSingleton(this IServiceCollection services)
        {
            //Get current Assembly
            var assembly = Assembly.GetExecutingAssembly();
            //Find all ViewModels for Pages
            var viewModels = ReflexionUtility.GetObjectsTypeInfo(assembly,
                (TypeInfo t) => 
                t is not null && t.Name.Contains("ViewModel")
                && (t.GetCustomAttribute<ViewModelType>()?.Usage.Equals(ViewModelUsage.Page) ?? false)
                && t.GetCustomAttribute<ReflexionDetectionIgnore>() is null);
            //Add the viewModels of the Pages to the Container as Singletone
            foreach (var viewModel in viewModels)
            {
                services.AddSingleton(viewModel.AsType());
            }
        }
    }
}
