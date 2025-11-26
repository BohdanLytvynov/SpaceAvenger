using System.Windows;
using System.Windows.Media.Imaging;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base.ImageComponents;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Component.Animations
{
    [VisibleInEditor(FactoryName = nameof(Animation),
        DisplayName = "Animation",
        GameObjectType = Enums.GEObjectType.Component)]
    public class Animation : ImageComponentBase<BitmapSource>, IAnimation
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
        #endregion

        #region Properties
        public override List<string> IncompatibleComponents => 
            new List<string> { nameof(Sprite), nameof(Animator) };

        public Dictionary<string, double> EaseConstants { get; }

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
        public bool Freeze { get; set; }
        public List<IAnimationFrame> AnimationFrames { get => m_frames; }
        public string EaseType { get; set; }
        public string EaseFactoryName { get; set; }
        public double TotalTime { get; set; }
        
        public bool IsCompleted 
        {
            get 
            {
                if (IsLooping)
                    return false;
                else
                {
                    if (!Reverse)
                        return m_curr_frame_index == AnimationFrames.Count - 1;
                    else
                        return m_curr_frame_index == 0;
                }
                    
            }
        }

        #endregion

        #region Ctor
        
        public Animation(IResourceLoader resourceLoader) : 
            base(nameof(Animation))
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));

            EaseConstants = new Dictionary<string, double>();

            Texture = null;

            Rows = 0;

            Columns = 0;

            IsLooping = false;

            AnimationSpeed = 1;

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

            m_acum = 0f;

            if (!reverse)//Case of the direct animation
            {
                m_current_row = 0;

                m_current_column = 0;

                m_curr_frame_index = 0;
            }
            else//Case of the Reverse Animation
            {
                m_current_row = Rows - 1;

                m_current_column = Columns - 1;

                m_curr_frame_index = AnimationFrames.Count - 1;
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

//#if DEBUG
//            Debug.WriteLine($"Rect: X:{rect.X} Y: {rect.Y} W:{rect.Width} H:{rect.Height}");
//#endif
            var cropped = new CroppedBitmap(Texture, rect);

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

#if DEBUG
            //Debug.WriteLine($"CLT: {m_current_local_time}");

            //Debug.WriteLine($"CFI: {m_curr_frame_index}");
#endif

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

            if (!m_start)
                return;

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
            if (IsLooping && m_current_row == 0 && m_current_column == 0 && m_curr_frame_index == 0)
            {
                Stop();

                Reset(Reverse);

                Start();
            }
            else if(m_curr_frame_index == 0 && m_current_row == 0 && m_current_column == 0)
            {
                Stop();
            }

            if (!m_start)
                return;

            if (m_current_local_time >= m_frames[m_curr_frame_index].Lifespan + m_acum)
            {
                m_acum += m_frames[m_curr_frame_index].Lifespan;
                --m_current_column;
                --m_curr_frame_index;
            }

            if (m_current_column < 0)
            {
                m_current_row--;
                m_current_column = Columns - 1;
            }

            //Debug.WriteLine($"r: {m_current_row} c: {m_current_column}" +
            //    $"  Current LT: {m_current_local_time} Acum: {m_acum} CurrFrame: {m_curr_frame_index}");
        }

        public bool Validate()
        {
            return AnimationFrames.Count > 0 && Texture != null
                && !string.IsNullOrEmpty(ResourceKey)
                && TotalTime > 0 && AnimationSpeed > 0 
                && !string.IsNullOrEmpty(EaseType)
                && !string.IsNullOrEmpty(EaseFactoryName);
        }

        public override AnimationDto ToDto()
        {
            var animDto = new AnimationDto()
            {
                ResourceKey = ResourceKey,
                Rows = Rows,
                Columns = Columns,
                Duration = TotalTime,
                Freeze = Freeze,
                EaseFactoryName = EaseFactoryName,
                EaseType = EaseType,
                IsLooping = IsLooping,
                Reverse = Reverse,
                AnimationSpeed = AnimationSpeed,
            };


            foreach (var frame in AnimationFrames)
            {
                animDto.AnimationFrames.Add(new AnimationFrame(frame.Lifespan));
            }

            foreach (var c in EaseConstants)
            {
                animDto.EaseConstants.Add(c.Key, c.Value);
            }

            return animDto;
        }

        #endregion

        #region IClonable

        public override object Clone()
        {
            var anim =  new Animation(ResourceLoader)
            { 
                Rows = Rows, Columns = Columns,
                Reverse = Reverse, AnimationSpeed = AnimationSpeed,
                TotalTime = TotalTime, IsLooping = IsLooping,
                Freeze = Freeze, ResourceKey = ResourceKey,
                EaseFactoryName = EaseFactoryName,
                EaseType = EaseType,
                Texture = Texture
            };

            foreach (var frame in AnimationFrames)
            {
                anim.AnimationFrames.Add(new AnimationFrame(frame.Lifespan));
            }

            return anim;
        }

        #endregion
    }
}
