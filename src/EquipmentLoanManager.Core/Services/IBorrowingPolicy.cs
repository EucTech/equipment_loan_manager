using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

public interface IBorrowingPolicy
{
    OperationResult CheckEligibility(string? borrowerId, EquipmentCategory category);
}
