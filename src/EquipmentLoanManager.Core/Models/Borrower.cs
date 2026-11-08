namespace EquipmentLoanManager.Core.Models;

public sealed class Borrower
{
    public Borrower(string id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Id = id.Trim();
        Name = name.Trim();
    }

    public string Id { get; }

    public string Name { get; }

    public string DisplayName => $"{Name} ({Id})";

    public override string ToString() => DisplayName;
}
