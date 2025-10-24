using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Animations
{
    [VisibleInEditor(FactoryName = nameof(Animation),
        DisplayName = "Animation",
        GameObjectType = Enums.GEObjectType.Component)]
    public class Animation : ComponentBase, IAnimation
    {
        #region Fields

        private double m_start_global_time;
        private double m_current_local_time;
        private double m_acum;
        private int m_Rows;
        private int m_Columns;
        private int m_current_row;
        private int m_current_column;
        private int m_curr_frame_index;
        private bool m_start;
        private bool m_glob_start_time_set;
        private bool m_reverse;
        private List<IAnimationFrame> m_frames;
        private double m_animSpeed;
        private bool m_IsLooping;
        private BitmapSource m_Texture;
        #endregion

        #region Properties
        public int CurrentFrameIndex { get => m_curr_frame_index; }

        public bool IsRunning { get => m_start; }

        public bool Reverse { get => m_reverse; set => m_reverse = value; }

        //Amount of rows in a sprite Sheet
        public int Rows
        {
            get => m_Rows;
            set
            {
                if (value < 0)
                    throw new ArgumentException("The amount of Rows can't be negative or Zero!");
                else
                    m_Rows = value;
            }
        }
        //Amount of columns in the Sprite Sheet
        public int Columns
        {
            get => m_Columns;
            set
            {
                if (value < 0)
                    throw new ArgumentException("The amount of Columns can't be negative or Zero!");
                else
                    m_Columns = value;
            }
        }
        //Count of frames
        public int FrameCount
        {
            get => Columns * Rows;
        }
        //Height of the frame
        public int FrameHeight
        {
            get 
            {
                if (Rows > 0)
                {
                    return Texture.PixelHeight / Rows;
                }
                return 0;
            }
        }

        //Animation Speed (R) 
        public double AnimationSpeed { get => m_animSpeed; set => m_animSpeed = value; }

        //Width of the frame
        public int FrameWidth 
        {
            get 
            {
                if (Columns > 0)
                {
                    return Texture.PixelWidth / Columns;
                }

                return 0;
                
            } 
        }

        //Weather repeating is required
        public bool IsLooping { get => m_IsLooping; set => m_IsLooping = value; }
        //Texture that represents sprite sheet
        public BitmapSource Texture { get => m_Texture; set => m_Texture = value; }
        public bool Freeze { get; init; }
        public List<IAnimationFrame> AnimationFrames { get => m_frames; }

        #endregion

        #region Ctor

        public Animation(
            BitmapSource texture,
            int rows_on_texture,
            int columns_on_texture,
            bool isLooping,
            double animationSpeed,
            List<IAnimationFrame> frames,
            bool reverse = false,
            bool freeze = true) : base(nameof(Animation))
        {
            m_Texture = texture ?? throw new ArgumentNullException(nameof(texture));

            Rows = rows_on_texture;

            Columns = columns_on_texture;

            IsLooping = isLooping;

            AnimationSpeed = animationSpeed;

            m_frames = frames;

            m_reverse = reverse;

            Freeze = freeze;

            Reset(reverse);

            Stop();
        }
        public Animation() : base(nameof(Animation))
        {
            m_Texture = null;

            Rows = 0;

            Columns = 0;

            IsLooping = false;

            AnimationSpeed = 0;

            m_frames = new List<IAnimationFrame>();

            m_reverse = false;

            Freeze = true;

            Stop();
        }

        #endregion

        #region Functions

        public void Start()
        {
            if (m_start)
                return;

            m_start = true;
        }

        public void Stop()
        {
            if (!m_start)
                return;

            m_start = false;
        }

        public void Reset(bool reverse)
        {
            m_glob_start_time_set = false;

            m_start_global_time = 0;

            m_current_local_time = 0f;

            m_curr_frame_index = 0;

            m_acum = 0f;

            if (!reverse)//Case of the direct animation
            {
                m_current_row = 0;

                m_current_column = 0;
            }
            else//Case of the Reverse Animation
            {
                m_current_row = Rows - 1;

                m_current_column = Columns - 1;
            }
        }

        public BitmapSource GetCurrentFrame()
        {
            Int32Rect rect = new Int32Rect()
            {
                Width = FrameWidth,
                Height = FrameHeight,
                X = FrameWidth * m_current_column,
                Y = FrameHeight * m_current_row
            };

            var cropped = new CroppedBitmap(m_Texture, rect);

#if DEBUG
            Debug.WriteLine($"Rect: X:{rect.X} Y: {rect.Y} W:{rect.Width} H:{rect.Height}");
#endif

            if (Freeze)
                cropped.Freeze();

            return cropped;
        }

        public void Update(IGameTimer gameTimer)
        {
            if (!m_start)
                return;

            var totalTime = gameTimer.totalTime;

            if (!m_glob_start_time_set)
            {
                m_start_global_time = totalTime.TotalMilliseconds;
                m_glob_start_time_set = true;
            }

            //1 Calculate local time
            m_current_local_time = (totalTime.TotalMilliseconds - m_start_global_time)
                * AnimationSpeed;

            if (!Reverse)
                Direct();
            else
                Inverse();
        }

        private void Direct()
        {
            if (IsLooping && m_curr_frame_index >= m_frames.Count - 1)
            {
                Stop();

                Reset(Reverse);

                Start();
            }
            else if (m_curr_frame_index >= m_frames.Count - 1)
            {
                Stop();
            }

            if (m_current_local_time >= m_frames[m_curr_frame_index].Lifespan + m_acum)
            {
                m_acum += m_frames[m_curr_frame_index].Lifespan;
                ++m_current_column;
                ++m_curr_frame_index;
            }

            if (m_current_column >= Columns)//The last column -> need to switch to the next row
            {
                ++m_current_row;
                m_current_column = 0;
            }

            //Debug.WriteLine($"r: {m_current_row} c: {m_current_column}" +
            //    $"  Current LT: {m_current_local_time} Acum: {m_acum} CurrFrame: {m_curr_frame_index}");
        }
        //Need modification
        private void Inverse()
        {
            if (IsLooping && m_current_row == 0 && m_current_column == 0)
            {
                Stop();

                Reset(Reverse);

                Start();
            }

            if (m_current_local_time >= m_frames[m_current_column].Lifespan + m_acum)
            {
                m_acum += m_frames[m_current_column].Lifespan;
                m_current_column--;
            }

            if (m_current_column <= 0)
            {
                m_current_row--;
                m_current_column = Columns - 1;
            }
        }

        #endregion
    }
}
