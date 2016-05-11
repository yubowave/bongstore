using System;
using System.Linq;

using Bong.Core.Helper;
using Bong.Core.Collections;
using Bong.Core.Data;
using Bong.Core.Domain.Customers;
using Bong.Data;

namespace Bong.Services.Customers
{
    public partial class SubscribeService : ISubscribeService
    {
        private readonly IRepository<Subscription> _subscriptionRepository;

        public SubscribeService(IRepository<Subscription> subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public virtual void InsertSubscription(Subscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException("Subscription");

            //Handle e-mail
            subscription.Email = CommonHelper.EnsureSubscriberEmail(subscription.Email);

            //Persist
            _subscriptionRepository.Insert(subscription);
        }

        public virtual void UpdateSubscription(Subscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException("Subscription");
            }

            //Handle e-mail
            subscription.Email = CommonHelper.EnsureSubscriberEmail(subscription.Email);
            _subscriptionRepository.Update(subscription);
        }

        public virtual void DeleteSubscription(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException("Subscription");

            _subscriptionRepository.Delete(subscription);
        }

        public virtual Subscription GetSubscriptionById(int subscriptionId)
        {
            if (subscriptionId == 0) return null;

            return _subscriptionRepository.GetById(subscriptionId);
        }

        public virtual Subscription GetSubscriptionByGuid(Guid subscriptionGuid)
        {
            if (subscriptionGuid == Guid.Empty) return null;

            var newsLetterSubscriptions = from nls in _subscriptionRepository.Table
                                          where nls.SubscriptionGuid == subscriptionGuid
                                          orderby nls.Id
                                          select nls;

            return newsLetterSubscriptions.FirstOrDefault();
        }

        public virtual Subscription GetSubscriptionByEmail(string email)
        {
            if (!CommonHelper.IsValidEmail(email))
                return null;

            email = email.Trim();
            var subscriptions = from nls in _subscriptionRepository.Table
                                          where nls.Email == email
                                          orderby nls.Id
                                          select nls;

            return subscriptions.FirstOrDefault();
        }

        public virtual IPagedList<Subscription> GetAllSubscriptions(string email = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _subscriptionRepository.Table;
            if (!String.IsNullOrEmpty(email))
            {
                query = query.Where(nls => nls.Email.Contains(email));
            }
            if (!showHidden)
            {
                query = query.Where(nls => nls.Active);
            }
            query = query.OrderBy(nls => nls.Email);

            var subscriptions = new PagedList<Subscription>(query, pageIndex, pageSize);
            return subscriptions;
        }
    }
}
