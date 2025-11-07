using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Spaceships.Base
{
    public abstract class SpaceShipBase : MapableObject
    {
        protected TransformComponent m_transform;
        protected Sprite m_sprite;
        protected Animator m_animator;

        #region Fields
        protected float m_horSpeed;
        protected float m_vertSpeed;
        #endregion

        protected SpaceShipBase(string name) : base(name)
        {
            m_horSpeed = 40;
            m_vertSpeed = 40;
        }

        public override void StartUp()
        {
            m_transform = GetComponent<TransformComponent>();
            m_sprite = GetComponent<Sprite>();
        }
    }
}
