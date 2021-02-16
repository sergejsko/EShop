using EShop.Api.Billing.Models;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Providers
{
    public interface IPaymentProvider
    {
        /// <summary>
        /// Gets the order payment information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task<PaymentResult> GetOrderPaymentInfoAsync(PaymentOrder data);
    }
}
