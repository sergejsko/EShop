using System.Collections.Specialized;

namespace EShop.Api.Billing.Models
{
    /// <summary>
    /// The payment model.
    /// </summary>
    public class Payment
    {
        public string OrderNumber { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public NameValueCollection Metadata { get; set; }
    }
}
