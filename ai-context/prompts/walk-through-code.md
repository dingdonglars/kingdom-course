# Walk Through Code

Use when: you want to understand code line-by-line — yours, the reference repo's, or something you found online.

## Template

> Walk me through what this `<language>` code does, line by line. Don't summarise; explain each line in plain English. Stop at any line I'd be confused by and explain *why it's written that way*.
>
> ```
> <paste code>
> ```

## Example

> Walk me through what this C# code does, line by line. Don't summarise; explain each line in plain English. Stop at any line I'd be confused by and explain *why it's written that way*.
>
> ```csharp
> var farms = kingdom.Buildings.OfType<Farm>().Where(f => f.HasFarmer).ToList();
> ```

## Why this prompt works

- "Line by line" prevents Claude from summarising at a high level (which is what you'd already done if you knew what the code did).
- "Plain English" keeps the explanation accessible.
- "Stop at any line I'd be confused by" invites Claude to elaborate on the surprising bits without you having to know in advance what's surprising.

## A meta-skill

This prompt is also Module 3.0's whole pedagogy — *senior developers read more than they write.* Use it on the reference repo when you open it; use it on Stack Overflow snippets you find; use it on your own code from six weeks ago. Reading code is a skill in itself.
