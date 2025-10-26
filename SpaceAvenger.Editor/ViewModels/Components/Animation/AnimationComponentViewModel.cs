using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.Views;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.Factories.Ease;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Animations
{
    internal class AnimationComponentViewModel : ComponentViewModel
    {
        #region Fields
        private IResourceLoader m_resourceLoader;
        private IAssemblyLoader m_assemblyLoader;
        private IEaseFactory m_easeFactory;
        private int m_rows;
        private int m_columns;
        private double m_duration;
        private string m_easeFunction;
        private string m_resourceName;
        private ImageSource m_imgSource;
        private AnimationConfigurationWindow m_animConfigurationWindow;
        #endregion

        #region Properties
        public int Rows 
        { get=> m_rows; set=> Set(ref m_rows, value); }
        public int Columns 
        { get=> m_columns; set=> Set(ref m_columns, value); }
        public double Duration
        { get=> m_duration; set=> Set(ref m_duration, value); }
        public string EaseFunction 
        { get=> m_easeFunction; set=> Set(ref m_easeFunction, value); }
        public string ResourceName 
        { get=> m_resourceName; set=> Set(ref m_resourceName, value); }
        public ImageSource ImageSource 
        { get=> m_imgSource; set => Set(ref m_imgSource, value); }
        #endregion

        #region Commands
        public ICommand OnConfigureButtonPressed { get; }
        #endregion

        #region Ctor
        public AnimationComponentViewModel(IGameObject gameObject, 
            IResourceLoader resourceLoader, IAssemblyLoader assemblyLoader,
            IEaseFactory easeFactory) : base(nameof(Animation), gameObject) 
        {
            #region Init Fields
            m_easeFactory = easeFactory ?? throw new ArgumentNullException(nameof(easeFactory));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            m_resourceName = string.Empty;
            m_easeFunction = string.Empty;
            m_imgSource = resourceLoader.Load<ImageSource>("Empty");
            #endregion

            #region Init Commands

            OnConfigureButtonPressed = new Command(
                OnConfigureButtonPressedExecute,
                CanOnConfigureButtonPressedExecute
                );

            #endregion
        }
        #endregion

        #region Methods
        #region On Configure Button Pressed

        private bool CanOnConfigureButtonPressedExecute(object p) => true;

        private void OnConfigureButtonPressedExecute(object p)
        {
            ShowConfig();
        }

        #endregion

        private void ShowConfig()
        {
            var animationConfigurationViewModel = new AnimationConfigurationViewModel(m_resourceLoader, m_assemblyLoader, 
                m_easeFactory, GameObject.GetComponent<Animation>());
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

        private void AnimationConfigurationViewModel_OnConfigurationFinished(Animation obj)
        {
            if (GameObject.GetComponent<Animation>() != null)
                GameObject.UnregisterComponent(nameof(Animation));

            GameObject.RegisterComponent(obj);
            Rows = obj.Rows;
            Columns = obj.Columns;
            Duration = obj.TotalTime;
            EaseFunction = obj.EaseType;
            ResourceName = obj.ResourceName;

            m_animConfigurationWindow.Close();
        }

        #endregion
    }
}
