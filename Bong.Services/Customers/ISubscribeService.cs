using System;
using Bong.Core.Collections;
using Bong.Core.Domain.Customers;

namespace Bong.Services.Customers
{
    public partial interface ISubscribeService
    {
        void InsertSubscription(Subscription subscription);

        void UpdateSubscription(Subscription subscription);

        void DeleteSubscription(Subscription subscription);

        Subscription GetSubscriptionById(int subscriptionId);

        Subscription GetSubscriptionByGuid(Guid subscriptionGuid);

        Subscription GetSubscriptionByEmail(string email);

        IPagedList<Subscription> GetAllSubscriptions(string email = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}
