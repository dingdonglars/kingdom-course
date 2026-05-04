# Quiz answers — Module 4.4

## 1. b
Components are folders for code. Each owns one rendering job; the main file becomes a thin orchestrator. The mental model — "function from data to UI" — is universal across frameworks. Build it from vanilla functions first, then frameworks make sense.

## 2. b
XSS lets attackers inject scripts via unescaped strings in `innerHTML`. If a kingdom name is `<script>alert(1)</script>` and you paste it raw into HTML, the script runs in every viewer's browser. `escapeHtml` neutralises the angle brackets.

## 3. a
Instead of `cards.forEach(c => c.addEventListener('click', fn))` — N handlers — you attach one handler on the parent. Events bubble up; `e.target.closest('article.card')` identifies which card was clicked. One handler scales to thousands of cards.

## 4. b
Real frameworks add change-detection (React's reconciler, Svelte's compiler), but the core shape is the same: a function from data to UI. Vanilla teaches the shape without the magic. When you adopt a framework later, you'll know exactly what's happening underneath.

## 5. a
Render-as-string is simple but rebuilds + re-parses on every change. For 10 cards, fine. For 10000, sluggish. Frameworks optimise by diffing virtual trees + only mutating changed bits. For a kingdom UI with dozens of items, render-as-string is appropriate.