namespace ViewModelBaseLibDotNetCore.Message.Base
{
    public interface IMessage<T>
    {
        public T Content { get; }
    }
}
