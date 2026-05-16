# Quiz — Module 4.6

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Why is Azure Static Web Apps a good frontend host for this project?

- **a.** Free tier, GitHub integration, native CDN and SSL, zero infrastructure to manage
- **b.** It's the only host that supports TypeScript builds in production environments
- **c.** Required by Azure when the API is also hosted on App Service in the same region
- **d.** It bundles a database, auth, and email service in the free tier by default

## 2. Why can't you use `AllowAnyOrigin()` together with `AllowCredentials()`?

- **a.** It would let any website send authenticated requests to your API on behalf of your user; the browser refuses the combination
- **b.** Performance — the combination causes measurable slowdown on every cross-origin request
- **c.** Required by Azure as a configuration constraint on Static Web Apps deployments
- **d.** They work fine together; the lesson is mistaken on this point

## 3. What does `import.meta.env.PROD` evaluate to in `npm run dev`?

- **a.** `true` — dev is treated as production for environment variable purposes
- **b.** `false` — dev mode and production mode are distinct
- **c.** `undefined` — the property is only set during builds, not during dev
- **d.** Throws an error because `import.meta` isn't allowed in dev mode

## 4. What's a CDN?

- **a.** A type of database that caches API responses across multiple servers
- **b.** Content Delivery Network — your files cached at edge servers worldwide; users get them from the nearest one
- **c.** A logging library used by Azure Static Web Apps to track frontend errors
- **d.** Required by HTTPS as part of the certificate validation chain on modern sites

## 5. Why two services (frontend and backend) instead of one?

- **a.** Each scales independently, deploys independently, can be replaced independently — the standard production layout
- **b.** Required by Azure for any free-tier hosting plan since the 2024 update
- **c.** Faster execution at runtime when the two services run on separate machines
- **d.** Tradition from the early hosting era of the 2000s; modern apps usually combine them

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
