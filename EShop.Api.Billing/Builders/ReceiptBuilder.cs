using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Models;

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
        /// <param name="data">The data.</param>
        /// <returns>
        /// The receipt.
        /// </returns>
        public Receipt Build(Order data)
        {
            return new Receipt
            {
                ReceiptId = "AA1111",
                OrderNumber = data.OrderNumber
            };
        }
    }
}
