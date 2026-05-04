# Quiz — Module 3.5

## 1. The first rule of auth is...

a. Always hash with bcrypt
b. Don't roll your own — use a well-tested provider (Google, Microsoft, GitHub, Auth0)
c. Always use 2FA
d. Use long passwords

## 2. What's a *claim*?

a. A C# attribute
b. A piece of identity info the IDP (Google) asserts about the user — `email`, `name`, `sub` (the stable user id)
c. A bug report
d. An OpenAPI annotation

## 3. Why is the *Client Secret* never stored in the repo?

a. Anyone reading the repo (or anyone scanning GitHub for leaked secrets — which bots do) gains the ability to impersonate your app to Google. Use user-secrets / env vars instead.
b. It's too long
c. Required by Google
d. Performance

## 4. What does `.RequireAuthorization()` on an endpoint do?

a. Logs the request
b. Returns `401 Unauthorized` for any request that doesn't have a valid auth cookie — before your handler runs
c. Logs the user in
d. Adds OpenAPI docs

## 5. Why prefer `sub` over `email` as the stable user identifier?

a. `email` can change (user updates Gmail address); `sub` is permanent and unique to that Google account
b. `sub` is shorter
c. Required by Google
d. They're identical