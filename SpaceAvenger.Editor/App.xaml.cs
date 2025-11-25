using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Editor.ViewModels;
using SpaceAvenger.Editor.Views;
using SpaceAvenger.Services;
using SpaceAvenger.Services.ResourceLoader;
using System.IO;
using System.Windows;
using ViewModelBaseLibDotNetCore.MessageBus;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using WPFGameEngine.FactoryWrapper;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.Services.Realizations;
using WPFGameEngine.Timers;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Serialization.GameObjects;

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
            services.AddSingleton<IMessageBus, MessageBusService>();
            services.AddSingleton<BufferWindowViewModel>();
            services.AddTransient<BufferWindowView>(c =>
                {
                    BufferWindowView bufferWindowView = new BufferWindowView();
                    var vm = c.GetRequiredService<BufferWindowViewModel>();
                    vm.OnCancelWindow += () =>
                    {
                        vm.Clear();
                        bufferWindowView.Close();
                    };
                    bufferWindowView.DataContext = vm;
                    vm.Dispatcher = bufferWindowView.Dispatcher;

                        return bufferWindowView;
                });
            services.AddSingleton<IGameObjectImporter, GameObjectImporter>();
            services.AddSingleton<IGameObjectExporter, GameObjectExporter>();
            services.AddSingleton<IAssemblyLoader>(c =>
            { 
                IAssemblyLoader assemblyLoader = new AssemblyLoader();
                string pathToAssembly = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "WPFGameEngine.dll";
                assemblyLoader.LoadAssembly(pathToAssembly);
                return assemblyLoader;
            });
            services.AddSingleton<IMessageBus, MessageBusService>();
            services.AddSingleton(c =>
            { 
                IResourceLoader resourceLoader = new WPFResourceLoader(Constants.PATH_TO_CONTENT);
                return resourceLoader;
            });
            services.AddSingleton<IFactoryWrapper>(c => 
                {
                    var loader = c.GetRequiredService<IResourceLoader>();
                    return new FactoryWrapper(loader);
                }
                );
            services.AddSingleton(c => {
                IGameTimer gameTimer = new GameTimer();
                return gameTimer;
            });
            services.AddSingleton(c =>
            {
                var factoryWrapper = c.GetRequiredService<IFactoryWrapper>();
                var gameTimer = c.GetRequiredService<IGameTimer>();
                var assemblyLoader = c.GetRequiredService<IAssemblyLoader>();
                var exporter = c.GetRequiredService<IGameObjectExporter>();
                var importer = c.GetRequiredService<IGameObjectImporter>();
                var mb = c.GetRequiredService<IMessageBus>();
                return new EditorMainWindowViewModel(
                    factoryWrapper,
                    gameTimer, 
                    assemblyLoader, 
                    exporter,
                    importer,
                    mb,
                    Services);
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

        protected override void OnExit(ExitEventArgs e)
        {
            var main = Services.GetRequiredService<EditorMainWindowViewModel>();
            var buffVM = Services.GetRequiredService<BufferWindowViewModel>();
            buffVM.Clear();
            main.Dispose();
            buffVM.Dispose();

            base.OnExit(e);
        }
    }

}
