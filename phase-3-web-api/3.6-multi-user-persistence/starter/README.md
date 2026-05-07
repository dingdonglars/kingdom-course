# Module 3.6 starter — multi-user persistence

Delta from Module 3.5:

- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEntity.cs` — adds `string OwnerSub`
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomDbContext.cs` — adds `HasIndex(k => k.OwnerSub)`
- **MODIFIED:** `Kingdom.Persistence/EfCore/KingdomEfStore.cs` — every method takes `ownerSub` and scopes its query
- **MODIFIED:** `Kingdom.Api/Program.cs` — extracts `sub` from the auth cookie, passes to the store
- **NEW:** `tests/Kingdom.Persistence.Tests/MultiUserTests.cs`

After applying:

```powershell
dotnet ef migrations add AddOwnerSub --project Kingdom.Persistence --startup-project Kingdom.Console
dotnet build
dotnet test                        # the multi-user tests pass
dotnet run --project Kingdom.Api
# Sign in as user A, create kingdoms; sign in as user B — only own kingdoms visible
```

The store's API is now intentionally noisy: every method requires `ownerSub`. **A caller who forgets it gets a compile error, not a security bug.**