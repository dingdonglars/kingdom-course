# Quiz — Module 3.8

## 1. What does Azure App Service Free F1 give you?

a. A managed VM
b. Free hosting (1GB RAM, shared CPU, sleeps after 20 min idle, 60 min/day compute) — perfect for a hobby project's URL
c. A database
d. Custom domain

## 2. What's CI/CD?

a. C++ Improvement / C# Development
b. Continuous Integration (auto-build + test on every push) and Continuous Deployment (auto-deploy on every push to a branch)
c. Compiler ID / Compiled
d. Two unrelated terms

## 3. Why does the workflow run tests *before* deploying?

a. Required by Azure
b. CI is the gatekeeper. A failing test fails the deploy. You never push broken code to prod by accident.
c. Performance
d. Tests are required by GitHub Actions

## 4. Why is `Google__ClientSecret` (double underscore) the env var name, not `Google:ClientSecret`?

a. They're the same
b. Linux env vars don't allow `:`. ASP.NET Core's config binder treats `__` (double underscore) as `:` when reading from env vars.
c. Required by .NET 10
d. Microsoft preference

## 5. The lesson says "never deploy by hand twice." What's the meaning?

a. Manual deploys are slow
b. The first manual deploy is fine; the second one means you should automate. Automation makes deploys boring → frequent → small changes → fast iteration.
c. Manual deploys are forbidden
d. Required by Azure