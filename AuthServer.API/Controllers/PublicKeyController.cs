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
