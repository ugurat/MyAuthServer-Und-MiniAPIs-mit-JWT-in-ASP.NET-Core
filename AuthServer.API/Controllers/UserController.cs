using AuthServer.API.DTOs;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController // <-- EINTRAGEN VERERBUNG
    {


        private readonly IUserService _userService; // EINTRAGEN using AuthServer.Services;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        //api/user
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            // EINTRAGEN using AuthServer.DTOs;


            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }


        [Authorize] // EINTRAGEN using Microsoft.AspNetCore.Authorization;
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }


        [HttpPost("CreateUserRoles/{userName}")]
        public async Task<IActionResult> CreateUserRoles(string userName)
        {
            // Hier werden ALLE Rollen eingetragen
            return ActionResultInstance(await _userService.CreateUserRoles(userName));
        }



    }
}
