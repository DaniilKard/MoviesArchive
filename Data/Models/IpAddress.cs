namespace MoviesArchive.Data.Models;

public class IpAddress
{
    public int Id { get; set; }
    public string Value { get; set; }
    public bool IsBanned { get; set; }
    public List<User>? Users { get; set; }
}
