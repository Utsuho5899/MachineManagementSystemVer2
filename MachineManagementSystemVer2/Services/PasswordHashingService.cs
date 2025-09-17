using System;
using System.Security.Cryptography;

namespace MachineManagementSystemVer2.Services
{
    // 這是一個專門用來處理密碼雜湊與驗證的服務
    public class PasswordHashingService
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';

        // 將明碼密碼轉換成包含鹽(salt)的雜湊字串
        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            // 【修正】加入完整的錯誤處理機制，讓驗證過程更穩健
            if (string.IsNullOrEmpty(passwordHash))
            {
                return false;
            }

            try
            {
                var elements = passwordHash.Split(Delimiter);

                // 如果分割後的格式不對 (不等於 2)，就直接判定為失敗
                if (elements.Length != 2)
                {
                    return false;
                }

                var salt = Convert.FromBase64String(elements[0]);
                var hash = Convert.FromBase64String(elements[1]);

                var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

                return CryptographicOperations.FixedTimeEquals(hash, hashInput);
            }
            catch (FormatException)
            {
                // 如果在 Convert.FromBase64String 的過程中發生任何格式錯誤，
                // 就安全地捕捉它，並回傳驗證失敗。
                return false;
            }
        }
    }
}
