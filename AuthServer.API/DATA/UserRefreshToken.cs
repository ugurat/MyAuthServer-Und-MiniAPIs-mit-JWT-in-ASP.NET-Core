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
