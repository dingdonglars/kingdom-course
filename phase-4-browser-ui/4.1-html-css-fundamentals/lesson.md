# Module 4.1 — HTML & CSS Fundamentals

Today the kingdom appears in a browser tab. Not as JSON, not as console text — as a page. We write the smallest useful HTML and CSS, open the file in a browser, and there it is. No framework, no build step, no `npm install`. Just a `.html` file and a `.css` file you can double-click. The next module makes the page talk to your live API. Today is about the layout.

If you've never written HTML before, it's simpler than it looks. **HTML** describes structure: this is a heading, this is a list, this is the main section. **CSS** describes appearance: this much spacing, that font, those colours. The two languages stay separate on purpose. If you ever find yourself writing colours into HTML, or structure into CSS, something is in the wrong place.

> **Words to watch**
>
> - **HTML** — markup language. Describes structure: headings, lists, sections, links.
> - **CSS** — style language. Describes appearance: colours, spacing, fonts, layout.
> - **DOM** — the in-memory tree the browser builds from your HTML. JavaScript will change it in Module 4.2.
> - **semantic markup** — using `<header>`, `<main>`, `<nav>` instead of `<div>` for everything.

---

## Step 1 — the smallest useful HTML

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

Eight elements, and that's the whole structure. Double-click the file in your file explorer — the browser opens it. You'll see the bare page: a heading "Eldoria", an empty Day, two empty lists. The page asks for a `kingdom.js` file that doesn't exist yet. The browser just skips it and keeps going. We'll write that file in the next module.

A note on the elements you'll see most: `<header>` is the page top, `<main>` is the body, `<nav>` is for menus, `<article>` and `<section>` group related content, and `<h1>` through `<h6>` are headings in order of importance. Using these instead of `<div>` everywhere is what **semantic markup** means. Screen readers, search engines, and you yourself later all read meaning from the tag name.

## Step 2 — the smallest useful CSS

Create `web/styles.css`:

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
header {
  border-bottom: 1px solid #ddd;
  padding-bottom: 1rem;
  margin-bottom: 1rem;
}
h1 { margin: 0 0 0.25rem 0; font-size: 2rem; }
ul { padding-left: 1.5rem; }
li { padding: 0.25rem 0; }
```

Those few lines hold three useful habits.

The first is **`box-sizing: border-box`** on every element. Without it, the width you set on a box does not include its padding or border. So a `width: 200px` element with `padding: 10px` actually takes 220 pixels. With `border-box`, the width is exactly what you set. Add it once, on `*`, at the top of the file.

The second is **`max-width` plus `margin: auto`** on the body. That centres the content and stops lines of text from getting too long to read. Long lines on a wide screen are tiring to read; this is the cheapest way to fix it.

The third is **system fonts** — `system-ui` uses whatever font the operating system already uses for its own menus (Segoe UI on Windows, San Francisco on macOS, Roboto on Android). Nothing to download, fast, and it looks right by default. Custom fonts are fine when you need them, but for the kingdom UI, the system font is plenty.

Refresh the page. It already looks like a real page.

## Step 3 — what's not in this module

A short list, so you know where the limits are for today.

JavaScript comes in Module 4.2 and Module 4.3 — today the page just sits there. Frameworks like React or Svelte come later, and they're optional. Build tools (Vite) come in Module 4.3. Real CSS layout — Grid, Flexbox — is a big topic on its own. The basics here are enough for the kingdom page, and you can read more about layout on your own when you need it.

This module is the starting point. The next five build on top of it.

## Step 4 — the delta starter

- **NEW:** `web/index.html`
- **NEW:** `web/styles.css`
- **NEW:** `web/kingdom.js` — empty placeholder; we fill it in Module 4.2.

Create the empty `kingdom.js` so the `<script>` tag has something to load. One line is fine: `// kingdom.js — populated in Module 4.2`.

## Tinker

Change the body background to your favourite colour. Refresh. You see the result at once — that's one of the nice things about frontend work. There's no compile step, no rebuild, nothing between your edit and seeing it on screen.

Add a `<nav>` with three links (just `href="#"` for now) inside the `<header>`. Notice the browser puts them in a row by default. CSS is what makes them look like a menu; the HTML just says "these are navigation links."

Open browser DevTools (F12 in most browsers) and click the Elements panel. Look at the `<header>`. You're seeing the live tree — the DOM. We'll talk to it from JavaScript in the next module.

Add `<meta name="viewport" content="width=device-width, initial-scale=1">` to the `<head>`. The page now displays correctly on a phone. One line, big effect.

## What you just did

You wrote a webpage by hand. The HTML set out the structure — a header with a name and a day counter, a main section with two lists. The CSS gave it spacing, a sensible width, and a system font, plus the `box-sizing` line that fixes the most common width surprise. You opened the file in a browser and it rendered. No framework, no build step, no install — just two files and a tab. About forty lines of code in total. You've already met the three useful habits (`box-sizing: border-box`, `max-width` for readable line length, system fonts) that handle most simple pages. HTML first, CSS second.

**Key concepts you can now name:**

- **HTML** — markup; the structure of the page
- **CSS** — styles; the appearance of the page
- **semantic elements** — `<header>`, `<main>`, `<nav>`, `<article>`, `<section>`
- **`box-sizing: border-box`** — width includes padding and border
- **system fonts** — `system-ui` resolves to the OS's native UI font

## On your own

Time to put the book away. Don't scroll back up to the steps — make a new empty `.html` file and, from your own head, write a tiny page: a full HTML skeleton with one `<h1>` with your kingdom's name in it, and one CSS rule (in a `<style>` tag in the head, or a linked file) that colours that heading. Open it in the browser. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for. The browser is your grader: if your heading shows up coloured, you got it right.

<details><summary>Stuck? Open this to check yourself.</summary>

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>My Page</title>
  <style>
    h1 { color: teal; }
  </style>
</head>
<body>
  <h1>Eldoria</h1>
</body>
</html>
```

The exact tags and colour don't matter. What matters: a `<!DOCTYPE html>` line, the `<head>`/`<body>` split, structure in the HTML, and appearance in the CSS — never the other way around.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.1 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.2 brings the browser to life. You'll meet the DOM properly, open DevTools, and write JavaScript that calls your live API — the page learns to fetch JSON and render it.
