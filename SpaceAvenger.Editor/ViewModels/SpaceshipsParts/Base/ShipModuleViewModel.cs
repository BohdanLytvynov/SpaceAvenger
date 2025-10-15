using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base
{
    internal class ShipModuleViewModel : ValidationViewModel
    {
        #region Events
        public Action<int, int> OnDelete;

        public Action<int> OnSelect;
        #endregion

        #region Fields
        private ObservableCollection<string> m_resourceNames;
        private string m_moduleName;
        private bool m_isValid;
        #endregion

        #region Properties
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

        public ObservableCollection<string> ResourceNames
        { get => m_resourceNames; set => Set(ref m_resourceNames, value); }

        public int Id { get; set; }
        public IGameObject GameObject { get; set; }

        #endregion

        #region ICommands

        public ICommand OnDeleteButtonPressed { get; }
        public ICommand OnSelectButtonPressed { get; }

        #endregion

        #region Ctor

        public ShipModuleViewModel(IGameObject gameObject, ObservableCollection<string> resNames)
        {
            if (resNames == null)
                throw new ArgumentNullException(nameof(resNames));
            m_resourceNames = new ObservableCollection<string>();
            m_moduleName = string.Empty;

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
