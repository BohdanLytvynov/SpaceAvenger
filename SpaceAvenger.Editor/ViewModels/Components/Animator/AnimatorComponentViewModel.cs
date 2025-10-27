using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.ViewModels.AnimatorOptions;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.Factories.Ease;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Animators
{
    internal class AnimatorComponentViewModel : ComponentViewModel
    {
        #region Fields
        private IResourceLoader m_resourceLodaer;
        private IAssemblyLoader m_assemblyLoader;
        private IEaseFactory m_easeFactory;

        private ObservableCollection<AnimatorOptionViewModel> m_animatorOptionsViewModel;
        private AnimatorOptionViewModel m_selectedOption;
        #endregion

        #region Properties
        public ObservableCollection<AnimatorOptionViewModel> AnimatorOptions
        { get => m_animatorOptionsViewModel; set => m_animatorOptionsViewModel = value; }

        public AnimatorOptionViewModel SelectedOption 
        { get => m_selectedOption; set => Set(ref m_selectedOption, value); }
        #endregion

        #region Commmands

        public ICommand OnAddButtonPressed { get; }

        public ICommand OnRemoveButtonPressed { get; }

        #endregion

        #region Ctor
        public AnimatorComponentViewModel(IGameObject gameObject,
            IResourceLoader resourceLoader, IAssemblyLoader assemblyLoader, IEaseFactory easeFactory)
            : base(nameof(Animator), gameObject) 
        {
            #region Init Fields

            var anim = GameObject.GetComponent<Animator>(false);

            if (anim != null)
            {

            }
            else
            {
                GameObject.RegisterComponent(new Animator());
            }

            m_resourceLodaer = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_easeFactory = easeFactory ?? throw new ArgumentNullException(nameof(easeFactory));

            m_animatorOptionsViewModel = new ObservableCollection<AnimatorOptionViewModel>();
            m_selectedOption = new AnimatorOptionViewModel();
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
            #endregion
        }
        #endregion

        #region Methods

        #region On Add Button Pressed
        private bool CanOnAddButtonPressedExecute(object p) => true;

        private void OnAddButtonPressedExecute(object p)
        {
            var option = new AnimatorOptionViewModel(AnimatorOptions.Count + 1, m_resourceLodaer, m_assemblyLoader, m_easeFactory, null);
            option.OnAnimatorChanged += Option_OnAnimatorChanged;
            AnimatorOptions.Add(option);
        }

        #endregion

        #region On Delete Button Pressed
        private bool CanOnDeleteButtonPressedExecute(object p) => m_selectedOption != null && m_selectedOption.ShowNumber > 0;

        private void OnDeleteButtonPressedExecute(object p)
        { 
            m_selectedOption.OnAnimatorChanged -= Option_OnAnimatorChanged;
            AnimatorOptions.Remove(m_selectedOption);

            var anim = GameObject.GetComponent<Animator>(false);
            if (anim != null)
            {
                anim.RemoveAnimation(m_selectedOption.AnimationName);
            }

            m_selectedOption = new AnimatorOptionViewModel();
        }
        #endregion

        private void Option_OnAnimatorChanged(string arg1, WPFGameEngine.WPF.GE.Component.Animations.Animation arg2)
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

        #endregion


    }
}
