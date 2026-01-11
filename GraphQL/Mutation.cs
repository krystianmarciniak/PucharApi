using Microsoft.EntityFrameworkCore;
using PucharApi.Application.Auth;
using PucharApi.Domain;
using PucharApi.Infrastructure;

namespace PucharApi.GraphQL;

public class Mutation
{
  public async Task<AuthPayload> Register(
      RegisterInput input,
      [Service] PucharDbContext db,
      [Service] JwtTokenService jwt)
  {
    var exists = await db.Users.AnyAsync(u => u.Email == input.Email);
    if (exists)
      throw new GraphQLException("Email jest już zajęty.");

    var (hash, salt) = PasswordHasher.Hash(input.Password);

    var user = new User
    {
      FirstName = input.FirstName,
      LastName = input.LastName,
      Email = input.Email,
      PasswordHash = hash,
      PasswordSalt = salt
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    var token = jwt.CreateToken(user);
    return new AuthPayload(token, user);
  }

  public async Task<AuthPayload> Login(
      LoginInput input,
      [Service] PucharDbContext db,
      [Service] JwtTokenService jwt)
  {
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == input.Email);
    if (user is null)
      throw new GraphQLException("Nieprawidłowy email lub hasło.");

    var ok = PasswordHasher.Verify(input.Password, user.PasswordHash, user.PasswordSalt);
    if (!ok)
      throw new GraphQLException("Nieprawidłowy email lub hasło.");

    var token = jwt.CreateToken(user);
    return new AuthPayload(token, user);
  }
}

public record RegisterInput(string FirstName, string LastName, string Email, string Password);
public record LoginInput(string Email, string Password);
public record AuthPayload(string Token, User User);
