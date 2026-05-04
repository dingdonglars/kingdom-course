# Quiz answers — Module 4.6

## 1. a
SWA is the lowest-friction free option that scales: built-in HTTPS, global CDN, GitHub auto-deploys, custom domain support. The Free tier (100GB/mo bandwidth) is generous for a hobby project.

## 2. a
`AllowAnyOrigin` + `AllowCredentials` would let any website on the internet send authenticated requests to your API on behalf of your user — disastrous. The browser refuses. Use `WithOrigins(...)` to enumerate the trusted domains.

## 3. b
`import.meta.env.PROD` is `false` in dev, `true` in `npm run build` output. The opposite (`DEV`) is also available. Use to conditionally tweak behaviour per environment.

## 4. b
A CDN caches static assets at servers around the world. A user in Brazil gets your JS/CSS from a São Paulo edge server, not from wherever your origin is. Faster + cheaper at scale.

## 5. a
Frontend and backend have different deploy/scale rhythms. The frontend is static (rebuild + push); the backend has DB, auth, secrets. Separating them lets each evolve at its own pace, and lets you swap either independently. Standard production shape since the early 2000s.