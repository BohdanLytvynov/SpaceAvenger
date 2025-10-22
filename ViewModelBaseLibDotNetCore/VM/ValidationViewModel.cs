using System.ComponentModel;

namespace ViewModelBaseLibDotNetCore.VM
{
    public abstract class ValidationViewModel : ViewModelBase, IDataErrorInfo
    {
        #region IDataErrorInfo
        public virtual string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();
        #endregion

        #region Fields
        private bool[] m_ValidArray;
        #endregion

        #region Ctor
        public ValidationViewModel()
        {
            
        }
        #endregion

        #region Methods
        protected void InitValidArray(int count)
        {
            m_ValidArray = new bool[count];
        }

        protected bool GetValidArrayValue(int index)
        {
            return m_ValidArray[index];
        }

        protected void SetValidArrayValue(int index, bool value)
        {
            m_ValidArray[index] = value;
        }

        protected int GetLastValidArrayIndex()
        {
            return m_ValidArray.Length - 1;
        }

        protected bool Validate(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; ++i)
            {
                if (!m_ValidArray[i])
                    return false;
            }

            return true;
        }
        #endregion
    }
}
