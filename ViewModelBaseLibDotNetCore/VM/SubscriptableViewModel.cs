using ViewModelBaseLibDotNetCore.VM;

namespace ViewModelBaseLibDotNetCore.VM
{
    public class SubscriptableViewModel : ViewModelBase, IDisposable
    {
        #region Fields

        private List<IDisposable> m_subscriptions;

        protected List<IDisposable> Subscriptions { get => m_subscriptions; }

        #endregion

        #region Ctor

        public SubscriptableViewModel()
        {
            m_subscriptions = new List<IDisposable>();
        }

        #endregion

        #region Methods
        
        protected virtual void Unsubscribe()
        { 
            foreach (var subscription in m_subscriptions) 
                subscription.Dispose();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        #endregion
    }
}
