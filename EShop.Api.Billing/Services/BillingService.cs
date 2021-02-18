using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Models;
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
        /// <param name="orderService">The order service.</param>
        public BillingService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The order info.</returns>
        public async Task<OrderInfo> ProcessOrderAsync(Order order)
        {
            var result = await _orderService.ProcessOrderAsync(order);
            return result;
        }

        private readonly IOrderService _orderService;
    }
}
