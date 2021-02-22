using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Models;
using System.Collections.Specialized;

namespace EShop.Api.Billing.Builders
{
    public class PaymentOrderBuilder : IPaymentOrderBuilder
    {
        /// <summary>
        /// Builds the specified payment order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>
        /// The payment.
        /// </returns>
        public Payment Build(Order order, NameValueCollection metadata)
        {
            return new Payment()
            {
                Amount = order.Amount.ToString(),
                Description = order.Description,
                Metadata = metadata,
                OrderNumber = order.OrderNumber
            };
        }
    }
}
