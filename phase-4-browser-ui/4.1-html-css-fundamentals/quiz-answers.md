# Quiz answers — Module 4.1

## 1. b
HTML = structure. CSS = appearance. JavaScript = behaviour. Three orthogonal concerns; that's the web's foundational separation. Mixing them (inline `style=` everywhere, JS that builds CSS strings) creates unmaintainable code.

## 2. b
Semantic tags carry meaning. `<header>` says "this is the page header"; `<main>` says "this is the main content." Screen readers, search engines, and your future self all benefit. `<div>` says nothing.

## 3. b
Default CSS box model: element width = declared width *plus* padding *plus* border. Easy to miscalculate. `border-box` makes width *include* padding and border — the math is what you'd expect. Set on `*` once at the top.

## 4. a
`system-ui` resolves to the OS's native UI font (San Francisco on macOS, Segoe UI on Windows, Roboto on Android). Zero download, fast, looks "native" everywhere. Custom fonts have a place; for the kingdom UI, system is fine.

## 5. a
Build semantic HTML first — works with CSS off, with JS off, in screen readers. *Then* style. *Then* add interactivity. The web is supposed to *degrade gracefully*; building backwards (JS first, then markup) creates pages that break easily.