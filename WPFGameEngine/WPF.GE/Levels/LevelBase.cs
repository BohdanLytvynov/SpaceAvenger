using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.Levels
{
    public struct LevelStatistics
    {
        public int EnemyCount { get; set; }
        public int ShipsDestroyed { get; set; }
        public bool Win { get => EnemyCount == ShipsDestroyed; }
        public bool IsAlive { get; set; }
    }

    public abstract class LevelBase : UpdatableBase, ILevel
    {
        public int EnemyCount { get; set; }
        public int CurrentEnemyCount { get; protected set; }
        public int ShipsDestroyed { get; protected set; }
        public IControllerComponent? ControllerComponent { get; set; }

        public event Action<LevelStatistics> OnGameFinished;

        public abstract LevelStatistics GetCurrentLevelStatistics();

        protected void OnLevelFinished(LevelStatistics statistics)
        { 
            OnGameFinished?.Invoke(statistics);
        }
    }
}
