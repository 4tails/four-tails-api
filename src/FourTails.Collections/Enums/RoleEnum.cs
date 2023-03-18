using System.ComponentModel;

namespace FourTails.Collections.Enums;

public enum RoleEnum {
    [Description("Site administrator")]
    Administrator = 1,
    [Description("Pet minder")]
    PetMinder = 2,
    [Description("Pet owner")]
    PetOwner = 3
}