using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.AnimatorOptions;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animators;

namespace SpaceAvenger.Editor.ViewModels.Components.Animators
{
    internal class AnimatorComponentViewModel : ComponentViewModel
    {
        #region Fields
        private readonly IResourceLoader m_resourceLodaer;
        private readonly IAssemblyLoader m_assemblyLoader;
        private readonly IFactoryWrapper m_factoryWrapper;

        private ObservableCollection<AnimatorOptionViewModel> m_animatorOptionsViewModel;
        private AnimatorOptionViewModel m_selectedOption;
        #endregion

        #region Properties
        public ObservableCollection<AnimatorOptionViewModel> AnimatorOptions
        { get => m_animatorOptionsViewModel; set => m_animatorOptionsViewModel = value; }

        public AnimatorOptionViewModel SelectedOption 
        { 
            get => m_selectedOption;
            set 
            {
                Set(ref m_selectedOption, value);
                if (value == null)
                { 
                    m_selectedOption = new AnimatorOptionViewModel();
                    return;
                }
            }
        }
        #endregion

        #region Commmands

        public ICommand OnAddButtonPressed { get; }
        public ICommand OnRemoveButtonPressed { get; }
        public ICommand OnStartButtonPressed { get; }
        public ICommand OnPauseButtonPressed { get; }
        public ICommand OnResetButtonPressed { get; }

        #endregion

        #region Ctor
        public AnimatorComponentViewModel(IGameObjectMock gameObject,
             IAssemblyLoader assemblyLoader, IFactoryWrapper factoryWrapper)
            : base(nameof(Animator), gameObject) 
        {
            #region Init Fields
            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_resourceLodaer = m_factoryWrapper.ResourceLoader ?? throw new ArgumentNullException("ResourceLoader");
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_animatorOptionsViewModel = new ObservableCollection<AnimatorOptionViewModel>();
            m_selectedOption = new AnimatorOptionViewModel();

            LoadCurrentGameObjProperties();
            #endregion

            #region Init Commands
            OnAddButtonPressed = new Command
                (
                    OnAddButtonPressedExecute,
                    CanOnAddButtonPressedExecute
                );

            OnRemoveButtonPressed = new Command
                (
                    OnDeleteButtonPressedExecute,
                    CanOnDeleteButtonPressedExecute
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
        #endregion

        #region Methods

        #region On Add Button Pressed
        private bool CanOnAddButtonPressedExecute(object p) => true;

        private void OnAddButtonPressedExecute(object p)
        {
            var option = new AnimatorOptionViewModel(AnimatorOptions.Count + 1, 
                m_factoryWrapper, m_assemblyLoader, null, string.Empty);
            option.OnAnimatorChanged += Option_OnAnimatorChanged;
            option.OnAnimationSelected += Option_OnAnimationSelected;
            AnimatorOptions.Add(option);
        }

        private void Option_OnAnimationSelected(string animation)
        {
            if (GameObject == null) return;
            var animator = GameObject.GetComponent<Animator>();
            if (animator == null) return;
            if (!animator.Contains(animation)) return;
            animator.SetAnimationForPlay(animation);
        }

        #endregion

        #region On Delete Button Pressed
        private bool CanOnDeleteButtonPressedExecute(object p) => m_selectedOption != null && m_selectedOption.ShowNumber > 0;

        private void OnDeleteButtonPressedExecute(object p)
        { 
            m_selectedOption.OnAnimatorChanged -= Option_OnAnimatorChanged;
            m_selectedOption.OnAnimationSelected -= Option_OnAnimationSelected;

            var anim = GameObject.GetComponent<Animator>(false);
            if (anim != null)
            {
                anim.Stop();
                anim.RemoveAnimation(m_selectedOption.AnimationName);
            }
            AnimatorOptions.Remove(m_selectedOption);
            m_selectedOption = new AnimatorOptionViewModel();
        }
        #endregion

        #region On Start Button Pressed
        private bool CanOnStartButtonPressedExecute(object p)
        {
            if (GameObject == null) return false;
            var anim = GameObject.GetComponent<Animator>().Current;
            if (anim == null) return false;
            if (anim.IsRunning) return false;
            if (!anim.Validate()) return false;
            return true;
        }

        private void OnStartButtonPressedExecute(object p)
        {
            if (GameObject == null) return;
            var anim = GameObject.GetComponent<Animator>();
            if (anim == null) return;
            anim.Start();
        }
        #endregion

        #region On Pause Button Pressed
        private bool CanOnPauseButtonPressedExecute(object p)
        {
            if (GameObject == null) return false;
            var anim = GameObject.GetComponent<Animator>().Current;
            if (anim == null) return false;
            if (!anim.IsRunning) return false;
            if (!anim.Validate()) return false;
            return true;
        }

        private void OnPauseButtonPressedExecute(object p)
        {
            if (GameObject == null) return;
            var anim = GameObject.GetComponent<Animator>();
            if (anim == null) return;
            anim.Stop();
        }
        #endregion

        #region On Reset Button Pressed
        private bool CanOnResetButtonPressedExecute(object p)
        {
            if (GameObject == null) return false;
            var anim = GameObject.GetComponent<Animator>().Current;
            if (anim == null) return false;
            if (!anim.IsCompleted) return false;
            if (anim.IsRunning) return false;
            if (!anim.Validate()) return false;
            return true;
        }

        private void OnResetButtonPressedExecute(object p)
        {
            if (GameObject == null) return;
            var anim = GameObject.GetComponent<Animator>();
            if (anim == null) return;
            anim.Reset();
        }
        #endregion

        private void Option_OnAnimatorChanged(string arg1, WPFGameEngine.WPF.GE.Component.Animations.IAnimation arg2)
        {
            var anim = GameObject.GetComponent<Animator>(false);
            if (anim != null)
            {
                if (anim.Contains(arg1))
                {
                    anim[arg1] = arg2;
                }
                else
                {
                    anim.AddAnimation(arg1, arg2);
                }
            }
        }

        protected override void LoadCurrentGameObjProperties()
        {
            if (GameObject == null)
                return;

            var anim = GameObject.GetComponent<Animator>();

            if (anim == null)
                return;

            foreach (var item in anim.GetAllKeys())
            {
                var option = new AnimatorOptionViewModel(AnimatorOptions.Count + 1,
                    m_factoryWrapper, m_assemblyLoader, anim[item], item);
                option.OnAnimatorChanged += Option_OnAnimatorChanged;
                option.OnAnimationSelected += Option_OnAnimationSelected;
                AnimatorOptions.Add(option);
            }
        }

        #endregion
    }
}
