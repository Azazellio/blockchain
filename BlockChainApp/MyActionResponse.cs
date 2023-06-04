using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainApp;

public class MyActionResponse: IActionResult
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public object? Payload { get; set; }

    public static MyActionResponse OkResponse(object? payload)
    {
        return new MyActionResponse
        {
            IsSuccess = true,
            Payload = payload
        };
    }
    
    public static MyActionResponse ValidationErrorResponse(object error)
    {
        return new MyActionResponse
        {
            IsSuccess = false,
            Error = "400",
            Payload = JsonSerializer.Serialize(error)
        };
    }


    public static MyActionResponse ErrorResponse(string error, string code = null)
    {
        return new MyActionResponse
        {
            IsSuccess = false,
            Error = code,
            Payload = error
        };
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var json = JsonSerializer.Serialize(this);
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(json);
    }
}