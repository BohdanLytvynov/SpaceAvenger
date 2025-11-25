using SpaceAvenger.Editor.Mock;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.GameObjects;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SpaceAvenger.Editor.ViewModels.TreeItems
{
    internal class TreeItemViewModel : ValidationViewModel, 
        ICloneable
    {
        #region Events
        public event Action<int> ItemSelected;
        #endregion

        #region Fields
        private int m_ShowNumber;
        private int m_Id;
        private string m_ObjectName;
        private string m_UniqueName;
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
                if(GameObject is IExportable exp)
                    exp.IsExported = value;
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
                    ItemSelected?.Invoke(GameObject.Id);
                }
                else
                {
                    ItemSelected?.Invoke(-1);
                }
            }
        }

        public IGameObjectMock? GameObject { get; set; }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get => m_children;
            set => m_children = value;
        }

        public int Id
        {
            get => m_Id;
        }

        public string ObjectName
        {
            get => m_ObjectName;
            set => Set(ref m_ObjectName, value);
        }

        public string UniqueName 
        {
            get => m_UniqueName;
            set => Set(ref m_UniqueName, value); 
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
                    case nameof(ObjectName):
                        if (!ValidationHelper.TextIsEmpty(ObjectName, out error))
                        {
                            if (GameObject != null)
                            {
                                GameObject.ObjectName = ObjectName;
                            }
                        }
                        break;
                    case nameof(UniqueName):
                        if (!ValidationHelper.TextIsEmpty(UniqueName, out error))
                        {
                            if (GameObject != null)
                            { 
                                GameObject.UniqueName = UniqueName;
                            }
                        }
                        break;
                }

                return error;
            }
        }

        #endregion

        #region Ctor
        public TreeItemViewModel(int showNumber, IGameObjectMock gameObject)
        {
            GameObject = gameObject;
            m_ShowNumber = showNumber;
            if (gameObject == null) return;
            m_Id = gameObject.Id;
            m_ObjectName = gameObject.ObjectName;
            m_UniqueName = gameObject.UniqueName;
            m_children = new ObservableCollection<TreeItemViewModel>();            
            RaiseEvent = true;
        }

        #endregion

        private TreeItemViewModel CloneRec(TreeItemViewModel item)
        {
            if (item == null)
                return null;

            TreeItemViewModel treeItemViewModel = new TreeItemViewModel(item.ShowNumber, item.GameObject);

            foreach (var ch in item.Children)
            {
                treeItemViewModel.Children.Add((TreeItemViewModel)ch.Clone());
            }

            return treeItemViewModel;
        }

        public object Clone()
        {
            return CloneRec(this);
        }

        public static void Find(int GameObjectId, TreeItemViewModel vm, ref TreeItemViewModel res)
        {
            if (vm == null)
                return;
            if (res != null)
                return;

            if(vm.GameObject.Id == GameObjectId)
                res = vm;

            foreach (var item in vm.Children)
            {
                Find(GameObjectId, item, ref res);
            }
        }

        public static void FindInCollection(int GameObjectId,
            ObservableCollection<TreeItemViewModel> col,
            ref TreeItemViewModel res)
        {
            foreach (var item in col)
            {
                Find(GameObjectId, item, ref res);

                if (res != null)
                    return;
            }
        }

        public static void Unselect(TreeItemViewModel src)
        {
            src.RaiseEvent = false;
            src.Selected = false;
            src.RaiseEvent = true;

            foreach (TreeItemViewModel item in src.Children)
            {
                item.RaiseEvent = false;
                item.Selected = false;
                item.RaiseEvent = true;
            }
        }

        public static void UnselectAll(ObservableCollection<TreeItemViewModel> items)
        {
            foreach (var item in items)
            {
                Unselect(item);
            }
        }
    }
}
