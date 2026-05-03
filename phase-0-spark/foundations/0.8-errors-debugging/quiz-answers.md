# Quiz answers — Module 0.8

## 1. b

`try { A } catch (E e) { B }` runs `A`. If `A` runs to completion without throwing, `B` is skipped. If `A` throws an exception of type `E` (or a subclass), `B` runs and execution then continues *after* the `catch` block. If `A` throws an exception that doesn't match `E`, the exception propagates up to the next `try/catch` (or crashes the program if none).

## 2. c

C# matches catches top-to-bottom; the *first* matching one runs. If you put `catch (Exception)` first, it'd swallow everything — `IOException`, `UnauthorizedAccessException`, all of them — and you couldn't react differently to different problems. Putting specific types first lets you handle "file is locked" differently from "permission denied" differently from "something we didn't think of."

## 3. b

A breakpoint is a marked line in your code where the debugger pauses. When the program reaches that line during a debug session, it stops *before* executing it. You can then inspect the values of variables, the call stack, etc. — and step through manually.

## 4. a

`F10` (*Step Over*) executes the current line and moves to the next line *in the same method*. If the current line calls a method, the entire method runs; the debugger doesn't dive in. To dive into a called method, use `F11` (*Step Into*).

## 5. b

The call stack is the chain: `Main` called `LoadInventory` called `ParseLine` called `int.Parse`, and an exception was thrown in `int.Parse`. The debugger (and the exception's `.StackTrace`) shows that chain so you can see *how you got here* — often more useful than knowing where the error happened.