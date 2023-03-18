using FourTails.Collections.Enums;
using FourTails.Core.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace FourTails.DataAccess.SeedData;

public static class RolesBuilderExtension
{
    public static void AddRoles(this ModelBuilder builder) 
    {
        builder.Entity<Role>().HasData
        (
            new Role(RoleEnum.Administrator, "SiteAdministrator"),
            new Role(RoleEnum.PetOwner, "PetOwner"),
            new Role(RoleEnum.PetMinder, "PetMinder")
        );
    }
}