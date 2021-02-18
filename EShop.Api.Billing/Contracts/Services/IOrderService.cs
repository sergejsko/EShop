using EShop.Api.Billing.Models;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>The order info.</returns>
        Task<OrderInfo> ProcessOrderAsync(Order order);
    }
}
