using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F1.Corvettes;
using SpaceAvenger.Game.Core.Factions.F10.Destroyer;
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

                    c.Metadata.Add("Player");

                    if (ControllerComponent == null)
                        throw new ArgumentNullException(nameof(ControllerComponent));

                    c.RegisterComponent(ControllerComponent);
                    if (c is ITransformable t)
                    {
                        t.Scale(new WPFGameEngine.WPF.GE.Math.Sizes.Size(0.7f, 0.7f));
                    }
                },
                c => {
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
                    }
                }
                );

            mapView.Instantiate<F1Corvette>(c => 
            {
                //(c as SpaceShipBase).ConfigureAI(new AI.SpaceShipControlModule());
                if (c is ITransformable t)
                {
                    t.Scale(new WPFGameEngine.WPF.GE.Math.Sizes.Size(0.7f, 0.7f));
                }
            });
        }

        public override void Update()
        {
            
        }
    }
}
