namespace PucharApi.Domain;

public class Bracket
{
  public int Id { get; set; }

  public List<Match> Matches { get; set; } = new();

  // ⭐ Diagram → później mutacja GraphQL
  public void GenerateBracket(List<User> participants)
  {
    if (participants.Count < 2)
      throw new InvalidOperationException("Brak wystarczającej liczby uczestników.");

    // Prosto: parujemy po kolei (B-approach: czytelnie, bez przesady)
    Matches.Clear();

    int round = 1;
    for (int i = 0; i < participants.Count; i += 2)
    {
      var p1 = participants[i];
      User? p2 = (i + 1 < participants.Count) ? participants[i + 1] : null;

      // Jeżeli nieparzysta liczba, ostatni dostaje "wolny los" (bye)
      var match = new Match
      {
        Round = round,
        Player1 = p1,
        Player2 = p2,
        Winner = p2 is null ? p1 : null
      };

      Matches.Add(match);
    }
  }

  // ⭐ Diagram → później query albo mutation (u prowadzącego: metoda z diagramu = mutacja)
  public List<Match> GetMatchesForRound(int round)
      => Matches.Where(m => m.Round == round).ToList();
}
