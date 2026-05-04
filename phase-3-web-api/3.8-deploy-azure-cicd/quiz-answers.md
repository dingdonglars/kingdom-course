# Quiz answers — Module 3.8

## 1. b
F1 Free is Azure's "always free" tier for App Service. Limits are real (1GB RAM, idle sleep, 60 min/day CPU) but more than enough for a learning project. $0 forever — no surprise billing.

## 2. b
CI = "every push runs build + tests automatically." CD = "every push (or merge to main) deploys automatically." Together they make small changes safe to ship. Without them, you batch up risky big releases.

## 3. b
The build job runs `dotnet test`. If any test fails, the artifact upload step never runs, so the deploy job has nothing to deploy. **Tests are the deploy gatekeeper.** Skipping this step is how teams ship broken code to prod by accident.

## 4. b
Linux environment variable names cannot contain `:`. ASP.NET Core's configuration binder interprets `__` (double underscore) as the section separator `:`. So `Google__ClientId` env var = `Google:ClientId` config key.

## 5. b
First manual deploy: you're learning the steps. Second manual deploy: you're wasting time you could have spent automating once. Automated deploys are repeatable, reviewable (the workflow YAML is in git), reversible. **Boring deploys = frequent deploys = small changes = fast iteration.**