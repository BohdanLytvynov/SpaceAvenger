using SpaceAvenger.Editor.Mock;
using System.Collections.ObjectModel;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.TreeItems
{
    internal class TreeItemViewModel : ValidationViewModel
    {
        #region Events
        public event Action<TreeItemViewModel> ItemSelected;
        #endregion

        #region Fields
        private int m_ShowNumber;
        private int m_Id;
        private string m_Name;
        private bool m_Selected;
        private bool m_Exported;
        ObservableCollection<TreeItemViewModel> m_children;
        #endregion

        #region Properties
        public int ShowNumber
        {
            get=> m_ShowNumber;
            set=> Set(ref m_ShowNumber, value);
        }

        public bool IsExported
        {
            get => m_Exported;
            set 
            {
                Set(ref m_Exported, value); 
                GameObject.IsExported = value;
            }
        }
        //Indicates if we need to raise events
        public bool RaiseEvent { get; set; }

        public bool Selected 
        { 
            get=> m_Selected;
            set 
            {
                Set(ref m_Selected, value);

                if (!RaiseEvent)
                    return;

                if (Selected)
                {
                    ItemSelected?.Invoke(this);
                }
                else
                {
                    ItemSelected?.Invoke(null);
                }
            }
        }

        public IGameObject? GameObject { get; set; }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get => m_children;
            set => m_children = value;
        }

        public int Id
        {
            get => m_Id;
        }

        public string Name
        {
            get => m_Name;
            set => Set(ref m_Name, value);
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
                        if (!ValidationHelper.TextIsEmpty(Name, out error))
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
        public TreeItemViewModel(int showNumber, IGameObject gameObject)
        {
            GameObject = gameObject;
            m_ShowNumber = showNumber;
            m_Id = (gameObject as IGameObjectMock).Id;
            m_Name = gameObject.Name;
            m_children = new ObservableCollection<TreeItemViewModel>();
            RaiseEvent = true;
        }
        #endregion

        #region Methods
        
        #endregion
    }
}
