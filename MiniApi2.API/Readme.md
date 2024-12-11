
# MiniApi2.API

## Inhaltsverzeichnis
- [MiniApi2.API](#miniapi2api)
  - [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Einführung](#einführung)
  - [Zugriff für Manager- oder Admin-Rolle (User-Token)](#zugriff-für-manager--oder-admin-rolle-user-token)
    - [Anforderungen:](#anforderungen)
    - [Beispiel für eine HTTP-Anfrage:](#beispiel-für-eine-http-anfrage)
    - [Beispiel-Antwort:](#beispiel-antwort)
    - [Hinweise:](#hinweise)
  - [Nuget Pakete installieren](#nuget-pakete-installieren)
  - [Properties / launchSettings.json](#properties--launchsettingsjson)
  - [appsettings.json](#appsettingsjson)
  - [Services / PublicKeyService.cs](#services--publickeyservicecs)
  - [Program.cs](#programcs)
  - [Controllers / ManagerController.cs](#controllers--managercontrollercs)

## Einführung
MiniApi2.API ist ein sicheres Web-API-Projekt, das JWT-basierte Authentifizierung verwendet. Diese API bietet geschützte Endpunkte, auf die nur Benutzer mit Administrator-Rolle zugreifen können.

Hauptfunktionen:
- JWT-basierte Authentifizierung
- Rollenbasierte Autorisierung (Admin-Rolle)
- RSA-signierte Token-Validierung
- Swagger API-Dokumentation
- Entity Framework Core Datenbankintegration

Diese API arbeitet mit JWT-Tokens, die vom AuthServer.API bereitgestellt werden, und bezieht den öffentlichen Schlüssel vom AuthServer. Für die Token-Validierung wird RSA asymmetrische Verschlüsselung verwendet.

## Zugriff für Manager- oder Admin-Rolle (User-Token)

Die API bietet eine abgesicherte Schnittstelle, auf die nur authentifizierte Benutzer mit einem gültigen JWT-Token zugreifen können. Der Endpunkt für den Zugriff lautet:

**URL:**  
`http://localhost:5020/api/Manager`

Auf die URL können nur von Benutzer mit der Manager-Rolle (Admin-Rolle) zugreifen.

### Anforderungen:
- **Authentifizierung:** Der Zugriff erfordert einen gültigen JWT-Token, der im `Authorization`-Header mit dem Präfix `Bearer` übermittelt wird.
- **Endpunktbeschreibung:** Die Anfrage liefert Benutzerinformationen wie Benutzername und Benutzer-ID, die aus dem Token extrahiert werden.

### Beispiel für eine HTTP-Anfrage:
```http
GET /api/Device HTTP/1.1
Host: localhost:5020
Authorization: Bearer <Ihr_JWT_Token>
```

### Beispiel-Antwort:
```json
{
  "message": "Manager => UserName: JohnDoe; UserId: 12345; Roles: manager, admin"
}
```

### Hinweise:
- Stellen Sie sicher, dass der JWT-Token gültig ist und die konfigurierten Anforderungen (Issuer, Audience, Signaturschlüssel) erfüllt.
- Dieser Endpunkt dient zur Verifizierung und Abruf von grundlegenden Benutzerinformationen aus dem Token.




----

## Nuget Pakete installieren

```` bash

Install-Package AutoMapper -Version 10.1.1
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 6.0.36
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 6.0.36
Install-Package Microsoft.EntityFrameworkCore -Version 6.0.36
Install-Package Microsoft.EntityFrameworkCore.Design -Version 6.0.36
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.36
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 6.0.36
Install-Package Swashbuckle.AspNetCore -Version 7.0.0
Install-Package System.IdentityModel.Tokens.Jwt -Version 8.2.1

````

Die aufgelisteten NuGet-Pakete werden installiert, um verschiedene Funktionalitäten in einem .NET-Projekt zu integrieren. `AutoMapper` erleichtert die Objektzuordnung zwischen unterschiedlichen Klassen. `Microsoft.AspNetCore.Authentication.JwtBearer` ermöglicht die JWT-Authentifizierung in einer ASP.NET Core-Anwendung. `Microsoft.AspNetCore.Identity.EntityFrameworkCore` unterstützt die Benutzerverwaltung und Authentifizierung mit Entity Framework Core. Die `Microsoft.EntityFrameworkCore`-Pakete (inklusive `Design`, `SqlServer` und `Tools`) bieten Unterstützung für die Datenbankinteraktion, insbesondere mit SQL Server, und ermöglichen das Arbeiten mit Migrations- und Design-Tools. `Swashbuckle.AspNetCore` integriert Swagger für API-Dokumentation, und `System.IdentityModel.Tokens.Jwt` wird für die Erstellung und Validierung von JWT-Token verwendet. Diese Pakete zusammen bilden die Grundlage für eine Anwendung mit Authentifizierung, Datenbankzugriff und API-Dokumentation.


----

## Properties / launchSettings.json

```` bash
...

  "profiles": {
    "MiniApi2.API": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5020",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },

...
````

Die Datei `launchSettings.json` definiert die Startkonfiguration der Anwendung `MiniApi2.API` während der Entwicklungsphase. Die Anwendung wird mit dem Kommando `dotnet run` gestartet, wobei ausführliche Ausgaben (`dotnetRunMessages`) aktiviert sind. Beim Start öffnet sich automatisch ein Browser mit der URL `http://localhost:5020/swagger`, die zur API-Dokumentation führt. Die Basis-URL der Anwendung wird über `applicationUrl` auf `http://localhost:5020` gesetzt, und die Umgebung wird mit der Umgebungsvariablen `ASPNETCORE_ENVIRONMENT` auf `Development` festgelegt, was für Debugging und Entwicklerfunktionen genutzt wird.

----

## appsettings.json

```` bash

  "PublicKeySettings": {
    "PublicKeyUrl": "http://localhost:5000/api/publickey"
  },

  "TokenOption": {
    "Audience": [ "www.miniapi2.com" ],
    "Issuer": "www.authserver.com"
  },

````

Die Datei `appsettings.json` enthält Konfigurationsparameter für die Token-Validierung in einer Anwendung. Unter dem Abschnitt `TokenOption` wird die zulässige Zielgruppe (`Audience`) definiert, hier eine Liste mit der URL `www.miniapi2.com`. Der Aussteller des Tokens (`Issuer`) wird mit `www.authserver.com` angegeben. Zudem wird die Lebensdauer des Access Tokens in Minuten durch `AccessTokenExpiration` festgelegt, hier auf 5 Minuten. Diese Einstellungen werden verwendet, um die Gültigkeit und Vertrauenswürdigkeit von JWT-Token in der Anwendung zu prüfen.


----

## Services / PublicKeyService.cs

```` csharp

namespace MiniApi2.API.Services
{
    public class PublicKeyService
    {



        private readonly IConfiguration _configuration;

        private const string PublicKeyPath = "Keys/public.key"; // Speicherpfad für den Public Key

        public PublicKeyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOrFetchPublicKeyAsync()
        {

            // URL aus der appsettings.json auslesen - PublicKey-Endpunkt auf dem AuthServer
            var publicKeyUrl = _configuration["PublicKeySettings:PublicKeyUrl"];
            if (string.IsNullOrEmpty(publicKeyUrl))
            {
                throw new Exception("Die PublicKey-URL ist in der Konfiguration nicht definiert!");
            }

            // Wenn die Datei existiert, lese den Inhalt und gib ihn zurück
            if (File.Exists(PublicKeyPath))
            {
                return await File.ReadAllTextAsync(PublicKeyPath);
            }

            // Wenn die Datei nicht existiert, hole den Public Key vom AuthServer
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(publicKeyUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fehler beim Abrufen des Public Keys!");
            }

            var publicKey = await response.Content.ReadAsStringAsync();

            // Speichere den abgerufenen Public Key in der Datei
            Directory.CreateDirectory(Path.GetDirectoryName(PublicKeyPath)!); // Erstelle den Ordner "Keys", falls er nicht existiert
            await File.WriteAllTextAsync(PublicKeyPath, publicKey);

            return publicKey;
        }


    }
}

```` 

Der `PublicKeyService` stellt eine Methode bereit, um einen Public Key entweder aus einer lokalen Datei oder von einem konfigurierten AuthServer abzurufen. Die URL des Public Key-Endpunkts wird aus der `appsettings.json` ausgelesen. Wenn der Public Key bereits lokal unter einem definierten Pfad gespeichert ist, wird er aus der Datei geladen und zurückgegeben. Falls nicht, wird der Key von der angegebenen URL mit einer HTTP-Anfrage abgerufen. Nach erfolgreichem Abruf wird der Key lokal gespeichert, wobei der Zielordner bei Bedarf automatisch erstellt wird. Der Service stellt sicher, dass der Public Key verfügbar ist, indem er bei Bedarf die Datei neu erstellt oder den Key aus dem Server aktualisiert.

----

## Program.cs

```` csharp

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiniApi2.API.Services;
using System.Security.Cryptography;

namespace MiniApi2.API
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




            // JWT-Authentifizierungs-Konfiguration
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
                    IssuerSigningKey = rsaSecurityKey, // Doğru kullanımı burada

                    // Überprüft, ob der Token-Issuer (AUSSTELLER) mit dem angegebenen Issuer übereinstimmt.
                    ValidIssuer = configuration["TokenOption:Issuer"],

                    // Überprüft, ob die Audience (Zielgruppe) im Token mit den erlaubten Audiences übereinstimmt.
                    ValidAudiences = configuration.GetSection("TokenOption:Audience").Get<string[]>()

                };
            });






            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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

````

Der `Program.cs`-Code initialisiert eine ASP.NET Core-Webanwendung, die JWT-Authentifizierung verwendet. Dabei wird ein `PublicKeyService` registriert, um einen Public Key entweder lokal oder vom AuthServer abzurufen. Dieser Public Key wird anschließend in ein RSA-Objekt importiert und als `RsaSecurityKey` für die JWT-Token-Validierung verwendet. Die Authentifizierung wird so konfiguriert, dass die Tokens auf ihre Gültigkeit, den Aussteller (Issuer) und die Zielgruppe (Audience) geprüft werden. Weiterhin werden grundlegende Dienste wie Controller, Swagger für API-Dokumentation und Endpunkt-Explorer registriert. In der HTTP-Pipeline werden Swagger (nur in der Entwicklungsumgebung), Authentifizierung und Autorisierung aktiviert, und die Controller-Endpunkte werden mit `MapControllers` eingebunden. Abschließend wird die Anwendung gestartet.

----


## Controllers / ManagerController.cs

```` csharp

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {


        // http://localhost:5020/api/Manager

        //[Authorize]
        [Authorize(Roles = "admin")]
        //[Authorize(Policy = "AgePolicy")]
        //[Authorize(Roles = "admin", Policy = "CityPolicy")]
        [HttpGet]
        public IActionResult GetInfo()
        {

            // Benutzernamen auslesen
            var userName = User.Identity?.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value ?? string.Empty;

            // Rolle(n) auslesen und verkettet darstellen
            var userRoles = string.Join(", ", User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value));

            //return Ok(new { UserName = userName });
            //return Ok($" Community => UserName: {userName}; UserId: {userId}");
            return Ok($"Manager => UserName: {userName}; UserId: {userId}; Roles: {userRoles}");

        }

    }
}

````


