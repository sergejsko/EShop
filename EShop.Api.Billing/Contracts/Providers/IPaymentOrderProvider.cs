using EShop.Api.Billing.Models;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Providers
{
    public interface IPaymentOrderProvider
    {
        /// <summary>
        /// Creates the order transaction asynchronous.
        /// </summary>
        /// <param name="paymentOrder">The payment order.</param>
        /// <returns>The payment result.</returns>
        Task<PaymentResult> CreateOrderTransactionAsync(PaymentOrder paymentOrder);
    }
}
