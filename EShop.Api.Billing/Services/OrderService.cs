using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Models;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace EShop.Api.Billing.Services
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="paymentService">The payment service.</param>
        /// <param name="receiptBuilder">The receipt builder.</param>
        /// <param name="paymentOrderBuilder">The payment order builder.</param>
        /// <param name="orderValidator">The order validator.</param>
        public OrderService(
            IPaymentService paymentService,
            IReceiptBuilder receiptBuilder,
            IPaymentOrderBuilder paymentOrderBuilder,
            IOrderValidator orderValidator)
        {
            _paymentService = paymentService;
            _receiptBuilder = receiptBuilder;
            _paymentOrderBuilder = paymentOrderBuilder;
            _orderValidator = orderValidator;
        }

        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The order data.</returns>
        public async Task<OrderInfo> ProcessOrderAsync(Order order)
        {
            try
            {
                _orderValidator.Validate(order);

                var paymentOrder = _paymentOrderBuilder.Build(order);
                var result = await MakePaymentTransactionAsync(paymentOrder);

                if (result.Result == "OK" && result.Code == "000")
                {
                    var receipt = _receiptBuilder.Build(order);
                    return CreateOrderInfo(receipt, true, null);
                }

                return CreateOrderInfo(null, false, result.Status);
            }
            catch (Exception ex)
            {
                return CreateOrderInfo(null, false, ex.Message);
            }
        }

        /// <summary>
        /// Creates the order information.
        /// </summary>
        /// <param name="receipt">The receipt.</param>
        /// <param name="isSucess">if set to <c>true</c> [is sucess].</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The order info.</returns>
        private OrderInfo CreateOrderInfo(Receipt receipt, bool isSucess, string errorMessage)
        {
            return new OrderInfo()
            {
                ErrorMessage = errorMessage,
                IsSuccess = isSucess,
                Receipt = receipt
            };
        }

        // <summary>
        /// Make payment transaction asynchronous.
        /// </summary>
        /// <param name="paymentOrder">The payment order.</param>
        /// <returns></returns>
        private async Task<PaymentRespose> MakePaymentTransactionAsync(PaymentOrder paymentOrder)
        {
            if (paymentOrder == null)
            {
                throw new ArgumentNullException();
            }

            var query = HttpUtility.ParseQueryString(string.Empty);

            AddQueryParameter(query, "order_number", paymentOrder.OrderNumber);
            AddQueryParameter(query, "amount", paymentOrder.Amount);
            AddQueryParameter(query, "description", paymentOrder.Description);

            var request = await _paymentService.MakePaymentAsync(paymentOrder.Gateway, query);

            return request;
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

        private readonly IPaymentService _paymentService;
        private readonly IReceiptBuilder _receiptBuilder;
        private readonly IPaymentOrderBuilder _paymentOrderBuilder;
        private readonly IOrderValidator _orderValidator;
    }
}
