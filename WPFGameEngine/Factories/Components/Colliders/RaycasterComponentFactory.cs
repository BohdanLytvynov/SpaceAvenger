using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.Collider;

namespace WPFGameEngine.Factories.Components.Colliders
{
    public class RaycasterComponentFactory : FactoryBase, IRaycasterComponentFactory
    {
        public RaycasterComponentFactory()
        {
            ProductName = nameof(RaycastComponent);
        }

        public override IGameEngineEntity Create()
        {
            return new RaycastComponent();
        }        
    }
}
