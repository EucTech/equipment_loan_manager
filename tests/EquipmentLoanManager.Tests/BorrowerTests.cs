using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Tests;

public class BorrowerTests
{
    [Fact]
    public void AddBorrower_GeneratesUniqueIdsWhenNoneGiven()
    {
        var manager = TestSession.Create();

        var first = manager.Register("Amara");
        var second = manager.Register("Kwame");

        Assert.NotEqual(first.Id, second.Id);
        Assert.Equal(2, manager.Borrowers.All.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void AddBorrower_RejectsEmptyName(string? name)
    {
        var manager = TestSession.Create();

        var result = manager.AddBorrower(name);

        Assert.False(result.Succeeded);
        Assert.Empty(manager.Borrowers.All);
    }

    [Fact]
    public void AddBorrower_RejectsDuplicateId()
    {
        var manager = TestSession.Create();
        manager.AddBorrower("Amara", "S100");

        var result = manager.AddBorrower("Kwame", "s100");

        Assert.False(result.Succeeded);
        Assert.Single(manager.Borrowers.All);
    }

    [Fact]
    public void RemoveBorrower_RemovesBorrowerWithoutActiveLoans()
    {
        var manager = TestSession.Create();
        var borrower = manager.Register("Amara");

        var result = manager.RemoveBorrower(borrower.Id);

        Assert.True(result.Succeeded);
        Assert.Null(manager.Borrowers.Find(borrower.Id));
    }

    [Fact]
    public void RemoveBorrower_RejectsBorrowerWithActiveLoan()
    {
        var manager = TestSession.Create(laptops: 1);
        var borrower = manager.Register("Amara");
        manager.Borrow(borrower.Id, EquipmentCategory.Laptop);

        var result = manager.RemoveBorrower(borrower.Id);

        Assert.False(result.Succeeded);
        Assert.NotNull(manager.Borrowers.Find(borrower.Id));
    }

    [Fact]
    public void RemoveBorrower_AlsoRemovesTheirWaitingListEntries()
    {
        var manager = TestSession.Create(laptops: 1);
        var holder = manager.Register("Amara");
        var waiter = manager.Register("Kwame");
        manager.Borrow(holder.Id, EquipmentCategory.Laptop);
        manager.JoinWaitingList(waiter.Id, EquipmentCategory.Laptop);

        manager.RemoveBorrower(waiter.Id);

        Assert.Empty(manager.WaitingLists.GetQueue(EquipmentCategory.Laptop));
    }

    [Fact]
    public void RemoveBorrower_PreservesCompletedLoanHistory()
    {
        var manager = TestSession.Create(laptops: 1);
        var borrower = manager.Register("Amara");
        var loan = manager.Borrow(borrower.Id, EquipmentCategory.Laptop).Value!;
        manager.Return(loan.Id);

        manager.RemoveBorrower(borrower.Id);

        var history = Assert.Single(manager.Loans.Completed);
        Assert.Equal("Amara", history.Borrower.Name);
        Assert.Equal(loan.Item.Id, history.Item.Id);
    }
}
