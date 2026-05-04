# Module 4.1 — HTML & CSS Fundamentals

> **Hook:** today the browser tab shows your kingdom — not as JSON, but as *a page*. We write the smallest meaningful HTML + CSS. The page reads from your live API and renders the kingdom by hand. **No framework, no build step yet — just a `.html` file you open in the browser.**

> **Words to watch**
> - **HTML** — markup; describes *structure* (headings, lists, sections, links)
> - **CSS** — styles; describes *appearance* (colors, spacing, typography, layout)
> - **DOM** — the in-memory tree of your HTML, manipulable from JavaScript
> - **semantic markup** — `<header>`, `<main>`, `<nav>` rather than `<div>` everywhere

---

## The smallest useful HTML

Create `web/index.html` at your repo root:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>My Kingdom</title>
  <link rel="stylesheet" href="styles.css">
</head>
<body>
  <header>
    <h1>Eldoria</h1>
    <p>Day <span id="day">—</span></p>
  </header>
  <main>
    <h2>Resources</h2>
    <ul id="resources"></ul>
    <h2>Buildings</h2>
    <ul id="buildings"></ul>
  </main>
  <script src="kingdom.js"></script>
</body>
</html>
```

Eight elements; full semantic structure. **Open it in a browser** (double-click the file). You'll see the static skeleton.

## Minimal CSS

`web/styles.css`:

```css
* { box-sizing: border-box; }
body {
  font-family: system-ui, sans-serif;
  max-width: 720px;
  margin: 2rem auto;
  padding: 0 1rem;
  color: #222;
  background: #fafaf7;
}
header { border-bottom: 1px solid #ddd; padding-bottom: 1rem; margin-bottom: 1rem; }
h1 { margin: 0 0 0.25rem 0; font-size: 2rem; }
ul { padding-left: 1.5rem; }
li { padding: 0.25rem 0; }
```

Three rules of thumb:
1. **`box-sizing: border-box`** on everything — sane width math.
2. **`max-width` + `margin: auto`** — center the content; readable line length.
3. **System fonts** — fast, native-feeling, free.

Refresh the page. **Already looks like a real page.**

## What's *not* in this module

- JavaScript yet (4.2 + 4.3)
- Frameworks (4.5 — TypeScript; later — optional Svelte)
- Build tooling (4.6 — Vite)
- Real layout (Grid, Flexbox — covered in deeper CSS resources you can read on your own)

This module is the floor. The next 5 modules build up.

## Delta starter

- **NEW:** `web/index.html`
- **NEW:** `web/styles.css`
- **NEW:** `web/kingdom.js` — empty placeholder; populated in M4.2

## Tinker

- Change the body background to your favorite color. Refresh. **Instant feedback** is the joy of frontend.
- Add a `<nav>` with three links (just `href="#"` for now).
- Open browser DevTools → Elements panel → inspect the `<header>`. Notice the live tree.
- Add `<meta name="viewport" content="width=device-width, initial-scale=1">` to the head — page now renders correctly on mobile.

## Name it

- **HTML** — markup language; structure.
- **CSS** — style language; appearance.
- **Semantic elements** — `<header>`, `<main>`, `<nav>`, `<article>`, `<section>` — meaningful tags.
- **System fonts** — `system-ui`, fast + native.

## The rule of the through-line

> **Markup first, style second, behaviour third.** Build the page semantically (HTML), then style it (CSS), then make it interactive (JS). Skipping steps creates mess.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.2 brings the browser to life: **the DOM, DevTools, fetch.** The page learns to call your live API.