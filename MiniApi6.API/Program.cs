
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
