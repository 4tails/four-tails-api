using System.Text.Json.Serialization;

namespace FourTails.DTOs.Payload.Auth;

public record class AuthRequestDTO
(
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("password")]
    string Password
);