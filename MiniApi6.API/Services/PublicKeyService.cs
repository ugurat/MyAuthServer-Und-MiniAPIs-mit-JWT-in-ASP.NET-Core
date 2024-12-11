namespace MiniApi6.API.Services
{
    public class PublicKeyService
    {



        private const string PublicKeyUrl = "http://localhost:5000/api/publickey"; // AuthServer'daki PublicKey endpoint
        private const string PublicKeyPath = "Keys/public.key"; // Public key'in saklanacağı yol

        public async Task<string> GetOrFetchPublicKeyAsync()
        {

            // Eğer dosya varsa, içeriğini oku ve geri dön
            if (File.Exists(PublicKeyPath))
            {
                return await File.ReadAllTextAsync(PublicKeyPath);
            }

            // Eğer dosya yoksa, AuthServer'dan public key'i çek
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(PublicKeyUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Public key alınırken hata oluştu!");
            }

            var publicKey = await response.Content.ReadAsStringAsync();

            // Çekilen public key'i dosyaya kaydet
            Directory.CreateDirectory(Path.GetDirectoryName(PublicKeyPath)!); // Keys klasörü yoksa oluştur
            await File.WriteAllTextAsync(PublicKeyPath, publicKey);

            return publicKey;
        }


    }
}
