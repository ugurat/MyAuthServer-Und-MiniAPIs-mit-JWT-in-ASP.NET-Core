using AuthServer.API.DATA;
using AuthServer.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Services
{
    public class UserService : IUserService
    {


        private readonly UserManager<UserApp> _userManager;
        // EINTRAGEN using Microsoft.AspNetCore.Identity;
        // EINTRAGEN using AuthServer.DATA;

        private readonly RoleManager<IdentityRole> _roleManager;


        public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            // EINTRAGEN using AuthServer.DTOs;


            var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }


            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<NoContentResult>> CreateUserRoles(string userName)
        {
            // EINTRAGEN using Microsoft.AspNetCore.Mvc;



            if (!await _roleManager.RoleExistsAsync("admin")) // Wenn Admin-Rolle nicht vorhanden ist:
            {
                // Hier werden ALLE Rollen eingetragen
                await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
                await _roleManager.CreateAsync(new IdentityRole { Name = "manager" });
            }

            // Hier werden die Rollen dem Benutzer zugeordnet
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.AddToRoleAsync(user, "admin");
            await _userManager.AddToRoleAsync(user, "manager");

            // EINTRAGEN  using Microsoft.AspNetCore.Http;
            return Response<NoContentResult>.Success(StatusCodes.Status201Created);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName not found", 404, true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }




    }
}
