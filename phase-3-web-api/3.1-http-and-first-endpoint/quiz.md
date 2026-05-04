# Quiz — Module 3.1

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. Which part of `GET /kingdom HTTP/1.1` is the *verb*?

- **a.** `kingdom`
- **b.** `GET`
- **c.** `HTTP/1.1`
- **d.** `/kingdom`

## 2. What does it mean for an HTTP method to be *idempotent*?

- **a.** Doing it twice has the same effect as doing it once — `GET`, `PUT`, `DELETE` are idempotent; `POST` typically isn't
- **b.** It returns JSON instead of plain text
- **c.** It requires the client to be signed in
- **d.** It has been replaced by a newer method

## 3. What does the 4xx family of status codes mean?

- **a.** The request succeeded
- **b.** The client sent something wrong — bad request, not found, unauthorized, forbidden
- **c.** The server crashed or hit a problem of its own
- **d.** The server is asking the client to redirect somewhere else

## 4. What does `app.MapGet("/kingdom", () => ...)` actually do?

- **a.** Sends a `GET` request out to another server
- **b.** Registers a route — when a `GET /kingdom` arrives, run the lambda and turn its return value into JSON
- **c.** Creates a brand-new endpoint URL on the public internet
- **d.** Configures logging for the application

## 5. The lesson says *the API is another outer layer*. What's that mean here?

- **a.** ASP.NET runs inside an operating system shell
- **b.** HTTP is just one more way to talk to the engine; the engine itself doesn't change. Same as console, same as persistence, now web API.
- **c.** The shell is the operating system the server runs on
- **d.** It's a metaphor without a concrete meaning

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
