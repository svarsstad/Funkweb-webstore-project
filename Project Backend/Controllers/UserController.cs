using Microsoft.AspNetCore.Mvc;
using Project_Backend.Models;
using Project_Backend.Services;

namespace Project_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET ALL

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userService.GetAllUsersAsync();
        }

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var users = await _userService.GetAllUsersAsync();

            var user = users.FirstOrDefault(p => p.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _userService.CreateUserAsync(user);

            return Ok();
        }

        // UPDATE

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, User updatedUser)
        {
            updatedUser.Id = id;

            await _userService.UpdateUserAsync(id, updatedUser);

            return Ok();
        }

        // DELETE

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _userService.DeleteUserAsync(id);

            return Ok();
        }
    }
}
