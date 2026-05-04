# Bonus B2.4 — Reading AI Output Critically

The AI's output looks confident. It often shouldn't. Today's lesson is about the eval step — the part where you read what came back and decide whether it ships. There's a five-thing read you can do on every code response in about two minutes; do it twenty times and it becomes automatic.

The point isn't that AI output is bad. Most of the time it's fine. The point is that the AI's confidence is not your eval — and the cases where it produces confident-looking nonsense are exactly the cases where a careful read saves you from accepting something broken.

> **Words to watch**
>
> - **invented API** — the AI calls a method that doesn't exist
> - **silent edge case** — the happy path works; the empty/null/zero case crashes
> - **style drift** — the output uses different patterns than your codebase
> - **over-helpful** — the output adds features you didn't ask for
> - **swallowed error** — the output catches an exception and ignores it

---

## The five-thing read

For every code response, in order:

1. **Skim for invented APIs.** Any method call that smells unfamiliar — *did you write that? Does it exist?* Grep the suspicious ones.
2. **Check the unhappy path.** What happens when the input is null, empty, zero, or huge? Did the code handle it? Did the AI's test handle it?
3. **Compare style to a recent example.** Pull up `KingdomEfStore.cs` (or whichever file is the closest neighbour). Same patterns? Same naming? Same return types?
4. **Look for over-help.** Did it touch code you didn't ask about? *"I also refactored X"* is a yellow flag.
5. **Hunt for swallowed errors.** `catch (Exception)` blocks that don't rethrow and don't log are silent bugs in production.

Two minutes per response. After twenty responses this is automatic — your eyes go to the failure points first, like a chef tasting their own soup before serving.

## Worked example

**You ask:** *"In `KingdomEfStore.cs`, add `LoadRichest(string ownerSub)`."*

**The AI returns:**

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

The five-thing read flags:

1. **Invented API?** `FirstOrDefaultAsync` is real EF Core. Pass.
2. **Unhappy path?** No match returns `null` (correct via `FirstOrDefaultAsync`). Pass.
3. **Style drift?** Your existing methods aren't async. The AI made this one async without you asking. *Drift.*
4. **Over-help?** The async addition wasn't requested. *Yes — over-help.*
5. **Swallowed error?** `catch { return null; }` swallows everything — including network errors, out-of-memory, and programming bugs. A real failure becomes "no kingdoms" silently. *Bad.*

Your response back: *"Drop the async — existing methods aren't async. Drop the catch — let exceptions propagate."* Two more lines of feedback; clean code on the second pass.

## When to push back, when to accept

| Situation | What to do |
| --- | --- |
| Invented API | Always reject; ask for the real API |
| Style drift on a project-wide pattern | Push back |
| Style drift on a small variation | Accept if it reads cleanly |
| Over-help | Push back; ask only for what you specified |
| Swallowed errors | Always reject; named exception types and `throw` are required |
| Unhappy path missed | Push back; ask for the test case |

The standard isn't "perfect output" — it's *output you can defend in the viva.* If you can explain every line, ship it. If you can't, ask the AI more questions until you can.

## Tinker

Pick a recent AI output you accepted. Run the five-thing read on it now, in retrospect. Did you let something through? It happens — the read is a habit, and habits take repetition to build.

Try a deliberately vague prompt and watch the AI fill the void with assumptions. Now run the same task with a tight scope (B2.3) and a pointer to an example file (B2.2). The over-help disappears almost entirely.

Try a prompt with explicit *"no async"* and *"no try/catch unless asked."* Compare the output. The AI follows narrow constraints when you write them down.

## What you just did

You met the five-thing read — invented APIs, unhappy paths, style drift, over-help, swallowed errors — and walked through a worked example where four of the five came up in one response. Two minutes per AI response is enough to catch most of what slips through. The standard is *output you can defend*, not *output that looks right* — those are very different things, and the five-thing read is what tells them apart.

**Key concepts you can now name:**

- **invented API** — non-existent method call
- **silent edge case** — broken on null, empty, or zero
- **style drift** — different patterns than your code
- **over-help** — does more than you asked
- **swallowed error** — `catch` without rethrow or log

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

B2.5 closes the bonus: a tour of the AI tool landscape (Claude vs Copilot vs Cursor), then your written reflection on the year.
