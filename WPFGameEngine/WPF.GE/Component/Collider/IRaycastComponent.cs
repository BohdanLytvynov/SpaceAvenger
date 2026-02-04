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
        /// <summary>
        /// Called every Update cycle. Used to set current and previous position for Ray construction
        /// </summary>
        /// <param name="currentPosition"></param>
        void Update(Vector2 currentPosition);
        /// <summary>
        /// Should be called before we get the bject from the pool. current and prev position were set to the current objects position 
        /// They will become equal. 
        /// </summary>
        /// <param name="position"></param>
        void ResetPosition(Vector2 position);
    }
}
