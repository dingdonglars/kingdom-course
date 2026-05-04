# Quiz answers — Module 4.0

## 1. a
Context engineering = curating the slice of text the AI reads before answering. Same question + better context = better answer. Same question + sloppy context = generic tutorial code that breaks against your project.

## 2. a
Goal (one sentence) · where it goes (file path + surrounding code) · conventions (your standards) · one similar example · what to avoid (traps). Five things, each pre-empts a class of failure mode.

## 3. b
The AI confidently calls a method like `db.Kingdoms.GetRichest()` that doesn't exist anywhere in your codebase. It made it up. Audit AI output for invented APIs — they're the silent failure that compiles in some imagined alternate universe.

## 4. a
Without a reference set, the AI starts from "what does C#/EF Core code usually look like?" — a tutorial baseline. With your reference set (STANDARDS, ARCHITECTURE, examples), it starts from "what does *this project* look like?" — much closer to where you want it to land.

## 5. a
Context is leverage. The 5 minutes you spend writing a 30-line `ARCHITECTURE.md` saves 50 minutes per AI interaction over the next 6 months. The discipline of curating context is the post-gate skill.