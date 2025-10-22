namespace WPFGameEngine.WPF.GE.Component.Base
{
    public class ComponentBase : IGEComponent
    {
        public string Name { get; init; }

        public ComponentBase(string name)
        {
            Name = name;
        }
    }
}
