using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

public sealed record BorrowerRow(Borrower Borrower, int ActiveLoans, string WaitingFor);
