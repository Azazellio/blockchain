namespace BlockChainApp.DTOs;

public class TransferBlockDto
{
    public string From { get; init; }
    public string To { get; init; }
    public string Property { get; init; }
    public DateTime Time { get; init; }
    public string Sign { get; init; }
}