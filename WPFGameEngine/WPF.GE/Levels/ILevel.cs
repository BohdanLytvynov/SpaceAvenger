using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.GameObjects.Updatable;

namespace WPFGameEngine.WPF.GE.Levels
{
    public interface ILevel : IUpdatable
    {
        int EnemyCount { get; set; }
        int CurrentEnemyCount { get; }

        int ShipsDestroyed { get; }
        #region On Game finished
        event Action<LevelStatistics> OnGameFinished;
        #endregion

        IControllerComponent? ControllerComponent { get; set; }

        LevelStatistics GetCurrentLevelStatistics();
    }
}
