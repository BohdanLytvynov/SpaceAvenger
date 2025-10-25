using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels.EaseOptions;
using SpaceAvenger.Editor.ViewModels.Options;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Enums;
using WPFGameEngine.Extensions;
using WPFGameEngine.Factories.Ease;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers;
using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class AnimationConfigurationViewModel : ViewModelBase
    {
        #region Events
        public event Action<Animation> OnConfigurationFinished;
        #endregion

        #region Fields
        private string m_title;
        private GameViewHost m_gameView;
        private IGameObject m_gameObject;
        private Animation m_animation;
        private IResourceLoader m_resourceLoader;
        private ObservableCollection<string> m_resourceNames;
        private ObservableCollection<OptionsViewModel> m_EasingTypes;
        private ObservableCollection<EaseOptionsViewModel> m_EaseConstants;
        private OptionsViewModel m_SelectedEase;
        private string m_SelectedResourceName;
        private IAssemblyLoader m_assemblyLoader;
        private IEaseFactory m_easeFactory;
        private IEase m_SelectedEaseFunction;

        private double m_currentIndex;
        private double m_Rows;
        private double m_Columns;
        private double m_AnimationSpeed;
        private bool m_IsLooping;
        private bool m_IsReversed;
        private int m_TotalFrameCount;
        private double m_TotalTime;
        private PlotModel m_plotModel;
        #endregion

        #region Properties
        public double CurrentIndex 
        { get => m_currentIndex; set => Set(ref m_currentIndex, value); }

        public double TotalTime 
        { get=>m_TotalTime; set => Set(ref m_TotalTime, value); }
        public int TotalFrameCount 
        { get => m_TotalFrameCount; set => Set(ref m_TotalFrameCount, value); }
        public double Rows
        {
            get => m_Rows;
            set
            {
                Set(ref m_Rows, value);
                m_animation.Rows = (int)value;
                TotalFrameCount = m_animation.FrameCount;
            }
        }
        public double Columns
        {
            get => m_Columns;
            set
            {
                Set(ref m_Columns, value);
                m_animation.Columns = (int)value;
                TotalFrameCount = m_animation.FrameCount;
            }
        }
        public double AnimationSpeed
        {
            get => m_AnimationSpeed;
            set
            {
                Set(ref m_AnimationSpeed, value);
                m_animation.AnimationSpeed = value;
            }
        }
        public bool IsLooping
        {
            get => m_IsLooping;
            set
            {
                Set(ref m_IsLooping, value);
                m_animation.IsLooping = value;
            }
        }
        public bool IsReversed
        {
            get => m_IsReversed;
            set 
            {
                Set(ref m_IsReversed, value);
                m_animation.Reverse = value;
            }
        }

        public ObservableCollection<EaseOptionsViewModel> EaseConstants 
        { get=> m_EaseConstants; set => Set(ref m_EaseConstants, value); }

        public ObservableCollection<string> ResourceNames 
        {
            get=> m_resourceNames;
            set=> Set(ref m_resourceNames, value);
        }

        public ObservableCollection<OptionsViewModel> EasingTypes 
        {
            get=> m_EasingTypes;
            set=> Set(ref m_EasingTypes, value);
        }

        public OptionsViewModel SelectedEase
        {
            get => m_SelectedEase;
            set
            {
                Set(ref m_SelectedEase, value);
                if (value != null)
                {
                    m_SelectedEaseFunction = m_easeFactory.Create(value.FactoryName);

                    UpdateEaseFunctionsConstants(m_SelectedEaseFunction);

                    DrawSelectedFunction(m_SelectedEaseFunction);
                }
            }
        }

        public string Title  
        { get => m_title; set => Set(ref m_title, value); }

        public GameViewHost GameView 
        { get => m_gameView; set => Set(ref m_gameView, value); }

        public string SelectedResourceName
        {
            get => m_SelectedResourceName;
            set 
            {
                Set(ref m_SelectedResourceName, value);

                if (string.IsNullOrEmpty(SelectedResourceName))
                    return;

                m_animation.Texture = (BitmapSource)m_resourceLoader.Load<ImageSource>(SelectedResourceName);
            }
        }

        public PlotModel PlotModel
        {
            get => m_plotModel; 
            set => Set(ref m_plotModel, value); 
        }
        #endregion

        #region Commands
        public ICommand OnConfirmButtonPressed { get; }
        public ICommand OnStartButtonPressed { get; }
        public ICommand OnPauseButtonPressed { get; }
        public ICommand OnResetButtonPressed { get; }
        #endregion

        #region Ctor
        public AnimationConfigurationViewModel(IResourceLoader resourceLoader, IAssemblyLoader assemblyLoader,
            IEaseFactory easeFactory)
        {
            #region Init Fields
            m_easeFactory = easeFactory ?? throw new ArgumentNullException(nameof(easeFactory));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_SelectedResourceName = string.Empty;
            m_resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            m_resourceNames = new ObservableCollection<string>();
            m_EasingTypes = new ObservableCollection<OptionsViewModel>();
            m_EaseConstants = new ObservableCollection<EaseOptionsViewModel>();
            m_SelectedEase = new OptionsViewModel();

            var types = m_assemblyLoader["WPFGameEngine"].GetTypes();

            foreach (var type in types)
            {
                var attr = type.GetAttribute<VisibleInEditor>();
                if (attr != null &&
                    attr.GetValue<GEObjectType>("GameObjectType") == GEObjectType.Ease)
                {
                    m_EasingTypes.Add(new OptionsViewModel(
                        attr.GetValue<string>("DisplayName"),
                        attr.GetValue<string>("FactoryName")));
                }
            }

            foreach (var resourceName in m_resourceLoader.GetAllKeys())
            {
                m_resourceNames.Add(resourceName);
            }

            m_title = "Animation Configuration";
            m_gameView = new GameViewHost(new GameTimer());
            m_gameView.OnUpdate = Update;
            m_gameView.SizeChanged += M_gameView_SizeChanged;
            m_gameObject = new GameObjectMock();
            var t = m_gameObject.GetComponent<TransformComponent>();
            t.CenterPosition = new System.Numerics.Vector2(0, 0);
            m_animation = new Animation();
            m_gameObject.RegisterComponent(m_animation);
            m_gameView.AddObject(m_gameObject);
            m_gameView.StartGame();

            m_plotModel = new PlotModel();
            #endregion

            #region Init Commands

            OnConfirmButtonPressed = new Command
                (
                    OnConfirmButtonPressedExecute,
                    CanOnConfirmButtonPressedExecute
                );

            OnStartButtonPressed = new Command
                (
                    OnStartButtonPressedExecute,
                    CanOnStartButtonPressedExecute
                );

            OnPauseButtonPressed = new Command
                (
                    OnPauseButtonPressedExecute,
                    CanOnPauseButtonPressedExecute
                );

            OnResetButtonPressed = new Command
                (
                    OnResetButtonPressedExecute,
                    CanOnResetButtonPressedExecute
                );

            #endregion
        }

        private void M_gameView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var texture = m_animation.Texture;
            if (texture != null && m_gameObject != null)
            {
                var width = m_gameView.ActualWidth;
                var height = m_gameView.ActualHeight;

                double lamdaX = width/texture.Width;
                double lamdaY = height/texture.Height;

                m_gameObject.GetComponent<TransformComponent>().Scale = new System.Drawing.SizeF(
                    (float)lamdaX, (float)lamdaY);
            }
        }
        #endregion

        #region Methods

        #region On Confirm Button Pressed
        private bool CanOnConfirmButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnConfirmButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        #region On Start Button Pressed
        private bool CanOnStartButtonPressedExecute(object p) => 
            !m_animation.IsRunning && m_animation.AnimationFrames.Count > 0;
            
        private void OnStartButtonPressedExecute(object p)
        { 
            if(!m_animation.IsRunning)
                m_animation.Start();
        }
        #endregion

        #region On Pause Button Pressed
        private bool CanOnPauseButtonPressedExecute(object p) => m_animation.IsRunning;

        private void OnPauseButtonPressedExecute(object p)
        {
            if (m_animation.IsRunning)
                m_animation.Stop();
        }
        #endregion

        #region On Reset Button Pressed
        private bool CanOnResetButtonPressedExecute(object p) => !m_animation.IsRunning
            && m_animation.AnimationFrames.Count > 0;

        private void OnResetButtonPressedExecute(object p)
        {
            if (!m_animation.IsRunning)
                m_animation.Reset(m_animation.Reverse);
        }
        #endregion

        public void OnWindowClosing()
        {
            m_gameView.Stop();
        }

        private void UpdateEaseFunctionsConstants(IEase func)
        {
            foreach (var item in EaseConstants)
            {
                item.OnValueChanged -= EaseConstVM_OnValueChanged;
            }
            EaseConstants.Clear();
            foreach (var item in func.Constants)
            {
                EaseOptionsViewModel easeConstVM = new EaseOptionsViewModel
                (
                    item.Key, item.Value
                );
                easeConstVM.OnValueChanged += EaseConstVM_OnValueChanged;
                EaseConstants.Add(easeConstVM);
            }
        }

        private void EaseConstVM_OnValueChanged()
        {
            DrawSelectedFunction(m_SelectedEaseFunction);
        }

        private void DrawSelectedFunction(IEase func)
        {
            //1) Build Axes
            var Plot = new PlotModel();
            Plot.Axes.Add(new LinearColorAxis()
            {
                Position = AxisPosition.Bottom,
                Title = "X",
                AxislineColor = OxyColors.DarkRed
            });
            Plot.Axes.Add(new LinearColorAxis()
            {
                Position = AxisPosition.Left,
                Title = "Y",
                AxislineColor = OxyColors.DarkGreen
            });
            
            int start = 0;
            int end = m_animation.FrameCount;
            var graph = new LineSeries() 
            { Title = SelectedEase.DisplayName, 
                Color = OxyColors.DarkGray, 
                StrokeThickness = 3 };

            m_animation.AnimationFrames.Clear();
            //2) Apply Constants for Ease
            foreach (var c in EaseConstants)
            {
                func.Constants[c.ConstantName] = c.Value;
            }
            //3) Calculate Animation LifeSpan
            for (int i = start; i < end; ++i)
            {
                var y0 = func.Ease((float)i / (float)end);//Normalized Ease function y0
                var y1 = func.Ease((float)(i + 1) / (float)end);//Normalized Ease function y1
                var delta = func.GetDelta(y0, y1);
                m_animation.AnimationFrames.Add(new AnimationFrame(delta * TotalTime));
                graph.Points.Add(new DataPoint(i, y0));
            }

            Plot.Series.Add(graph);
            this.PlotModel = Plot;
        }

        private void Update()
        {
            if (m_animation.IsRunning)
                CurrentIndex = m_animation.CurrentFrameIndex;
        }

        #endregion
    }
}
