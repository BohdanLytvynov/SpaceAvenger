﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibDotNetCore.VM;

namespace SpaceAvenger.ViewModels.Base
{
    internal class SubscriptableViewModel : ViewModelBase
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

        #endregion




    }
}
