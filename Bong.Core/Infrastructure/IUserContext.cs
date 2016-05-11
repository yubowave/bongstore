using Bong.Core.Domain.Customers;

namespace Bong.Core.Infrastructure
{
    public interface IUserContext
    {
        Customer CurrentUser { get; set; }
    }
}
