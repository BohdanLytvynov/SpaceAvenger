using ViewModelBaseLibDotNetCore.VM;

namespace SpaceAvenger.Editor.ViewModels.Options
{
    internal class OptionsViewModel : ViewModelBase
    {
        #region Fields
        private string m_DisplayName;
        private string m_FactoryName;
        #endregion

        #region Properties
        public string DisplayName 
        { get => m_DisplayName; set => Set(ref m_DisplayName, value); }
        public string FactoryName 
        { get=> m_FactoryName; set=> Set(ref m_FactoryName, value); }
        #endregion

        #region Ctor
        public OptionsViewModel(string displayName, string factoryName)
        {
            m_DisplayName = displayName;
            m_FactoryName = factoryName;
        }

        public OptionsViewModel() : this(string.Empty, string.Empty)
        {
            
        }
        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            return obj is OptionsViewModel other &&
                   FactoryName.Equals(other.FactoryName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return FactoryName.GetHashCode();
        }

        #endregion
    }
}
