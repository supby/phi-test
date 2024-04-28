namespace Phi.Model.Client;

public class Story
{
    public int Id { get; set; }
    public required string By { get; set; }
    public int Score { get; set; }
    public uint Time { get; set; }
    public required string Title { get; set; }
}