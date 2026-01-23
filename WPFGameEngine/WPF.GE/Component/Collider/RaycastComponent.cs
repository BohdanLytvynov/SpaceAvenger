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
        private static Vector2 CENTER = new Vector2(0.5f, 0.5f);
        #region Properties
        /// <summary>
        /// Here I changed the logic of the default ActualCenterPosition calculation
        /// Ray must come from the center of the Raycastable object, So CENTER.X and CENTER.Y are set to 0.5f
        /// The Position - position of the left upper corner of the Raycastable and it is set externaly
        /// </summary>
        public override Vector2 ActualCenterPosition
        {
            get
            {
                var actX = CENTER.X * ActualObjectSize.Width;
                var actY = CENTER.Y * ActualObjectSize.Height;
                var locX = Basis.X * actX;
                var locY = Basis.Y * actY;
                return (locX + locY) + Position;
            }
        }

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
            return new RaycastComponent();
        }

        public override DtoBase ToDto()
        {
            return new RaycastDto();
        }

        public void Update()
        {
            PreviousPosition = CurrentPosition;
            CurrentPosition = ActualCenterPosition;
        }

        #endregion
    }
}
