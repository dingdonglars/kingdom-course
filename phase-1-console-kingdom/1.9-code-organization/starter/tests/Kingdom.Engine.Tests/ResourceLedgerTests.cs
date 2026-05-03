using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Resources;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;

namespace Kingdom.Engine.Tests;

public class ResourceLedgerTests
{
    [Fact]
    public void NewLedger_AllResourcesStartAtZero()
    {
        var ledger = new ResourceLedger();
        ledger.Get(Resource.Gold).ShouldBe(0);
        ledger.Get(Resource.Wood).ShouldBe(0);
        ledger.Get(Resource.Stone).ShouldBe(0);
        ledger.Get(Resource.Food).ShouldBe(0);
    }

    [Fact]
    public void Add_IncreasesAmount()
    {
        var ledger = new ResourceLedger();
        ledger.Add(Resource.Gold, 50);
        ledger.Get(Resource.Gold).ShouldBe(50);
    }

    [Fact]
    public void Spend_WhenSufficient_ReturnsTrueAndDecreases()
    {
        var ledger = new ResourceLedger();
        ledger.Add(Resource.Gold, 100);
        var ok = ledger.Spend(Resource.Gold, 30);
        ok.ShouldBeTrue();
        ledger.Get(Resource.Gold).ShouldBe(70);
    }

    [Fact]
    public void Spend_WhenInsufficient_ReturnsFalseAndDoesNotChange()
    {
        var ledger = new ResourceLedger();
        ledger.Add(Resource.Gold, 10);
        var ok = ledger.Spend(Resource.Gold, 50);
        ok.ShouldBeFalse();
        ledger.Get(Resource.Gold).ShouldBe(10);
    }

    [Fact]
    public void Add_NegativeAmount_Throws()
    {
        var ledger = new ResourceLedger();
        Should.Throw<ArgumentException>(() => ledger.Add(Resource.Gold, -5));
    }

    [Theory]
    [InlineData(Resource.Gold, 100, 30, 70)]
    [InlineData(Resource.Wood, 50, 50, 0)]
    [InlineData(Resource.Stone, 1, 1, 0)]
    public void Spend_VariousAmounts(Resource r, int initial, int spend, int expected)
    {
        var ledger = new ResourceLedger();
        ledger.Add(r, initial);
        ledger.Spend(r, spend).ShouldBeTrue();
        ledger.Get(r).ShouldBe(expected);
    }
}