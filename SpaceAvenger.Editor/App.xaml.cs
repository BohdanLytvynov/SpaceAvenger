using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Editor.Services;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels;
using System.Windows;

namespace SpaceAvenger.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider _Services;

        public static IServiceProvider Services => _Services ??= InitializeServices().BuildServiceProvider();

        private static IServiceCollection InitializeServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(c =>
            { 
                IResourceLoader resourceLoader = new ResourceLoader("pack://application:,,,/SpaceAvenger;component/Resources/Content.xaml");
                return resourceLoader;
            });
            services.AddSingleton(c =>
            { 
                var loader = c.GetRequiredService<IResourceLoader>();
                return new EditorMainWindowViewModel(loader);
            });
            services.AddSingleton<MainWindow>(c =>
            { 
                var vm = c.GetRequiredService<EditorMainWindowViewModel>();
                var view = new MainWindow();

                vm.Dispatcher = view.Dispatcher;
                view.DataContext = vm;
                return view;
            });
            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Services.GetRequiredService<MainWindow>().Show();

            base.OnStartup(e);
        }
    }

}
