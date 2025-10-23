using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.Views;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Animations
{
    internal class AnimationComponentViewModel : ComponentViewModel
    {
        #region Fields
        private IResourceLoader m_resourceLoader;
        #endregion

        #region Commands
        public ICommand OnConfigureButtonPressed { get; }
        #endregion

        #region Ctor
        public AnimationComponentViewModel(IGameObject gameObject, IResourceLoader resourceLoader) : base(nameof(Animation), gameObject) 
        {
            #region Init Fields
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
            var animationConfigurationViewModel = new AnimationConfigurationViewModel(m_resourceLoader);
            var animConfigurationWindow = new AnimationConfigurationWindow();
            animationConfigurationViewModel.Dispatcher = animConfigurationWindow.Dispatcher;
            animConfigurationWindow.DataContext = animationConfigurationViewModel;

            animConfigurationWindow.Topmost = true;
            animConfigurationWindow.Show();
        }

        #endregion
    }
}
