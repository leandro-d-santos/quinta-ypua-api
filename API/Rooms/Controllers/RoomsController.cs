using Application.Rooms.Services;
using Application.Rooms.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rooms.Controllers
{
    [Route("/api/rooms")]
    public sealed class RoomsController : ControllerBase
    {

        private readonly IRoomService roomService;

        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoomViewModel roomViewModel)
        {
            return Ok(await roomService.CreateAsync(roomViewModel));
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> FindAllAsync()
        {
            return Ok(await roomService.FindAllAsync());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindByIdAsync([FromRoute] Guid id)
        {
            return Ok(await roomService.FindByIdAsync(id));
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] RoomViewModel roomViewModel)
        {
            return Ok(await roomService.UpdateAsync(roomViewModel));
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromRoute] Guid id)
        {
            return Ok(await roomService.RemoveAsync(id));
        }
    }
}
