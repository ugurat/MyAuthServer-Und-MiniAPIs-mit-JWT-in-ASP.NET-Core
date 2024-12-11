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
