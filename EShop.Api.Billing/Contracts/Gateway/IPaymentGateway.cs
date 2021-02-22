using EShop.Api.Billing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Gateway
{
    public interface IPaymentGateway
    {
        /// <summary>
        /// Makes the payment asynchronous.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The payment response.</returns>
        Task<PaymentRespose> MakePaymentAsync(Payment payment, CancellationToken cancellationToken = default(CancellationToken));
    }
}
