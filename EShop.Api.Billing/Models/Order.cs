namespace EShop.Api.Billing.Models
{
    /// <summary>
    /// The order model.
    /// </summary>
    public class Order
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public double Amount { get; set; }
        public PaymentGatewayType Gateway { get; set; }
        public string Description { get; set; }
    }
}
