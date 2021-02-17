using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Providers;
using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Models;
using System;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Services
{
    /// <summary>
    /// The billing service.
    /// </summary>
    /// <seealso cref="EShop.Api.Billing.Contracts.Services.IBillingService" />
    public class BillingService : IBillingService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BillingService"/> class.
        /// </summary>
        /// <param name="paymentOrderProvider">The payment order provider.</param>
        /// <param name="receiptBuilder">The receipt builder.</param>
        /// <param name="paymentOrderBuilder">The payment order builder.</param>
        /// <param name="orderValidator">The order validator.</param>
        public BillingService(IPaymentOrderProvider paymentOrderProvider, IReceiptBuilder receiptBuilder, IPaymentOrderBuilder paymentOrderBuilder, IOrderValidator orderValidator)
        {
            _paymentOrderProvider = paymentOrderProvider;
            _receiptBuilder = receiptBuilder;
            _paymentOrderBuilder = paymentOrderBuilder;
            _orderValidator = orderValidator;
        }

        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The order data.</returns>
        public async Task<(Receipt receipt, bool IsSuccess, string ErrorMessage)> ProcessOrderAsync(Order order)
        {
            try
            {
                _orderValidator.Validate(order);

                var paymentOrder = _paymentOrderBuilder.Build(order);
                var result = await _paymentOrderProvider.CreateOrderTransactionAsync(paymentOrder);

                if (result.Status == "Success" && result.Code == "000")
                {
                    var receipt = _receiptBuilder.Build(order);
                    return (receipt, true, null);
                }

                return (null, false, result.Status);
            }
            catch (Exception ex)
            {
                return (null, false, ex.Message);
            }
        }

        private readonly IPaymentOrderProvider _paymentOrderProvider;
        private readonly IReceiptBuilder _receiptBuilder;
        private readonly IPaymentOrderBuilder _paymentOrderBuilder;
        private readonly IOrderValidator _orderValidator;
    }
}
