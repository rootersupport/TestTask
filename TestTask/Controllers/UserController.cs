using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")] // Только для администраторов
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = users.Select(u => new UserResponse
            {
                UserId = u.UserId,
                Login = u.Login,
                Password = u.Password,
                CreatedDate = u.CreatedDate,
                UserGroup = u.UserGroup.Code,
                UserState = u.UserState.Code
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")] //Чтение по id
        [Authorize] // Доступно всем
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Такого id не существует");
            }

            var response = new UserResponse
            {
                UserId = user.UserId,
                Login = user.Login,
                Password = user.Password,
                CreatedDate = user.CreatedDate,
                UserGroup = user.UserGroup.Code, // Code из UserGroup
                UserState = user.UserState.Code  // Code из UserState
            };

            return Ok(response);
        }

        [HttpPost] //Добавление
        [Authorize(Policy = "AdminOnly")] // Только для администраторов
        public async Task<ActionResult<UserResponse>> AddUser([FromBody] UserCreateRequest request)
        {
            var user = new User
            {
                Login = request.Login,
                Password = request.Password,
                UserGroupId = request.UserGroupId,
                UserStateId = request.UserStateId,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                await _userService.AddUserAsync(user);
                var response = new UserCreateResponse
                {
                    UserId = user.UserId,
                    Login = user.Login,
                    Password = user.Password,
                    CreatedDate = user.CreatedDate,
                    UserGroupId = user.UserGroupId,
                    UserStateId = user.UserStateId
                };
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")] //Редактирование
        [Authorize(Policy = "AdminOnly")] // Только для администраторов
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Такого id не существует");
            }

            user.Login = request.Login;
            user.Password = request.Password;
            user.UserGroupId = request.UserGroupId;
            user.UserStateId = request.UserStateId;

            try
            {
                await _userService.UpdateUserAsync(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")] //Удаление (блок)
        [Authorize(Policy = "AdminOnly")] // Только для администраторов
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Такого id не существует");
            }

            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}