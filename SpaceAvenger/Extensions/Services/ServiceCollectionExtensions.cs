using Domain.Utilities;
using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Attributes.PageManager;
using System.Reflection;

namespace SpaceAvenger.Extensions.Services
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddPageViewModelsAsSingleton(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var viewModels = ReflexionUtility.GetObjectsTypeInfo(assembly,
                (TypeInfo t) => 
                t is not null && t.Name.Contains("ViewModel")
                && (t.GetCustomAttribute<ViewModelType>()?.Usage.Equals(ViewModelUsage.Page) ?? false)
                && t.GetCustomAttribute<ReflexionDetectionIgnore>() is null);

            foreach (var viewModel in viewModels)
            {
                services.AddSingleton(viewModel.AsType());
            }
        }
    }
}
