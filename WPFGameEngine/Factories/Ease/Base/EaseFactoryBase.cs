using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.Factories.Ease.Base
{
    public class EaseFactoryBase<TEase> : FactoryBase, IEaseFactory<TEase>
        where TEase : IEase
    {
        public EaseFactoryBase()
        {
            ProductName = typeof(TEase).Name;
        }

        public override IEase Create()
        {
            return (EaseBase)Activator.CreateInstance(typeof(TEase));
        }
    }
}
