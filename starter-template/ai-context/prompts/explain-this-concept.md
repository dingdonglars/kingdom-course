# Explain This Concept (Like I'm a Beginner)

Use when: you've read about a concept and want a clearer walkthrough.

## Template

> Explain `<concept>` like I'm a beginner. I've read `<source>` and I think I understand `<what you got>`, but I'm confused about `<what you don't get>`.
>
> I know `<related concepts you've already met>`. I haven't yet met `<concepts you're not sure about>`.
>
> Use a small concrete example, ideally something I could try in C# / TypeScript / Luau.

## Example

> Explain *interfaces in C#* like I'm a beginner. I've read the Microsoft docs and I think I understand that an interface is a "contract" for what methods a class must have, but I'm confused about *why I'd use one* instead of just inheriting from a base class.
>
> I know classes, methods, and inheritance. I haven't yet met dependency injection or generics.
>
> Use a small concrete example, ideally something I could try in C#.

## Why this prompt works

- Names what you already understand → Claude doesn't waste time re-explaining.
- Names what you don't understand → Claude focuses on the gap.
- Names your level (concepts known/unknown) → Claude calibrates.
- Asks for a concrete example → no abstract pontification.
</content>
</invoke>