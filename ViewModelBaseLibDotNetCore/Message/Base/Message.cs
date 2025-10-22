namespace ViewModelBaseLibDotNetCore.Message.Base
{
    public abstract class Message<T> : IMessage<T>
    {
        private T m_content;

        public T Content
        { 
            get => m_content;
        }

        public Message(T content)
        {
            m_content = content;
        }

        public override string ToString()
        {
            return $"{m_content}";
        }
    }
}
