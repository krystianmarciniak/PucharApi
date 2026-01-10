namespace PucharApi.Domain;

public class Match
{
  public int Id { get; set; }

  public int Round { get; set; }

  public User Player1 { get; set; } = default!;
  public User? Player2 { get; set; }
  public User? Winner { get; set; }
  public void Play(User winner)
  {
    if (Player2 is null)
    {
      return;
    }

    bool winnerIsPlayer = winner.Id == Player1.Id || winner.Id == Player2.Id;
    if (!winnerIsPlayer)
      throw new InvalidOperationException("Zwycięzca musi być jednym z graczy meczu.");

    Winner = winner;
  }
}
