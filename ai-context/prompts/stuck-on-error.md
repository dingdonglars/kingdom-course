# Stuck on an Error

Use when: you've tried something and hit an error, and *you've already spent at least 20 minutes on it.*

## Template

> I'm stuck on this error:
>
> ```
> <paste exact error message and stack trace>
> ```
>
> Context: `<what I was trying to do, in one sentence>`.
>
> What I've tried:
> - `<thing 1 you tried, and what happened>`
> - `<thing 2 you tried, and what happened>`
>
> What I think might be wrong: `<your best guess, even if you're not sure>`.
>
> What's the next thing to try?

## Example

> I'm stuck on this error:
>
> ```
> System.IO.FileNotFoundException: Could not find file 'C:\Users\athos\Documents\kingdom\save.json'.
>    at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
>    ...
> ```
>
> Context: trying to load my saved kingdom on startup.
>
> What I've tried:
> - Checked the path — `Documents\kingdom\` doesn't exist on my machine.
> - Tried wrapping in `try/catch FileNotFoundException` and returning a fresh `Kingdom()` — works, but feels like a hack.
>
> What I think might be wrong: I shouldn't try to load if the file doesn't exist; I should check first with `File.Exists`.
>
> What's the next thing to try?

## Why this prompt works

- The exact error is pasted (not paraphrased).
- "What I've tried" shows you respected the 20-minute rule.
- Your guess gives Claude something to confirm or correct — and trains your debugging instinct.
- "Next thing to try" keeps Claude from solving the whole problem; it's a *hint*, not a fix.

## When NOT to use this

If you haven't tried anything yet, **don't paste the error to Claude**. Read the error first. Try something. Then come back.
