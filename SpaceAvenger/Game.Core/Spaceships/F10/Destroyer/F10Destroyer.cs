using SpaceAvenger.Game.Core.Spaceships.F10.Engines;
using System.Drawing;
using System.Numerics;
using WPFGameEngine.Atributes;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Spaceships.F10.Destroyer
{
    public class F10Destroyer : GameObject
    {
        #region Fields
        F10Jet mainL;
        Animator mainL_Animator;
        #endregion

        #region Ctor
        public F10Destroyer(string name) : base(name)
        {
            var sprite = new Sprite();
            sprite.Load(App.Current.Resources, "F10_Destr");
            RegisterComponent(sprite);
            mainL = new F10Jet("Main1Jet") ;
            mainL_Animator = mainL.GetComponent<Animator>();
            StartMainEngines();
            AddChild(mainL);
        }
        #endregion

        #region Methods
        private void StartMainEngines()
        {
            mainL_Animator.SetAnimationForPlay("Normal");
            mainL_Animator.Start();
        }
        #endregion
    }
}
