# Quiz — Module 3.8

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.

## 1. What does Azure App Service Free F1 give you?

- **a.** A dedicated virtual machine you administer yourself
- **b.** Free hosting (1 GB RAM, shared CPU, sleeps after 20 minutes idle, 60 minutes per day of compute) — fine for a hobby project's URL
- **c.** A managed database with daily backups
- **d.** A free custom domain name plus certificate

## 2. What does CI/CD stand for?

- **a.** C++ Improvement / C# Development
- **b.** Continuous Integration (auto-build and test on every push) and Continuous Deployment (auto-deploy on every push to a branch)
- **c.** Compiler ID / Compiled
- **d.** Two unrelated terms that just happen to share an abbreviation

## 3. Why does the workflow run tests *before* deploying?

- **a.** Azure refuses deployments without an attached test report
- **b.** CI is the guard. A failing test fails the deploy. You never push broken code to prod by accident.
- **c.** Pure performance — running tests warms the CPU cache for deploy
- **d.** GitHub Actions requires a test step in every workflow file

## 4. Why is `Google__ClientSecret` (double underscore) the env var name, not `Google:ClientSecret`?

- **a.** They are interchangeable on every operating system
- **b.** Linux env vars don't allow `:`. ASP.NET Core's config binder reads `__` (double underscore) as `:` when loading from env vars.
- **c.** It's required by .NET 10 specifically
- **d.** Microsoft preference with no real reason

## 5. The lesson says *never deploy by hand twice*. What does that mean here?

- **a.** Manual deploys are forbidden by Azure
- **b.** The first manual deploy is fine; the second one means you should automate. Automation makes deploys boring → frequent → small changes → fast iteration.
- **c.** Manual deploys are slow but otherwise fine
- **d.** Azure restricts how many manual deploys you can do per day

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
