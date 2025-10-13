using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Services.Interfaces.PageManager;
using SpaceAvenger.ViewModels.Base;
using System;
using System.Windows.Media;
using System.Windows;
using SpaceAvenger.Services.Interfaces.MessageBus;
using SpaceAvenger.Services.Realizations.Message;
using WPFGameEngine.Timers;
using System.Threading;
using WPFGameEngine.GameViewControl;
using c = SpaceAvenger.Services.Constants;
using WPFGameEngine.WPF.GE.GameObjects;
using System.Collections.Generic;
using System.Numerics;
using SpaceAvenger.Game.Core.Spaceships.F10.Destroyer;
using WPFGameEngine.WPF.GE.Settings;
using System.Drawing;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelName("Game_ViewModel")]
    [ViewModelType(ViewModelUsage.Page)]
    internal class GamePage_ViewModel : SubscriptableViewModel
    {
        #region Fields
        private CancellationTokenSource m_moveBackCancelToken;

        private Rect m_backViewport;
        
        private int m_backCount = 3;

        private double m_BackMoveSpeed;

        IPageManagerService<FrameType> m_PageManager;

        IMessageBus m_MessageBus;
        
        ImageSource m_GameBack;

        private GameViewHost m_GameView;

        private List<GameObject> m_objects;

        #endregion

        #region Properties

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
            IMessageBus messageBus) : this()
        {
            m_MessageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            m_PageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            Subscriptions.Add(m_MessageBus.RegisterHandler<GameMessage, string>(OnMessageRecieved));
        }

        public GamePage_ViewModel()
        {
            #region InitFields
            m_GameView = new GameViewHost();
            m_objects = new List<GameObject>();
            GESettings.DrawGizmo = true;
            #endregion

            m_BackMoveSpeed = 2;
            GameTimer.GameLoopFunction += GameLoop;
        }

        #endregion

        #region Methods

        #region Set BackGround

        private void OnMessageRecieved(GameMessage gameMessage)
        {
            if (gameMessage.Content.Equals(c.START_GAME_COMMAND))
            {
                GameTimer.Started = true;

                Initialize();
            }
            else if (gameMessage.Content.Equals(c.STOP_GAME_COMMAND))
            {
                GameTimer.Started = false;
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
            double newY = yCurrent + m_BackMoveSpeed * 0.01 * GameTimer.deltaTime.TotalSeconds;
            BackViewport = new Rect(xCurrent, newY, 1, 1);
        }

        private void GameLoop()
        { 
            GameView.ClearVisuals();

            MoveBackground();
            m_objects.Sort(new GameObject.GameObjectComparer());

            //Here All the main game logic will be placed and all components will be re-drawed
            foreach (GameObject obj in m_objects)
            {
                obj.Update();
                obj.Render(GameView);
            }
        }

        #endregion

        private void Initialize()
        {
            F10Destroyer destroyer = new F10Destroyer("Player")
            {
                Position = new Vector2(200, 300),
                Scale = new SizeF(0.7f, 0.7f),
                Rotation = 45
            };

            RegisterNewObject(destroyer);

            GameTimer.Init();
        }

        private void RegisterNewObject(GameObject gameObject)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));
            m_objects.Add(gameObject);
        }

        #endregion
    }

}
