using System.Text.Json.Serialization;

namespace FourTails.DTOs.PayLoad;

public record class AuthResponseDTO
(
    [property: JsonPropertyName("userName")]
    string UserName,
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("token")]
    string Token
);