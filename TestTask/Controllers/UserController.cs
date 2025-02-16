using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet] //Чтение всех
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = users.Select(u => new UserResponse
            {
                UserId = u.UserId,
                Login = u.Login,
                Password = u.Password,
                CreatedDate = u.CreatedDate,
                UserGroup = u.UserGroup.Code, // Code из UserGroup
                UserState = u.UserState.Code  // Code из UserState
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")] //Чтение по id
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
                    UserGroupId = user.UserGroupId, // UserGroup
                    UserStateId = user.UserStateId // UserState
                };
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")] //Редактирование
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