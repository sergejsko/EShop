using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Factory;
using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Models;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Services
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="paymentFactory">The payment factory.</param>
        /// <param name="receiptBuilder">The receipt builder.</param>
        /// <param name="paymentOrderBuilder">The payment order builder.</param>
        /// <param name="orderValidator">The order validator.</param>
        public OrderService(
            IPaymentFactory paymentFactory,
            IReceiptBuilder receiptBuilder,
            IPaymentOrderBuilder paymentOrderBuilder,
            IOrderValidator orderValidator)
        {
            _paymentFactory = paymentFactory;
            _receiptBuilder = receiptBuilder;
            _paymentOrderBuilder = paymentOrderBuilder;
            _orderValidator = orderValidator;
        }

        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The order info.</returns>
        public async Task<OrderInfo> ProcessOrderAsync(Order order)
        {
            try
            {
                _orderValidator.Validate(order);

                var result = await MakeBillingOrderAsync(order);

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
        /// <param name="success">if set to <c>true</c> [is success].</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The order info.</returns>
        private OrderInfo CreateOrderInfo(Receipt receipt, bool success, string errorMessage) => new OrderInfo(receipt, success, errorMessage);

        // <summary>
        /// Send billing order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The payment response.</returns>
        private async Task<PaymentRespose> MakeBillingOrderAsync(Order order)
        {
            var metadata = new NameValueCollection();

            switch (order.Gateway)
            {
                case PaymentGatewayType.FirstData:
                    AddMetadataParameter(metadata, "type", "MakeFirstData");
                    break;
                case PaymentGatewayType.PayPal:
                    AddMetadataParameter(metadata, "type", "MakePayPal");
                    break;
                default:
                    throw new NotSupportedException("Not implemented payment gateway.");
            }

            var gateway = _paymentFactory.CreatePaymentGateway(order.Gateway);
            var payment = _paymentOrderBuilder.Build(order, metadata);
            var response = await gateway.MakePaymentAsync(payment);

            return response;
        }

        /// <summary>
        /// Adds the metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        private void AddMetadataParameter(NameValueCollection metadata, string name, string value) => metadata.Add(name, value);

        private readonly IPaymentFactory _paymentFactory;
        private readonly IReceiptBuilder _receiptBuilder;
        private readonly IPaymentOrderBuilder _paymentOrderBuilder;
        private readonly IOrderValidator _orderValidator;
    }
}
