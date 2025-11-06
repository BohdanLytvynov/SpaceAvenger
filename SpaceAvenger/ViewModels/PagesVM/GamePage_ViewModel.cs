using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using System;
using System.Windows.Media;
using System.Windows;
using SpaceAvenger.Services.Realizations.Message;
using WPFGameEngine.GameViewControl;
using c = SpaceAvenger.Services.Constants;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Settings;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.ObjectBuilders.Base;
using SpaceAvenger.Game.Core.Spaceships.F10.Destroyer;
using System.Windows.Input;
using WPFGameEngine.WPF.GE.Component.Controllers;

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
        private GameViewHost m_GameView;
        private IGameTimer m_gameTimer;
        private IObjectBuilder m_objectBuilder;

        #endregion

        #region Properties
        public IControllerComponent ControlComponent { get; set; }

        public double ActualHeight
        {
            get=>App.Current.MainWindow.Height;
        }

        public double ActualWidth 
        {
            get => App.Current.MainWindow.Width;
        }

        public Rect BackViewport 
        {
            get=> m_backViewport; 
            set=> Set(ref m_backViewport, value);
        }

        public GameViewHost GameView { get=> m_GameView; set=> Set(ref m_GameView, value); }

        public ImageSource Background { get=> m_GameBack; set=> Set(ref m_GameBack, value); }
        #endregion

        #region Ctor

        public GamePage_ViewModel(
            IPageManagerService<FrameType> pageManager,
            IMessageBus messageBus,
            IGameTimer gameTimer,
            IObjectBuilder objectBuilder) : this()
        {
            m_objectBuilder = objectBuilder ?? throw new ArgumentNullException(nameof(objectBuilder));
            m_MessageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            m_PageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_gameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));
            m_GameView = new GameViewHost(m_gameTimer);
            GameView.OnUpdate += Update;
            Subscriptions.Add(m_MessageBus.RegisterHandler<GameMessage, string>(OnMessageRecieved));
        }

        public GamePage_ViewModel()
        {
            #region InitFields
            GESettings.DrawGizmo = true;
            GESettings.DrawBorders = true;
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
            var obj = m_objectBuilder.Build<F10Destroyer>();
            obj.RegisterComponent(ControlComponent);
            RegisterNewObject(obj);
        }

        private void RegisterNewObject(GameObject gameObject)
        {
            m_GameView.AddObject(gameObject);
        }

        public void MouseMove(object sender, MouseEventArgs e)
        { 
            
        }

        #endregion
    }

}
