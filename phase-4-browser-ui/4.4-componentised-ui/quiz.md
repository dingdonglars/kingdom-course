# Quiz — Module 4.4

## 1. Why componentise UI?

a. Required by frameworks
b. Reusable units with one job. Every framework copies this shape; vanilla functions teach the underlying idea.
c. Performance only
d. Tradition

## 2. What does `escapeHtml` prevent?

a. Type errors
b. XSS — Cross-Site Scripting; pasting user-controlled strings directly into `innerHTML` lets attackers inject `<script>` tags
c. CORS errors
d. CSS issues

## 3. What's event delegation?

a. Listening on a parent for events bubbling from many children — scales to lots of items without one handler per child
b. A C# pattern
c. Required for clicks
d. The default browser behavior

## 4. Why are the components in this lesson "just functions"?

a. The framework requires it
b. To teach the underlying shape; React / Vue / Svelte add change-detection on top of the same `(data) => UI` idea
c. Vanilla JS doesn't allow classes
d. Performance

## 5. The lesson's "render-as-string" components return HTML strings. What's the cost?

a. Slower for big trees (every render rebuilds + the browser re-parses); fine at small scale
b. They don't work
c. They have type issues
d. None — they're better in every way