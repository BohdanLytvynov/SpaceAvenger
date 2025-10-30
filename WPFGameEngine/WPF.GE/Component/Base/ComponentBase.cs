namespace WPFGameEngine.WPF.GE.Component.Base
{
    public class ComponentBase : IGEComponent
    {
        public string ComponentName { get; init; }

        public ComponentBase(string name)
        {
            ComponentName = name;
        }
    }
}
