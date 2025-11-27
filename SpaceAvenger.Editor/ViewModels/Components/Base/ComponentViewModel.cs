using SpaceAvenger.Editor.Mock;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Base;

namespace SpaceAvenger.Editor.ViewModels.Components.Base
{
    internal abstract class ComponentViewModel : ValidationViewModel
    {
        #region Fields
        private string m_componentName;

        private bool m_mangeComponentEnabled;
        #endregion

        #region Properties

        public bool ManageComponentEnabled
        { get => m_mangeComponentEnabled; set => Set(ref m_mangeComponentEnabled, value); }

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
            m_mangeComponentEnabled = true;
        }

        #endregion

        #region Methods
        protected abstract void LoadCurrentGameObjProperties();

        public abstract IGEComponent? GetComponent();

        public void EnableManageControls()
        {
            ManageComponentEnabled = true;
        }

        public void DisableManageControls()
        {
            ManageComponentEnabled = false;
        }
        #endregion
    }
}
