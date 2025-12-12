using SpaceAvenger.Game.Core.Factions.F10.Destroyer;
using SpaceAvenger.Game.Core.Factions.Neutrals;
using System;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;
using WPFGameEngine.WPF.GE.Levels;

namespace SpaceAvenger.Game.Core.Levels
{
    public class SurvivalLevel : LevelBase
    {
        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);

            //Init Player Here
            var w = App.Current.MainWindow;

            IMapableObjectViewHost mapView = GameView as IMapableObjectViewHost;

            if(mapView == null) return;
            //Set up Player
            mapView.Instantiate<F10Destroyer>(
                c => {
                    if (ControllerComponent == null)
                        throw new ArgumentNullException(nameof(ControllerComponent));

                    c.RegisterComponent(ControllerComponent);
                },
                c => {
                    if (c is ITransformable t)
                    {
                        //Calculate Player Position
                        //Horizontal - must be the center of the window
                        t.Rotate(-90);
                        t.Scale(new WPFGameEngine.WPF.GE.Math.Sizes.Size(0.7f, 0.7f));

                        var wScale = t.GetWorldScale();

                        float x = (float)(w.Width / 2) - (wScale.Width / 2);
                        //Vertical - half of the screen
                        float y = (float)(w.Height / 2);
                        t.Translate(new System.Numerics.Vector2(x, y));
                    }
                }
                );

            mapView.Instantiate<AstroBase>();
        }

        public override void Update()
        {
            
        }
    }
}
