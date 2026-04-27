using AutoMapper;
using Domain.Services.StringResourceLoaders;
using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.DAL.Repositories.SubFactions;
using SpaceAvenger.DAL.RepositoryWrappers;
using SpaceAvenger.ViewModels.SubFactionCard;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Services.Interfaces;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelType(ViewModelUsage.Page)]
    internal class ChooseSubFaction_ViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<SubFactionCardVM> m_subFactionCardVMs;
        private IResourceLoader m_resourceLoader;
        private IStringResourceLoader m_stringResourceLoader;
        private IRepositoryWrapper m_repoWrapper;
        private ImageSource m_factionIcon;
        #endregion

        #region Properties
        public ObservableCollection<SubFactionCardVM> SubFactions 
        { get => m_subFactionCardVMs; set => m_subFactionCardVMs = value; }

        public ImageSource FactionIcon { get => m_factionIcon; set => Set(ref m_factionIcon, value); }
        #endregion

        #region Commands
        public ICommand OnNextButtonPressed { get; }
        public ICommand OnBackButtonPressed { get; }
        #endregion

        #region Ctor

        public ChooseSubFaction_ViewModel(IResourceLoader resourceLoader,
            IStringResourceLoader stringResourceLoader,
            IRepositoryWrapper repoWrapper) : this()
        {
            m_resourceLoader = ExceptionHelper.ThrowIfNull(resourceLoader);
            m_stringResourceLoader = ExceptionHelper.ThrowIfNull(stringResourceLoader);
            m_repoWrapper = ExceptionHelper.ThrowIfNull(repoWrapper);
            //Get UEF Faction
            var faction = m_repoWrapper.FactionRepository.GetById(1).First();
            m_factionIcon = m_resourceLoader.Load<ImageSource>(faction.ImageName);
            InitSubFactions();
        }

        public ChooseSubFaction_ViewModel()
        {
            #region Init Commands
            OnNextButtonPressed = new Command(
                OnNextButtonPressedExecute,
                CanOnNextButtonPressedExecute
                );

            OnBackButtonPressed = new Command(
                OnBackButtonPressedExecute,
                CanOnBackButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods

        #region On Next Button Pressed
        private bool CanOnNextButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnNextButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        #region On Back Button Pressed
        private bool CanOnBackButtonPressedExecute(object p)
            => true;

        private void OnBackButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        private void InitSubFactions()
        {
            m_subFactionCardVMs = new ObservableCollection<SubFactionCardVM>();
            //Get UEF SubFaction
            var subFactions = m_repoWrapper.SubFactionRepository.GetSubFactions(1);
            ExceptionHelper.ThrowIfNull(subFactions);

            foreach (var sf in subFactions)
            {
                SubFactions.Add(new SubFactionCardVM(sf.Id, sf.NameKey, 
                    sf.ImageName, sf.ShortDescriptionKey, 
                    sf.DescriptionKey, m_resourceLoader, m_stringResourceLoader));
            }
        }

        #endregion
    }
}
