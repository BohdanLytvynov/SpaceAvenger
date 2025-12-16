using System;
using System.Windows.Input;
using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Services.Realizations.Message;
using SpaceAvenger.Views.Pages;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.VM;
using c = SpaceAvenger.Services.Constants;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelType(ViewModelUsage.Page)]
    internal class Main_ViewModel : ViewModelBase
    {
        #region Fields
        private IPageManagerService<FrameType> m_PageManager;

        private IMessageBus m_messageBus;
        #endregion

        #region Properties

        #endregion

        #region Commands

        public ICommand OnNewGameButtonPressed { get; }

        public ICommand OnSurvivalModeButtonPressed { get; }

        #endregion

        #region Ctor

        public Main_ViewModel() 
        {
            #region Init Commands

            OnNewGameButtonPressed = new Command(
                OnNewGameButtonPressedExecute,
                CanOnNewGameButtonPressedExecute
                );

            OnSurvivalModeButtonPressed = new Command(
                OnSurvivalModeButtonPressedExecute,
                CanOnSurvivalModeButtonPressedExecute
                );

            #endregion
        }

        public Main_ViewModel(
            IPageManagerService<FrameType> pageManagerService,
            IMessageBus messageBus) : this()
        {            
            #region Init Fields
            
            m_PageManager = pageManagerService ?? throw new ArgumentNullException(nameof(pageManagerService));

            m_messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            
            #endregion
        }
        #endregion

        #region Methods

        #region Command Methods

        #region OnNewGameButtonPressed

        public bool CanOnNewGameButtonPressedExecute(object p)
        {
            return true;
        }

        public void OnNewGameButtonPressedExecute(object p)
        {
            m_PageManager.SwitchPage(nameof(Levels_Page), FrameType.MainFrame);
        }

        #endregion

        #region OnSurvivalModeButtonPressed

        private bool CanOnSurvivalModeButtonPressedExecute(object p) => true;

        private void OnSurvivalModeButtonPressedExecute(object p)
        {
            m_PageManager.SwitchPage(nameof(Levels_Page), FrameType.MainFrame);
        }

        #endregion

        #endregion

        #endregion
    }
}
