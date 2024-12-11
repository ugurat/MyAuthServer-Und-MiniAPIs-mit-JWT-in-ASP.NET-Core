using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace MiniApi3.API.Requirements
{

    public class BirthdateRequirement : IAuthorizationRequirement
    {

        // prop
        public int Age { get; set; }

        // ctor
        public BirthdateRequirement(int age)
        {
            Age = age;
        }


    }


    public class BirthdateRequirementHandler
                    : AuthorizationHandler<BirthdateRequirement>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BirthdateRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        // strg + . > BirthdateRequirementHandler  / Abstrakte Klasse implementieren
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BirthdateRequirement requirement)
        {

            // DI Objekt übernehmen
            var httpContext = _httpContextAccessor.HttpContext;


            var birthdateClaim = context.User.FindFirst("birth-date"); // // "birth-date" NICHT zusammenschreiben! Schlüsselwort "birthdate" ist ClaimTypes.DateOfBirth

            if (birthdateClaim == null)
            {


                httpContext.Response.Headers.Add("WWW-Authenticate",
                    "Bearer error=\"invalid_token\", error_description=\"Birthdate claim is missing.\"");


                // Debugging: Log or print the claims to see what's available
                foreach (var claim in context.User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }

                context.Fail();
                return Task.CompletedTask;
            }

            var today = DateTime.Now;
            var birthdate = DateTime.Parse(birthdateClaim.Value);
            var age = today.Year - birthdate.Year;

            if (age < 18)
            {

                httpContext.Response.Headers.Add("WWW-Authenticate",
                    $"Bearer error=\"invalid_token\", error_description=\"User age is {age}. Required age: {requirement.Age}.\"");


                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }


    }




}
