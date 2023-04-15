using FourTails.Collections.Enums;

namespace FourTails.Core.DomainModels;

public class Pet {

    public int Id {get; set;}
    public int Age {get; set;}
    public string Moniker {get; set;}
    public GenderEnum Gender {get; set;}
    public User PetOwner {get; set;}
    public string PetOwnerId {get; set;}

    public Pet(){}

    public Pet(int id, int age, string moniker, GenderEnum gender, User petOwner, string petOwnerId)
    {
        Id = id;
        Age = age;
        Moniker = moniker ?? throw new ArgumentNullException(nameof(moniker));
        Gender = gender;
        PetOwner = petOwner ?? throw new ArgumentNullException(nameof(petOwner));
        PetOwnerId = petOwnerId;
    }
}