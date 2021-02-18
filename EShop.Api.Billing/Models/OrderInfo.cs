namespace EShop.Api.Billing.Models
{
    public class OrderInfo
    {
        public Receipt Receipt { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }
}
