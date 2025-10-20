using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.Base
{
    internal abstract class ComponentViewModel : ValidationViewModel
    {
        #region Fields
        private string m_name;
        #endregion

        #region Properties
        public string Name 
        {
            get=> m_name;
            set=> Set(ref m_name, value);
        }

        public IGameObject GameObject { get; set; }
        #endregion

        #region Ctor
        public ComponentViewModel(string name, IGameObject gameObject)
        {
            m_name = name;
            GameObject = gameObject;
        }

        #endregion

        #region Methods

        #endregion
    }
}
