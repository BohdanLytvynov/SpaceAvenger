using SpaceAvenger.Editor.Mock;
using ViewModelBaseLibDotNetCore.VM;

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
        #endregion

        #region Ctor
        public ComponentViewModel(string name, IGameObjectMock gameObject)
        {
            m_componentName = name;
            GameObject = gameObject ?? throw new ArgumentNullException(nameof(gameObject));
        }

        #endregion

        #region Methods
        protected abstract void LoadCurrentGameObjProperties();
        #endregion
    }
}
