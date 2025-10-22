using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Editor.Services;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels;
using System.Windows;
using ViewModelBaseLibDotNetCore.MessageBus;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.Factories.Components;
using WPFGameEngine.Timers;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Base;

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
            services.AddSingleton<IComponentFactory, ComponentFactory>();
            services.AddSingleton<IMessageBus, MessageBusService>();
            services.AddSingleton(c =>
            { 
                IResourceLoader resourceLoader = new ResourceLoader("pack://application:,,,/SpaceAvenger;component/Resources/Content.xaml");
                return resourceLoader;
            });
            services.AddSingleton(c => {
                IGameTimer gameTimer = new GameTimer();
                return gameTimer;
            });
            services.AddSingleton(c =>
            { 
                var loader = c.GetRequiredService<IResourceLoader>();
                var componentFactory = c.GetRequiredService<IComponentFactory>();
                var gameTimer = c.GetRequiredService<IGameTimer>();
                return new EditorMainWindowViewModel(loader, componentFactory, gameTimer);
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
