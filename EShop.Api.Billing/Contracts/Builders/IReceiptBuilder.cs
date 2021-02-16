using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Contracts.Builders
{
    public interface IReceiptBuilder
    {
        /// <summary>
        /// Builds the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The receipt.</returns>
        Receipt Build(Order data);
    }
}
