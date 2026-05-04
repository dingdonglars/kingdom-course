using Kingdom.Api.Dtos;
using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoint_GET_Kingdom_Tests
{
    [Fact]
    public void TickResponse_Record_HasExpectedProperties()
    {
        var tr = new TickResponse(1, "X", 2, 100, 50, 20, 30);
        tr.DaysAdvanced.ShouldBe(1);
        tr.CurrentDay.ShouldBe(2);
    }
}
