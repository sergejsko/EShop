﻿using EShop.Api.Billing.Constants;
using EShop.Api.Billing.Contracts.Providers;
using EShop.Api.Billing.Models;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EShop.Api.Billing.Providers
{
    public class PaymentProvider : IPaymentProvider
    {
        /// <summary>
        /// Gets the order payment information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public async Task<PaymentResult> GetOrderPaymentInfoAsync(PaymentOrder data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            var query = HttpUtility.ParseQueryString(string.Empty);

            AddQueryParameter(query, "order_number", data.OrderNumber);
            AddQueryParameter(query, "amount", data.Amount);
            AddQueryParameter(query, "description", data.Description);

            var request = await ExecuteRequestAsync(data.Gateway, query);

            var paymentResult = new PaymentResult()
            {
                Code = request.Code,
                Status = request.Status
            };

            return paymentResult;
        }

        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="paymentGateway">The payment gateway.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private async Task<PaymentRespose> ExecuteRequestAsync(PaymentGatewayType paymentGateway, NameValueCollection query)
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
        /// Adds the query parameter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        private void AddQueryParameter(NameValueCollection query, string name, string value)
        {
            query.Add(name, value);
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
