using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using System;
using System.Windows.Media;
using System.Windows;
using SpaceAvenger.Services.Realizations.Message;
using c = SpaceAvenger.Services.Constants;
using WPFGameEngine.WPF.GE.GameObjects;
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
using SpaceAvenger.Game.Core.Factions.F10.Destroyer;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using SpaceAvenger.Game.Core.Factions.Neutrals;
using WPFGameEngine.WPF.GE.LevelManagers.Base;

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
        private IObjectBuilder m_objectBuilder;
        private IObjectPoolManager m_objPoolManager;
        private IServiceProvider m_serviceProvider;
        private IControllerComponent m_controllerComponent;
        private ICollisionManager m_collisionManager;
        #endregion

        #region Properties
        public Rect BackViewport 
        {
            get=> m_backViewport; 
            set=> Set(ref m_backViewport, value);
        }

        public WpfMapableObjectViewHost GameView { get=> m_GameView; set=> Set(ref m_GameView, value); }

        public ImageSource Background { get=> m_GameBack; set=> Set(ref m_GameBack, value); }
        #endregion

        #region Ctor

        public GamePage_ViewModel(
            IPageManagerService<FrameType> pageManager,
            IMessageBus messageBus,
            IGameTimer gameTimer,
            IObjectBuilder objectBuilder,
            IObjectPoolManager objectPoolManager,
            IServiceProvider serviceProvider,
            ICollisionManager collisionManager) : this()
        {
            m_collisionManager = collisionManager ?? throw new ArgumentNullException(nameof(collisionManager));
            m_objPoolManager = objectPoolManager ?? throw new ArgumentNullException(nameof(objectPoolManager));
            m_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            m_objectBuilder = objectBuilder ?? throw new ArgumentNullException(nameof(objectBuilder));
            m_MessageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            m_PageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_gameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));
            m_GameView = new WpfMapableObjectViewHost(m_gameTimer, 
                m_objectBuilder, m_objPoolManager, m_collisionManager);
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
                Initialize();
                m_GameView.StartGame();
            }
            else if (gameMessage.Content.Equals(c.STOP_GAME_COMMAND))
            {
                m_GameView.Stop();
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
            MoveBackground();
        }

        #endregion

        private void Initialize()
        {
            m_controllerComponent = m_serviceProvider.GetRequiredService<IControllerComponent>();
            GameView.Instantiate<F10Destroyer>(c => c.RegisterComponent(m_controllerComponent));            
            GameView.Instantiate<AstroBase>();
        }

        private void RegisterNewObject(GameObject gameObject)
        {
            m_GameView.AddObject(gameObject);
        }

        protected override void Unsubscribe()
        {
            m_controllerComponent.Dispose();
            base.Unsubscribe();
        }

        #endregion
    }

}
