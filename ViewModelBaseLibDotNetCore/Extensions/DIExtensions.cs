using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace ViewModelBaseLibDotNetCore.Extensions
{
    public static class DIExtensions
    {
        public static void ConfigureMapper(
            this IServiceCollection services,
            Assembly assembly)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                var profiles = assembly.DefinedTypes
                .Where(t => typeof(Profile).IsAssignableFrom(t) 
                && !t.IsAbstract);

                foreach (var p in profiles)
                {
                    mc.AddProfile((Profile)Activator.CreateInstance(p));
                }
            }, new NullLoggerFactory());

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
