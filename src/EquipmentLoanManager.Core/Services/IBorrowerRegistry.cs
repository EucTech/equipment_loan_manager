using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

public interface IBorrowerRegistry
{
    IReadOnlyList<Borrower> All { get; }

    OperationResult<Borrower> Add(string? name, string? id = null);

    bool Remove(string id);

    Borrower? Find(string? id);
}
