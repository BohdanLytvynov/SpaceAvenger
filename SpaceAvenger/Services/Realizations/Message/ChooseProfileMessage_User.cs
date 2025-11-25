using Models.DAL.Entities.User;
using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Services.Realizations.Message
{
    internal class ChooseProfileMessage_User : Message<User>
    {
        public ChooseProfileMessage_User(User user) : base(user)
        {
                
        }
    }
}
