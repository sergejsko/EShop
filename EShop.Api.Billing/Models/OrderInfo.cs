namespace EShop.Api.Billing.Models
{
    /// <summary>
    /// The order info model.
    /// </summary>
    public class OrderInfo
    {
        public Receipt Receipt { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        public OrderInfo(Receipt receipt, bool success, string errorMessage)
        {
            Receipt = receipt;
            IsSuccess = success;
            ErrorMessage = errorMessage;
        }
    }
}
