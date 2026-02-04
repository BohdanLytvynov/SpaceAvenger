namespace SpaceAvenger.Services
{
    public static class Constants
    {
        #region Command Text

        public const string START_GAME_COMMAND = "start";
        public const string STOP_GAME_COMMAND = "stop";
        public const string PAUSE_GAME_COMMAND = "pause";
        public const string RESUME_GAME_COMMAND = "resume";

        #endregion

        #region Pathes
        public const string PATH_TO_CONTENT = "pack://application:,,,/SpaceAvenger;component/Resources/Content.xaml";
        #endregion

        #region Roles
        public const string PLAYER = "Player";
        public const string ENEMY = "Enemy";
        #endregion
    }
}
