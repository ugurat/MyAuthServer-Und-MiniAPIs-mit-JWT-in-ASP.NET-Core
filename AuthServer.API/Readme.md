
# AuthServer.API - Anleitung 

## Inhaltsverzeichnis
- [AuthServer.API - Anleitung](#authserverapi---anleitung)
  - [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Einführung](#einführung)
  - [Nuget Pakete installieren](#nuget-pakete-installieren)
  - [Properties / launchSettings.json](#properties--launchsettingsjson)
  - [appsettings.json](#appsettingsjson)
  - [Configuration / Client.cs](#configuration--clientcs)
  - [Configuration / CustomTokenOption.cs](#configuration--customtokenoptioncs)
  - [DTOs](#dtos)
    - [DTOs / UserAppDto.cs](#dtos--userappdtocs)
    - [DTOs / TokenDto.cs](#dtos--tokendtocs)
    - [DTOs / ErrorDto.cs](#dtos--errordtocs)
    - [DTOs / LoginDto.cs](#dtos--logindtocs)
    - [DTOs / ClientLoginDto.cs](#dtos--clientlogindtocs)
    - [DTOs / ClientTokenDto.cs](#dtos--clienttokendtocs)
    - [DTOs / CreateUserDto.cs](#dtos--createuserdtocs)
    - [DTOs / Response.cs](#dtos--responsecs)
  - [DTOs / DtoMapper.cs](#dtos--dtomappercs)
  - [DATA](#data)
    - [DATA / UserApp.cs](#data--userappcs)
    - [DATA / UserRefreshToken.cs](#data--userrefreshtokencs)
  - [DATA / AppDbContext.cs](#data--appdbcontextcs)
  - [Program.cs](#programcs)
  - [Controllers / CustomBaseController.cs](#controllers--custombasecontrollercs)
  - [Controllers / AuthController.cs](#controllers--authcontrollercs)
  - [Controllers / PublicKeyController.cs](#controllers--publickeycontrollercs)
  - [Controllers / UserController.cs](#controllers--usercontrollercs)
- [AuthServer Testen mit Postman](#authserver-testen-mit-postman)
  - [REGISTRIEREN](#registrieren)
  - [LOGIN](#login)
  - [USERINFO](#userinfo)
  - [Create User Roles](#create-user-roles)
  - [Create Tocken By Refresh Token](#create-tocken-by-refresh-token)
  - [Revoke Refresh Token](#revoke-refresh-token)
  - [Create Tocken By Client](#create-tocken-by-client)
  - [LOGOUT (NICHT NICHT ENTWICKELT)](#logout-nicht-nicht-entwickelt)

## Einführung
Die `AuthServer.API`-Anwendung bietet eine umfassende Lösung für die Authentifizierung und Autorisierung von Benutzern. Diese API ist darauf ausgelegt, sichere Token-basierte Authentifizierungsmechanismen bereitzustellen.

[Nach oben](#inhaltsverzeichnis)

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

Die aufgelisteten NuGet-Pakete werden installiert, um verschiedene Funktionalitäten in einem .NET-Projekt zu integrieren. 
- `AutoMapper` erleichtert die Objektzuordnung zwischen unterschiedlichen Klassen. 
- `Microsoft.AspNetCore.Authentication.JwtBearer` ermöglicht die JWT-Authentifizierung in einer ASP.NET Core-Anwendung. 
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` unterstützt die Benutzerverwaltung und Authentifizierung mit Entity Framework Core. Die `Microsoft.EntityFrameworkCore`-Pakete (inklusive `Design`, `SqlServer` und `Tools`) bieten Unterstützung für die Datenbankinteraktion, insbesondere mit SQL Server, und ermöglichen das Arbeiten mit Migrations- und Design-Tools. 
- `Swashbuckle.AspNetCore` integriert Swagger für API-Dokumentation, und `System.IdentityModel.Tokens.Jwt` wird für die Erstellung und Validierung von JWT-Token verwendet. 
 
Diese Pakete zusammen bilden die Grundlage für eine Anwendung mit Authentifizierung, Datenbankzugriff und API-Dokumentation.

[Nach oben](#inhaltsverzeichnis)


----

## Properties / launchSettings.json

```` bash
...

  "profiles": {
    "AuthServer.API": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },

...
````

Die Datei `launchSettings.json` definiert die Startkonfiguration der Anwendung `AuthServer.API` während der Entwicklungsphase. Die Anwendung wird mit dem Kommando `dotnet run` gestartet, wobei ausführliche Ausgaben (`dotnetRunMessages`) aktiviert sind. Beim Start öffnet sich automatisch ein Browser mit der URL `http://localhost:5000/swagger`, die zur API-Dokumentation führt. Die Basis-URL der Anwendung wird über `applicationUrl` auf `http://localhost:5000` gesetzt, und die Umgebung wird mit der Umgebungsvariablen `ASPNETCORE_ENVIRONMENT` auf `Development` festgelegt, was für Debugging und Entwicklerfunktionen genutzt wird.

[Nach oben](#inhaltsverzeichnis)

----

## appsettings.json

```` bash

  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.\\sqlexpress;Initial Catalog=TokenDb2;User ID=sa;Password=www;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },

````

Die Verbindungszeichenfolge in der `appsettings.json`, die für die Verbindung zu einer SQL Server-Datenbank verwendet wird. `Data Source` gibt die Serveradresse an, `Initial Catalog` den Datenbanknamen. `User ID` und `Password` liefern die Anmeldeinformationen. Weitere Einstellungen konfigurieren die Sicherheit und Leistung der Verbindung.

```` bash

  "TokenOption": {
    "Audience": [ "www.authserver.com", "www.miniapi1.com", "www.miniapi2.com", "www.miniapi3.com", "www.miniapi4.com", "www.miniapi5.com", "www.miniapi6.com" ],
    "Issuer": "www.authserver.com",
    "AccessTokenExpiration": 5,
    "RefreshTokenExpiration": 600,
    "SecurityKey": "mysecuritykeymysecuritykeymysecuritykeymysecuritykey"
  },

````

`TokenOption` definiert die Konfigurationseinstellungen für die Token-Authentifizierung in der Anwendung. Es enthält eine Liste von gültigen Zielgruppen (`Audience`), den Herausgeber des Tokens (`Issuer`), die Gültigkeitsdauer des Zugriffstokens (`AccessTokenExpiration`) und des Auffrischungstokens (`RefreshTokenExpiration`) in Minuten sowie den Sicherheitskey (`SecurityKey`) zur Validierung und Signierung der Tokens. Diese Einstellungen sind entscheidend für die Sicherheit und Verwaltung der Token-basierten Authentifizierung.



```` bash

  "Clients": [
    {
      "Id": "MiniApi5API",
      "Secret": "secret",
      "Audiences": [ "www.miniapi5.com" ]
    },
    {
      "Id": "MobileApp",
      "Secret": "secret",
      "Audiences": [ "www.miniapi3.com", "www.miniapi4.com" ]
    },
    {
      "Id": "MiniApi6API",
      "Secret": "secret",
      "Audiences": [ "miniapi6" ]
    }
  ],

````

Die Konfiguration im Abschnitt `"Clients"` der `appsettings.json`-Datei definiert verschiedene Clients, die auf die Authentifizierungsdienste der API zugreifen können. Jeder Client hat eine eindeutige `Id`, ein `Secret` zur Authentifizierung und eine Liste von `Audiences`, die die gültigen Zielgruppen für den jeweiligen Client angeben. Diese Einstellungen sind entscheidend, um sicherzustellen, dass nur autorisierte Clients Zugriff auf die API erhalten und die Kommunikation sicher bleibt.

[Nach oben](#inhaltsverzeichnis)

----

## Configuration / Client.cs

```` csharp

namespace AuthServer.API.Configuration
{
    public class Client
    {

        public string Id { get; set; }

        public string Secret { get; set; }

        public List<String> Audiences { get; set; }


    }
}

```` 

Die `Client`-Klasse in der `AuthServer.API.Configuration`-Namespace definiert die Struktur für Client-Objekte, die auf die Authentifizierungsdienste der API zugreifen können. Jedes Client-Objekt besitzt eine eindeutige `Id`, ein `Secret` zur Authentifizierung und eine Liste von `Audiences`, die die gültigen Zielgruppen für den jeweiligen Client angeben. Diese Konfigurationen sind entscheidend, um sicherzustellen, dass nur autorisierte Clients Zugriff auf die API erhalten und die Kommunikation sicher bleibt.

[Nach oben](#inhaltsverzeichnis)

----

## Configuration / CustomTokenOption.cs

```` csharp

namespace AuthServer.API.Configuration
{
    public class CustomTokenOption
    {

        // Liste der gültigen Zielgruppen (Audiences) für das Token.
        public List<String> Audience { get; set; }

        // Der Herausgeber (Issuer) des Tokens.
        public string Issuer { get; set; }

        // Gültigkeitsdauer des Access Tokens in Minuten.
        public int AccessTokenExpiration { get; set; }

        // Gültigkeitsdauer des Refresh Tokens in Minuten.
        public int RefreshTokenExpiration { get; set; }

        // Der Sicherheitskey für die Token-Validierung und -Signierung.
        public string SecurityKey { get; set; }


    }
}

```` 

`CustomTokenOption` ist eine Klasse, die die Konfigurationseinstellungen für die Token-Authentifizierung in der Anwendung definiert. Sie enthält eine Liste von gültigen Zielgruppen (`Audience`), den Herausgeber des Tokens (`Issuer`), die Gültigkeitsdauer des Zugriffstokens (`AccessTokenExpiration`) und des Auffrischungstokens (`RefreshTokenExpiration`) in Minuten sowie den Sicherheitskey (`SecurityKey`) zur Validierung und Signierung der Tokens. Diese Einstellungen sind entscheidend für die Sicherheit und Verwaltung der Token-basierten Authentifizierung.

[Nach oben](#inhaltsverzeichnis)

----

## DTOs

Diese Dateien definieren Datenübertragungsobjekte (DTOs), die von anderen Dateien verwendet werden können, sind jedoch selbst nicht von anderen Dateien abhängig.

DTO-Klassen als Antwort: UserAppDto, TokenDto, ErrorDto
DTO-Klassen als Parameter: LoginDto, ClientLoginDto, ClientTokenDto, CreateUserDto 

Response<T> wird in AuthenticationService.cs und UserService.cs aufgerufen.


### DTOs / UserAppDto.cs

```` csharp

namespace AuthServer.API.DTOs
{
    public class UserAppDto
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string City { get; set; }

    }
}

````

`UserAppDto` ist eine Datenübertragungsobjekt-Klasse. Sie enthält Eigenschaften wie `Id`, `UserName`, `Email` und `City`, die die Benutzerdaten repräsentieren. Diese Klasse wird verwendet, um Benutzerdaten zwischen verschiedenen Schichten der Anwendung zu übertragen, ohne die zugrunde liegenden Datenmodelle direkt offenzulegen.

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / TokenDto.cs

```` csharp

namespace AuthServer.API.DTOs
{
    public class TokenDto
    {

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

    }
}

````

`TokenDto` ist eine Datenübertragungsobjekt-Klasse. Diese Klasse enthält Eigenschaften für `AccessToken` und `RefreshToken` sowie deren jeweilige Ablaufdaten (`AccessTokenExpiration` und `RefreshTokenExpiration`). Sie wird verwendet, um Token-Informationen zwischen verschiedenen Schichten der Anwendung zu übertragen, insbesondere im Kontext der Authentifizierung und Autorisierung.

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / ErrorDto.cs

```` csharp

namespace AuthServer.API.DTOs
{
    public class ErrorDto
    {

        public List<String> Errors { get; private set; } = new List<string>();

        public bool IsShow { get; private set; }

        public ErrorDto(string error, bool isShow)
        {
            Errors.Add(error);
            IsShow = isShow;
        }

        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }

    }
}

````

`ErrorDto` ist eine Klasse, die zur Darstellung von Fehlerinformationen in der Anwendung dient. Sie enthält eine Liste von Fehlernachrichten (`Errors`) und eine boolesche Eigenschaft (`IsShow`), die angibt, ob die Fehler angezeigt werden sollen. Die Klasse bietet zwei Konstruktoren: einen, der eine einzelne Fehlermeldung und einen Anzeigestatus akzeptiert, und einen anderen, der eine Liste von Fehlermeldungen und einen Anzeigestatus entgegennimmt. Diese Struktur ermöglicht eine flexible Handhabung und Darstellung von Fehlern in der API.

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / LoginDto.cs 

```` csharp

namespace AuthServer.API.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

````

`LoginDto` ist eine Datenübertragungsobjekt-Klasse, die die Anmeldedaten eines Benutzers repräsentiert. Sie enthält zwei Eigenschaften: `Email` und `Password`, die für die Authentifizierung eines Benutzers in der Anwendung verwendet werden. Diese Klasse dient dazu, die Anmeldedaten sicher und strukturiert zwischen den verschiedenen Schichten der Anwendung zu übertragen.

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / ClientLoginDto.cs 

```` csharp

namespace AuthServer.API.DTOs
{
    public class ClientLoginDto
    {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

    }
}

````

`ClientLoginDto` ist eine Datenübertragungsobjekt-Klasse, die die Anmeldedaten eines Clients repräsentiert. Sie enthält zwei Eigenschaften: `ClientId` und `ClientSecret`, die für die Authentifizierung eines Clients in der Anwendung verwendet werden. Diese Klasse dient dazu, die Anmeldedaten sicher und strukturiert zwischen den verschiedenen Schichten der Anwendung zu übertragen. 

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / ClientTokenDto.cs 

```` csharp

namespace AuthServer.API.DTOs
{
    public class ClientTokenDto
    {

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }

    }
}

````

`ClientTokenDto` ist eine Datenübertragungsobjekt-Klasse. Diese Klasse enthält zwei Eigenschaften: `AccessToken`, das den Zugriffstoken repräsentiert, und `AccessTokenExpiration`, das das Ablaufdatum des Zugriffstokens angibt. Sie wird verwendet, um Token-Informationen zwischen verschiedenen Schichten der Anwendung zu übertragen, insbesondere im Kontext der Authentifizierung und Autorisierung von Clients.

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / CreateUserDto.cs 

```` csharp

namespace AuthServer.API.DTOs
{
    public class CreateUserDto
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

````

`CreateUserDto` ist eine Datenübertragungsobjekt-Klasse, die die Informationen eines neuen Benutzers repräsentiert, die bei der Erstellung eines Benutzerkontos benötigt werden. Sie enthält die Eigenschaften `UserName`, `Email` und `Password`, die verwendet werden, um die Anmeldedaten des Benutzers zu erfassen und sicher zwischen den verschiedenen Schichten der Anwendung zu übertragen.

[Nach oben](#inhaltsverzeichnis)

----

### DTOs / Response.cs

```` csharp

using System.Text.Json.Serialization;

namespace AuthServer.API.DTOs
{
    public class Response<T> where T : class
    {

        public T Data { get; private set; }
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public ErrorDto Error { get; private set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Response<T>
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);

            return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }


    }
}

````

`Response<T>` ist eine generische Klasse, die einheitliche API-Antworten in einer Anwendung ermöglicht. Sie enthält Eigenschaften wie `Data` für die Antwortdaten, `StatusCode` für den HTTP-Statuscode, `IsSuccessful` zur Angabe des Erfolgsstatus und `Error` für Fehlerdetails. Die Klasse bietet statische Methoden zur Erstellung von erfolgreichen oder fehlgeschlagenen Antworten, wobei bei Fehlern ein `ErrorDto`-Objekt verwendet wird, um detaillierte Fehlermeldungen zu übermitteln. Diese Struktur erleichtert die konsistente Handhabung von API-Antworten und Fehlern.

[Nach oben](#inhaltsverzeichnis)

----

## DTOs / DtoMapper.cs

```` csharp

using AuthServer.API.DATA;
using AutoMapper;

namespace AuthServer.API.DTOs
{
    public class DtoMapper : Profile // EINTRAGEN
    {

        public DtoMapper()
        {

            CreateMap<UserAppDto, UserApp>().ReverseMap();
            //CreateMap<ProductDto, Product>().ReverseMap();

        }

    }
}

````

`DtoMapper` ist eine Klasse, die von `Profile` erbt und die Konfiguration für die Objektzuordnung in der Anwendung bereitstellt. Sie verwendet `AutoMapper`, um die Zuordnung zwischen `UserAppDto` und `UserApp` zu definieren und ermöglicht eine bidirektionale Konvertierung zwischen diesen beiden Objekten. Dies erleichtert die Transformation von Datenübertragungsobjekten in Datenmodelle und umgekehrt, was die Datenverarbeitung und -übertragung innerhalb der Anwendung vereinfacht.

[Nach oben](#inhaltsverzeichnis)

----

## DATA

Diese Dateien definieren Datenmodelle, die von `AppDbContext.cs` verwendet werden.


### DATA / UserApp.cs

```` csharp

using Microsoft.AspNetCore.Identity;

namespace AuthServer.API.DATA
{
    public class UserApp : IdentityUser // EINTRAGEN 
    {

        public DateTime? BirthDate { get; set; }

        public string? City { get; set; }

        // ---- Später migrieren ! ----
        // add-migration BirthDateNullable
        // update-database
        //
        // add-migration CityNullable
        // update-database

    }
}

````

`UserApp` ist eine Klasse, die von `IdentityUser` erbt und zusätzliche Eigenschaften für die Benutzerverwaltung in einer ASP.NET Core-Anwendung bereitstellt. Sie enthält optionale Felder wie `BirthDate` und `City`, die das Geburtsdatum und die Stadt des Benutzers speichern. Diese Klasse ist Teil der Datenmodelle, die in der Datenbankkontextklasse `AppDbContext` verwendet werden, und ermöglicht die Erweiterung der Standardbenutzerinformationen um benutzerdefinierte Attribute.

[Nach oben](#inhaltsverzeichnis)

----

### DATA / UserRefreshToken.cs

```` csharp

using System.ComponentModel.DataAnnotations;

namespace AuthServer.API.DATA
{
    public class UserRefreshToken
    {

        [Key] // EINTRAGEN using System.ComponentModel.DataAnnotations;
        public string UserId { get; set; }

        public string Code { get; set; }
        public DateTime Expiration { get; set; }

    }
}

````

`UserRefreshToken` ist eine Klasse, die die Struktur für Refresh-Token in der Anwendung definiert. Sie enthält die Eigenschaften `UserId`, `Code` und `Expiration`. `UserId` ist der Primärschlüssel, der den Benutzer identifiziert, `Code` speichert den eigentlichen Refresh-Token, und `Expiration` gibt das Ablaufdatum des Tokens an. Diese Klasse wird verwendet, um die Verwaltung und Validierung von Refresh-Tokens zu unterstützen, die für die Erneuerung von Zugriffstokens in der Authentifizierungslogik der Anwendung erforderlich sind.

[Nach oben](#inhaltsverzeichnis)

----

## DATA / AppDbContext.cs

```` csharp

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.API.DATA
{
    public class AppDbContext : IdentityDbContext<UserApp, IdentityRole, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        //public DbSet<Product> Products { get; set; }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
        }



    }
}

````

`AppDbContext` ist eine Klasse, die von `IdentityDbContext` erbt und die Datenbankkontextklasse für die Anwendung darstellt. Sie konfiguriert die Datenbankverbindung und definiert die `DbSet`-Eigenschaften für die Entitäten, die in der Datenbank gespeichert werden sollen, wie z.B. `UserRefreshToken`. Die Methode `OnModelCreating` wird überschrieben, um Konfigurationen aus der Assembly anzuwenden, was die Anpassung der Datenbankstruktur ermöglicht. Diese Klasse ist entscheidend für die Verwaltung der Datenbankoperationen und die Integration von Identitätsfunktionen in der Anwendung.

[Nach oben](#inhaltsverzeichnis)

----

## Program.cs

```` csharp

using AuthServer.API.Configuration;
using AuthServer.API.DATA;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // `configuration` nesnesini tanımlayın
            var configuration = builder.Configuration; // Appsettings.json'u okumak için gerekli




            // appsettings.json'dan TokenOption yapılandırmasını yükle
            builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));

            // KeyService'i Dependency Injection'a ekle
            builder.Services.AddSingleton<KeyService>();

            // TokenOption yapılandırmasını yükle
            var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();



            // EINTRAGEN
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                // EINTRAGEN using Microsoft.EntityFrameworkCore;
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                    //, sqlOptions =>
                    //{
                    //    sqlOptions.MigrationsAssembly("UdemyAuthServer.Data");
                    //}
                    );
            });


            // EINTRAGEN - AddScoped für DB Tabellen
            builder.Services.AddScoped<IUserService, UserService>();

            // EINTRAGEN
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            // EINTRAGEN
            builder.Services.AddIdentity<UserApp, IdentityRole>(Opt =>
            {
                Opt.User.RequireUniqueEmail = true;
                Opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



            // EINTRAGEN
            builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));

            // EINTRAGEN * Client
            builder.Services.Configure<List<Client>>( builder.Configuration.GetSection("Clients"));



            // EINTRAGEN
            // Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 5.0.17
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(KeyService.LoadPublicKey()), // Public key'i yükle
                    ValidIssuer = configuration["TokenOption:Issuer"],
                    //ValidAudience = configuration["TokenOption:Audience"]
                    ValidAudiences = configuration.GetSection("TokenOption:Audience").Get<List<string>>()

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

            // Token Prüfen
            app.UseAuthentication(); // EINTRAGEN

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

````

`Program.cs` ist die Hauptdatei der Anwendung, die den Einstiegspunkt darstellt und die Konfiguration sowie den Aufbau der Webanwendung übernimmt. Sie initialisiert den `WebApplication`-Builder, lädt Konfigurationen aus der `appsettings.json`, und registriert verschiedene Dienste für Dependency Injection, darunter Datenbankkontext, Authentifizierungs- und Autorisierungsdienste. Die Datei konfiguriert die JWT-Authentifizierung mit einem öffentlichen Schlüssel und definiert die Middleware-Pipeline, die Swagger für die API-Dokumentation einbindet. Schließlich wird die Anwendung gestartet, indem die HTTP-Anfragen über die konfigurierten Controller verarbeitet werden.

[Nach oben](#inhaltsverzeichnis)

-----

## Controllers / CustomBaseController.cs

```` csharp

using AuthServer.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{

    /// <summary>
    /// Benutzerdefinierter Basiskontroller für einheitliche API-Antworten.
    /// </summary>
    public class CustomBaseController : ControllerBase
    {

        /// <summary>
        /// Gibt eine einheitliche API-Antwort zurück, basierend auf dem generischen Response-Objekt.
        /// </summary>
        /// <typeparam name="T">Typ der Daten, die in der Antwort enthalten sind.</typeparam>
        /// <param name="response">Antwortobjekt mit Statuscode und Daten.</param>
        /// <returns>Ein HTTP-Objekt mit dem Antwortinhalt und dem Statuscode.</returns>
        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
            // EINTRAGEN using AuthServer.DTOs;

            return new ObjectResult(response) // Erstellt eine HTTP-Antwort mit dem gegebenen Inhalt
            {
                StatusCode = response.StatusCode // Setzt den HTTP-Statuscode entsprechend dem Response-Objekt
            };
        }


    }
}

````

`CustomBaseController` ist eine benutzerdefinierte Basisklasse für Controller, die eine einheitliche API-Antwortstruktur bereitstellt. Sie erbt von `ControllerBase` und enthält die Methode `ActionResultInstance<T>`, die ein generisches `Response<T>`-Objekt entgegennimmt. Diese Methode erstellt eine HTTP-Antwort mit dem Inhalt und dem Statuscode des übergebenen `Response`-Objekts. Dies ermöglicht eine konsistente und zentrale Handhabung von API-Antworten in der Anwendung.

[Nach oben](#inhaltsverzeichnis)

----

## Controllers / AuthController.cs

```` csharp

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

````

`AuthController` ist ein API-Controller, der von einer benutzerdefinierten Basisklasse erbt und für die Verwaltung von Authentifizierungsprozessen zuständig ist. Er bietet Endpunkte zur Erstellung von JWT-Tokens basierend auf Benutzer- oder Client-Anmeldedaten, zur Erneuerung von Tokens mittels Refresh-Tokens und zum Widerrufen von Refresh-Tokens. Die Klasse verwendet Dependency Injection, um auf den Authentifizierungsdienst zuzugreifen, und stellt sicher, dass die API-Antworten einheitlich und strukturiert zurückgegeben werden.

[Nach oben](#inhaltsverzeichnis)

----

## Controllers / PublicKeyController.cs

```` csharp

using AuthServer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicKeyController : ControllerBase
    {

        private readonly KeyService _keyService;

        public PublicKeyController(KeyService keyService)
        {
            _keyService = keyService;
        }

        [HttpGet]
        public IActionResult GetPublicKey()
        {
            return Ok(_keyService.GetPublicKey());
        }

    }
}

````

`PublicKeyController` ist ein API-Controller, der für die Bereitstellung des öffentlichen Schlüssels zuständig ist. Er verwendet Dependency Injection, um auf den `KeyService` zuzugreifen, der die Logik zur Schlüsselverwaltung enthält. Der Controller bietet einen Endpunkt, der über einen HTTP-GET-Aufruf den öffentlichen Schlüssel zurückgibt. Dies ermöglicht es anderen Diensten oder Anwendungen, den öffentlichen Schlüssel abzurufen, um die Integrität und Authentizität von JWT-Tokens zu überprüfen.

[Nach oben](#inhaltsverzeichnis)

----

## Controllers / UserController.cs

```` csharp

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

````

`UserController` ist ein API-Controller, der für die Verwaltung von Benutzeroperationen zuständig ist. Er erbt von einer benutzerdefinierten Basisklasse und verwendet den `IUserService`, um Benutzeraktionen durchzuführen. Der Controller bietet Endpunkte zum Erstellen eines neuen Benutzers, zum Abrufen von Benutzerdaten basierend auf dem aktuellen Benutzernamen und zum Zuweisen von Rollen zu einem Benutzer. Die Methoden sind asynchron und verwenden die `ActionResultInstance`-Methode, um einheitliche API-Antworten zu generieren. Der Zugriff auf Benutzerdaten ist durch das `Authorize`-Attribut geschützt, um sicherzustellen, dass nur authentifizierte Benutzer auf bestimmte Endpunkte zugreifen können.

[Nach oben](#inhaltsverzeichnis)

-----



# AuthServer Testen mit Postman


----

## REGISTRIEREN

POST http://localhost:5000/api/user [REGISTRIEREN]

Body:
```` bash
{
    "userName": "UgurCigdem",
    "email": "ugurcigdem@gmail.com",
    "password": "Password12*"
}
````

Antwort mit Erfolg:

```` bash
{
    "data": {
        "id": "ab391f54-a362-4b28-bee5-2288638d7cf4",
        "userName": "UgurCigdem",
        "email": "ugurcigdem@gmail.com",
        "city": null
    },
    "statusCode": 200,
    "error": null
}
````

Antwort mit Fehler:

```` bash
{
    "data": null,
    "statusCode": 400,
    "error": {
        "errors": [
            "Username 'UgurCigdem' is already taken.",
            "Email 'ugurcigdem@gmail.com' is already taken."
        ],
        "isShow": true
    }
}
````

[Nach oben](#inhaltsverzeichnis)

----

## LOGIN

POST http://localhost:5000/api/auth/CreateToken [LOGIN]

Body:

```` bash
{
    "userName": "UgurCigdem",
    "password": "Password12*"
}
````

- Antwort mit Erfolg:

```` bash
{
    "data": {
        "accessToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImIxNTExMWI4LTg3NTAtNDMyOS05ODUxLWUyMTk2ZDM0ZmYzYyIsImVtYWlsIjoidWd1cmNpZ2RlbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVWd1ckNpZ2RlbSIsImp0aSI6ImYwNzJkMzcyLTYzMTItNGY4Yi04ZjkyLWQwMjMwYzIzNTA2MyIsImNpdHkiOiJXaWVuIiwiYmlydGgtZGF0ZSI6IjIwMTUtMDUtMjEiLCJhdWQiOlsid3d3LmF1dGhzZXJ2ZXIuY29tIiwid3d3Lm1pbmlhcGkxLmNvbSIsInd3dy5taW5pYXBpMi5jb20iLCJ3d3cubWluaWFwaTMuY29tIiwid3d3Lm1pbmlhcGk0LmNvbSIsInd3dy5taW5pYXBpNS5jb20iLCJ3d3cubWluaWFwaTYuY29tIl0sImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJtYW5hZ2VyIiwiYWRtaW4iXSwibmJmIjoxNzMzNjIxNzAzLCJleHAiOjE3MzM2MjIwMDMsImlzcyI6Ind3dy5hdXRoc2VydmVyLmNvbSJ9.wSKq4WYVtlNDSPBSBkkzcrh_RVOd_PU_YcyvhQkIC_9F1mf5nZzQ2dlW68S50YtkTTQfC2J1DkuJa9UmhMA77MI720nbfnFuyQxaCf-Jcn4WilIhuHJ_ROCAJFqWpmUB_QF98c8e9uXphfAriXWK-xCHRiiU26aXabHp0DPEP7TQJpIoYR8Y9ng0RAn2s1OUBI2-GIziqY9zZoD_W53DgNIfZT5FmbtO4hzJkuMVJIKhrwnTZE7NtmVSv1DIxltpYckgQBfe9rNdxNwTNLd3MasKusR8jsvJe4DBzLNGQ0HYQN5CjMOhiwnbldryq8jdNMskqtLJjkBGhVyc1nKHuQ",
        "accessTokenExpiration": "2024-12-08T02:40:03.6512545+01:00",
        "refreshToken": "VCVCZ82qE90MZGXZH/EsycDPGAoO+R46TvirziVSBVE=",
        "refreshTokenExpiration": "2024-12-08T12:35:03.6514827+01:00"
    },
    "statusCode": 200,
    "error": null
}
````

- Antwort mit Fehler:

Code: 400 Bad Request

Body:

```` bash
{
    "data": null,
    "statusCode": 400,
    "error": {
        "errors": [
            "Username 'Ugur.Cigdem' is already taken.",
            "Email 'ugur.cigdem@gmail.com' is already taken."
        ],
        "isShow": true
    }
}
````

[Nach oben](#inhaltsverzeichnis)

----

## USERINFO

GET http://localhost:5000/api/user [USER INFO]

Authorization: Bearer <accessToken>

- Antwort mit Erfolg:

Code: 200 OK

- Antwort mit Fehler:

Code: 401 Unauthorized

Header:

```` bash
WWW-Authenticate: Bearer error="invalid_token", error_description="The token expired at '12/08/2024 01:40:03'"
````

[Nach oben](#inhaltsverzeichnis)

----

## Create User Roles

POST http://localhost:5000/api/user/CreateUserRoles/UgurCigdem [CREATE-USER-ROLES]

Nur URL aufrufen. Body nicht erforderlich.

[Nach oben](#inhaltsverzeichnis)

----

## Create Tocken By Refresh Token

POST http://localhost:5000/api/auth/CreateTokenByRefreshToken [NEW TOKEN]

Body:

```` bash
{
  "token": "/HP9/C1K2vbr8JxeLUSoBHEsPp2PtYMUhHSq+Y47SbA="
}````

- Antwort mit Erfolg:

```` bash
{
    "data": {
        "accessToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImIxNTExMWI4LTg3NTAtNDMyOS05ODUxLWUyMTk2ZDM0ZmYzYyIsImVtYWlsIjoidWd1cmNpZ2RlbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVWd1ckNpZ2RlbSIsImp0aSI6ImYwYWIyOGRlLTAzYTEtNDkyYi05Yzc4LTI5NDU1YjZiOGExYiIsImNpdHkiOiJXaWVuIiwiYmlydGgtZGF0ZSI6IjIwMTUtMDUtMjEiLCJhdWQiOlsid3d3LmF1dGhzZXJ2ZXIuY29tIiwid3d3Lm1pbmlhcGkxLmNvbSIsInd3dy5taW5pYXBpMi5jb20iLCJ3d3cubWluaWFwaTMuY29tIiwid3d3Lm1pbmlhcGk0LmNvbSIsInd3dy5taW5pYXBpNS5jb20iLCJ3d3cubWluaWFwaTYuY29tIl0sImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJtYW5hZ2VyIiwiYWRtaW4iXSwibmJmIjoxNzMzNzc2MDE0LCJleHAiOjE3MzM3NzYzMTQsImlzcyI6Ind3dy5hdXRoc2VydmVyLmNvbSJ9.UjvHMPfabG5Pf9VM-Q8xBBp28QPUjdAucx1_SNlaNhE1deFtPH9bwObi7i2u8SGwg1M-AIAxrD-gDt3FzAFNUNn8JfXyghU955vBLe7QpWXvUUsD3c9rTZpjUcFpqe4M9BbbSnamfE5ZSbuK6r97-EEpTlQePHQ8w4Oi-B3xxMesnKU7fX7IHl67K8DPqYOahWMyv8hrNyzP921x7O74A8Q2sfDOcVCgNoyPTlYrKmLaapepWxkiuH5oBnW9xwbRRhgGmoIBu8VFtt2IFY03X5OAOXmoPubplkzLgBlbiXVDuFwWHaDDMepPTedAG9FUvaB5DcW_YFjLeavonzJ23A",
        "accessTokenExpiration": "2024-12-09T21:31:54.0800757+01:00",
        "refreshToken": "N5fL5ZVCgxPoNAen3sb/qwxc3bS2NzctjXtsv1NM2R4=",
        "refreshTokenExpiration": "2024-12-10T07:26:32.5394853"
    },
    "statusCode": 200,
    "error": null
}
````

- Antwort mit Fehler:

Status Code: 404 Not Found

```` bash
{
    "data": null,
    "statusCode": 404,
    "error": {
        "errors": [
            "Refresh token not found"
        ],
        "isShow": true
    }
}
````

[Nach oben](#inhaltsverzeichnis)

----

## Revoke Refresh Token

POST http://localhost:5000/api/auth/RevokeRefreshToken [DELETE TOKEN]


- Antwort mit Erfolg:

Status Code: 200 OK

```` bash
{
    "data": null,
    "statusCode": 200,
    "error": null
}
````


- Antwort mit Fehler:

```` bash
{
    "data": null,
    "statusCode": 404,
    "error": {
        "errors": [
            "Refresh token not found"
        ],
        "isShow": true
    }
}
````

[Nach oben](#inhaltsverzeichnis)

----

## Create Tocken By Client

POST http://localhost:5000/api/auth/CreateTokenByClient [NEW CLIENT TOKEN]

Body:

```` bash
{
  "clientId": "SpaApp",
  "clientSecret": "secret"
}
````

Bsp:

```` bash
{
    "data": {
        "accessToken": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJ3d3cubWluaWFwaTMuY29tIiwibmJmIjoxNzMzMjU3MjMxLCJleHAiOjE3MzMyNTc1MzEsImlzcyI6Ind3dy5hdXRoc2VydmVyLmNvbSJ9.8GhlfE_uRK1eCCTOAwlIgydifI2iyv4StBFRhGIlnm8",
        "accessTokenExpiration": "2024-12-03T21:25:31.3139914+01:00"
    },
    "statusCode": 200,
    "error": null
}
````

[Nach oben](#inhaltsverzeichnis)

-----

## LOGOUT (NICHT NICHT ENTWICKELT)

POST http://localhost:5000/api/auth/logout [LOGOUT]

[Nach oben](#inhaltsverzeichnis)


