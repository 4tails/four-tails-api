using FourTails.Collections.Enums;

namespace FourTails.Core.DomainModels;

public class User : EntityBase
{
    public int Id {get; set;}
    public string Address {get; set;}
    public int Age {get; set;}
    public string Email {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public ICollection<Message> Messages {get; init;}
    public RoleEnum Role {get; set;}
    public ICollection<Pet> Pets {get; init;}

    public User(string address, int age, string email, string firstName, string lastName, RoleEnum role, User user) : base(user)
    {
        Address = address;
        Age = age;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName?? throw new ArgumentNullException(nameof(lastName));
        Messages = new List<Message>();
        Role = role;
        Pets = new List<Pet>();
    }
}
