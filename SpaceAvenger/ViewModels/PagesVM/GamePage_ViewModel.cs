using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Services.Interfaces.PageManager;
using SpaceAvenger.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using WPFGameEngine.Realizations.Loader;
using SpaceAvenger.Services.Interfaces.MessageBus;
using SpaceAvenger.Services.Realizations.Message;
using WPFGameEngine.Timers;
using System.Threading;
using WPFGameEngine.GameViewControl;
using c = SpaceAvenger.Services.Constants;
using System.Diagnostics;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelName("Game_ViewModel")]
    [ViewModelType(ViewModelUsage.Page)]
    internal class GamePage_ViewModel : SubscriptableViewModel
    {
        #region event

        public event Func<Task> OnPageBuild;

        #endregion

        #region Fields

        private double m_ActualHeight;

        private double m_ActualWidth;

        private CancellationTokenSource m_moveBackCancelToken;

        private bool m_GameStarted;

        private Rect m_backViewport;

        private string m_pathToImages;

        private string m_pathToBackGrounds;

        private int m_backCount = 3;

        private double m_BackMoveSpeed;

        IPageManagerService<FrameType> m_PageManager;

        IMessageBus m_MessageBus;
        
        ImageSource m_GameBack;

        private GameViewHost m_GameView;

        #endregion

        #region Properties

        public double ActualHeight
        {
            get=> m_ActualHeight;
            set=>Set(ref m_ActualHeight, value);
        }

        public double ActualWidth 
        { 
            get=>m_ActualWidth;
            set=>Set(ref m_ActualWidth, value);
        }

        public Rect BackViewport 
        {
            get=> m_backViewport; 
            set=> Set(ref m_backViewport, value);
        }

        public GameViewHost GameView { get=> m_GameView; set=> Set(ref m_GameView, value); }

        public ImageSource Background { get=> m_GameBack; set=> Set(ref m_GameBack, value); }

        private RecursiveBitmapImageLoader m_RecursiveBitmapImageLoader;
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
            #endregion

            m_BackMoveSpeed = 2;
            GameTimer.UpdateFunction += Update;
            GameTimer.Init();
        }

        #endregion

        #region Methods

        #region Set BackGround

        private void OnMessageRecieved(GameMessage gameMessage)
        {
            if (gameMessage.Content.Equals(c.START_GAME_COMMAND))
            {
                GameTimer.Started = true;

                MoveBackground();
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

        private void Update()
        { 
            GameView.ClearVisuals();

            MoveBackground();

            //Here All the main game logic will be placed and all components will be re-drawed

            //Testing
            DrawingVisual test = new DrawingVisual();

            using (var context = test.RenderOpen())
            {
                Brush fill = new SolidColorBrush(Colors.Red);
                Pen border = new Pen(Brushes.Black, 1);
                fill.Freeze();
                border.Freeze();

                context.DrawRectangle(fill, border, new Rect(50,50, 20, 20));
            }

            GameView.AddVisual(test);
        }

        #endregion
        
        #endregion
    }

}
