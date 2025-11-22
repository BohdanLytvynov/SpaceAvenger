using SpaceAvenger.Editor.Views;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;

namespace SpaceAvenger.Editor.ViewModels.AnimatorOptions
{
    internal class AnimatorOptionViewModel : ValidationViewModel
    {
        #region Events
        public event Action<string, IAnimation> OnAnimatorChanged;
        public event Action<string> OnAnimationSelected;
        #endregion

        #region Fields
        private int m_ShowNumber;
        private string m_AnimationName;
        private ImageSource m_imageSource;
        private int m_rows;
        private int m_columns;
        private double m_duration;
        private string m_easeFunction;
        private string m_resourceKeyName;
        private IAssemblyLoader m_assemblyLoader;
        private IFactoryWrapper m_factoryWrapper;
        private AnimationConfigurationWindow m_animConfigurationWindow;
        private IAnimation m_animation;
        #endregion

        #region Properties
        public int ShowNumber 
        { get => m_ShowNumber; set => Set(ref m_ShowNumber, value); }
        public string AnimationName 
        { get => m_AnimationName; set => Set(ref m_AnimationName, value); }
        public ImageSource ImageSource
        { get => m_imageSource; set => Set(ref m_imageSource, value); }
        public int Rows 
        { get => m_rows; set => Set(ref m_rows, value); }
        public int Columns
        { get => m_columns; set => Set(ref m_columns, value); }
        public double Duration 
        { get => m_duration; set => Set(ref m_duration, value); }
        public string EaseFunction 
        { get => m_easeFunction; set => Set(ref m_easeFunction, value); }
        public string ResourceName 
        { get=> m_resourceKeyName; set => Set(ref m_resourceKeyName, value); }
        #endregion

        #region Commands
        public ICommand OnConfigureButtonPressed { get; }

        public ICommand OnSelectAnimationButtonPressed { get; }
        #endregion

        #region IDataErrorInfo
        public override string this[string columnName]
        {
            get 
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(AnimationName):
                        SetValidArrayValue(0, !ValidationHelper.TextIsEmpty(AnimationName, out error));
                        break;
                }

                return error;
            }
        }
        #endregion

        #region Ctor

        public AnimatorOptionViewModel()
        {
            m_ShowNumber = -1;
            m_AnimationName = string.Empty;
        }

        public AnimatorOptionViewModel(int showNumber, 
            IFactoryWrapper factoryWrapper,
            IAssemblyLoader assemblyLoader,
            IAnimation animation)
        {
            #region Fields
            InitValidArray(1);
            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_ShowNumber = showNumber;

            m_animation = animation;
            if (animation == null)
            {
                m_AnimationName = string.Empty;
                m_imageSource = m_factoryWrapper.ResourceLoader.Load<ImageSource>("Empty");
                m_easeFunction = string.Empty;
                m_resourceKeyName = string.Empty;
            }
            else
            {
                m_AnimationName = string.Empty;
                m_rows = m_animation.Rows;
                m_columns = m_animation.Columns;
                m_duration = m_animation.TotalTime;
                m_easeFunction = m_animation.EaseType;
                m_resourceKeyName = m_animation.ResourceKey;
                m_imageSource = m_factoryWrapper.ResourceLoader.Load<ImageSource>(m_resourceKeyName);
            }
            
            #endregion

            #region Init Commands
            OnConfigureButtonPressed = new Command
                (
                    OnConfigureButtonPressedExecute,
                    CanOnConfigureButtonPressedExecute
                );

            OnSelectAnimationButtonPressed = new Command
                (
                    OnSelectAnimationButtonPressedExecute,
                    CanOnSelectAnimationButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods

        #region On Select Animation Button Pressed

        private bool CanOnSelectAnimationButtonPressedExecute(object p)
        {
            return m_animation?.Validate() ?? false;
        }

        private void OnSelectAnimationButtonPressedExecute(object p)
        {
            OnAnimationSelected?.Invoke(AnimationName);
        }

        #endregion

        #region Can On Configure Button Pressed 
        private bool CanOnConfigureButtonPressedExecute(object p) => GetValidArrayValue(0);

        private void OnConfigureButtonPressedExecute(object p)
        {
            ShowConfig();
        }
        #endregion

        private void ShowConfig()
        {
            var animationConfigurationViewModel = new AnimationConfigurationViewModel(m_assemblyLoader,
                m_factoryWrapper, m_animation);
            m_animConfigurationWindow = new AnimationConfigurationWindow();
            animationConfigurationViewModel.Dispatcher = m_animConfigurationWindow.Dispatcher;
            animationConfigurationViewModel.OnConfigurationFinished += AnimationConfigurationViewModel_OnConfigurationFinished;
            animationConfigurationViewModel.OnConfigurationCanceled += () =>
            {
                m_animConfigurationWindow.Close();
            };
            m_animConfigurationWindow.DataContext = animationConfigurationViewModel;

            m_animConfigurationWindow.Closed += (object o, EventArgs e) =>
            {
                animationConfigurationViewModel.OnWindowClosing();
                animationConfigurationViewModel.OnConfigurationFinished -= AnimationConfigurationViewModel_OnConfigurationFinished;
                animationConfigurationViewModel.OnConfigurationCanceled -= () => { };
            };

            m_animConfigurationWindow.Topmost = true;
            m_animConfigurationWindow.Show();
        }

        private void AnimationConfigurationViewModel_OnConfigurationFinished(IAnimation obj)
        {
            m_animation = obj;
            Rows = obj.Rows;
            Columns = obj.Columns;
            Duration = obj.TotalTime;
            EaseFunction = obj.EaseType;
            ResourceName = obj.ResourceKey;
            ImageSource = m_factoryWrapper.ResourceLoader.Load<ImageSource>(ResourceName);
            m_animConfigurationWindow.Close();

            OnAnimatorChanged?.Invoke(AnimationName, m_animation);
        }

        #endregion
    }
}
