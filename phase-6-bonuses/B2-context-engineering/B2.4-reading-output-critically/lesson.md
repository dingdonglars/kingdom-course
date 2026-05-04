# Bonus B2.4 — Reading AI Output Critically

> **Hook:** the AI's output looks confident. **It often shouldn't.** Today: the eval step. Five things to check on every code response, with the muscle memory to do them in 2 minutes per response.

> **Words to watch**
> - **invented API** — the AI calls a method that doesn't exist
> - **silent edge case** — the happy path works; the empty/null/zero case crashes
> - **style drift** — output uses different patterns than your codebase
> - **over-helpful** — output adds features you didn't ask for
> - **swallowed error** — output catches an exception and ignores it ("just to be safe")

---

## The 5-thing read

For every code response, in order:

1. **Skim for invented APIs.** Any method call that smells unfamiliar — *did you write that? Does it exist?* Grep the suspicious ones.
2. **Check the unhappy path.** What happens when the input is null / empty / zero / huge? Did the code handle it? Did the AI test handle it?
3. **Compare style to a recent example.** Pull up `KingdomEfStore.cs`. Same patterns? Same naming? Same return types?
4. **Look for over-help.** Did it touch code you didn't ask about? "I also refactored X" is a yellow flag.
5. **Hunt for swallowed errors.** `catch (Exception)` blocks that don't rethrow + don't log = silent bugs in production.

Two minutes. Every response. After 20 responses, this is automatic.

## Worked example

**You ask:** *"In `KingdomEfStore.cs`, add `LoadRichest(string ownerSub)`."*

**AI returns:**

```csharp
public async Task<KingdomSlotInfo?> LoadRichestAsync(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    try
    {
        return await ctx.Kingdoms
            .Where(k => k.OwnerSub == ownerSub)
            .OrderByDescending(k => k.Gold)
            .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
            .FirstOrDefaultAsync();
    }
    catch
    {
        return null;
    }
}
```

The 5-thing read flags:

1. **Invented API?** `FirstOrDefaultAsync` exists in EF — fine. ✓
2. **Unhappy path?** `null` is returned on no match (correct via `FirstOrDefaultAsync`). ✓
3. **Style?** Your existing methods aren't async. Should the new one be? AI added `Async` without you asking. Drift.
4. **Over-help?** Async without being asked. Yes — over-help.
5. **Swallowed error?** `catch { return null; }` swallows everything — **bad**. A network error should propagate, not return null silently (your test would think "no kingdoms" instead of "the DB is down").

You'd respond: *"Drop the async (existing methods aren't); drop the catch (let exceptions propagate)."* Three more lines of feedback; clean code on the second pass.

## When to push back vs accept

| Situation | Action |
|---|---|
| Invented API | Always reject; ask for the real API |
| Style drift | Push back if it's a project-wide pattern; accept if it's a small variation |
| Over-help | Push back; ask only for what you specified |
| Swallowed errors | Always reject; named exception types and `throw` are required |
| Unhappy path missed | Push back; ask for the test case |

The bar isn't "perfect output" — it's "output I can defend in the viva." If you can explain it, ship it.

## Tinker

- Pick a recent AI output you accepted. Re-apply the 5-thing read. **Did you let something through?**
- Try a deliberately-vague prompt; expect over-help. Notice how the AI fills the void with assumptions.
- Try a prompt with explicit "no async" + "no try/catch unless asked." Compare outputs.

## Name it

- **Invented API** — non-existent method call.
- **Silent edge case** — broken on null/empty/zero.
- **Style drift** — different patterns than your code.
- **Over-help** — does more than asked.
- **Swallowed error** — `catch { }` without re-throw or log.

## The rule of the through-line

> **The AI's confidence is not your eval.** Read carefully. The 5-thing read is the muscle memory that catches AI-rot before it lands.

## Quiz / challenge

Open `quiz.md`.

## Connect

B2.5 closes B2: comparing tools (Claude vs Copilot vs Cursor) + the reflection write-up.