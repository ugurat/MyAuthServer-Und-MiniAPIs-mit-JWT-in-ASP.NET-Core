using AuthServer.API.DTOs;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{


    [Route("api/[controller]/[action]")] // Legt die Route des Controllers fest
    [ApiController] // Markiert die Klasse als API-Controller
    public class AuthController : CustomBaseController // Erbt von einer benutzerdefinierten Basisklasse für API-Antworten
    {


        private readonly IAuthenticationService _authenticationService; // Schnittstelle für Authentifizierungsdienste

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService; // Dependency Injection für den Authentifizierungsdienst

        }



        //api/auth/
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            // EINTRAGEN using AuthServer.DTOs;
            // Erstellt ein JWT-Token basierend auf den Benutzeranmeldedaten.

            var result = await _authenticationService.CreateTokenAsync(loginDto); // Token erstellen

            return ActionResultInstance(result);
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            // Erstellt ein Token für den Client basierend auf dessen Anmeldedaten.

            var result = _authenticationService.CreateTokenByClient(clientLoginDto); // Client-Token erstellen

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            // Widerruft ein Refresh-Token, um es ungültig zu machen.

            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token); // Refresh-Token widerrufen

            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            // Erstellt ein neues Token basierend auf einem gültigen Refresh-Token.

            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token); // Neues Token aus Refresh-Token erstellen

            return ActionResultInstance(result);
        }





    }


}
