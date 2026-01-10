namespace PucharApi.Domain;

public class Tournament
{
  public int Id { get; set; }

  public string Name { get; set; } = default!;
  public DateTime StartDate { get; set; }
  public string Status { get; set; } = "DRAFT";

  public List<User> Participants { get; set; } = new();
  public Bracket? Bracket { get; set; }

  public void AddParticipant(User user)
  {
    if (Status != "DRAFT")
      throw new InvalidOperationException("Nie można dodać uczestnika po starcie turnieju.");

    if (Participants.Any(p => p.Id == user.Id))
      return;

    Participants.Add(user);
  }

  public void Start()
  {
    if (Status != "DRAFT")
      throw new InvalidOperationException("Turniej można uruchomić tylko ze statusu DRAFT.");

    if (Participants.Count < 2)
      throw new InvalidOperationException("Turniej wymaga co najmniej 2 uczestników.");

    Status = "ACTIVE";
    StartDate = DateTime.UtcNow;
  }

  public void Finish()
  {
    if (Status != "ACTIVE")
      throw new InvalidOperationException("Turniej można zakończyć tylko ze statusu ACTIVE.");

    Status = "FINISHED";
  }
}
