# Bonus B2.4 — Reading AI Output Critically

The AI's output looks confident. Often it shouldn't. Today's lesson is about the eval step — the part where you read what came back and decide whether to keep it. There is a five-point read you can do on every code response in about two minutes. Do it twenty times and it becomes automatic.

The point isn't that AI output is bad. Most of the time it is fine. The point is that the AI being confident is not the same as you checking it. The times it produces confident-looking nonsense are exactly the times a careful read saves you from accepting something broken.

> **Words to watch**
>
> - **invented API** — the AI calls a method that doesn't exist
> - **silent edge case** — the happy path works; the empty/null/zero case crashes
> - **style drift** — the output uses different patterns than your codebase
> - **over-helpful** — the output adds features you didn't ask for
> - **swallowed error** — the output catches an exception and ignores it

---

## The five-point read

For every code response, in order:

1. **Look for invented APIs.** Any method call that looks unfamiliar — *did you write that? Does it exist?* Grep the ones you are unsure about.
2. **Check the unhappy path.** What happens when the input is null, empty, zero, or huge? Did the code handle it? Did the AI's test handle it?
3. **Compare the style to a recent example.** Open `KingdomEfStore.cs` (or whichever file is the closest match). Same patterns? Same naming? Same return types?
4. **Look for over-help.** Did it change code you didn't ask about? *"I also refactored X"* is a warning sign.
5. **Look for swallowed errors.** `catch (Exception)` blocks that don't rethrow and don't log become silent bugs in production.

Two minutes per response. After twenty responses this is automatic — your eyes go to the weak points first, like a chef tasting their own soup before serving it.

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

The five-point read flags:

1. **Invented API?** `FirstOrDefaultAsync` is real EF Core. Pass.
2. **Unhappy path?** No match returns `null` (correct, through `FirstOrDefaultAsync`). Pass.
3. **Style drift?** Your existing methods aren't async. The AI made this one async without you asking. *Drift.*
4. **Over-help?** You didn't ask for async. *Yes — over-help.*
5. **Swallowed error?** `catch { return null; }` hides everything — including network errors, out-of-memory, and programming bugs. A real failure quietly becomes "no kingdoms." *Bad.*

Your reply back: *"Drop the async — existing methods aren't async. Drop the catch — let exceptions propagate."* Two more lines of feedback, and clean code on the second pass.

## When to push back, when to accept

| Situation | What to do |
| --- | --- |
| Invented API | Always reject; ask for the real API |
| Style drift on a project-wide pattern | Push back |
| Style drift on a small variation | Accept if it reads cleanly |
| Over-help | Push back; ask only for what you specified |
| Swallowed errors | Always reject; named exception types and `throw` are required |
| Unhappy path missed | Push back; ask for the test case |

The standard isn't "perfect output" — it is *output you can explain in the viva.* If you can explain every line, keep it. If you can't, ask the AI more questions until you can.

## Tinker

Pick a recent AI output you accepted. Run the five-point read on it now, looking back. Did you let something through? It happens — the read is a habit, and habits take repeating to build.

Try a vague prompt on purpose and watch the AI fill the gaps with guesses. Now run the same task with a tight scope (B2.3) and a pointer to an example file (B2.2). The over-help almost disappears.

Try a prompt that clearly says *"no async"* and *"no try/catch unless asked."* Compare the output. The AI follows narrow rules when you write them down.

## What you just did

You met the five-point read — invented APIs, unhappy paths, style drift, over-help, swallowed errors — and walked through a worked example where four of the five came up in one response. Two minutes per AI response is enough to catch most of what slips through. The standard is *output you can explain*, not *output that looks right* — those are very different things, and the five-point read is what shows you the difference.

**Key concepts you can now name:**

- **invented API** — non-existent method call
- **silent edge case** — broken on null, empty, or zero
- **style drift** — different patterns than your code
- **over-help** — does more than you asked
- **swallowed error** — `catch` without rethrow or log

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the big idea stuck. No one marks this one — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Without scrolling back up, write out the five-point read from memory. List all five checks you run on every code response. Then say the one standard that decides whether you keep the output or push back.

<details><summary>Stuck? Open this to check yourself.</summary>

The five-point read:

1. **Invented APIs** — does every method call actually exist? Grep the ones you're unsure about.
2. **The unhappy path** — what happens on null, empty, zero, or huge input?
3. **Style drift** — same patterns, naming, and return types as a recent file like `KingdomEfStore.cs`?
4. **Over-help** — did it change code you didn't ask about?
5. **Swallowed errors** — a `catch` that doesn't rethrow or log.

The standard: keep the output only if you can explain every line in the viva. If you can't explain it, ask more questions until you can — *output you can explain*, not *output that looks right*.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module B2.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module B2.4 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

B2.5 closes the bonus: a tour of the AI tools out there (Claude vs Copilot vs Cursor), then your written reflection on the year.
