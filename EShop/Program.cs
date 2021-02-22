using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EShop
{
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            var order = new
            {
                OrderNumber = "AA3333",
                UserId = 123456789,
                Amount = 10.10,
                Gateway = 1,
                Description = ""
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(order, Formatting.None), Encoding.UTF8, "application/json");
                using (var response = client.PostAsync(BILLING_API, contentPost))
                {
                    var result = response.Result.Content.ReadAsStringAsync();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var receipt = JsonConvert.DeserializeObject<Receipt>(result.Result);
                        Console.WriteLine($"ReceiptId: {receipt.ReceiptId}");
                    } 
                    else
                    {
                        Console.WriteLine(result.Result);
                    }
                }
            }
        }
        private const string BILLING_API = "http://localhost:50567/api/billing/order";
    }
}
