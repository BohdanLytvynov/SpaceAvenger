using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.ViewModels.MainWindowVM;
using SpaceAvenger.Extensions.Services;
using System;
using System.Windows;
using SpaceAvenger.Views.Pages;
using System.Reflection;
using System.Linq;
using System.Windows.Controls;
using ViewModelBaseLibDotNetCore.VM;
using SpaceAvenger.Attributes.PageManager;
using Domain.Utilities;
using ViewModelBaseLibDotNetCore.PageManagers;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.MessageBus;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.Timers;
using WPFGameEngine.WPF.GE.Serialization.GameObjects;
using System.IO;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using SpaceAvenger.Services.ResourceLoader;
using SpaceAvenger.Services;
using WPFGameEngine.ObjectBuilders;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.FactoryWrapper;
using SpaceAvenger.ViewModels.PagesVM;
using SpaceAvenger.Services.WPFInputControllers;

namespace SpaceAvenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string SEPARATOR = "_";

        private static IServiceProvider? _Services;

        public static IServiceProvider Services => _Services ??= InitializeServices().BuildServiceProvider();
        
        private static IServiceCollection InitializeServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IResourceLoader>(c =>
            {
                return new WPFResourceLoader(Constants.PATH_TO_CONTENT);
            });

            services.AddSingleton<IFactoryWrapper>(c =>
            {
                var loader = c.GetRequiredService<IResourceLoader>();
                return new FactoryWrapper(loader);
            });

            services.AddSingleton<IGameObjectImporter>(c =>
            { 
                string pathToObjects = Environment.CurrentDirectory
                + Path.DirectorySeparatorChar + "Resources" 
                + Path.DirectorySeparatorChar + "GameObjects";

                return new GameObjectImporter(pathToObjects);
            });

            services.AddSingleton<IObjectBuilder>(c =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var importer = c.GetRequiredService<IGameObjectImporter>();
                var factoryWrapper = c.GetRequiredService<IFactoryWrapper>();
                return new ObjectBuilder(
                    assembly,
                    typeof(MapableObject),
                    importer,
                    factoryWrapper);
            });
            // Add ViewModels (Windows)
            services.AddSingleton<MainWindowViewModel>();
            // Add ViewModels (Pages)

            services.AddSingleton<MainWindow>();

            services.AddPageViewModelsAsSingleton();

            services.AddSingleton<IPageManagerService<FrameType>, PageManagerService<FrameType>>();

            services.AddSingleton<IMessageBus, MessageBusService>();

            services.AddSingleton<IGameTimer, GameTimer>();

            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var pm = Services.GetRequiredService<IPageManagerService<FrameType>>();

            if (pm is null)
                throw new Exception($"Fail to get {nameof(PageManagerService<FrameType>)} on Startup!");

            // Init PageManager

            var assembly = Assembly.GetExecutingAssembly();

            var pages = ReflexionUtility.GetObjectsTypeInfo(assembly,
                (TypeInfo t) => t is not null &&
             (t.BaseType?.Name.Equals(nameof(Page)) ?? false)
             && t.Name.Contains("Page"));

            var pageViewModels = ReflexionUtility.GetObjectsTypeInfo(assembly,
                (TypeInfo t) => t is not null &&
            (t.GetCustomAttribute<ViewModelType>()?.Usage.Equals(ViewModelUsage.Page) ?? false)
            && t.Name.Contains("ViewModel")
            && t.GetCustomAttribute<ReflexionDetectionIgnore>() is null); 

            TypeInfo? viewModelInfo = null;

            Page? view = null;

            ViewModelBase? vm = null;

            foreach (var page in pages)
            {
                string pageName = page.Name.Split(SEPARATOR)[0];

                viewModelInfo = pageViewModels.FirstOrDefault(
                    vm => vm.GetCustomAttribute<ViewModelName>() is null? 
                    vm.Name.Split(SEPARATOR)[0].Equals(pageName)
                    : vm.GetCustomAttribute<ViewModelName>()?.Name?.Split(SEPARATOR)[0].Equals(pageName) ?? false);

                if (viewModelInfo is null)
                    throw new Exception($"Can't find corresponding ViewModel to the View {pageName}! Please check your page's and viewmodel's namings.");

                vm = Services.GetRequiredService(viewModelInfo.AsType()) as ViewModelBase;
                view = Activator.CreateInstance(page.AsType()) as Page;

                view.DataContext = vm;
                vm.Dispatcher = view.Dispatcher;

                if (view is Game_Page gp && vm is GamePage_ViewModel gpvm)
                {
                    //Init controller for player
                    gpvm.ControlComponent = new WPFInputController();
                    gp.OnInputFired += gpvm.ControlComponent.OnInputFired;
                }

                pm.AddPage(
                page.Name, view);
            }

            var mainWindow = Services.GetRequiredService<MainWindow>();
            App.Current.MainWindow = mainWindow;

            var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();

            mainWindow.DataContext = mainWindowViewModel;
            mainWindowViewModel.Dispatcher = mainWindow.Dispatcher;

            mainWindow.Show();

            pm.SwitchPage(nameof(Main_Page), FrameType.MainFrame);
            pm.SwitchPage(nameof(UserProfileInfo_Page), FrameType.InfoFrame);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
