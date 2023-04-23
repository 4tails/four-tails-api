namespace FourTails.DTOs.Payload;

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