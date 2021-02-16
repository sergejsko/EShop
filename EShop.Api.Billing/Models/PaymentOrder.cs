namespace EShop.Api.Billing.Models
{
    /// <summary>
    /// The payment order model.
    /// </summary>
    public class PaymentOrder
    {
        public string OrderNumber { get; set; }
        public string Amount { get; set; }
        public PaymentGatewayType Gateway { get; set; }
        public string Description { get; set; }
    }
}
