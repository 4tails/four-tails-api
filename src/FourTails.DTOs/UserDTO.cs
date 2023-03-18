namespace FourTails.DTOs;

public record class UserDTO
(
    int Id,
    int Age,
    string FirstName,
    string Lastname,
    IEnumerable<Message> Messages,
    IEnumerable<Pet> Pets
);