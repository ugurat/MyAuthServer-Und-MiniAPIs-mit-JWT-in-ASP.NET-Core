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
