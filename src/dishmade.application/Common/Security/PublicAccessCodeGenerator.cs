using System.Security.Cryptography;

namespace dishmade.application.Common.Security;

public static class PublicAccessCodeGenerator
{
    public static string Generate()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}