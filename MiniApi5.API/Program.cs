
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiniApi5.API.Services;
using System.Security.Cryptography;

namespace MiniApi5.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            var configuration = builder.Configuration;

            // Konfiguration hinzufügen
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // PublicKeyService zum DI-Container hinzufügen
            builder.Services.AddSingleton<PublicKeyService>();

            // PublicKeyService aus dem DI-Container abrufen
            var publicKeyService = builder.Services.BuildServiceProvider().GetRequiredService<PublicKeyService>();

            // Public Key laden oder vom AuthServer abrufen
            var publicKey = Task.Run(() => publicKeyService.GetOrFetchPublicKeyAsync()).Result;

            // RSA-Objekt erstellen und den Public Key laden
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey.ToCharArray());

            // RsaSecurityKey erstellen
            var rsaSecurityKey = new RsaSecurityKey(rsa);



            // Konfiguration für JWT-Authentifizierung
            builder.Services.AddAuthentication(options =>
            {
                // EINTRAGEN: Standard-Authentifizierungs- und Herausforderungs-Schema auf JWT setzen
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Aktiviert die Validierung des Issuers. (AUSSTELLER)
                    ValidateIssuer = true,

                    // Aktiviert die Validierung der Audience. (Zielgruppe)
                    ValidateAudience = true,

                    // Aktiviert die Validierung der Lebenszeit des Tokens.
                    ValidateLifetime = true,


                    ValidateIssuerSigningKey = true,

                    // Verwendet den angegebenen Sicherheitskey, um das Token zu validieren.
                    IssuerSigningKey = rsaSecurityKey,

                    // Überprüft, ob der Token-Issuer (AUSSTELLER) mit dem angegebenen Issuer übereinstimmt.
                    ValidIssuer = configuration["TokenOption:Issuer"],

                    // Überprüft, ob die Audience (Zielgruppe) im Token mit den erlaubten Audiences übereinstimmt.
                    ValidAudiences = configuration.GetSection("TokenOption:Audience").Get<string[]>()

                };
            });



            // Dienste zum Container hinzufügen
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // HTTP-Anfrage-Pipeline konfigurieren
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthentication(); // EINTRAGEN Authentifizierung aktivieren

            app.UseAuthorization(); // Autorisierung aktivieren


            app.MapControllers();

            app.Run();
        }
    }
}
