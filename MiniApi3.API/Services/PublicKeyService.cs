namespace MiniApi3.API.Services
{
    public class PublicKeyService
    {

        private readonly IConfiguration _configuration;

        private const string PublicKeyPath = "Keys/public.key"; // Speicherpfad für den Public Key

        public PublicKeyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOrFetchPublicKeyAsync()
        {

            // URL aus der appsettings.json auslesen - PublicKey-Endpunkt auf dem AuthServer
            var publicKeyUrl = _configuration["PublicKeySettings:PublicKeyUrl"];
            if (string.IsNullOrEmpty(publicKeyUrl))
            {
                throw new Exception("Die PublicKey-URL ist in der Konfiguration nicht definiert!");
            }

            // Wenn die Datei existiert, lese den Inhalt und gib ihn zurück
            if (File.Exists(PublicKeyPath))
            {
                return await File.ReadAllTextAsync(PublicKeyPath);
            }

            // Wenn die Datei nicht existiert, hole den Public Key vom AuthServer
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(publicKeyUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fehler beim Abrufen des Public Keys!");
            }

            var publicKey = await response.Content.ReadAsStringAsync();

            // Speichere den abgerufenen Public Key in der Datei
            Directory.CreateDirectory(Path.GetDirectoryName(PublicKeyPath)!); // Erstelle den Ordner "Keys", falls er nicht existiert
            await File.WriteAllTextAsync(PublicKeyPath, publicKey);

            return publicKey;
        }

    }
}
