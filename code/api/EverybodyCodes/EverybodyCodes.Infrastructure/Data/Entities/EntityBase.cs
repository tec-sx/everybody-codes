namespace EverybodyCodes.Infrastructure.Data.Entities;

public abstract record EntityBase
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
