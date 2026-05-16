# Quiz — Module 4.4

> Don't write your answers in this file — open `journal/quiz-notes.md` and write them there.
>
> Travou no inglês? Abra o `quiz.pt.md` — é este mesmo quiz em português. Tente em inglês primeiro.

## 1. Why componentise the UI?

- **a.** Required by every modern frontend framework before you can render a page
- **b.** Reusable units with one job; every framework copies this idea on top of plain functions
- **c.** Faster execution at runtime in the browser's rendering engine
- **d.** Tradition from the early jQuery era of frontend development

## 2. What does `escapeHtml` prevent?

- **a.** Type errors in the TypeScript compiler when string values flow through templates
- **b.** XSS — Cross-Site Scripting; pasting user-controlled strings into `innerHTML` lets attackers inject `<script>` tags
- **c.** CORS errors when fetching from a different origin than the current page
- **d.** Layout problems caused by special characters in CSS class names

## 3. What's event delegation?

- **a.** Listening on a parent element for events bubbling from many children — one handler scales to thousands of items
- **b.** A C# pattern for raising events from one class to another in the same project
- **c.** Required for click handlers to work in modern browsers since the 2020 update
- **d.** The default browser behaviour when no event listener has been attached yet

## 4. Why are the components in this lesson "just functions"?

- **a.** A framework constraint — Vite refuses to compile non-function components
- **b.** To teach the underlying idea; React, Vue, and Svelte all add change-detection on top of the same `(data) => UI` pattern
- **c.** Plain JavaScript doesn't allow class-based components in modern browsers
- **d.** Functions execute faster than classes in the V8 engine by a measurable margin

## 5. The lesson's "render-as-string" components return HTML strings. What's the cost?

- **a.** Slower for big trees because every render rebuilds the string and the browser re-parses; fine at small scale
- **b.** They don't run at all without a framework wrapping them first
- **c.** They have type-safety problems that the compiler can't help with
- **d.** None — render-as-string is faster and safer than render-as-DOM in every case

---

When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.
