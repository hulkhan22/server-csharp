namespace SPTarkov.Server.Core.Utils.Logger;

public record FileCheckpoint
{
    public DateTime Date { get; set; }
    public int Sequence { get; set; }
}
