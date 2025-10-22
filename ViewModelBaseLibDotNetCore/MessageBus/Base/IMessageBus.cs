using ViewModelBaseLibDotNetCore.Message.Base;

namespace ViewModelBaseLibDotNetCore.MessageBus.Base
{
    public interface IMessageBus
    {
        IDisposable RegisterHandler<T, U>(Action<T> handler)
            where T : IMessage<U>;
            
        public ReaderWriterLockSlim Lock { get; }

        public Dictionary<string, IEnumerable<WeakReference>> Subscriptions { get; }

        void Send<T, U>(T message)
            where T : IMessage<U>;
            
    }
}
