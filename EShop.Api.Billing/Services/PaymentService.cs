using EShop.Api.Billing.Constants;
using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Models;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Services
{
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// Make payment asynchronous.
        /// </summary>
        /// <param name="paymentGateway">The payment gateway.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<PaymentRespose> MakePaymentAsync(PaymentGatewayType paymentGateway, NameValueCollection query)
        {
            var urlGateway = string.Empty;

            switch (paymentGateway)
            {
                case PaymentGatewayType.FirstData:
                    urlGateway = PaymentGatewayApi.FIRST_DATA_GATEWAY;
                    break;
                case PaymentGatewayType.PayPal:
                    urlGateway = PaymentGatewayApi.PAY_PAL_GATEWAY;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await ExecuteRequestAsync(urlGateway, query);
        }

        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="urlGateway">The URL gateway.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The payment response.</returns>
        private async Task<PaymentRespose> ExecuteRequestAsync(string urlGateway, NameValueCollection query, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await Task<PaymentRespose>.Factory.StartNew(() => new PaymentRespose { Result = "OK", Status = "Success", Code = "000" }, cancellationToken);
            return result;
        }
    }
}
