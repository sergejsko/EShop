using EShop.Api.Billing.Models;

namespace EShop.Api.Billing.Contracts.Validators
{
    /// <summary>
    /// The order validator.
    /// </summary>
    /// <seealso cref="EShop.Api.Billing.Contracts.Validators.IValidator{EShop.Api.Billing.Models.Order}" />
    public interface IOrderValidator : IValidator<Order>
    {

    }
}
