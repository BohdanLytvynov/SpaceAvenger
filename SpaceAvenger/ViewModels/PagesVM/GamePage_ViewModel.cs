using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Services.Interfaces.PageManager;
using SpaceAvenger.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using WPFGameEngine.AnimatedControls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WPFGameEngine.Extensions;
using WPFGameEngine.Realizations.Loader;
using WPFGameEngine.Extensions.Animations;
using SpaceAvenger.Services.Interfaces.MessageBus;
using SpaceAvenger.Services.Realizations.Message;
using System.Diagnostics;
using WPFGameEngine.Timers;
using System.Threading;

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

        private CancellationTokenSource m_moveBackCancelToken;

        private bool m_GameStarted;
        
        private Vector m_backOffset;

        private string m_pathToImages;

        private string m_pathToBackGrounds;

        private int m_backCount = 3;

        IPageManagerService<FrameType> m_PageManager;

        IMessageBus m_MessageBus;

        List<BitmapImage>? m_GameBacks;

        ImageSource m_GameBack;

        private Canvas m_Canvas;

        #endregion

        #region Properties

        public Vector BackOffset 
        {
            get=>m_backOffset;
            set=>Set(ref m_backOffset, value);
        }

        public Canvas Canvas { get=> m_Canvas; set=> Set(ref m_Canvas, value); }

        public ImageSource Background { get=> m_GameBack; set=> Set(ref m_GameBack, value); }

        private RecursiveBitmapImageLoader m_RecursiveBitmapImageLoader;
        #endregion

        #region Ctor
        public GamePage_ViewModel()
        {
            #region InitFields
            m_Canvas = new Canvas();
            #endregion

            GameTimer.Init();

            MoveBackground();
        }
        
        public GamePage_ViewModel(IPageManagerService<FrameType> pageManager, 
            IMessageBus messageBus) : this() 
        {
            m_MessageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

            Subscriptions.Add(m_MessageBus.RegisterHandler<GameMessage, string>(OnMessageRecieved));

            m_PageManager = pageManager;
        }

        #endregion

        #region Methods

        #region Set BackGround

        private void OnMessageRecieved(GameMessage gameMessage)
        {
            //We need to Start Game Here
        }

        private void MoveBackground()
        {
            m_moveBackCancelToken = new CancellationTokenSource();

            Task t = Task.Run(() => 
            {
                for (; ;)
                { 
                    
                }
            });

            t.ContinueWith(t => t.Dispose());
        }

        private void SetBackGround()
        {
            Random r = new Random();
            
            var index = r.Next(0, m_backCount);
        }

        private void SetEnvironment()
        { 
            AnimatedControl pulseStar = new AnimatedControl();
            pulseStar.Width = 128;
            pulseStar.Height = 128;                                    

            Canvas.Children.Add(pulseStar);

            pulseStar.ConfigureAnimation(async sb =>
            {
                ObjectAnimationUsingKeyFrames m_anim = new();

                m_anim.AutoReverse = true;

                sb.RepeatBehavior = new RepeatBehavior(1.4);
                                
                sb.Duration = new Duration(TimeSpan.FromSeconds(0.7));

                var images = App.Current.TryGetResourceOrLoad("PulsatingStar", 
                    m_RecursiveBitmapImageLoader, "Anim");

                m_anim.AddKeyFrames(images);
                                
                sb.Children.Add(m_anim);

                Storyboard.SetTarget(m_anim, pulseStar.Image);
                Storyboard.SetTargetProperty(m_anim, new PropertyPath(Image.SourceProperty));

                sb.AccelerationRatio = 0.33;
                sb.DecelerationRatio = 0.55;
            });

            pulseStar.Begin();
        }

        #endregion

        

        #endregion
    }

}
