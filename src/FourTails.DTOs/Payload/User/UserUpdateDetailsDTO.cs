using FourTails.Core.DomainModels;

namespace FourTails.DTOs.Payload.User;

public record class UserUpdateDetailsDTO
(
    string Id,
    int Age,
    string Address,
    string FirstName,
    string LastName,
    string UserName
);