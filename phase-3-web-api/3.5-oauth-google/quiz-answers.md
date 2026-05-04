# Quiz answers — Module 3.5

## 1. b
DIY auth is a 6-month project to do well, with serious security consequences if done badly. Even huge companies have leaked databases over auth bugs. Use a provider; let them carry the risk. For a learning project, "Sign in with Google" is the smallest correct answer.

## 2. b
A claim is a key/value pair that the identity provider (Google, Microsoft, etc.) asserts about the user — `email`, `name`, `sub`, `iss` (issuer), `iat` (issued-at), etc. After cookie auth, your endpoint reads claims via `ctx.User.FindFirst("name")`.

## 3. a
The Client Secret is the credential identifying *your app* to Google. If it leaks, attackers can pretend to be your app, gather user data, etc. GitHub has bots that scan public repos for leaked secrets and revoke them within minutes — but that's no consolation if a real attacker grabbed it first. Use user-secrets locally + env vars in production.

## 4. b
`.RequireAuthorization()` is a guard. Before the handler runs, the framework checks the auth cookie. No cookie → 401 immediately, no handler invocation. You don't have to think about auth in every handler — the guard centralises it.

## 5. a
`email` is mutable — a user can change their Gmail. `sub` (subject id) is the stable, permanent, globally-unique identifier Google assigns to the account. Always store `sub` as your primary user reference; use `email`/`name` for display purposes.