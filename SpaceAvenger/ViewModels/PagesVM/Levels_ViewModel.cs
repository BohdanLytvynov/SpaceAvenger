using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Game.Core.Levels;
using SpaceAvenger.Services.Realizations.Message;
using SpaceAvenger.Views.Pages;
using System;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.VM;
using c = SpaceAvenger.Services.Constants;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelType(ViewModelUsage.Page)]
    public class Levels_ViewModel : ValidationViewModel
    {
        #region Fields
        private string m_enemiesCount;
        private uint m_enemCount;
        private IPageManagerService<FrameType> m_PageManager;
        private IMessageBus m_messageBus;
        #endregion

        #region Properties
        public string EnemiesCount 
        { get => m_enemiesCount; set => Set(ref m_enemiesCount, value); }
        #endregion

        #region Commands

        public ICommand OnBackButtonPressed { get; }

        public ICommand OnStartGameButtonPressed { get; }

        #endregion

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get 
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(EnemiesCount):
                        bool valid = uint.TryParse(EnemiesCount, out m_enemCount);
                        SetValidArrayValue(0, valid);
                        if (!valid)
                            error = "Невірний ввод!";
                        break;
                }

                return error;
            }
        }

        #endregion

        #region Ctor

        public Levels_ViewModel(IPageManagerService<FrameType> pageManager,
            IMessageBus messageBus) : this()
        {
            m_messageBus = messageBus ?? throw new ArgumentNullException(nameof(pageManager));
            m_PageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
        }

        public Levels_ViewModel()
        {
            InitValidArray(1);
            m_enemiesCount = "0";

            OnBackButtonPressed = new Command
                (
                    OnBackButtonPressedExecute,
                    CanOnBackButtonPressedExecute
                );

            OnStartGameButtonPressed = new Command
                (OnStartGameButtonPressedExecute,
                CanOnStartGameButtonPressedExecute);
        }
        #endregion

        #region Methods

        private bool CanOnBackButtonPressedExecute(object p)
            => true;

        private void OnBackButtonPressedExecute(object p)
        {
            m_PageManager.SwitchPage(nameof(Main_Page), FrameType.MainFrame);
        }

        private bool CanOnStartGameButtonPressedExecute(object p)
        => this.GetValidArrayValue(0) && m_enemCount > 0;

        private void OnStartGameButtonPressedExecute(object p)
        {
            m_PageManager.SwitchPage(nameof(Game_Page), FrameType.MainFrame);
            m_messageBus.Send<GameMessage, string>(new GameMessage(c.START_GAME_COMMAND)
            {
                Level = new SurvivalLevel() { EnemyCount = (int)m_enemCount }
            });
        }

        #endregion
    }
}
