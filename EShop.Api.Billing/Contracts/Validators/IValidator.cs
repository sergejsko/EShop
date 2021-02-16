namespace EShop.Api.Billing.Contracts.Validators
{
    /// <summary>
    /// The main validator.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Validates the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Validate(T data);
    }
}
