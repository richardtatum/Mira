namespace Polling.Core.Models;

public class Host
{
    public string Url { get; set; } = null!;
    public int PollIntervalSeconds { get; set; }
    public TimeSpan PollInterval =>
        PollIntervalSeconds < 30 ? TimeSpan.FromSeconds(30) : TimeSpan.FromSeconds(PollIntervalSeconds);
}