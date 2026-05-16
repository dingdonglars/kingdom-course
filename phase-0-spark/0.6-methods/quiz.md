# Quiz — Module 0.6

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What does `void` mean as a return type?

- **a.** The method takes no parameters and ignores anything passed to it
- **b.** The method runs but gives no value back to whoever called it
- **c.** The method is undefined and will fail when the program runs
- **d.** The method runs in the background while other code keeps going

## 2. Given `int Square(int x) => x * x;`, what is `Square(7)`?

- **a.** `7` — the original input is returned unchanged
- **b.** `14` — the input doubled, since `=>` is sugar for addition
- **c.** `49` — the body `x * x` evaluated with `x = 7`
- **d.** A compile error — methods can't use `*` directly

## 3. Can two static methods in the same class share the name `Square`?

- **a.** No — every method name in a class must be globally unique
- **b.** Yes, if they take different parameter *types* (an overload pair)
- **c.** Yes, if they take the same parameters but return different types
- **d.** Yes, if they have different parameter *names* but the same types

## 4. What's the difference between a *parameter* and an *argument*?

- **a.** They mean the same thing in C# and are interchangeable
- **b.** Parameter is the name in the definition; argument is the value passed at the call
- **c.** Parameter is the value passed; argument is the name in the definition
- **d.** Parameters are required; arguments are optional and can be skipped

## 5. Are `int AddGold(int a, int b) => a + b;` and `int AddGold(int a, int b) { return a + b; }` the same?

- **a.** No — the arrow form runs faster because it skips the `return` keyword
- **b.** No — the arrow form is for properties only, not methods
- **c.** Yes — the arrow form is expression-bodied shorthand for the block form
- **d.** No — they look the same but compile to different intermediate code

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
