using AuthServer.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Services
{
    public interface IUserService
    {

        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);

        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);

        Task<Response<NoContentResult>> CreateUserRoles(string userName);


    }
}
