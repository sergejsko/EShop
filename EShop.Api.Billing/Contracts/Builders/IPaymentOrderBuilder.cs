using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Contracts.Builders
{
    public interface IPaymentOrderBuilder
    {
        /// <summary>
        /// Builds the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The payment order.</returns>
        PaymentOrder Build(Order order);
    }
}
