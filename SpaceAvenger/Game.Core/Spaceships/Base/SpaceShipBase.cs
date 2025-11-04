using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Spaceships.Base
{
    public abstract class SpaceShipBase : MapableObject
    {
        protected TransformComponent transform;
        protected Sprite sprite;
        protected Animator animator;


        protected SpaceShipBase(string name) : base(name)
        {
            
        }

        public override void StartUp()
        {
            transform = GetComponent<TransformComponent>();
            sprite = GetComponent<Sprite>();
        }
    }
}
