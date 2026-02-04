using SpaceAvenger.DAL.Models;
using SpaceAvenger.ViewModels.UserProfile;
using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Services.Realizations.Message
{
    internal class ChooseProfileMessage_User : Message<UserProfileVM>
    {
        public ChooseProfileMessage_User(UserProfileVM user) : base(user)
        {

        }
    }
}
