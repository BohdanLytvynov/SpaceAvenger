using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;

namespace SpaceAvenger.Editor.ViewModels.Components.Collider
{
    internal class RaycastComponentViewModel : ComponentViewModel
    {
        public RaycastComponentViewModel(IGameObjectMock gameObject) : 
            base(nameof(RaycastComponent), gameObject)
        {
        }

        public override IGEComponent? GetComponent()
        {
            return GameObject.GetComponent<RaycastComponent>(false);
        }

        protected override void LoadCurrentGameObjProperties()
        {
        }
    }
}
