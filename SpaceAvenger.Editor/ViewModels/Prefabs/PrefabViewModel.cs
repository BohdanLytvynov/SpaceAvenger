using ViewModelBaseLibDotNetCore.VM;

namespace SpaceAvenger.Editor.ViewModels.Prefabs
{
    internal class PrefabViewModel : ViewModelBase
    {
        #region Fields
        private int m_showNumber;
        private string m_PrefabName;
        #endregion

        #region Properties
        public int ShowNumber
        { get => m_showNumber; set => Set(ref m_showNumber, value); }

        public string PrefabName
        { get=> m_PrefabName; set => Set(ref m_PrefabName, value); }
        #endregion

        #region Ctor
        public PrefabViewModel()
        {
            m_showNumber = -1;
            m_PrefabName = string.Empty;
        }

        public PrefabViewModel(int showNumber)
        {
            m_showNumber = showNumber;
            m_PrefabName = string.Empty;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{m_showNumber}) {m_PrefabName}";
        }
        #endregion
    }
}
