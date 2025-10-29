using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.Views;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Animations
{
    internal class AnimationComponentViewModel : ComponentViewModel
    {
        #region Fields
        private IFactoryWrapper m_factoryWrapper;
        private IAssemblyLoader m_assemblyLoader;
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
            IFactoryWrapper factoryWrapper, IAssemblyLoader assemblyLoader
            ) : base(nameof(Animation), gameObject) 
        {
            #region Init Fields
            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            
            m_resourceName = string.Empty;
            m_easeFunction = string.Empty;
            m_imgSource = m_factoryWrapper.ResourceLoader.Load<ImageSource>("Empty");
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
            var animationConfigurationViewModel = new AnimationConfigurationViewModel(m_assemblyLoader, 
                m_factoryWrapper, GameObject.GetComponent<Animation>());
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
            ResourceName = obj.ResourceKey;
            ImageSource = m_factoryWrapper.ResourceLoader.Load<ImageSource>(ResourceName);

            m_animConfigurationWindow.Close();
        }

        #endregion
    }
}
