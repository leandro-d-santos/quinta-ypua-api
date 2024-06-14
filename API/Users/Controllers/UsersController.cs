using Application.Users.Services;
using Application.Users.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Users.Controllers
{
    [Route("/api/users")]
    public sealed class UsersController : ControllerBase
    {

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserViewModel userViewModel)
        {
            return Ok(await userService.CreateAsync(userViewModel));
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> FindAllAsync()
        {
            return Ok(await userService.FindAllAsync());
        }

        [Route("{userName}")]
        [HttpGet]
        public async Task<IActionResult> FindByUserNameAsync([FromRoute] string userName)
        {
            return Ok(await userService.FindByUserNameAsync(userName));
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserViewModel userViewModel)
        {
            return Ok(await userService.UpdateAsync(userViewModel));
        }

        [Route("{userName}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromRoute] string userName)
        {
            return Ok(await userService.RemoveAsync(userName));
        }
    }
}
