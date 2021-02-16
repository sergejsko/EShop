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
        /// <param name="data">The data.</param>
        /// <returns>
        /// Payment order.
        /// </returns>
        public PaymentOrder Build(Order data)
        {
            return new PaymentOrder
            {
                OrderNumber = data.OrderNumber,
                Amount = PaymentHelper.ConvertToCoins(data.Amount).ToString(),
                Gateway = data.Gateway,
                Description = data.Description
            };
        }
    }
}
