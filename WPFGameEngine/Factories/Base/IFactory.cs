namespace WPFGameEngine.Factories.Base
{
    public interface IFactory<TProduct>
    {
        string ProductName { get; }

        TProduct Create();
    }
}
