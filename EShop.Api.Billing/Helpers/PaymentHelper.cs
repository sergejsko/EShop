namespace EShop.Api.Billing.Helpers
{
    public static class PaymentHelper
    {
        /// <summary>
        /// Converts to coins.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Amount.</returns>
        public static int ConvertToCoins(double amount)
        {
            return (int)(double)(amount * 100);
        }
    }
}
