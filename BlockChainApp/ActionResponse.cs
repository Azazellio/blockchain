namespace BlockChainApp;

public class ActionResponse
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public object? Payload { get; set; }

    public static ActionResponse OkResponse(object? payload)
    {
        return new ActionResponse
        {
            IsSuccess = true,
            Payload = payload
        };
    }

    public static ActionResponse ErrorResponse(string error)
    {
        return new ActionResponse
        {
            IsSuccess = false,
            Error = error
        };
    }
}