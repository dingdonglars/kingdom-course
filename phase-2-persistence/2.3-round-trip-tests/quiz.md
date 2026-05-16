# Quiz — Module 2.3

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. What's a *round-trip test*?

- **a.** A test that runs through the same code path twice
- **b.** A test that saves the model, loads it back, and asserts the loaded model equals the original
- **c.** A test for travel-booking applications
- **d.** A test that calls a remote server and checks the response

## 2. Why does the lesson say *"persistence is one of the most honest pressures on a model"*?

- **a.** Saving to disk is slow enough to expose timing bugs
- **b.** To save and load exactly, every meaningful piece of state must be reachable; hidden state surfaces as a missing field
- **c.** JSON is strict about field types and rejects loose models
- **d.** Persistence simply requires more tests than other features

## 3. What's a *factory method*?

- **a.** An ASP.NET dependency-injection concept for building services
- **b.** A static method that returns an instance — used in place of (or alongside) a constructor
- **c.** A constructor with a different name, otherwise identical
- **d.** A method required for inheritance to work in C#

## 4. Why is `Building`'s second constructor `protected` instead of `public`?

- **a.** It only makes sense for subclasses (Farm/Lumberyard/Mine), not callers from outside the engine
- **b.** Without `protected`, the file would not compile
- **c.** .NET requires the second constructor to be protected by convention
- **d.** Protected constructors run faster than public ones

## 5. The lesson uses `[Theory] + [InlineData]` to run the same test with four different day counts. What's that pattern called in its more general form?

- **a.** Multiple inheritance, applied to test cases
- **b.** Property-based testing — assert that some *property* holds across many inputs, not just one
- **c.** Mocking, where the inputs replace real dependencies
- **d.** Integration testing, where many components are exercised together

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
