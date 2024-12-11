using AuthServer.API.Configuration;
using AuthServer.API.DATA;
using AuthServer.API.DTOs;

namespace AuthServer.API.Services
{
    public interface ITokenService
    {

        TokenDto CreateToken(UserApp userApp);

        ClientTokenDto CreateTokenByClient(Client client);


    }
}
