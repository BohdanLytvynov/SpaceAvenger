using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Collider.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    public interface IRaycastComponent : IColliderComponentBase
    {
        /// <summary>
        /// Previous object position
        /// </summary>
        Vector2 PreviousPosition { get; set; }
        /// <summary>
        /// Current Calculated Object Position
        /// </summary>
        Vector2 CurrentPosition { get; set; }

        void Update();
    }
}
