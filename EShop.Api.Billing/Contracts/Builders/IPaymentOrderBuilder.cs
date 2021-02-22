using EShop.Api.Billing.Models;
using System.Collections.Specialized;

namespace EShop.Api.Billing.Contracts.Builders
{
    public interface IPaymentOrderBuilder
    {
        /// <summary>
        /// Builds the specified payment order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The payment.</returns>
        Payment Build(Order order, NameValueCollection metadata);
    }
}
