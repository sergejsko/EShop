using EShop.Api.Billing.Contracts.Gateway;
using EShop.Api.Billing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Tests.Mocks.Gateways
{
    public class PayPalMock : IPaymentGateway
    {
        public PayPalMock(PaymentRespose response)
        {
            _response = response;
        }

        public async Task<PaymentRespose> MakePaymentAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            var result = await Task<PaymentRespose>.Factory.StartNew(() => _response, cancellationToken);
            return result;
        }

        private readonly PaymentRespose _response;
    }
}