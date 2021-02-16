using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Contracts.Builders
{
    public interface IPaymentOrderBuilder
    {
        /// <summary>
        /// Builds the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Payment order.</returns>
        PaymentOrder Build(Order data);
    }
}
