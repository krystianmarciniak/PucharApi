using HotChocolate;

namespace PucharApi.Domain;

public class User
{
  public int Id { get; set; }

  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string Email { get; set; } = default!;

  [GraphQLIgnore]
  public string PasswordHash { get; set; } = default!;

  [GraphQLIgnore]
  public string PasswordSalt { get; set; } = default!;
}
