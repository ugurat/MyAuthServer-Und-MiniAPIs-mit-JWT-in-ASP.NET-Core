using AuthServer.API.Configuration;
using AuthServer.API.DATA;
using AuthServer.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthServer.API.Services
{
    public class TokenService : ITokenService
    {


        private readonly UserManager<UserApp> _userManager;
        // EINTRAGEN using AuthServer.DATA;
        // EINTRAGEN using Microsoft.AspNetCore.Identity;

        private readonly CustomTokenOption _tokenOption; // EINTRAGEN using AuthServer.Configuration;

        private readonly KeyService _keyService;


        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> options, KeyService keyService)
        {
            // EINTRAGEN using Microsoft.Extensions.Options;

            _userManager = userManager;
            _tokenOption = options.Value;
            _keyService = keyService; // KeyService, Private ve Public Key erişimi için kullanılır

        }


        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create(); // EINTRAGEN using System.Security.Cryptography;

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        //VORHER: private async IEnumerable<Claim> GetClaims(UserApp userApp, List<String> audiences)
        private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<String> audiences)
        {
            // EINTRAGEN using System.Security.Claims;


            var userRoles = await _userManager.GetRolesAsync(userApp);

            var userList = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier, userApp.Id),

                // --- EMAIL in TOKEN ---
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email),

                new Claim(ClaimTypes.Name, userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // --- ORT in TOKEN ---
                new Claim("city", userApp.City ?? string.Empty), // EINTRAGEN City Eintrag in Claim für Claim-Based Autorizastion

                // --- GEBURTSDATUM in TOKEN ---
                new Claim("birth-date", userApp.BirthDate?.ToString("yyyy-MM-dd") ?? string.Empty) // EINTRAGEN BirthDate Eintrag in Claim für Claim-Based Autorizastion
                // "birth-date" NICHT zusammenschreiben! Schlüsselwort "birthdate" ist ClaimTypes.DateOfBirth

            };

            // Benzersiz audience değerleri ekleniyor
            foreach (var audience in audiences.Distinct())
            {
                userList.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
            }

            userList.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return userList;


            /*
            var userRoles = await _userManager.GetRolesAsync(userApp);
            // ["admin","manager"]

            var userList = new List<Claim> {

                new Claim(ClaimTypes.NameIdentifier,userApp.Id),

                // --- EMAIL in TOKEN ---
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email), 
                // EINTRAGEN using System.IdentityModel.Tokens.Jwt;

                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),


                // --- ORT in TOKEN ---
                new Claim("city", userApp.City ?? string.Empty), // EINTRAGEN City Eintrag in Claim für Claim-Based Autorizastion

                // --- GEBURTSDATUM in TOKEN ---
                new Claim("birth-date", userApp.BirthDate?.ToString("yyyy-MM-dd") ?? string.Empty) // EINTRAGEN BirthDate Eintrag in Claim für Claim-Based Autorizastion
                // "birth-date" NICHT zusammenschreiben! Schlüsselwort "birthdate" ist ClaimTypes.DateOfBirth

            };

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            userList.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return userList;
            */

        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();

            claims.AddRange(
                client.Audiences.Select(
                  x => new Claim(JwtRegisteredClaimNames.Aud, x)
                )
            );

            // JWT ID und Subject Claims hinzufügen
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()));

            return claims;
        }

        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);

            // Private Key'i yükle
            var privateKey = _keyService.GetPrivateKey();

            // Private Key ile imzalama
            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(privateKey),
                SecurityAlgorithms.RsaSha256
            );


            // JWT Token oluştur
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(userApp, _tokenOption.Audience).Result,
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }


        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);

            // Private Key'i yükle
            var privateKey = _keyService.GetPrivateKey();

            // Private Key ile imzalama
            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(privateKey),
                SecurityAlgorithms.RsaSha256
            );

            // JWT Token oluştur
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
            };

            return tokenDto;
        }


        /*
        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);

            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaimsByClient(client),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,

                AccessTokenExpiration = accessTokenExpiration,
            };

            return tokenDto;
        }
        */


    }
}
