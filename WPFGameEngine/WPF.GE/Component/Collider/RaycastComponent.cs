using System.Numerics;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Collider.Base;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    [VisibleInEditor(FactoryName = nameof(RaycastComponent),
        DisplayName = "Raycast Component",
        GameObjectType = Enums.GEObjectType.Component)]
    public class RaycastComponent : ColliderComponentBase, IRaycastComponent
    {
        #region Properties
        public Vector2 PreviousPosition { get; set; }
        public Vector2 CurrentPosition { get; set; }
        #endregion

        #region Ctor
        public RaycastComponent() : base(nameof(RaycastComponent))
        {
        }

        public override List<string> IncompatibleComponents => new List<string>()
        { nameof(RaycastComponent), nameof(ColliderComponent) };

        #endregion

        #region Methods
        public override object Clone()
        {
            return new RaycastComponent() { Position = Position };
        }

        public override DtoBase ToDto()
        {
            return new RaycastDto() { Position = Position };
        }

        public void Update(Vector2 currentPosition)
        {
            PreviousPosition = CurrentPosition;
            CurrentPosition = currentPosition;
        }

        public void ResetPosition(Vector2 position)
        {
            PreviousPosition = position;
            CurrentPosition = position;
        }

        #endregion
    }
}
