using FourTails.Collections.Enums;

namespace FourTails.Core.DomainModels;

public class Role {
    public RoleEnum Id {get; set;}
    public string Name {get; set;}
    
    public Role(RoleEnum id, string name) 
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}