using SpaceAvenger.DAL.Models;
using System;
using System.Threading;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.VM;

namespace SpaceAvenger.ViewModels.UserProfile
{
    public class UserProfileVM : ViewModelBase, IEquatable<UserProfileVM>
    {
        #region Events
        public event Action<UserProfileVM>? OnUserProfileConfirmedEvent;

        public event Action<UserProfileVM>? OnUserProfileSelectedEvent;
        #endregion

        #region Fields

        private bool m_MaleFemale;
        private string m_UserName;
        private string m_RankName;
        private int m_Number;
        private bool m_Confirmed;
        private DateTime m_enlistedDate;
        private int m_MissionsCount;
        private float m_points;
        #endregion

        #region Properties
        public int Id { get; init; }
        public bool MaleFemale { get => m_MaleFemale; set => Set(ref m_MaleFemale, value); }
        public string UserName { get => m_UserName; set => Set(ref m_UserName, value); }
        public string RankName { get => m_RankName; set => Set(ref m_RankName, value); }
        public int Number { get => m_Number; set => Set(ref m_Number, value); }
        public bool Confirmed
        {
            get => m_Confirmed;
            set => Set(ref m_Confirmed, value);
        }
        public DateTime EnlistedDate { get => m_enlistedDate; set => Set(ref m_enlistedDate, value); }
        public int MissionsCount { get => m_MissionsCount; set => Set(ref m_MissionsCount, value); }
        public float Points { get => m_points; set => Set(ref m_points, value); }
        #endregion

        #region Commands
        public ICommand? OnConfirmButtonPressed { get; }

        public ICommand OnSelectButtonPressed { get; }
        #endregion

        #region

        public UserProfileVM(int number,
            int id,
            bool maleFemale,
            string userName,
            DateTime enlistedDate,
            string rankName,
            bool confirmed = false)
        {
            Id = id;
            m_UserName = userName;
            m_enlistedDate = enlistedDate;
            m_Number = number;
            m_Confirmed = confirmed;
            m_RankName = rankName;
            m_MaleFemale = maleFemale;
            #region Init Commands
            OnConfirmButtonPressed = new Command(
                canExecute: CanOnConfirmedButtonPressedExecute,
                execute: OnConfirmButtonPressedExecute);

            OnSelectButtonPressed = new Command(
                canExecute: CanOnSelectButtonPressedExecute,
                execute: OnSelectButtonPressedExecute);
            #endregion
        }

        #endregion

        #region Methods

        private void OnUserProfileConfirmed(UserProfileVM user)
        {
            var temp = Volatile.Read(ref OnUserProfileConfirmedEvent);

            temp?.Invoke(user);
        }

        private void OnUserProfileSelected(UserProfileVM user)
        {
            var temp = Volatile.Read(ref OnUserProfileSelectedEvent);

            temp?.Invoke(user);
        }

        public override bool Equals(object? obj)
        {
            var user = obj as UserProfileVM;

            if (user is null) return false;

            return user.Id.Equals(this.Id);
        }

        public bool Equals(UserProfileVM? other)
        {
            if (other == null) return false;

            return other.Id.Equals(this.Id);
        }

        #region On Confirmed Button Pressed Execute

        private bool CanOnConfirmedButtonPressedExecute(object p) => true;

        private void OnConfirmButtonPressedExecute(object p)
        {
            Confirmed = true;
            EnlistedDate = DateTime.UtcNow;
            OnUserProfileConfirmed(this);
        }

        #endregion

        #region On Select Button Pressed Execute

        private bool CanOnSelectButtonPressedExecute(object p) => true;

        private void OnSelectButtonPressedExecute(object p)
        {
            OnUserProfileSelected(this);
        }
        #endregion

        #endregion
    }
}