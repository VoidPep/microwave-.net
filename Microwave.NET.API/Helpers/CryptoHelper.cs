using System.Security.Cryptography;
using System.Text;

namespace Microwave.NET.API.Helpers;

public static class CryptoHelper
{
    public static string HashPasswordSha256(string plainText)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static bool VerifyPassword(string plainText, string hash)
        => HashPasswordSha256(plainText) == hash.ToLowerInvariant();
}