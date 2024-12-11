using AuthServer.API.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;


namespace AuthServer.API.Services
{
    public class KeyService
    {

        private const string PrivateKeyPath = "Keys/private.key";
        private const string PublicKeyPath = "Keys/public.key";

        private readonly string _securityKey;

        public KeyService(IOptions<CustomTokenOption> tokenOptions)
        {
            _securityKey = tokenOptions.Value.SecurityKey;

            // Anahtarları kontrol et ve gerekirse oluştur
            EnsureKeysExist();
        }




        /// <summary>
        /// RSA Private ve Public Key oluşturur ve /Keys klasörüne kaydeder.
        /// Eğer keyler mevcutsa, dosyalardan yükler.
        /// </summary>
        public void EnsureKeysExist()
        {
            if (!Directory.Exists("Keys"))
            {
                Directory.CreateDirectory("Keys");
            }

            if (!File.Exists(PrivateKeyPath) || !File.Exists(PublicKeyPath))
            {
                GenerateAndSaveKeys();
            }
        }

        /// <summary>
        /// Private ve Public Key üretir ve dosyalara kaydeder.
        /// </summary>
        private void GenerateAndSaveKeys()
        {

            using var rsa = RSA.Create();
            rsa.KeySize = 2048;

            // Private Key'i oluştur ve PEM formatında kaydet
            var privateKeyBytes = rsa.ExportRSAPrivateKey();
            var privateKeyPem = "-----BEGIN PRIVATE KEY-----\n" +
                                Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                                "\n-----END PRIVATE KEY-----";
            File.WriteAllText(PrivateKeyPath, privateKeyPem);

            // Public Key'i oluştur ve PEM formatında kaydet
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            var publicKeyPem = "-----BEGIN PUBLIC KEY-----\n" +
                               Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                               "\n-----END PUBLIC KEY-----";
            File.WriteAllText(PublicKeyPath, publicKeyPem);

            /*
            // Private Key ve Public Key oluştur
            var privateKeyBytes = rsa.ExportRSAPrivateKey();
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();

            // SecurityKey'i bir salt (tuz) olarak kullanarak keylerin bütünlüğünü sağlayın
            var saltedPrivateKey = ApplySalt(privateKeyBytes, _securityKey);
            var saltedPublicKey = ApplySalt(publicKeyBytes, _securityKey);

            // Private Key'i kaydet
            File.WriteAllBytes(PrivateKeyPath, saltedPrivateKey);

            // Public Key'i kaydet
            File.WriteAllBytes(PublicKeyPath, saltedPublicKey);
            */

        }


        /// <summary>
        /// Bir byte dizisine salt (tuz) uygular.
        /// </summary>
        private byte[] ApplySalt(byte[] keyData, string salt)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            return hmac.ComputeHash(keyData);
        }



        /// <summary>
        /// Private Key'i dosyadan yükler.
        /// </summary>
        public RSA GetPrivateKey()
        {

            var privateKeyPem = File.ReadAllText(PrivateKeyPath);

            // PEM başlık ve sonlarını temizle
            var privateKeyBase64 = privateKeyPem
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            // Base64'ten byte array'e dönüştür
            var privateKeyBytes = Convert.FromBase64String(privateKeyBase64);

            // Private Key'i içe aktar
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            return rsa;


            /*
            var saltedPrivateKeyBytes = File.ReadAllBytes(PrivateKeyPath);
            var privateKeyBytes = RemoveSalt(saltedPrivateKeyBytes, _securityKey);

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            return rsa;
            */
        }


        /// <summary>
        /// Public Key'i dosyadan yükler.
        /// </summary>
        public string GetPublicKey()
        {

            // Public key'i dosyadan PEM formatında okuyup döndür
            return File.ReadAllText(PublicKeyPath);

            /*
            var saltedPublicKeyBytes = File.ReadAllBytes(PublicKeyPath);
            var publicKeyBytes = RemoveSalt(saltedPublicKeyBytes, _securityKey);

            return "-----BEGIN PUBLIC KEY-----\n" +
                   Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                   "\n-----END PUBLIC KEY-----";
            */
        }
    

        /// <summary>
        /// Salt uygulanmış bir byte dizisinden orijinal key'i çıkarır.
        /// </summary>
        private byte[] RemoveSalt(byte[] saltedKeyData, string salt)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            return hmac.ComputeHash(saltedKeyData);
        }



        public static RSA LoadPublicKey()
        {
            var publicKeyPem = File.ReadAllText("Keys/public.key");

            // PEM formatındaki public key'i temizle
            var publicKeyBase64 = publicKeyPem
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            // Base64'ten byte array'e dönüştür ve yükle
            var publicKeyBytes = Convert.FromBase64String(publicKeyBase64);
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
            return rsa;
        }



    }
}
