namespace FourTails.DTOs.Payload.User;

public record class UserRegistrationDTO  
(
    int Age,
    string Address,
    string Email,
    string FirstName,
    string LastName,
    string Password
);