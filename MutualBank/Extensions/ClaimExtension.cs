using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MutualBank.Extensions
{
    public static class ClaimExtension
    {
        public static int GetId(this ClaimsPrincipal User)
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

        }
    }

    public static class LambdaUtil
    {
        /// <summary>
        /// Dictionary 轉為物件(欄位不分大小寫)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dict">IDictionary 物件</param>
        /// <returns></returns>
        public static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                try
                {
                    if (dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                        Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                        Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                        object newA = Convert.ChangeType(item.Value, newT);
                        t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                    }
                }
                catch { }

            }
            return t;
        }

        /// <summary>
        /// 將model 轉換為KeyValuePair List, null值不轉
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">欲轉換之Model</param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ModelToKeyValuePairList<T>(T model)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            try
            {
                Type t = model.GetType();
                foreach (var p in t.GetProperties())
                {
                    string name = p.Name;
                    object value = p.GetValue(model, null);
                    if (value != null)
                    {
                        result.Add(new KeyValuePair<string, string>(name, value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }
    }

    public class CryptoUtil
    {
        /// <summary>
        /// 字串加密AES
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>加密後字串</returns>
        public static byte[] EncryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = dataKey;
                aes.IV = dataIV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(source, 0, source.Length);
                }
            }
        }

        /// <summary>
        /// 字串解密AES
        /// </summary>
        /// <param name="source">解密前字串</param>
        /// <param name="cryptoKey">解密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>解密後字串</returns>
        public static byte[] DecryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                // 智付通無法直接用PaddingMode.PKCS7，會跳"填補無效，而且無法移除。"
                // 所以改為PaddingMode.None並搭配RemovePKCS7Padding
                aes.Padding = PaddingMode.None;
                aes.Key = dataKey;
                aes.IV = dataIV;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return RemovePKCS7Padding(decryptor.TransformFinalBlock(source, 0, source.Length));
                }
            }
        }

        /// <summary>
        /// 加密後再轉 16 進制字串
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>加密後的字串</returns>
        public static string EncryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                var encryptValue = EncryptAES(Encoding.UTF8.GetBytes(source), cryptoKey, cryptoIV);

                if (encryptValue != null)
                {
                    result = BitConverter.ToString(encryptValue)?.Replace("-", string.Empty)?.ToLower();
                }
            }

            return result;
        }

        /// <summary>
        /// 16 進制字串解密
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>解密後的字串</returns>
        public static string DecryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                // 將 16 進制字串 轉為 byte[] 後
                byte[] sourceBytes = source.ToByteArray();

                if (sourceBytes != null)
                {
                    // 使用金鑰解密後，轉回 加密前 value
                    result = Encoding.UTF8.GetString(DecryptAES(sourceBytes, cryptoKey, cryptoIV)).Trim();
                }
            }

            return result;
        }


        /// <summary>
        /// 字串加密SHA256
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <returns>加密後字串</returns>
        public static string EncryptSHA256(string source)
        {
            string result = string.Empty;

            using (SHA256 algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(source));

                if (hash != null)
                {
                    result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
                }

            }

            return result;
        }

        private static byte[] RemovePKCS7Padding(byte[] data)
        {
            int iLength = data[data.Length - 1];
            var output = new byte[data.Length - iLength];
            Buffer.BlockCopy(data, 0, output, 0, output.Length);
            return output;
        }
    }
}
