using Domain.Services.StringResourceLoaders;
using SpaceAvenger.Resources.Strings;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Services.Interfaces;

namespace SpaceAvenger.ViewModels.SubFactionCard
{
    internal class SubFactionCardVM : ViewModelBase
    {
        #region Events
        public event Action<int> OnCardSelected;
        #endregion

        #region Fields
        private string m_title;
        private ImageSource m_icon;
        private string m_shortDesc;
        private string m_description;
        private string m_MoreButtonName;
        private bool m_MoreLess;
        private int m_subFacId;

        private IStringResourceLoader m_resourceLoader;
        #endregion

        #region Properties
        public string IconName { get; set; }
        public string Title { get => m_title; set => Set(ref m_title, value); }
        public ImageSource Icon { get => m_icon; set => Set(ref m_icon, value); }
        public string ShortDesc { get => m_shortDesc; set => Set(ref m_shortDesc, value); }
        public string Description { get => m_description; set => Set(ref m_description, value); }
        public string MoreButtonName { get => m_MoreButtonName; set => Set(ref m_MoreButtonName, value); }
        public bool MoreLess { get => m_MoreLess; set => Set(ref m_MoreLess, value); }
        #endregion

        #region Commands
        public ICommand OnReadMoreButtonPressed { get; }
        public ICommand OnSelectButtonPressed { get; }
        #endregion

        #region Ctor
        public SubFactionCardVM(int id, string title, 
            string iconName, string shortDesc, string description,
            IResourceLoader resourceLoader, 
            IStringResourceLoader stringResourceLoader)
        {
            m_subFacId = id;
            m_title = stringResourceLoader.GetString(title);
            m_description = stringResourceLoader.GetString(description);
            m_shortDesc = stringResourceLoader.GetString(shortDesc);
            m_icon = resourceLoader.Load<ImageSource>(iconName);
            m_MoreButtonName = UIStrings.MoreButton;
            m_MoreLess = false;
            m_resourceLoader = ExceptionHelper.ThrowIfNull(stringResourceLoader);
            OnReadMoreButtonPressed = new Command(
                OnReadMoreButtonPressedExecute,
                CanReadMoreButtonPressedExecute
                );

            OnSelectButtonPressed = new Command(
                OnSelectButtonPressedExecute,
                CanOnSelectButtonPressedExecute);
        }
        #endregion

        #region Methods

        #region On ReadMore Button Pressed
        private bool CanReadMoreButtonPressedExecute(object p)
            => true;

        private void OnReadMoreButtonPressedExecute(object p)
        {
            MoreLess = !MoreLess;
            if (MoreLess)
            {
                MoreButtonName = UIStrings.LessButton;
            }
            else
            {
                MoreButtonName = UIStrings.MoreButton;
            }
        }
        #endregion

        #region On Select Button Pressed
        private bool CanOnSelectButtonPressedExecute(object p) => true;

        private void OnSelectButtonPressedExecute(object p)
        {
            OnCardSelected?.Invoke(m_subFacId);
        }
        #endregion

        #endregion
    }
}
