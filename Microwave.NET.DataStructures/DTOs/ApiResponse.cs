using System.Text.Json;

namespace Microwave.NET.DataStructures.DTOs;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Data { get; set; }
    public string? Error { get; set; }

    public static ApiResponse Ok(string data) => new() { Success = true, Data = data };
    public static ApiResponse Ok(object data) => new() { Success = true, Data = JsonSerializer.Serialize(data) };
    public static ApiResponse Fail(string error) => new() { Success = false, Error = error };
}
