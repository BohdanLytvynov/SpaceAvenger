using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.Views;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.Factories.Ease;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Animations
{
    internal class AnimationComponentViewModel : ComponentViewModel
    {
        #region Fields
        private IResourceLoader m_resourceLoader;
        private IAssemblyLoader m_assemblyLoader;
        private IEaseFactory m_easeFactory;
        #endregion

        #region Commands
        public ICommand OnConfigureButtonPressed { get; }
        #endregion

        #region Ctor
        public AnimationComponentViewModel(IGameObject gameObject, 
            IResourceLoader resourceLoader, IAssemblyLoader assemblyLoader,
            IEaseFactory easeFactory) : base(nameof(Animation), gameObject) 
        {
            #region Init Fields
            m_easeFactory = easeFactory ?? throw new ArgumentNullException(nameof(easeFactory));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            #endregion

            #region Init Commands

            OnConfigureButtonPressed = new Command(
                OnConfigureButtonPressedExecute,
                CanOnConfigureButtonPressedExecute
                );

            #endregion
        }
        #endregion

        #region Methods
        #region On Configure Button Pressed

        private bool CanOnConfigureButtonPressedExecute(object p) => true;

        private void OnConfigureButtonPressedExecute(object p)
        {
            ShowConfig();
        }

        #endregion

        private void ShowConfig()
        {
            var animationConfigurationViewModel = new AnimationConfigurationViewModel(m_resourceLoader, m_assemblyLoader, 
                m_easeFactory);
            var animConfigurationWindow = new AnimationConfigurationWindow();
            animationConfigurationViewModel.Dispatcher = animConfigurationWindow.Dispatcher;
            animConfigurationWindow.DataContext = animationConfigurationViewModel;

            animConfigurationWindow.Topmost = true;
            animConfigurationWindow.Show();
        }

        #endregion
    }
}
