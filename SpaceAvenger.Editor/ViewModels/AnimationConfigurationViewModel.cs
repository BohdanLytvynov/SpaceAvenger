using SpaceAvenger.Editor.Views;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class AnimationConfigurationViewModel : ViewModelBase
    {
        #region Fields
        private string m_title;
        private GameViewHost m_gameView;
        #endregion

        #region Properties
        public string Title 
        { get => m_title; set => Set(ref m_title, value); }

        public GameViewHost GameView 
        { get => m_gameView; set => Set(ref m_gameView, value); }
        #endregion

        #region Ctor
        public AnimationConfigurationViewModel()
        {
            #region Init Fields
            m_title = "Animation Configuration";
            m_gameView = new GameViewHost(new GameTimer());
            #endregion
        }
        #endregion

        #region Methods
        
        #endregion
    }
}
