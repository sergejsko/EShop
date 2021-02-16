using EShop.Api.Billing.Models;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Services
{
    public interface IBillingService
    {
        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Process order data.</returns>
        Task<(Receipt receipt, bool IsSuccess, string ErrorMessage)> ProcessOrderAsync(Order data);
    }
}
