using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Helpers;
using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Builders
{
    /// <summary>
    /// The payment order builder.
    /// </summary>
    /// <seealso cref="EShop.Api.Billing.Contracts.Builders.IPaymentOrderBuilder" />
    public class PaymentOrderBuilder : IPaymentOrderBuilder
    {
        /// <summary>
        /// Builds the specified data.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>
        /// Payment order.
        /// </returns>
        public PaymentOrder Build(Order order)
        {
            return new PaymentOrder
            {
                OrderNumber = order.OrderNumber,
                Amount = PaymentHelper.ConvertToCoins(order.Amount).ToString(),
                Gateway = order.Gateway,
                Description = order.Description
            };
        }
    }
}
