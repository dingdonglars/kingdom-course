using Kingdom.Engine;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence.EfCore;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class MultiUserTests
{
    [Fact]
    public void Save_ScopedToOwner_OtherUserCannotSee()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));
            store.Save("bob",   new global::Kingdom.Engine.Kingdom("BobsTown"));

            store.ListSlots("alice").Single().Name.ShouldBe("AliceVille");
            store.ListSlots("bob").Single().Name.ShouldBe("BobsTown");
            store.ListSlots("eve").ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Load_OfOtherUsersKingdom_Throws()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var aliceId = store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));

            Should.Throw<InvalidOperationException>(() =>
                store.Load("bob", aliceId, new SystemRandom(0), new SystemClock()));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void Delete_OfOtherUsersKingdom_NoOps()
    {
        var path = Path.Combine(Path.GetTempPath(), $"mu-{Guid.NewGuid():N}.db");
        try
        {
            var store = new KingdomEfStore(path);
            var aliceId = store.Save("alice", new global::Kingdom.Engine.Kingdom("AliceVille"));

            store.Delete("bob", aliceId);                           // no-op (scope doesn't match)
            store.ListSlots("alice").Count.ShouldBe(1);             // alice's still there

            store.Delete("alice", aliceId);                         // real delete
            store.ListSlots("alice").ShouldBeEmpty();
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
