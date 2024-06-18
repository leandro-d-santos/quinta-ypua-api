using Application.Reservations.Services;
using Application.Reservations.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Reservations.Controllers
{
    [Route("/api/reservations")]
    public sealed class ReservationsController : ControllerBase
    {

        private readonly IReservationService reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ReservationViewModel reservationViewModel)
        {
            return Ok(await reservationService.CreateAsync(reservationViewModel));
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> FindAllAsync()
        {
            return Ok(await reservationService.FindAllAsync());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindByIdAsync([FromRoute] Guid id)
        {
            return Ok(await reservationService.FindByIdAsync(id));
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ReservationViewModel reservationViewModel)
        {
            return Ok(await reservationService.UpdateAsync(reservationViewModel));
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromRoute] Guid id)
        {
            return Ok(await reservationService.RemoveAsync(id));
        }
    }
}
