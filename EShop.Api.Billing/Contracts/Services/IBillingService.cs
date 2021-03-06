﻿using EShop.Api.Billing.Models;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Contracts.Services
{
    public interface IBillingService
    {
        /// <summary>
        /// Processes the order asynchronous.
        /// </summary>
        /// <param name="data">The order.</param>
        /// <returns>Process order info.</returns>
        Task<OrderInfo> ProcessOrderAsync(Order order);
    }
}
