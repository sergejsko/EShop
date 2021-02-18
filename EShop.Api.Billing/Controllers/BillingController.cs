using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Controllers
{
    [Route("api/billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        public BillingController(IBillingService billingService) => _billingService = billingService;

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> ProcessOrderAsync(Order data)
        {
            var result = await _billingService.ProcessOrderAsync(data);
            if (result.IsSuccess)
            {
                return Ok(result.Receipt);
            }
            return NotFound(result.ErrorMessage);
        }

        private readonly IBillingService _billingService;
    }
}
