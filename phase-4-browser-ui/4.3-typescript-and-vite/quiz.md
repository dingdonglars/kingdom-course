# Quiz — Module 4.3

## 1. What does TypeScript add to JavaScript?

a. New runtime
b. Static type-checking — same JS underneath, but the compiler refuses obvious type bugs (typos, wrong types, wrong shapes)
c. Faster execution
d. Replaces JavaScript

## 2. What does Vite do?

a. A type checker
b. Modern frontend dev server + bundler — fast HMR, TS compilation, production build, near-zero config
c. A testing framework
d. A backend

## 3. What's HMR?

a. Hard Memory Reset
b. Hot Module Replacement — edit code, save, browser updates without losing page state. Saves seconds × 1000s of edits per project.
c. A bug
d. Required by HTTPS

## 4. The `as KingdomSlot[]` cast on the parsed JSON — why is it needed?

a. Required by JSON
b. The compiler can't know the runtime API shape; `as` tells it "trust me, this matches my interface." Real apps would validate at runtime (Zod, etc.)
c. Performance
d. Style

## 5. Why is the rule "types at every boundary"?

a. Boundaries are where shape mistakes happen — wire format, file format, API contract. Types make the contract explicit. The discipline you used in C# DTOs applies here.
b. Required by TypeScript
c. SEO
d. Performance