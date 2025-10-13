namespace WPFGameEngine.WPF.GE.Component.Base
{
    public class ComponentBase : IComponent
    {
        public string Name { get; init; }

        public ComponentBase(string name)
        {
            Name = name;
        }
    }
}
