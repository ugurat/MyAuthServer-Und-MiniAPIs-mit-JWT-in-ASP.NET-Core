

# MiniApi6.API

## Inhaltsverzeichnis

- [MiniApi6.API](#miniapi6api)
  - [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Einführung](#einführung)
  - [Zugriff über User-Token und Client-Token](#zugriff-über-user-token-und-client-token)
  - [Nuget Pakete Installieren](#nuget-pakete-installieren)
  - [Properties / launchSettings.json](#properties--launchsettingsjson)
  - [appsettings.json](#appsettingsjson)
  - [Services / PublicKeyService.cs](#services--publickeyservicecs)
  - [Program.cs](#programcs)
  - [Controllers / InfoController.cs](#controllers--infocontrollercs)

----

## Einführung

Die MiniApi6.API ist eine moderne Web-API, die auf ASP.NET Core basiert und eine robuste Authentifizierungs- und Autorisierungsinfrastruktur bietet. Sie unterstützt sowohl Benutzer- als auch Client-Token und ist darauf ausgelegt, Entwicklern eine einfache Möglichkeit zu bieten, sichere und skalierbare Webanwendungen zu erstellen.

Kernfunktionen:
- Unterstützung für JWT-Authentifizierung
- Verwendung von Entity Framework Core für Datenbankzugriffe
- Integration von Swagger für API-Dokumentation
- Verteilung von Authentifizierungs- und Autorisierungsschemata für Benutzer- und Client-Token
- Asymmetrische Verschlüsselung mittels RSA für die Token-Validierung

Die API bezieht ihre Authentifizierungsinformationen von einem zentralen AuthServer, wobei JSON Web Tokens (JWT) verwendet werden, die von einem Authentifizierungsdienst innerhalb der Infrastruktur ausgestellt werden. Dies ermöglicht eine sichere und effiziente Verwaltung von Zugriffskontrollen und Berechtigungen für unterschiedliche Benutzerkategorien und Anwendungen.


[Nach oben](#inhaltsverzeichnis)

----    

## Zugriff über User-Token und Client-Token

Die API bietet zwei verschiedene Authentifizierungsmöglichkeiten:

1. **UserToken**: Für authentifizierte Benutzer, die auf geschützte Benutzerressourcen zugreifen möchten
2. **ClientToken**: Für authentifizierte Client-Anwendungen, die auf geschützte Client-Ressourcen zugreifen möchten

Beide Token-Arten sind als JWT (JSON Web Token) implementiert und müssen bei jeder Anfrage im Authorization-Header mitgesendet werden.

Die entsprechenden Endpunkte lauten:

- **Benutzer-Endpunkt:**  
 `http://localhost:5060/api/Info/User`


- Beispiel für eine HTTP-Anfrage:

```http
GET /api/Info/User HTTP/1.1
Host: localhost:5060
Authorization: Bearer <Ihr_JWT_Token>
```

- Beispiel-Antwort:

```json
{
  "message": "Community => UserInfo "
}
```


 - **Client-Endpunkt:**  
`http://localhost:5060/api/Info/Client`

- Beispiel für eine HTTP-Anfrage:

```http
GET /api/Info/Client HTTP/1.1
Host: localhost:5060
Authorization: Bearer <Ihr_JWT_Token>
```

- Beispiel-Antwort:

```json
{
  "message": "Community => ClientInfo "
}
```

[Nach oben](#inhaltsverzeichnis)

----

## Nuget Pakete Installieren

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

[Nach oben](#inhaltsverzeichnis)

----

## Properties / launchSettings.json

URL: http://localhost:5060

```` bash
...

  "profiles": {
    "MiniApi6.API": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5060",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },

...
````

Die Datei `launchSettings.json` definiert die Startkonfiguration der Anwendung `MiniApi6.API` während der Entwicklungsphase. Die Anwendung wird mit dem Kommando `dotnet run` gestartet, wobei ausführliche Ausgaben (`dotnetRunMessages`) aktiviert sind. Beim Start öffnet sich automatisch ein Browser mit der URL `http://localhost:5060/swagger`, die zur API-Dokumentation führt. Die Basis-URL der Anwendung wird über `applicationUrl` auf `http://localhost:5060` gesetzt, und die Umgebung wird mit der Umgebungsvariablen `ASPNETCORE_ENVIRONMENT` auf `Development` festgelegt, was für Debugging und Entwicklerfunktionen genutzt wird.

[Nach oben](#inhaltsverzeichnis)

----

## appsettings.json

```` bash

  "UserToken": {
    "Audience": [ "www.miniapi6.com" ],
    "Issuer": "www.authserver.com",
    "AccessTokenExpiration": 5
  },


  "ClientToken": {
    "Audience": [ "miniapi6" ],
    "Issuer": "www.authserver.com",
    "AccessTokenExpiration": 5
  },

````

In der `appsettings.json`-Datei sind die Konfigurationen für zwei Arten von Token, `UserToken` und `ClientToken`, definiert. Beide Token haben spezifische Einstellungen für `Audience`, `Issuer` und `AccessTokenExpiration`. Die `Audience` gibt an, für welche Empfänger das Token bestimmt ist, während der `Issuer` die ausstellende Instanz des Tokens beschreibt. `AccessTokenExpiration` legt die Gültigkeitsdauer des Tokens in Minuten fest. Diese Konfigurationen sind essenziell für die Authentifizierung und Autorisierung in einer Anwendung, die JSON Web Tokens (JWT) verwendet.

[Nach oben](#inhaltsverzeichnis)

----

## Services / PublicKeyService.cs

```` csharp

namespace MiniApi6.API.Services
{
    public class PublicKeyService
    {



        private const string PublicKeyUrl = "http://localhost:5000/api/publickey"; // AuthServer'daki PublicKey endpoint
        private const string PublicKeyPath = "Keys/public.key"; // Public key'in saklanacağı yol

        public async Task<string> GetOrFetchPublicKeyAsync()
        {

            // Eğer dosya varsa, içeriğini oku ve geri dön
            if (File.Exists(PublicKeyPath))
            {
                return await File.ReadAllTextAsync(PublicKeyPath);
            }

            // Eğer dosya yoksa, AuthServer'dan public key'i çek
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(PublicKeyUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Public key alınırken hata oluştu!");
            }

            var publicKey = await response.Content.ReadAsStringAsync();

            // Çekilen public key'i dosyaya kaydet
            Directory.CreateDirectory(Path.GetDirectoryName(PublicKeyPath)!); // Keys klasörü yoksa oluştur
            await File.WriteAllTextAsync(PublicKeyPath, publicKey);

            return publicKey;
        }


    }
}

````

Der gegebene Code definiert einen `PublicKeyService`, der für das Abrufen und Speichern eines öffentlichen Schlüssels verantwortlich ist. Der Service überprüft zunächst, ob der öffentliche Schlüssel bereits lokal in einer Datei gespeichert ist. Falls ja, wird der Schlüssel aus der Datei gelesen und zurückgegeben. Falls nicht, wird der Schlüssel von einem Authentifizierungsserver über eine HTTP-Anfrage abgerufen. Bei erfolgreichem Abruf wird der Schlüssel in einer lokalen Datei gespeichert, um zukünftige Anfragen zu optimieren. Tritt ein Fehler beim Abrufen des Schlüssels auf, wird eine Ausnahme ausgelöst.

[Nach oben](#inhaltsverzeichnis)

----

## Program.cs


```` csharp


using Microsoft.IdentityModel.Tokens;
using MiniApi6.API.Services;
using System.Security.Cryptography;

namespace MiniApi6.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var configuration = builder.Configuration;

            // PublicKeyService'i DI konteynerine kaydedin
            builder.Services.AddSingleton<PublicKeyService>();

            // PublicKeyService'i DI'dan al
            var publicKeyService = builder.Services.BuildServiceProvider().GetRequiredService<PublicKeyService>();


            try
            {

                // Public key'i yükle veya fetch et
                var publicKey = Task.Run(() => publicKeyService.GetOrFetchPublicKeyAsync()).Result;


                // RSA nesnesini oluştur ve public key'i yükle
                var rsa = RSA.Create();
                rsa.ImportFromPem(publicKey.ToCharArray());

                // RsaSecurityKey'i oluştur
                var rsaSecurityKey = new RsaSecurityKey(rsa);




                // UserToken doğrulama yapılandırması
                builder.Services.AddAuthentication()
                    .AddJwtBearer("UserToken", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = rsaSecurityKey,
                            ValidIssuer = configuration["UserToken:Issuer"],
                            ValidAudiences = configuration.GetSection("UserToken:Audience").Get<string[]>()
                        };
                    });


                // ClientToken doğrulama yapılandırması
                builder.Services.AddAuthentication()
                    .AddJwtBearer("ClientToken", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = rsaSecurityKey,
                            ValidIssuer = configuration["ClientToken:Issuer"],
                            ValidAudiences = configuration.GetSection("ClientToken:Audience").Get<string[]>()
                        };
                    });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }




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



            app.UseAuthentication(); // EINTRAGEN

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

````

Dieser Code definiert den Einstiegspunkt einer ASP.NET Core-Webanwendung. In der Datei `Program.cs` wird ein Dienst namens `PublicKeyService` in den Dependency Injection (DI) Container aufgenommen, um einen RSA-Schlüssel zu erstellen, der für die Validierung von JWT (JSON Web Token) verwendet wird. Die Anwendung enthält Konfigurationen für die JWT-Validierung sowohl für Benutzer (`UserToken`) als auch für Clients (`ClientToken`). Für beide Token-Typen werden spezifische Validierungsparameter festgelegt, wie z.B. die Überprüfung des Ausstellers (`Issuer`) und der Zielgruppe (`Audience`). Während der Entwicklung wird Swagger zur Erstellung von API-Dokumentationen verwendet, und die Authentifizierung sowie Autorisierung werden aktiviert. Die Anwendung mappt die Controller, um HTTP-Anfragen zu verarbeiten, und startet den Betrieb.

[Nach oben](#inhaltsverzeichnis)

----

## Controllers / InfoController.cs

```` csharp

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi6.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {


        // Nur Benutzer können darauf zugreifen (UserToken)
        [Authorize(AuthenticationSchemes = "UserToken")]
        [HttpGet("User")]
        public IActionResult GetUserInfo()
        {
            // Benutzeroperationen

            return Ok($"Community => UserInfo ");
        }


        // Nur Clients können darauf zugreifen (ClientToken)
        [Authorize(AuthenticationSchemes = "ClientToken")]
        [HttpGet("Client")]
        public IActionResult GetClientInfo()
        {
            // Clientoperationen

            return Ok($"Community => ClientInfo ");
        }


    }
}


````

Dieser Code definiert den Einstiegspunkt einer ASP.NET Core-Webanwendung. In der Datei `Program.cs` wird ein Dienst namens `PublicKeyService` in den Dependency Injection (DI) Container aufgenommen, um einen RSA-Schlüssel zu erstellen, der für die Validierung von JWT (JSON Web Token) verwendet wird. Die Anwendung enthält Konfigurationen für die JWT-Validierung sowohl für Benutzer (`UserToken`) als auch für Clients (`ClientToken`). Für beide Token-Typen werden spezifische Validierungsparameter festgelegt, wie z.B. die Überprüfung des Ausstellers (`Issuer`) und der Zielgruppe (`Audience`). Während der Entwicklung wird Swagger zur Erstellung von API-Dokumentationen verwendet, und die Authentifizierung sowie Autorisierung werden aktiviert. Die Anwendung mappt die Controller, um HTTP-Anfragen zu verarbeiten, und startet den Betrieb.

[Nach oben](#inhaltsverzeichnis)

----

