using SpaceAvenger.Editor.Mock;
using System.Windows;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Base;

namespace SpaceAvenger.Editor.ViewModels.Components.Base
{
    internal abstract class ComponentViewModel : ValidationViewModel
    {
        #region Fields
        private string m_componentName;

        private Visibility m_mangeControlsVisible;
        #endregion

        #region Properties

        public Visibility ManageControlsVisible 
        { get => m_mangeControlsVisible; set => Set(ref m_mangeControlsVisible, value); }

        public string ComponentName 
        {
            get=> m_componentName;
            set=> Set(ref m_componentName, value);
        }

        public IGameObjectMock GameObject { get; set; }

        #endregion

        #region Ctor
        public ComponentViewModel(string name, IGameObjectMock gameObject)
        {
            m_componentName = name;
            GameObject = gameObject ?? throw new ArgumentNullException(nameof(gameObject));
            m_mangeControlsVisible = Visibility.Visible;
        }

        #endregion

        #region Methods
        protected abstract void LoadCurrentGameObjProperties();

        public abstract IGEComponent? GetComponent();

        public void ShowManageControls()
        {
            ManageControlsVisible = Visibility.Visible;
        }

        public void HideManageControls()
        {
            ManageControlsVisible = Visibility.Hidden;
        }
        #endregion
    }
}
