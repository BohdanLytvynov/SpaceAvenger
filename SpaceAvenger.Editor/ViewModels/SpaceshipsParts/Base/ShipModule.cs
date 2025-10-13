using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Sprites;

namespace SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base
{
    internal class ShipModule : ViewModelBase
    {
        private ObservableCollection<string> m_resourceNames;

        public ObservableCollection<string> ResourceNames 
        { get => m_resourceNames; set => Set(ref m_resourceNames, value); }

        public int Id { get; set; }

        public Sprite Sprite { get; set; }

        public ShipModule(ObservableCollection<string> resNames)
        {
            if(resNames == null)
                throw new ArgumentNullException(nameof(resNames));

            m_resourceNames = new ObservableCollection<string>();

            foreach (string name in resNames)
            { 
                ResourceNames.Add(name);
            }
        }
    }
}
