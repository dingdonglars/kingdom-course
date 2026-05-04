# Quiz — B2.2

## 1. What's a *scaffold file*?

a. A project-level doc the AI reads on session start (e.g., ARCHITECTURE.md, STANDARDS.md, ai-context/CLAUDE.md)
b. A new C# project type
c. A test fixture
d. Required for AI to work

## 2. Why is *short* a virtue for scaffold files?

a. They cost tokens. Padding hurts. Aim for 30-60 lines per doc — anything more is bloat that the AI has to read past.
b. They run faster
c. Required by Claude
d. Style preference

## 3. What does an *example file* in `ai-context/examples/` give you?

a. A hand-picked, in-style snippet you can point the AI at — *"match the style of this example."* Cheap leverage.
b. Test cases
c. Documentation only
d. Nothing useful

## 4. Why mention "stale scaffolds are worse than no scaffolds"?

a. They mislead. The AI follows them confidently to the wrong answer. Audit + update after every block.
b. They look bad
c. Required by .NET
d. Performance

## 5. The "you are here" file header — what's it for?

a. Top-of-file 5-line comment naming the file's role + conventions + related files. Orientation hint for any reader (AI or human) opening mid-task.
b. Required by Visual Studio
c. SEO
d. Nothing special