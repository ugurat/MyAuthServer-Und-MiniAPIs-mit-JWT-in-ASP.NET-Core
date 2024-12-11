using AuthServer.API.Configuration;
using AuthServer.API.DATA;
using AuthServer.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthServer.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {



        private readonly List<Client> _clients; // EINTRAGEN using AuthServer.Configuration;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService; // EINTRAGEN using AuthServer.Repositories;


        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = optionsClient.Value;

            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            }
            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync(); // EINTRAGEN using Microsoft.EntityFrameworkCore;

            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommmitAsync();

            return Response<TokenDto>.Success(token, 200);
        }


        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            // Client doğrulama işlemi
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                // Hata mesajı ve durum kodu döndürülüyor
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }

            // Client için yeni bir Token oluşturuluyor
            var token = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(token, 200);
        }


        /*
        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }

            var token = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(token, 200);
        }
        */

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404, true);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);

            // ORGINAL:
            //existRefreshToken.Code = tokenDto.RefreshToken;
            //existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            // KORREKTUR:
            // Logik:
            // Wenn das aktuelle Refresh Token noch gültig ist,
            // wird es nicht erneuert.
            // Ein Access Token wird nur mit einem gültigen
            // Refresh Token erstellt.
            // Auf diese Weise wird das Refresh Token
            // nur erneuert, wenn seine Gültigkeitsdauer abläuft.
            if (existRefreshToken.Expiration < DateTime.UtcNow)
            {
                // Die Gültigkeitsdauer des Refresh Tokens ist abgelaufen, ein neues Token wird erstellt und die Ablaufzeit wird aktualisiert
                existRefreshToken.Code = tokenDto.RefreshToken;
                existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
            }
            else
            {
                // Das Refresh Token ist noch gültig, ein neues Access Token wird erstellt, aber die Ablaufzeit des Refresh Tokens bleibt unverändert
                tokenDto.RefreshToken = existRefreshToken.Code; // Das alte Token wird weiterverwendet
                tokenDto.RefreshTokenExpiration = existRefreshToken.Expiration; // Die alte Ablaufzeit wird weiterverwendet
            }



            await _unitOfWork.CommmitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
            }

            _userRefreshTokenService.Remove(existRefreshToken);

            await _unitOfWork.CommmitAsync();

            return Response<NoDataDto>.Success(200);
        }




    }
}
