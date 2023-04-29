using FourTails.Core.DomainModels;

namespace FourTails.UnitTests.TestHelpers;

public static class UserHelper
{
    public static User GetUser()
    {
        return new User
        {
            Id = "9f44cb79-821d-4b42-8550-4ea01569d6b6",
            FirstName = "John",
            LastName = "Smith",
            UserName = "JSmith",
            Email = "john_smyth@test.com",
            Address = "Cork, Co.Cork, Ireland",
            IsActive = true
        };
    }
}