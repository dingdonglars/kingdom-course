# Quiz — Module 3.5

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. The first rule of authentication is...

- **a.** Always hash passwords with bcrypt before storing them
- **b.** Don't roll your own — use a well-tested provider (Google, Microsoft, GitHub, Auth0)
- **c.** Always require two-factor authentication on every account
- **d.** Always make passwords at least sixteen characters long

## 2. What is a *claim* in this context?

- **a.** A C# attribute used on a class
- **b.** A piece of identity info the provider (Google) asserts about the user — `email`, `name`, `sub` (the stable user id)
- **c.** A bug report filed against an authentication library
- **d.** An OpenAPI annotation marking a field as required

## 3. Why is the *Client Secret* never stored in the repo?

- **a.** Anyone reading the repo (or any of the bots that scan public repos for leaked secrets) gains the ability to impersonate your app to Google. Use user-secrets or environment variables instead.
- **b.** It's too long to fit in a typical config file
- **c.** Google requires it to live somewhere with a `.secret` extension
- **d.** Pure performance — secrets in the repo are slower to load

## 4. What does `.RequireAuthorization()` on an endpoint do?

- **a.** Logs the request and continues normally
- **b.** Returns `401 Unauthorized` for any request without a valid auth cookie — before the handler runs
- **c.** Logs the user in automatically using a default account
- **d.** Adds an OpenAPI annotation that the endpoint requires auth, but doesn't block requests

## 5. Why prefer `sub` over `email` as the stable user identifier?

- **a.** `email` can change (user updates their Gmail); `sub` is permanent and unique to that Google account
- **b.** `sub` is shorter and saves database space
- **c.** `email` is forbidden by privacy regulations
- **d.** They are exactly equivalent for identification purposes

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
