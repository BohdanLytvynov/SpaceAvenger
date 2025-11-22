using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Sprites
{
    internal class SpriteComponentViewModel : ComponentViewModel
    {
        #region fields
        private ObservableCollection<string> m_resourceNames;
        private string m_selectedResource;
        private ImageSource m_ImgSource;
        private IResourceLoader m_ResourceLoader;
        #endregion

        #region Properties
        public ObservableCollection<string> ResourceNames 
        { 
            get=> m_resourceNames; 
            set=> m_resourceNames = value;
        }

        public string SelectedResource 
        { 
            get=> m_selectedResource;
            set 
            {
                Set(ref m_selectedResource, value);

                if (string.IsNullOrEmpty(value) || m_ResourceLoader == null)
                    return;

                ImageSource = m_ResourceLoader.Load<ImageSource>(SelectedResource);
            }
        }

        public ImageSource ImageSource 
        {
            get=> m_ImgSource;
            set=> Set(ref m_ImgSource, value);
        }
        #endregion

        #region Commands
        public ICommand OnApplyButtonPressed { get; }
        #endregion

        #region Ctor
        public SpriteComponentViewModel(IGameObjectMock gameObject, IResourceLoader resourceLoader) : base(nameof(Sprite), gameObject)
        {
            #region Init Fields

            m_selectedResource = string.Empty;
            m_resourceNames = new ObservableCollection<string>();
            m_ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));

            m_ImgSource = m_ResourceLoader.Load<ImageSource>("Empty");

            foreach (var item in m_ResourceLoader.GetAllKeys())
            { 
                m_resourceNames.Add(item);
            }

            LoadCurrentGameObjProperties();

            #endregion

            #region Init Commands
            OnApplyButtonPressed = new Command(
                OnApplyButtonPressedExecute,
                CanOnApplyButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods

        #region OnApplyButtonPressedExecute

        private bool CanOnApplyButtonPressedExecute(object p) => 
            !(string.IsNullOrEmpty(SelectedResource) || GameObject == null || m_ResourceLoader == null);

        private void OnApplyButtonPressedExecute(object p)
        {
            GameObject.GetComponent<Sprite>().Load(SelectedResource);
        }

        protected override void LoadCurrentGameObjProperties()
        {
            if (GameObject == null)
                return;

            var s = GameObject.GetComponent<Sprite>();

            if (s == null)
                return;
            if(!string.IsNullOrEmpty(s.ResourceKey))
                SelectedResource = s.ResourceKey;
        }

        #endregion
        #endregion
    }
}
