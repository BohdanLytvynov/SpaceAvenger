using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Services.Realizations.Message;
using SpaceAvenger.Views.Pages;
using System;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Levels;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelType(ViewModelUsage.Page)]
    public class LevelStatistics_ViewModel : SubscriptableViewModel
    {
        #region Constants
        private const string WIN_HEADER = "Перемога!";
        private const string LOSE_HEADER = "Поразка!";
        private const string ESCAPE_HEADER = "Зрада!";

        private const string WIN_Msg = "Чудова робота командер! Командування виражае подяку! Корабель на дозаправці і поповненні БК...";
        private const string LOSE_Msg = "Мені дуже шкода! Корабель сильно пошкоджений потрібен ремонт.. Рапорт відправлений вищому командуванню!";
        private const string ESCAPE_Msg = "Ви відступили з поля бою зберігши екіпаж і корабель, проте це призвело до втрати частини сектору!";
        #endregion

        #region Fields
        private IPageManagerService<FrameType> m_PageManager;
        private IMessageBus m_messageBus;

        private string m_Header;
        private int m_ShipsDestroyed;
        private int m_EnemyCount;
        private string m_Msg;
        #endregion

        #region Properties
        public string Header 
        { get =>  m_Header; set => Set(ref m_Header, value); }
        public int ShipsDestroyed 
        { get => m_ShipsDestroyed; set => Set(ref m_ShipsDestroyed, value); }
        public int EnemyCount 
        { 
            get => m_EnemyCount; 
            set => Set(ref m_EnemyCount, value); 
        }

        public string Msg 
        { get => m_Msg; set => Set(ref m_Msg, value); }

        #endregion

        #region ICommand

        public ICommand OnBackToMenu { get; }

        #endregion

        #region Ctor
        public LevelStatistics_ViewModel
            (IPageManagerService<FrameType> pageManagerService,
            IMessageBus messageBus)
            : this()
        {
            m_messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            m_PageManager = pageManagerService ?? throw new ArgumentNullException(nameof(pageManagerService));

            Subscriptions.Add(
                m_messageBus.RegisterHandler<LevelStatisticMessage, LevelStatistics>(
                    OnNewLevelStatistic
                    ));
        }

        public LevelStatistics_ViewModel()
        {
            m_Header = string.Empty;
            m_Msg = string.Empty;

            OnBackToMenu = new Command(
                OnBackToMenuPressedExecute,
                CanOnBackToMenuPressedExecute
                );
        }
        #endregion

        #region Methods

        #region On Back To Menu Pressed

        private bool CanOnBackToMenuPressedExecute(object p) => true;

        private void OnBackToMenuPressedExecute(object p)
        {
            m_PageManager.SwitchPage(nameof(Main_Page), FrameType.MainFrame);
        }

        #endregion

        private void OnNewLevelStatistic(LevelStatisticMessage msg)
        {
            var statistics = msg.Content;

            if (statistics.Win && statistics.IsAlive)
            {
                Header = WIN_HEADER;
                Msg = WIN_Msg;
            }
            else if (!statistics.IsAlive)
            {
                Header = LOSE_HEADER;
                Msg = LOSE_Msg;
            }
            else if (!statistics.Win)
            {
                Header = ESCAPE_HEADER;
                Msg = ESCAPE_Msg;
            }

            ShipsDestroyed = statistics.ShipsDestroyed;
            EnemyCount = statistics.EnemyCount;
        }
        #endregion
    }
}
