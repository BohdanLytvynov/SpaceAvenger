using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.Factories.Ease.Base
{
    public interface IEaseFactory<EaseType> : IFactory
        where EaseType : IEase
    {
    }
}
