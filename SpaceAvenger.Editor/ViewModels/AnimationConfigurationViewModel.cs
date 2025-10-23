using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class AnimationConfigurationViewModel : ViewModelBase
    {
        #region Events
        public event Action<Animation> OnConfigurationFinished;
        #endregion

        #region Fields
        private string m_title;
        private GameViewHost m_gameView;
        private IGameObject m_gameObject;
        private Animation m_animation;
        private IResourceLoader m_resourceLoader;
        private ObservableCollection<string> m_resourceNames;
        private string m_SelectedResourceName;
        #endregion

        #region Properties
        public ObservableCollection<string> ResourceNames 
        {
            get=> m_resourceNames;
            set=> Set(ref m_resourceNames, value);
        }

        public string Title 
        { get => m_title; set => Set(ref m_title, value); }

        public GameViewHost GameView 
        { get => m_gameView; set => Set(ref m_gameView, value); }

        public string SelectedResourceName 
        {
            get => m_SelectedResourceName;
            set => Set(ref m_SelectedResourceName, value);
        }

        #endregion

        #region Commands
        public ICommand OnConfirmButtonPressed { get; }
        public ICommand OnStartButtonPressed { get; }
        public ICommand OnPauseButtonPressed { get; }
        #endregion

        #region Ctor
        public AnimationConfigurationViewModel(IResourceLoader resourceLoader)
        {
            #region Init Fields
            m_SelectedResourceName = string.Empty;
            m_resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            m_resourceNames = new ObservableCollection<string>();            
            foreach (var resourceName in m_resourceLoader.GetAllKeys())
            {
                m_resourceNames.Add(resourceName);
            }

            m_title = "Animation Configuration";
            m_gameView = new GameViewHost(new GameTimer());
            m_gameObject = new GameObjectMock();
            m_gameView.AddObject(m_gameObject);
            m_gameView.StartGame();

            #endregion

            #region Init Commands

            OnConfirmButtonPressed = new Command
                (
                    OnConfirmButtonPressedExecute,
                    CanOnConfirmButtonPressedExecute
                );

            #endregion
        }
        #endregion

        #region Methods

        #region On Confirm Button Pressed
        private bool CanOnConfirmButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnConfirmButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        #region On Start Button Pressed
        private bool CanOnStartButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnStartButtonPressedExecute(object p)
        { 
        
        }
        #endregion

        #region On Pause Button Pressed
        private bool CanOnPauseButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnPauseButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        public void OnWindowClosing()
        {
            m_gameView.Stop();
        }

        #endregion
    }
}
