using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F1.Corvettes;
using SpaceAvenger.Game.Core.Factions.F10.Destroyer;
using SpaceAvenger.Services;
using System;
using System.Numerics;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;
using WPFGameEngine.WPF.GE.Levels;

namespace SpaceAvenger.Game.Core.Levels
{
    public class SurvivalLevel : LevelBase
    {
        SpaceShipBase m_curr;

        SpaceShipBase m_player;

        public override LevelStatistics GetCurrentLevelStatistics()
        {
            return new LevelStatistics() 
            {
                EnemyCount = EnemyCount,
                ShipsDestroyed = ShipsDestroyed,
                IsAlive = m_player.IsAlive,
            };
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
            CurrentEnemyCount = EnemyCount;
            //Init Player Here
            var w = App.Current.MainWindow;

            IMapableObjectViewHost mapView = GameView as IMapableObjectViewHost;

            if (mapView == null) return;
            //Set up Player
            m_player = mapView.Instantiate<F10Destroyer>(
                c =>
                {

                    c.Metadata.Add(Constants.PLAYER);
                    (c as SpaceShipBase).ProjectileCollisionLayer = CollisionLayer.PlayerProjectile;
                    if (ControllerComponent == null)
                        throw new ArgumentNullException(nameof(ControllerComponent));

                    c.RegisterComponent(ControllerComponent);
                    if (c is ITransformable t)
                    {
                        t.Scale(new WPFGameEngine.WPF.GE.Math.Sizes.Size(0.7f, 0.7f));
                    }
                },
                c =>
                {
                    if (c is ITransformable t)
                    {
                        //Calculate Player Position
                        //Horizontal - must be the center of the window
                        t.Rotate(-90);

                        var wScale = t.GetWorldScale();

                        float x = (float)(w.ActualWidth / 2) - (wScale.Width / 2);
                        //Vertical - half of the screen
                        float y = (float)(w.ActualHeight / 2);
                        t.Translate(new System.Numerics.Vector2(x, y));
                        var ship = c as SpaceShipBase;
                        if (ship != null)
                        {
                            ship.UseCaching = false;
                        }
                    }
                }
                );

            m_player.CollisionLayer = CollisionLayer.Player;
        }

        public override void Update()
        {
            bool isAlive = m_player.IsAlive;
            //Player was killed
            if (m_player.IsDestroyed)
            {
                OnLevelFinished(new LevelStatistics()
                {
                    ShipsDestroyed = ShipsDestroyed,
                    EnemyCount = EnemyCount,
                    IsAlive = isAlive
                });
                return;
            }

            if (m_curr?.IsDestroyed ?? false)
            {
                ShipsDestroyed++;
            }

            //Win
            if (EnemyCount == ShipsDestroyed && m_player.IsAlive
                && m_curr.IsDestroyed)
            {
                OnLevelFinished(new LevelStatistics()
                {
                    ShipsDestroyed = ShipsDestroyed,
                    EnemyCount = EnemyCount,
                    IsAlive = isAlive
                });
                return;
            }

            if (m_curr?.IsDestroyed ?? true && CurrentEnemyCount > 0 && m_player.IsAlive)
            {
                IMapableObjectViewHost mapView = GameView as IMapableObjectViewHost;

                m_curr = mapView.Instantiate<F1Corvette>(c =>
                {
                    c.Metadata.Add(Constants.ENEMY);
                    (c as SpaceShipBase).ProjectileCollisionLayer = CollisionLayer.EnemyProjectile;
                    if (c is ITransformable t)
                    {
                        t.Scale(new WPFGameEngine.WPF.GE.Math.Sizes.Size(0.7f, 0.7f));
                    }
                });
                m_curr.CollisionLayer = CollisionLayer.Enemy;

                m_curr.Rotate(90);

                CurrentEnemyCount--;
            }
        }
    }
}
