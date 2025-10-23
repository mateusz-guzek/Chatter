using System.Security.Cryptography;

namespace Server.Shared.Utility;

public class Passwords
{
    
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }

    public static string HashPassword(string password)
    {
        var salt = GenerateSalt();
        return HashPassword(password, salt);
    }

    private static string HashPassword(string password, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512);
        var hash = pbkdf2.GetBytes(32);
        var hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);
        return Convert.ToBase64String(hashBytes);
        
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[16];
        Array.Copy(hashBytes, 0, salt,0, 16);
        
        var newPasswordHash =  Convert.FromBase64String(HashPassword(password, salt));
        return CryptographicOperations.FixedTimeEquals(hashBytes, newPasswordHash);
    }
}