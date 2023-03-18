using FourTails.Collections.Enums;

namespace FourTails.DTOs;

public record class Pet
(
    int Id,
    int Age,
    string Moniker,
    GenderEnum Gender
);