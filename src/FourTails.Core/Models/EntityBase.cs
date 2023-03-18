namespace FourTails.Core.DomainModels;

public abstract class EntityBase
{
    public string CreatedBy {get; set;}
    public DateTime CreatedOn {get; set;}
    public bool IsActive {get; set;}
    public string? UpdatedBy {get; set;}
    public DateTime? UpdatedOn {get; set;}

    public EntityBase(User user)
    {
        CreatedOn = DateTime.UtcNow;
        CreatedBy = user.CreatedBy ?? throw new ArgumentNullException(nameof(user.CreatedBy));
        IsActive = true;
    }

    public void UpdatedByUser(User user)
    {
        UpdatedBy = user.UpdatedBy;
        UpdatedOn = DateTime.UtcNow;
        IsActive = user.IsActive;
    }
}