using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Contracts.Builders
{
    public interface IReceiptBuilder
    {
        /// <summary>
        /// Builds the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The receipt.</returns>
        Receipt Build(Order order);
    }
}
