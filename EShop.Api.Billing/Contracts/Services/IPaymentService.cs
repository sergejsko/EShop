using EShop.Api.Billing.Models;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Services
{
    public interface IPaymentService
    {
        /// <summary>
        /// Make payment asynchronous.
        /// </summary>
        /// <param name="paymentGateway">The payment gateway.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        Task<PaymentRespose> MakePaymentAsync(PaymentGatewayType paymentGateway, NameValueCollection query);
    }
}
