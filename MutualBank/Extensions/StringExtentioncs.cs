namespace MutualBank.Extensions
{
    public static class StringExtentioncs
    {
        /// <summary>
        /// 將16進位字串轉換為byteArray
        /// </summary>
        /// <param name="source">欲轉換之字串</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string source)
        {
            byte[] result = null;

            if (!string.IsNullOrWhiteSpace(source))
            {
                var outputLength = source.Length / 2;
                var output = new byte[outputLength];

                for (var i = 0; i < outputLength; i++)
                {
                    output[i] = Convert.ToByte(source.Substring(i * 2, 2), 16);
                }
                result = output;
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToKvpList<T>(this T model)
        {
            return model.GetType().GetProperties()
                .Where(x => x.GetValue(model) != null)
                .Select(x => new KeyValuePair<string, string>(x.Name, x.GetValue(model).ToString()))
                .ToList();
        }
    }
}
