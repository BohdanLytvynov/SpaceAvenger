using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.TreeItems
{
    internal class TreeItemViewModel : ValidationViewModel
    {
        #region Fields
        private int m_Id;
        private string m_Name;
        ObservableCollection<TreeItemViewModel> m_children;
        #endregion

        #region Properties
        public IGameObject? GameObject { get; set; }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get=> m_children;
            set=> m_children = value;
        }

        public int Id 
        {
            get=> m_Id;
            set=> Set(ref m_Id, value);
        }

        public string Name 
        {
            get=> m_Name;
            set=> Set(ref m_Name, value);
        }
        #endregion

        #region IData Error Info

        public override string this[string columnName]
        {
            get 
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(Name):
                        if (ValidationHelper.TextIsEmpty(Name, out error))
                        {
                            if (GameObject != null)
                            { 
                                GameObject.Name = Name;
                            }
                        }
                        break;
                }

                return error;
            }
        }

        #endregion

        #region Ctor
        public TreeItemViewModel(IGameObject gameObject)
        {            
            m_Id = gameObject.Id;
            m_Name = gameObject.Name;
            m_children = new ObservableCollection<TreeItemViewModel>();
        }

        public TreeItemViewModel()
        {
            m_Id = -1;
            m_Name = string.Empty;
            m_children = new ObservableCollection<TreeItemViewModel>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
