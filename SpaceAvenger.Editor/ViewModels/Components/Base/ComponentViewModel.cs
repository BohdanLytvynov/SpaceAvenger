using SpaceAvenger.Editor.Mock;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Base;

namespace SpaceAvenger.Editor.ViewModels.Components.Base
{
    internal abstract class ComponentViewModel : ValidationViewModel
    {
        #region Fields
        private string m_componentName;
        #endregion

        #region Properties
        public string ComponentName 
        {
            get=> m_componentName;
            set=> Set(ref m_componentName, value);
        }

        public IGameObjectMock GameObject { get; set; }
        public IGEComponent Component { get; set; }
        #endregion

        #region Ctor
        public ComponentViewModel(string name, IGameObjectMock gameObject, IGEComponent component)
        {
            m_componentName = name;
            GameObject = gameObject ?? throw new ArgumentNullException(nameof(gameObject));
            Component = component ?? throw new ArgumentNullException(nameof(component));
        }

        #endregion

        #region Methods
        protected abstract void LoadCurrentGameObjProperties();
        #endregion
    }
}
