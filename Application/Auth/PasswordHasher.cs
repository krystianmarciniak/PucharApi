using System.Security.Cryptography;

namespace PucharApi.Application.Auth;

public static class PasswordHasher
{
  public static (string hashBase64, string saltBase64) Hash(string password)
  {
    byte[] salt = RandomNumberGenerator.GetBytes(16);

    using var pbkdf2 = new Rfc2898DeriveBytes(
        password,
        salt,
        iterations: 100_000,
        hashAlgorithm: HashAlgorithmName.SHA256);

    byte[] hash = pbkdf2.GetBytes(32);

    return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
  }

  public static bool Verify(string password, string hashBase64, string saltBase64)
  {
    byte[] salt = Convert.FromBase64String(saltBase64);
    byte[] expectedHash = Convert.FromBase64String(hashBase64);

    using var pbkdf2 = new Rfc2898DeriveBytes(
        password,
        salt,
        iterations: 100_000,
        hashAlgorithm: HashAlgorithmName.SHA256);

    byte[] actualHash = pbkdf2.GetBytes(32);

    return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
  }
}
