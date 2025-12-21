namespace Server.Shared.Models.Dtos;

public class ErrorDto
{
    public string Code { get; set; }
    public string Message { get; set; }

    public static ErrorDto Of(string code, string message) => new ErrorDto { Code = code, Message = message };
}
