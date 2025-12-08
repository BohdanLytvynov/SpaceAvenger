using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.EaseOptions;
using SpaceAvenger.Editor.ViewModels.Options;
using SpaceAvenger.Services.WpfGameViewHost;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Enums;
using WPFGameEngine.Extensions;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.Timers;
using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Ease.Base;
using WPFGameEngine.WPF.GE.Settings;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class AnimationConfigurationViewModel : ViewModelBase
    {
        #region Events
        public event Action<IAnimation> OnConfigurationFinished;

        public event Action OnConfigurationCanceled;
        #endregion

        #region Fields
        private string m_title;
        private WpfGameObjectViewHost m_gameView;
        private IGameObjectMock m_gameObject;
        private IAnimation m_animation;
        private ObservableCollection<string> m_resourceNames;
        private ObservableCollection<OptionsViewModel> m_EasingTypes;
        private ObservableCollection<EaseOptionsViewModel> m_EaseConstants;
        private OptionsViewModel m_SelectedEase;
        private string m_SelectedResourceName;
        private IAssemblyLoader m_assemblyLoader;
        private IFactoryWrapper m_factoryWrapper;
        private IEase m_SelectedEaseFunction;

        private bool m_ResourceKeyEnabled;
        private bool m_EaseEnabled;

        private double m_currentIndex;
        private double m_Rows;
        private double m_Columns;
        private double m_AnimationSpeed;
        private bool m_IsLooping;
        private bool m_IsReversed;
        private int m_TotalFrameCount;
        private double m_TotalTime;
        private PlotModel m_plotModel;
        private double m_FrameIndexes;
        #endregion

        #region Properties
        public bool ResourceKeyEnabled 
        { get=> m_ResourceKeyEnabled; set => Set(ref m_ResourceKeyEnabled, value); }
        public bool EaseEnabled 
        { get=> m_EaseEnabled; set => Set(ref m_EaseEnabled, value); }
        public double FrameIndexes 
        { get => m_FrameIndexes; set => Set(ref m_FrameIndexes, value); }
        public double CurrentIndex 
        { get => m_currentIndex; set => Set(ref m_currentIndex, value); }
        public double TotalTime 
        { 
            get=>m_TotalTime;
            set 
            {
                Set(ref m_TotalTime, value); 
                m_animation.TotalTime = value;
                EnableEaseCombobox();
                EnableResourceCombobox();
                if (m_SelectedEaseFunction != null)
                    DrawSelectedFunction(m_SelectedEaseFunction);
            }
        }
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
                FrameIndexes = TotalFrameCount - 2;//Frame LastIndex is n-1 and also we need to subtract 1 cause we have already displayed the first frame
                EnableEaseCombobox();
                EnableResourceCombobox();
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
                FrameIndexes = TotalFrameCount - 2;
                EnableEaseCombobox();
                EnableResourceCombobox();
            }
        }
        public double AnimationSpeed
        {
            get => m_AnimationSpeed;
            set
            {
                Set(ref m_AnimationSpeed, value);
                m_animation.AnimationSpeed = value;
                EnableEaseCombobox();
                EnableResourceCombobox();
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
                m_animation.Reset(m_animation.Reverse);
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
                SetDefaultIfNull(ref m_SelectedEase, value, new OptionsViewModel());
                if (value != null)
                {
                    m_SelectedEaseFunction = (IEase)m_factoryWrapper.CreateObject(value.FactoryName);

                    if (m_animation != null && m_animation.EaseConstants.Count > 0)
                    {
                        foreach (var item in m_animation.EaseConstants)
                        {
                            m_SelectedEaseFunction.Constants.Add(item.Key, item.Value);
                        }
                    }

                    UpdateEaseFunctionsConstants(m_SelectedEaseFunction);

                    DrawSelectedFunction(m_SelectedEaseFunction);
                    m_animation.EaseFactoryName = value.FactoryName;
                    m_animation.EaseType = value.DisplayName;

                    EnableEaseCombobox();
                    EnableResourceCombobox();
                }
            }
        }
        public string Title  
        { get => m_title; set => Set(ref m_title, value); }
        public WpfGameObjectViewHost GameView 
        { get => m_gameView; set => Set(ref m_gameView, value); }
        public string SelectedResourceName
        {
            get => m_SelectedResourceName;
            set 
            {
                SetDefaultIfNull(ref m_SelectedResourceName, value, string.Empty);

                if (string.IsNullOrEmpty(SelectedResourceName))
                    return;

                m_animation.Load(value);
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
        public ICommand OnCancelButtonPressed { get; }
        #endregion

        #region Ctor
        public AnimationConfigurationViewModel(IAssemblyLoader assemblyLoader,
            IFactoryWrapper factoryWrapper, IAnimation old)
        {
            #region Init Fields
            GESettings.DrawGizmo = false;
            m_TotalTime = 1000f;
            m_plotModel = new PlotModel();
            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_SelectedResourceName = string.Empty;
            m_resourceNames = new ObservableCollection<string>();
            m_EasingTypes = new ObservableCollection<OptionsViewModel>();
            m_EaseConstants = new ObservableCollection<EaseOptionsViewModel>();
            m_SelectedEase = new OptionsViewModel();
            m_AnimationSpeed = 1;

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

            foreach (var resourceName in m_factoryWrapper.ResourceLoader.GetAllKeys())
            {
                m_resourceNames.Add(resourceName);
            }

            m_title = "Animation Configuration";
            m_gameView = new WpfGameObjectViewHost(new GameTimer());
            m_gameView.OnUpdate = Update;
            m_gameObject = new GameObjectMock();
            m_gameObject.RegisterComponent(m_factoryWrapper.CreateObject<TransformComponent>());

            //Load Old Data About Animation
            if (old != null && old.Validate())
            {
                m_animation = old;
                Rows = old.Rows;
                Columns = old.Columns;
                AnimationSpeed = old.AnimationSpeed;
                TotalTime = old.TotalTime;
                SelectedEase = new OptionsViewModel(old.EaseType, old.EaseFactoryName);
                SelectedResourceName = old.ResourceKey;
                IsLooping = old.IsLooping;
                IsReversed = old.Reverse;
                EnableEaseCombobox();
                EnableResourceCombobox();
            }
            else
            {
                m_animation = m_factoryWrapper.CreateObject<Animation>();
            }

            m_gameObject.RegisterComponent(m_animation);
            m_gameView.AddObject(m_gameObject);
            m_gameView.StartGame();
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

            OnCancelButtonPressed = new Command
                (
                    OnCancelButtonPressedExecute,
                    CanOnCancelButtonPressedExecute
                );

            #endregion
        }

        #endregion

        #region Methods

        #region On Confirm Button Pressed
        private bool CanOnConfirmButtonPressedExecute(object p)
            => m_animation != null && m_animation.Validate();

        private void OnConfirmButtonPressedExecute(object p)
        {
            m_animation.Stop();
            m_animation.Reset(m_animation.Reverse);

            OnConfigurationFinished?.Invoke(m_animation);
        }
        #endregion

        #region On Start Button Pressed
        private bool CanOnStartButtonPressedExecute(object p) =>
           m_animation.Validate() && !m_animation.IsRunning
            && !m_animation.IsCompleted;
            
        private void OnStartButtonPressedExecute(object p)
        {
            m_animation.Start();
        }
        #endregion

        #region On Pause Button Pressed
        private bool CanOnPauseButtonPressedExecute(object p) => 
            m_animation.IsRunning && !m_animation.IsCompleted;

        private void OnPauseButtonPressedExecute(object p)
        {
            m_animation.Stop();
        }
        #endregion

        #region On Reset Button Pressed
        private bool CanOnResetButtonPressedExecute(object p)
        {
            if (!IsLooping)
            {
                return !m_animation.IsRunning
                && m_animation.Validate() && m_animation.IsCompleted;
            }
            return !m_animation.IsRunning && m_animation.Validate();
        }

        private void OnResetButtonPressedExecute(object p)
        {
            if (!m_animation.IsRunning)
                m_animation.Reset(m_animation.Reverse);
        }
        #endregion

        #region On Cancel Button Pressed

        private bool CanOnCancelButtonPressedExecute(object p) => true;

        private void OnCancelButtonPressedExecute(object p)
        {
            OnConfigurationCanceled?.Invoke();
        }

        #endregion

        public void OnWindowClosing()
        {
            GESettings.DrawGizmo = true;
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
                var delta = Math.Abs(y1 - y0);
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

        private void EnableEaseCombobox()
        {
            EaseEnabled = TotalTime > 0 && TotalFrameCount > 0 && AnimationSpeed > 0;
        }

        private void EnableResourceCombobox()
        {
            ResourceKeyEnabled = TotalTime > 0 && 
                TotalFrameCount > 0 
                && AnimationSpeed > 0 
                && m_SelectedEaseFunction != null 
                && m_animation.AnimationFrames.Count > 0;
        }
        #endregion
    }
}
