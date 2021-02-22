namespace EShop.Api.Billing.Models
{
    /// <summary>
    /// The payment gateway response model.
    /// </summary>
    public class PaymentRespose
    {
        public string Status { get; set; }
        public string Result { get; set; }
        public string Code { get; set; }
    }
}
