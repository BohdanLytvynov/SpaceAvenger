using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using System;
using System.Windows.Media;
using System.Windows;
using SpaceAvenger.Services.Realizations.Message;
using c = SpaceAvenger.Services.Constants;
using WPFGameEngine.WPF.GE.Settings;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Services.WpfGameViewHost;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.WPF.GE.Levels;
using System.Windows.Input;
using SpaceAvenger.Views.DialogWindow;
using SpaceAvenger.Views.Pages;
using WPFGameEngine.ObjectInstantiators;
using WPFGameEngine.CollisionDetection.RaycastManager;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelName("Game_ViewModel")]
    [ViewModelType(ViewModelUsage.Page)]
    internal class GamePage_ViewModel : SubscriptableViewModel
    {
        #region Fields
        private Rect m_backViewport;
        private int m_backCount = 3;
        private double m_BackMoveSpeed;
        private IPageManagerService<FrameType> m_PageManager;
        private IMessageBus m_MessageBus;
        private ImageSource m_GameBack;
        private WpfMapableObjectViewHost m_GameView;
        private IGameTimer m_gameTimer;
        private IObjectInstantiator m_ObjectInstantiator;
        private IServiceProvider m_serviceProvider;
        private IControllerComponent m_controllerComponent;
        private ICollisionManager m_collisionManager;
        private IRaycastManager m_raycastManager;

        private int m_ShipsDestroyed;
        private int m_EnemyShips;

        private ILevel m_curr;

        #endregion

        #region Properties

        public int ShipsDestroyed 
        { get => m_ShipsDestroyed; set => Set(ref m_ShipsDestroyed, value); }

        public int EnemyShips 
        { get => m_EnemyShips; set => Set(ref m_EnemyShips, value); }

        public Rect BackViewport 
        {
            get=> m_backViewport; 
            set=> Set(ref m_backViewport, value);
        }

        public WpfMapableObjectViewHost GameView { get=> m_GameView; set=> Set(ref m_GameView, value); }

        public ImageSource Background { get=> m_GameBack; set=> Set(ref m_GameBack, value); }
        #endregion

        #region Commands
        public ICommand OnEscapeButtonPressed { get; }
        public ICommand OnResumeButtonPressed { get; }
        public ICommand OnExitButtonPressed { get; }
        #endregion

        #region Ctor

        public GamePage_ViewModel(
            IPageManagerService<FrameType> pageManager,
            IMessageBus messageBus,
            IGameTimer gameTimer,
            IObjectInstantiator instantiator,
            IServiceProvider serviceProvider,
            ICollisionManager collisionManager,
            IRaycastManager raycastManager) : this()
        {
            m_collisionManager = collisionManager ?? throw new ArgumentNullException(nameof(collisionManager));
            m_ObjectInstantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
            m_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            m_MessageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            m_PageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_gameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));
            m_GameView = new WpfMapableObjectViewHost(m_gameTimer, 
                m_ObjectInstantiator, m_collisionManager, m_raycastManager);
            GameView.OnUpdate += Update;
            Subscriptions.Add(m_MessageBus.RegisterHandler<GameMessage, string>(OnMessageRecieved));
        }

        public GamePage_ViewModel()
        {
            #region InitFields
            GESettings.DrawGizmo = false;
            GESettings.DrawBorders = false;
            GESettings.DrawColliders = false;
            #endregion

            m_BackMoveSpeed = 2;
        }

        #endregion

        #region Methods

        #region Set BackGround

        private void OnMessageRecieved(GameMessage gameMessage)
        {
            if (gameMessage.Content.Equals(c.START_GAME_COMMAND))
            {
                m_curr = gameMessage.Level;
                Initialize(m_curr);
                m_GameView.StartGame();
            }
            else if (gameMessage.Content.Equals(c.STOP_GAME_COMMAND))
            {
                //Escaping from the Game
                m_GameView.Stop();
                m_GameView.ClearWorld();
                m_PageManager.SwitchPage(nameof(LevelStatistics_Page), FrameType.MainFrame);
                m_MessageBus.Send<LevelStatisticMessage, LevelStatistics>(
                        new LevelStatisticMessage(m_curr.GetCurrentLevelStatistics())
                        );

            }
            else if (gameMessage.Content.Equals(c.PAUSE_GAME_COMMAND))
            {
                //Pausing the game
                m_GameView.Pause();
            }
            else if (gameMessage.Content.Equals(c.RESUME_GAME_COMMAND))
            { 
                //Resume the Game
                m_GameView.Resume();
            }
        }

        private void MoveBackground()
        {
            double xCurrent = BackViewport.X;
            double yCurrent = BackViewport.Y;

            if (yCurrent >= 1)
                yCurrent = 0;

            if (xCurrent >= 1)
                xCurrent = 0;
            double newY = yCurrent + m_BackMoveSpeed * 0.01 * m_gameTimer.deltaTime.TotalSeconds;
            BackViewport = new Rect(xCurrent, newY, 1, 1);
        }

        private void Update()
        {
            if (m_controllerComponent != null 
                && GameView.GameState == WPFGameEngine.Enums.GameState.Running &&
                m_controllerComponent.IsKeyDown(Key.Escape))
            {
                GameView.Pause();
                EscDialog escDialog = new EscDialog(m_MessageBus);
                escDialog.Topmost = true;
                m_controllerComponent.ReleaseKey(Key.Escape);
                escDialog.ShowDialog();
            }

            if (GameView.GameState == WPFGameEngine.Enums.GameState.Running)
            {
                MoveBackground();
            }

            if (m_curr != null)
            { 
                EnemyShips = m_curr.CurrentEnemyCount;
                ShipsDestroyed = m_curr.ShipsDestroyed;
            }
        }

        #endregion

        private void Initialize(ILevel level)
        {
            m_controllerComponent = m_serviceProvider.GetRequiredService<IControllerComponent>();
            level.ControllerComponent = m_controllerComponent;
            level.OnGameFinished += Level_OnGameFinished;
            GameView.AddObject(level);
        }

        private void Level_OnGameFinished(LevelStatistics obj)
        {
            GameView.Stop();
            GameView.ClearWorld();
            m_PageManager.SwitchPage(nameof(LevelStatistics_Page), FrameType.MainFrame);
            m_MessageBus.Send<LevelStatisticMessage, LevelStatistics>(
                    new LevelStatisticMessage(obj)
                    );
        }

        protected override void Unsubscribe()
        {
            m_controllerComponent.Dispose();
            base.Unsubscribe();
        }

        #endregion
    }

}
