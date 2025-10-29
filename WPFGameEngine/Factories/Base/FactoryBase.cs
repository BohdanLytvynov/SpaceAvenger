namespace WPFGameEngine.Factories.Base
{
    public abstract class FactoryBase<TProduct> : IFactory<TProduct>
    {
        public string ProductName { get; init; }

        protected FactoryBase()
        {
            ProductName = typeof(TProduct).Name;
        }

        public abstract TProduct Create();
    }
}
