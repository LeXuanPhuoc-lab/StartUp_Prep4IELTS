using System.Security.Cryptography;
using System.Text;

namespace Prep4IELTS.Business.Utils;

public static class HashHelper
{
    public static string HmacSha256(string text, string key)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();

        Byte[] textBytes = encoding.GetBytes(text);
        Byte[] keyBytes = encoding.GetBytes(key);
        Byte[] hashBytes;
        using (HMACSHA256 hash = new HMACSHA256(keyBytes))
            hashBytes = hash.ComputeHash(textBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static string ConvertToUtf8(string value)
    {
        Byte[] utf8Bytes = Encoding.UTF8.GetBytes(value);
        return Encoding.UTF8.GetString(utf8Bytes);
    }
}