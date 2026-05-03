# Quiz answers — Module 0.1

## 1. b

`Console.ReadLine()` pauses your program and waits for the user to type a line and press Enter. It then *returns* that line as a string, which you usually store in a variable.

## 2. c

Both write text to the console. The difference is the newline. `WriteLine("hi")` outputs `hi\n` (the cursor moves to the next line). `Write("hi")` outputs `hi` (the cursor stays on the same line). Use `Write` when you want the user's input to appear right after your prompt; `WriteLine` for everything else.

## 3. a

`var` declares a new variable, `name` is the name you're giving it, `=` assigns a value, and `Console.ReadLine()` is the value (whatever the user types). After this line runs, `name` holds the user's input.

## 4. b

The `$` prefix turns a regular string into an *interpolated* string. Anything inside `{curly braces}` is treated as a C# expression, evaluated, converted to a string, and dropped into the result. So `$"Hey {name}"` becomes `Hey alice` if `name` is `"alice"`.

## 5. b

`name?.ToUpper()` calls the `ToUpper()` method on `name`, which gives back the uppercase version. The `?.` is the *null-conditional* operator — if `name` is `null` (which `ReadLine()` *can* return if the input stream is closed), `?.` skips the call and the whole expression is `null`. Otherwise, you get `ALICE`.
