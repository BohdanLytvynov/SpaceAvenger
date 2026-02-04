using SpaceAvenger.Game.Core.Levels;
using SpaceAvenger.Services.Realizations.Message;
using System.Windows;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using c = SpaceAvenger.Services.Constants;

namespace SpaceAvenger.Views.DialogWindow
{
    /// <summary>
    /// Interaction logic for EscDialog.xaml
    /// </summary>
    public partial class EscDialog : Window
    {
        IMessageBus m_messageBus;
        public EscDialog(IMessageBus messageBus)
        {
            m_messageBus = messageBus;
            InitializeComponent();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            m_messageBus.Send<GameMessage, string>
                (new GameMessage(c.RESUME_GAME_COMMAND));
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            m_messageBus.Send<GameMessage, string>
                (new GameMessage(c.STOP_GAME_COMMAND));
            this.Close();
        }
    }
}
