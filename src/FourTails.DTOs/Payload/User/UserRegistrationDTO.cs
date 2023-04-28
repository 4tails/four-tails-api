namespace FourTails.DTOs.Payload.User;

public record class UserRegistrationDTO  
(
    int Age,
    string Address,
    string CreatedBy,
    DateTime CreatedOn,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string Username
);