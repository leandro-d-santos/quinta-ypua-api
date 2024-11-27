using Application.Guests.Services;
using Application.Guests.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Guests.Controllers
{
    [Route("/api/guests")]
    public sealed class GuestsController : ControllerBase
    {

        private readonly IGuestService guestService;

        public GuestsController(IGuestService guestService)
        {
            this.guestService = guestService;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] GuestViewModel guestViewModel)
        {
            return Ok(await guestService.CreateAsync(guestViewModel));
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> FindAllAsync()
        {
            return Ok(await guestService.FindAllAsync());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindByIdAsync([FromRoute] Guid id)
        {
            return Ok(await guestService.FindByIdAsync(id));
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] GuestViewModel guestViewModel)
        {
            return Ok(await guestService.UpdateAsync(guestViewModel));
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromRoute] Guid id)
        {
            return Ok(await guestService.RemoveAsync(id));
        }
    }
}
