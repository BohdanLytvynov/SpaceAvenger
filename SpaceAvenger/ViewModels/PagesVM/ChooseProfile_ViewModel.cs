using AutoMapper;
using Domain.Services.StringResourceLoaders;
using Microsoft.EntityFrameworkCore;
using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Users;
using SpaceAvenger.DAL.RepositoryWrappers;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Services.Realizations.Message;
using SpaceAvenger.ViewModels.UserProfile;
using SpaceAvenger.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.PageManager.Base;
using ViewModelBaseLibDotNetCore.VM;

namespace SpaceAvenger.ViewModels.PagesVM
{
    [ViewModelType(ViewModelUsage.Page)]
    internal class ChooseProfile_ViewModel : ViewModelBase
    {
        #region Fields
        private IUserRepository m_userRepository;
        private ObservableCollection<UserProfileVM> m_profileList;
        private int m_SelectedUserIndex;
        private IPageManagerService<FrameType> m_PageManager;
        private IMessageBus m_messageBus;
        private IRepositoryWrapper m_repositoryWrapper;
        private IStringResourceLoader m_StringResourceLoader;
        private IMapper m_mapper;
        private const int NEW_ID = -1;
        private Faction m_selectedFaction;
        #endregion

        #region Properties

        public ObservableCollection<UserProfileVM> ProfileList
        { get => m_profileList; set => m_profileList = value; }

        public int SelectedUserIndex
        { get => m_SelectedUserIndex; set => Set(ref m_SelectedUserIndex, value); }

        #endregion

        #region Commands

        public ICommand OnAddNewProfileButtonPressed { get; }
        public ICommand OnEditUserProfileButtonPressed { get; }
        public ICommand OnDeleteUserProfileButtonPressed { get; }

        #endregion

        #region Ctor

        public ChooseProfile_ViewModel()
        {

        }

        public ChooseProfile_ViewModel(
            IPageManagerService<FrameType> pageManager,
            IRepositoryWrapper repositoryWrapper,
            IMessageBus messageBus,
            IStringResourceLoader stringResourceLoader,
            IMapper mapper) : this()
        {
            m_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            m_messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            m_PageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            m_SelectedUserIndex = -1;
            m_profileList = new ObservableCollection<UserProfileVM>();
            m_userRepository = m_repositoryWrapper.UserRepository;
            m_StringResourceLoader = stringResourceLoader ?? throw new ArgumentNullException(nameof(stringResourceLoader));

            var users = m_userRepository.GetAll().Where(t => true)
                .Include(c => c.Commander);
            
            foreach (var user in users)
            {
                var rank = m_repositoryWrapper.CommanderRankRepository.GetCommanderRank(user.Commander.Id);

                var upvm = new UserProfileVM(
                    m_profileList.Count + 1,
                    user.Id,
                    user.Commander.MaleFemale, 
                    user.ProfileName, 
                    user.Commander.CreatedDate,
                    m_StringResourceLoader.GetString(rank.LevelNameKey),
                    user.Commander.Confirmed);

                upvm.OnUserProfileConfirmedEvent += Up_OnUserProfileConfirmedEvent;
                upvm.OnUserProfileSelectedEvent += Up_OnUserProfileSelectedEvent;
                m_profileList.Add(upvm);
            }

            OnAddNewProfileButtonPressed = new Command(
                OnAddNewProfileButtonPressedExecute,
                CanOnAddNewProfileButtonPressedExecute
                );

            OnEditUserProfileButtonPressed = new Command(
                OnEditUserProfileButtonpressedExecute,
                CanOnEditUserProfileButtonpressedExecute
                );

            OnDeleteUserProfileButtonPressed = new Command(
                OnDeleteUserProfileButtonPressedExecute,
                CanOnDeleteUserProfileButtonPressedExxecute
                );

            #region Subscriptions

            #endregion
        }

        private void Up_OnUserProfileSelectedEvent(UserProfileVM obj)
        {
            m_PageManager.SwitchPage(nameof(Main_Page), FrameType.MainFrame);
            m_messageBus.Send<ChooseProfileMessage_User, UserProfileVM>(new ChooseProfileMessage_User(obj));
        }


        #endregion

        #region Methods

        #region On Add New Profile Button Pressed
        private bool CanOnAddNewProfileButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnAddNewProfileButtonPressedExecute(object p)
        {
            m_selectedFaction = m_repositoryWrapper
                    .FactionRepository.GetUEF();
            var lowestRank = m_repositoryWrapper.StarFleetRankRepository.GetLowest(m_selectedFaction);
            var up = new UserProfileVM(
                    ProfileList.Count + 1, NEW_ID,
                    true, "Please enter your name Commander", default,
                    m_StringResourceLoader.GetString(lowestRank.LevelNameKey),
                    false);

            up.OnUserProfileConfirmedEvent += Up_OnUserProfileConfirmedEvent;
            up.OnUserProfileSelectedEvent += Up_OnUserProfileSelectedEvent;

            ProfileList.Add(up);
        }

        private void Up_OnUserProfileConfirmedEvent(UserProfileVM obj)
        {
            if (obj.Id.Equals(NEW_ID))
            {
                var lowestRank = m_repositoryWrapper
                    .StarFleetRankRepository.GetLowest(m_selectedFaction);

                var user = m_mapper.Map<User>(obj);
                if (user == null) return; //To Do Exception Throw
                var com = m_mapper.Map<Commander>(obj);
                if (com == null) return; //To Do Exception Throw
                com.Faction = m_selectedFaction;
                m_repositoryWrapper.CommanderRankRepository
                    .SetCommanderRank(com, lowestRank, m_selectedFaction);
                user.Commander = com;
                m_userRepository.Add(user);
            }
            else
            {
                var user = m_userRepository.GetById(obj.Id)
                    .Include(x => x.Commander).FirstOrDefault();

                if (user == null) return;//To Do Exception Throw
                var com = user.Commander;
                if (com == null) return;//To Do Exception Throw

                user.ProfileName = obj.UserName;
                com.Name = obj.UserName;
                com.MaleFemale = obj.MaleFemale;

                var comRepo = m_repositoryWrapper.CommanderRepository;
                comRepo.Edit(com.Id, com);

                if (comRepo.Save() == 0) return;//To Do Exception Throw

                m_userRepository.Edit(user.Id, user);
            }
            m_userRepository.Save();
        }

        #endregion

        #region On Edit User Profile Button Pressed

        private bool CanOnEditUserProfileButtonpressedExecute(object p)
        {
            return m_SelectedUserIndex >= 0;
        }

        private void OnEditUserProfileButtonpressedExecute(object p)
        {
            var profile = ProfileList[m_SelectedUserIndex];

            if (profile == null) return;

            profile.Confirmed = false;
        }

        #endregion

        #region On Delete User Profile Button Presssed
        private bool CanOnDeleteUserProfileButtonPressedExxecute(object p)
        {
            return SelectedUserIndex >= 0;
        }

        private void OnDeleteUserProfileButtonPressedExecute(object p)
        {
            var selectedProfile = ProfileList[m_SelectedUserIndex];

            if (selectedProfile.Id == NEW_ID)
            {
                ProfileList.RemoveAt(m_SelectedUserIndex);
                return;
            }

            m_userRepository.DeleteById(selectedProfile.Id);

            var r = m_userRepository.Save();
            if (r > 0)
            {
                ProfileList.RemoveAt(SelectedUserIndex);
                SelectedUserIndex = -1;
            }
        }
        #endregion

        #endregion
    }
}