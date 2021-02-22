using EShop.Api.Billing.Contracts.Factory;
using EShop.Api.Billing.Contracts.Gateway;
using EShop.Api.Billing.Gateways;
using EShop.Api.Billing.Models;
using System;

namespace EShop.Api.Billing.Factory
{
    public class PaymentFactory : IPaymentFactory
    {
        /// <summary>
        /// Creates the payment gateway.
        /// </summary>
        /// <param name="paymentGateway">The payment gateway.</param>
        /// <returns>
        /// The payment gateway.
        /// </returns>
        /// <exception cref="NotSupportedException">The payment gateway {paymentGateway} is not supported.</exception>
        public IPaymentGateway CreatePaymentGateway(PaymentGatewayType paymentGateway)
        {
            switch(paymentGateway)
            {
                case PaymentGatewayType.FirstData:
                    return new FirstData();
                case PaymentGatewayType.PayPal:
                    return new PayPal();
                default:
                    throw new NotSupportedException($"The payment gateway {paymentGateway} is not supported.");
            }
        }
    }
}
