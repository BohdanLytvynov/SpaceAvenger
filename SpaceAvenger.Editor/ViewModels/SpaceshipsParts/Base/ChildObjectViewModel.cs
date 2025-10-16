using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Spaceships;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base
{
    internal class ChildObjectViewModel : ValidationViewModel
    {
        #region Events
        public Action<int, int> OnDelete;

        public Action<int> OnSelect;
        #endregion

        #region Fields
        private ObservableCollection<string> m_resourceNames;
        private string m_moduleName;
        private bool m_isValid;
        private int m_showNumber;
        private string m_selectedResourceName;
        private IResourceLoader m_ResourceLoader;
        #endregion

        #region Properties
        public int ShowNumber 
        {
            get=> m_showNumber; 
            set=> Set(ref m_showNumber, value);
        }

        public bool IsValid 
        {
            get=> m_isValid;
            set=> Set(ref m_isValid, value);
        }

        public string ModuleName 
        { 
            get=> m_moduleName;
            set=> Set(ref m_moduleName, value);
        }

        public string SelectedresourceName 
        {
            get=> m_selectedResourceName;
            set 
            {
                Set(ref m_selectedResourceName, value);

                if (GameObject == null && !string.IsNullOrEmpty(value))
                {
                    GameObject = new ModuleMock(ModuleName);
                    GameObject.GetComponent<Sprite>(nameof(Sprite)).Load(m_ResourceLoader.ResourceDictionary, value);                    
                }
                else
                {
                    GameObject.GetComponent<Sprite>(nameof(Sprite)).Load(m_ResourceLoader.ResourceDictionary, value);
                }
            }
        }

        public ObservableCollection<string> ResourceNames
        { get => m_resourceNames; set => Set(ref m_resourceNames, value); }

        public int Id { get; set; }
        public IGameObject GameObject { get; set; }

        #endregion

        #region IDataErrorInfo
        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(ModuleName):
                        if (!ValidationHelper.TextIsEmpty(ModuleName, out error))
                        { }
                        break;
                }

                return error;
            }
        }
        #endregion

        #region ICommands

        public ICommand OnDeleteButtonPressed { get; }
        public ICommand OnSelectButtonPressed { get; }

        #endregion

        #region Ctor

        public ChildObjectViewModel(int showNumber, 
            string moduleName, 
            IResourceLoader resourceLoader,
            IGameObject gameObject, 
            ObservableCollection<string> resNames)
        {
            m_showNumber = showNumber;

            m_ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));

            if (resNames == null)
                throw new ArgumentNullException(nameof(resNames));
            m_resourceNames = new ObservableCollection<string>();
            m_moduleName = moduleName;

            foreach (string name in resNames)
            {
                ResourceNames.Add(name);
            }
            #region Init Commands
            OnDeleteButtonPressed = new Command(
                OnDeleteButtonPressedExecute,
                CanOnDeleteButtonPressedExecute
                );

            OnSelectButtonPressed = new Command(
                OnSelectButtoPressedExecute,
                CanOnSelectButtonPressedExecute
                );
            #endregion
        }

        #endregion

        #region Methods

        #region On Delete Button Presssed
        private bool CanOnDeleteButtonPressedExecute(object p) => true;

        private void OnDeleteButtonPressedExecute(object p)
        {
            OnDelete?.Invoke(Id, GameObject.Id);
        }
        #endregion

        #region OnSelect Button pressed Execute
        private bool CanOnSelectButtonPressedExecute(object p) => true;

        private void OnSelectButtoPressedExecute(object p)
        {
            OnSelect?.Invoke(GameObject.Id);
        }
        #endregion

        #endregion
    }
}
