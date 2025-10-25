using ViewModelBaseLibDotNetCore.VM;

namespace SpaceAvenger.Editor.ViewModels.EaseOptions
{
    internal class EaseOptionsViewModel : ViewModelBase
    {
        #region Events
        public event Action OnValueChanged;
        #endregion

        #region Fields
        private string m_constantName;

        private double m_constantValue;
        #endregion

        #region Properties
        public string ConstantName 
        { get => m_constantName; set => Set(ref m_constantName, value); }

        public double Value 
        { 
            get => m_constantValue;
            set 
            {
                Set(ref m_constantValue, value);
                OnValueChanged?.Invoke();
            }
        }
        #endregion

        #region Ctor
        public EaseOptionsViewModel(string constantName, double constantValue)
        {
            m_constantName = constantName;
            m_constantValue = constantValue;
        }
        #endregion
    }
}
