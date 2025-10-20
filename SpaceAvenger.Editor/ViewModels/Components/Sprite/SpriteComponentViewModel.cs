using SpaceAvenger.Editor.ViewModels.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Sprite
{
    internal class SpriteComponentViewModel : ComponentViewModel
    {
        #region Ctor
        public SpriteComponentViewModel(IGameObject gameObject) : base("Sprite: ", gameObject)
        {
            
        }
        #endregion
    }
}
