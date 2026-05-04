# Quiz — Module 4.2

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What's the DOM?

- **a.** A C# library used by ASP.NET to render server-side pages
- **b.** The Document Object Model — the live in-memory tree of your page; JavaScript reads and mutates it
- **c.** A type of database that stores HTML documents efficiently
- **d.** A font format that ships with most modern browsers today

## 2. What does `await fetch(url)` do?

- **a.** Fires the request and immediately returns a placeholder string value
- **b.** Pauses execution until the HTTP response arrives, then returns a `Response` object
- **c.** Throws an error if the network connection isn't ready right now
- **d.** Returns the request body as a parsed JSON object directly

## 3. Why do you need `async` on a function that uses `await`?

- **a.** Performance — async functions run faster than synchronous ones in modern engines
- **b.** `await` can only appear inside `async` functions; the keyword tells the compiler to handle suspending and resuming
- **c.** Required by Chrome but other browsers will run it without the marker
- **d.** Optional in modern JavaScript; older browsers needed it for compatibility

## 4. What's CORS?

- **a.** A common bug pattern in async JavaScript code that beginners hit often
- **b.** Cross-Origin Resource Sharing — browser security where the server must declare which origins may read its responses
- **c.** A logging library shipped with every modern browser by default now
- **d.** A built-in browser auth scheme replacing OAuth for simple use cases

## 5. The lesson says "open DevTools every time you work on a page." Why?

- **a.** Tradition in the frontend community since the early 2000s
- **b.** Elements, Console, and Network together show you what's actually happening; closing them is flying blind
- **c.** Required by some browsers before they'll execute JavaScript on your local files
- **d.** Reviewers expect to see DevTools open in any screenshot you submit

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
