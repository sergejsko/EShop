using EShop.Api.Billing.Contracts.Gateway;
using EShop.Api.Billing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Gateways
{
    public class PayPal : IPaymentGateway
    {
        /// <summary>
        /// Makes the payment asynchronous.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The payment response.
        /// </returns>
        public async Task<PaymentRespose> MakePaymentAsync(Payment payment, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await Task<PaymentRespose>.Factory.StartNew(() => new PaymentRespose { Result = "OK", Status = "Success", Code = "000" }, cancellationToken);
            return result;
        }
    }
}
