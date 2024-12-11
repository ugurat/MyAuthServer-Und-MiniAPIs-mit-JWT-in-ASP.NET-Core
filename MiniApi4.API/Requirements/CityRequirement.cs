using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiniApi4.API.Requirements
{


    public class CityRequirement : IAuthorizationRequirement
    {
        public string RequiredCity { get; }

        public CityRequirement(string requiredCity)
        {
            RequiredCity = requiredCity;
        }
    }


    public class CityRequirementHandler : AuthorizationHandler<CityRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CityRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CityRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var cityClaim = context.User.FindFirst("city");

            if (cityClaim == null || cityClaim.Value != requirement.RequiredCity)
            {

                // Hata mesajı ayarla
                httpContext.Response.Headers.Add("WWW-Authenticate",
                    $"Bearer error=\"invalid_token\", error_description=\"User city is invalid or missing. Required city: {requirement.RequiredCity}.\"");

                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }


}
