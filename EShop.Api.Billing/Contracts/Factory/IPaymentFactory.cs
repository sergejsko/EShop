using EShop.Api.Billing.Contracts.Gateway;
using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Contracts.Factory
{
    public interface IPaymentFactory
    {

        /// <summary>
        /// Creates the payment gateway.
        /// </summary>
        /// <param name="paymentGateway">The payment gateway.</param>
        /// <returns>The payment gateway.</returns>
        IPaymentGateway CreatePaymentGateway(PaymentGatewayType paymentGateway);
    }
}
