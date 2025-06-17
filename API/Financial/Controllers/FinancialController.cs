using Application.Financial.Services;
using Application.Financial.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Financial.Controllers
{
    [Route("/api/financial")]
    public sealed class FinancialController : ControllerBase
    {

        private readonly IFinancialService financeiroService;

        public FinancialController(IFinancialService financeiroService)
        {
            this.financeiroService = financeiroService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> FindAllAsync()
        {
            return Ok(await financeiroService.FindAllAsync());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindByIdAsync([FromRoute] Guid id)
        {
            return Ok(await financeiroService.FindByIdAsync(id));
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] FinancialViewModel financeiroViewModel)
        {
            return Ok(await financeiroService.UpdateAsync(financeiroViewModel));
        }

        [Route("{id}/checkout")]
        [HttpPut]
        public async Task<IActionResult> CheckOutAsync([FromRoute] Guid id)
        {
            return Ok(await financeiroService.CheckOutAsync(id));
        }
    }
}
