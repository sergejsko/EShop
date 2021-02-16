using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Models;
using System;

namespace EShop.Api.Billing.Validators
{
    /// <summary>
    /// The order validator.
    /// </summary>
    /// <seealso cref="EShop.Api.Billing.Contracts.Validators.IOrderValidator" />
    public class OrderValidator : IOrderValidator
    {
        /// <summary>
        /// Validates the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.ArgumentException">
        /// Amount
        /// or
        /// UserId
        /// or
        /// OrderNumber
        /// </exception>
        public void Validate(Order data)
        {
            if (data.Amount <= 0)
            {
                throw new ArgumentException(nameof(data.Amount));
            }

            if (data.UserId <= 0)
            {
                throw new ArgumentException(nameof(data.UserId));
            }

            if (string.IsNullOrWhiteSpace(data.OrderNumber))
            {
                throw new ArgumentException(nameof(data.OrderNumber));
            }
        }
    }
}
