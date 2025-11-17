using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Animations.Explosions
{
    internal class Explosion1 : MapableObject
    {
        public IAnimation Animation { get; protected set; }

        public Explosion1() : base(nameof(Explosion1))
        {
        }

        public override void StartUp()
        {
            Animation = GetComponent<Animation>();
            base.StartUp();
        }
    }
}
