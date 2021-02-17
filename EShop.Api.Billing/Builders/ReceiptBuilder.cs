using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Models;
using System;

namespace EShop.Api.Billing.Builders
{
    /// <summary>
    /// The receipt builder.
    /// </summary>
    /// <seealso cref="EShop.Api.Billing.Contracts.Builders.IReceiptBuilder" />
    public class ReceiptBuilder : IReceiptBuilder
    {
        /// <summary>
        /// Builds the specified data.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>
        /// The receipt.
        /// </returns>
        public Receipt Build(Order order)
        {
            return new Receipt
            {
                ReceiptId = Guid.NewGuid().ToString(),
                OrderNumber = order.OrderNumber
            };
        }
    }
}
