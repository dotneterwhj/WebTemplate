namespace Abner.Domain.Core;

public abstract class FullAuditAggregateRoot : AggregateRoot, IFullAuditable
{
    public DateTime CreationTime { get; private set; }
    public string CreatorId { get; private set; }
    public DateTime? ModificationTime { get; private set; }
    public string? ModificatorId { get; private set; }
    public DateTime? DeletionTime { get; private set; }
    public string? DeletorId { get; private set; }

    public override void SoftDelete()
    {
        base.SoftDelete();
        DeletionTime = DateTime.Now;
    }
}

public abstract class FullAuditAggregateRoot<TKey> : AggregateRoot<TKey>, IFullAuditable
{
    public DateTime CreationTime { get; private set; }
    public string CreatorId { get; private set; }
    public DateTime? ModificationTime { get; private set; }
    public string? ModificatorId { get; private set; }
    public DateTime? DeletionTime { get; private set; }
    public string? DeletorId { get; private set; }

    public override void SoftDelete()
    {
        base.SoftDelete();
        DeletionTime = DateTime.Now;
    }
}