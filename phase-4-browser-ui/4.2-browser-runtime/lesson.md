# Module 4.2 — The Browser as a Runtime: DOM, DevTools, fetch

Today the page comes alive. JavaScript reads from the live `https://localhost:5xxx/kingdoms` API and writes the result into the page. The same engine that's been ticking since Phase 1, the same JSON your `curl` saw in Phase 3 — now flowing into a browser tab and rendering as HTML you can see. Plus you'll meet the browser's three-tab developer toolkit: Elements (the DOM), Console (run JavaScript live), Network (every request).

> **Words to watch**
>
> - **DOM** — Document Object Model. The in-memory tree of your page; JavaScript reads and mutates it.
> - **`document.querySelector`** — find one element by CSS selector.
> - **`fetch(url)`** — modern way to make HTTP requests from the browser.
> - **`async` / `await`** — JavaScript's way of writing async code that *reads* synchronous.
> - **CORS** — Cross-Origin Resource Sharing — pronounced *"kors"*. Browser security around API calls. First time we name it; once-per-term explanation lives below.

---

## Step 1 — open DevTools

Press F12 in most browsers (Cmd+Option+I on Mac). Three tabs you'll live in:

- **Elements** — the live DOM. Click any node, expand it, edit attributes inline. The page updates as you edit.
- **Console** — run JavaScript live. Type `2 + 2` and press Enter. Type `document.querySelector('h1')`, see the H1 element printed back.
- **Network** — every HTTP request the page makes. Click any one to see its headers, response body, and timing.

Open DevTools every time you work on a page. There's nothing it hides; closing it is flying blind.

## Step 2 — DOM basics

The DOM has a small vocabulary that does most of the work. Here it is, in five lines:

```js
// Find one element
const h1 = document.querySelector('h1');
h1.textContent = "Eldoria, the Brave";

// Find an element by id
const dayEl = document.getElementById('day');
dayEl.textContent = "12";

// Build new elements
const ul = document.querySelector('#resources');
const li = document.createElement('li');
li.textContent = "Gold: 100";
ul.appendChild(li);
```

`querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`. Every browser app does most of its work with these five. The longer your file is, the more these five appear.

## Step 3 — `fetch` and `await`

`fetch(url)` is how JavaScript makes HTTP requests in the modern browser. It returns a *promise* — a value that arrives later. The `await` keyword unwraps the promise into the value once it shows up. The function has to be marked `async` for `await` to be allowed inside it.

```js
async function loadKingdom() {
  const response = await fetch('https://localhost:5xxx/kingdom');
  if (!response.ok) {
    console.error("Request failed:", response.status);
    return;
  }
  const data = await response.json();
  return data;
}
```

Two `await`s is the standard pattern. The first waits for the response headers to arrive. The second waits for the body to parse as JSON. Both are network round-trips; both are worth waiting for.

Async errors are a class of bug. Forget the `await` and you'll be calling `.json()` on a Promise object, then `.name` on `undefined`, then chasing the wrong stack trace. Read DevTools' console output religiously when working with `fetch`.

## Step 4 — the CORS heads-up

When your page lives at `http://localhost:5500` and your API lives at `https://localhost:5xxx`, those count as two different origins. The browser checks the API's `Access-Control-Allow-Origin` header. If your API doesn't say *"this origin is allowed,"* the browser refuses to even let your JavaScript see the response. The security feature is called **CORS** (pronounced *"kors"*) — Cross-Origin Resource Sharing. It exists so a malicious page can't silently call another site's API while pretending to be you.

A two-line fix in `Kingdom.Api/Program.cs`:

```csharp
builder.Services.AddCors();
// ...
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
```

For production, replace `AllowAnyOrigin` with the specific frontend origin. We'll do that in Module 4.6 when the frontend deploys.

## Step 5 — the delta starter

- **MODIFIED:** `web/kingdom.js` — fetch + DOM update
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds CORS

`web/kingdom.js`:

```js
const API = 'https://localhost:5xxx';   // CHANGE to your API port

async function loadKingdom() {
  try {
    const resp = await fetch(`${API}/kingdoms`);   // requires auth — adjust as needed
    if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
    const slots = await resp.json();
    if (slots.length === 0) {
      document.querySelector('main').textContent = "No kingdoms yet — create one via the API.";
      return;
    }
    renderSummary(slots[0]);
  } catch (err) {
    console.error('Failed to load kingdom:', err);
    document.querySelector('main').textContent = `Error: ${err.message}`;
  }
}

function renderSummary(slot) {
  document.getElementById('day').textContent = slot.day;
  document.querySelector('h1').textContent = slot.name;
}

loadKingdom();
```

Open `web/index.html` in the browser. The page loads, calls your API, and renders the live kingdom data. First end-to-end browser-to-API call.

## Tinker

Open DevTools, switch to the Network tab, reload the page. Find the `/kingdoms` request. Click it. Read the response panel. Same JSON your `curl` saw in Phase 3 — same engine, new shell.

Forget the `await` once on purpose. Look at the console error. That's the most common async bug you'll see this year; learn to recognise it now.

Add a `console.log(slots)` line in your handler. DevTools logs are the friendly way to inspect what's actually flowing through your code.

Try `fetch` to a wrong port. Notice how a CORS error message looks different from a network error message — they tell you different things. The difference matters when you're debugging.

## What you just did

The kingdom now loads in a browser tab. JavaScript called your live API, got JSON back, and wrote the values into the DOM — `slot.name` became the page title, `slot.day` filled the day counter. You met the five DOM functions you'll use most (`querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`), the `fetch` plus `async` / `await` pattern, and **CORS** — the browser security rule that requires the server to say "this origin is allowed" before your JavaScript can read the response. About thirty lines of JavaScript; a complete browser-to-API loop.

**Key concepts you can now name:**

- **DOM** — the live tree of your page; JavaScript reads and mutates it
- **`querySelector` / `textContent` / `createElement` / `appendChild`** — the small vocabulary that does most of the work
- **`fetch(url)`** — modern HTTP request from JavaScript
- **`async` / `await`** — write async code that reads synchronous
- **CORS** — the server must allow your origin before the browser shows you the response

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.3 introduces TypeScript and Vite. Types come back; build tooling brings hot-reload, modules, and the rest of the modern frontend dev experience.
