namespace Microwave.NET.DataStructures.DTOs;

public class MicrowaveStatusDto
{
    public int RemainingTime { get; set; }
    public int PowerLevel { get; set; }
    public string Progress { get; set; }
    public bool IsRunning { get; set; }
    public bool IsPaused { get; set; }
}
