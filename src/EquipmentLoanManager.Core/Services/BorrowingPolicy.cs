using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

public sealed class BorrowingPolicy(IBorrowerRegistry borrowers, ILoanLedger loans) : IBorrowingPolicy
{
    public const int MaxActiveLoans = 2;

    public OperationResult CheckEligibility(string? borrowerId, EquipmentCategory category)
    {
        var borrower = borrowers.Find(borrowerId);
        if (borrower is null)
            return OperationResult.Failure("Select a borrower who is currently added to the application.");

        var activeLoans = loans.ActiveFor(borrower.Id);

        if (activeLoans.Count >= MaxActiveLoans)
            return OperationResult.Failure($"{borrower.Name} already has {MaxActiveLoans} active loans.");

        if (activeLoans.Any(l => l.Item.Category == category))
            return OperationResult.Failure(
                $"{borrower.Name} already holds a {EquipmentCatalog.Label(category)}.");

        return OperationResult.Success($"{borrower.Name} is eligible.");
    }
}
