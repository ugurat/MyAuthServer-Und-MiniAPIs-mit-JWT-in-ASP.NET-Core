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
