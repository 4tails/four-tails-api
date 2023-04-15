using Microsoft.AspNetCore.Identity;
namespace FourTails.Core.DomainModels;

public class EntityBase : IdentityUser
{
    public bool IsActive {get; set;}
    public DateTime CreatedOn {get; set;}
    public DateTime? UpdatedOn {get; set;}
    public string? CreatedBy {get; set;}
    public string? UpdatedBy {get; set;}

    public void Timestamp(User user)
    {
        CreatedBy = user.CreatedBy ?? throw new ArgumentNullException(nameof(user.CreatedBy));
        CreatedOn = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdatedByUser(User user)
    {
        IsActive = user.IsActive;
        UpdatedBy = user.UpdatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}