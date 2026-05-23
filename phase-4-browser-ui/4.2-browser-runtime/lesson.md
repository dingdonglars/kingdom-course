# Module 4.2 — The Browser as a Runtime: DOM, DevTools, fetch

Today the page starts working. JavaScript reads from the live `https://localhost:5xxx/kingdoms` API and writes the result into the page. The same engine that's been running since Phase 1, the same JSON your `curl` saw in Phase 3 — now arriving in a browser tab and showing up as HTML you can see. You'll also meet the browser's three developer tabs: Elements (the DOM), Console (run JavaScript live), and Network (every request).

> **Words to watch**
>
> - **DOM** — Document Object Model. The in-memory tree of your page; JavaScript reads and changes it.
> - **`document.querySelector`** — find one element by CSS selector.
> - **`fetch(url)`** — modern way to make HTTP requests from the browser.
> - **`async` / `await`** — JavaScript's way of writing async code that *reads* synchronous.
> - **CORS** — Cross-Origin Resource Sharing — pronounced *"kors"*. Browser security around API calls. First time we name it; once-per-term explanation lives below.

---

## Step 1 — open DevTools

Press F12 in most browsers (Cmd+Option+I on Mac). Three tabs you'll use all the time:

- **Elements** — the live DOM. Click any node, expand it, edit its attributes right there. The page updates as you edit.
- **Console** — run JavaScript live. Type `2 + 2` and press Enter. Type `document.querySelector('h1')` and see the H1 element printed back.
- **Network** — every HTTP request the page makes. Click any one to see its headers, response body, and timing.

Open DevTools every time you work on a page. It doesn't hide anything from you; working with it closed means you can't see what's happening.

## Step 2 — DOM basics

The DOM has a small set of commands that do most of the work. Here they are, in five lines:

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

`querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`. Every browser app does most of its work with these five. The longer your file gets, the more often these five turn up.

## Step 3 — `fetch` and `await`

`fetch(url)` is how JavaScript makes HTTP requests in a modern browser. It returns a *promise* — a value that arrives later. The `await` keyword waits for the promise and gives you the value once it arrives. The function has to be marked `async` before you're allowed to use `await` inside it.

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

Two `await`s is the normal pattern. The first waits for the response headers to arrive. The second waits for the body to be read as JSON. Both are trips across the network, and both are worth the wait.

Async errors are their own kind of bug. Forget the `await` and you'll be calling `.json()` on a Promise object, then `.name` on `undefined`, and the error you see points you at the wrong place. Read the DevTools console output carefully whenever you work with `fetch`.

## Step 4 — the CORS heads-up

When your page is at `http://localhost:5500` and your API is at `https://localhost:5xxx`, those count as two different origins. The browser checks the API's `Access-Control-Allow-Origin` header. If your API doesn't say *"this origin is allowed,"* the browser won't even let your JavaScript see the response. This security feature is called **CORS** (pronounced *"kors"*) — Cross-Origin Resource Sharing. It exists so a harmful page can't quietly call another site's API while pretending to be you.

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

Open `web/index.html` in the browser. The page loads, calls your API, and shows the live kingdom data. This is your first full call from the browser all the way to the API and back.

## Tinker

Open DevTools, switch to the Network tab, and reload the page. Find the `/kingdoms` request. Click it. Read the response panel. It's the same JSON your `curl` saw in Phase 3 — same engine, shown in a new place.

Forget the `await` once on purpose. Look at the console error. That's the most common async bug you'll see this year, so learn to recognise it now.

Add a `console.log(slots)` line in your handler. Logging to the DevTools console is an easy way to see what's actually moving through your code.

Try a `fetch` to a wrong port. Notice that a CORS error message looks different from a network error message — they're telling you different things. That difference matters when you're trying to find a bug.

## What you just did

The kingdom now loads in a browser tab. JavaScript called your live API, got JSON back, and wrote the values into the DOM — `slot.name` became the page title, `slot.day` filled the day counter. You met the five DOM functions you'll use most (`querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`), the `fetch` plus `async` / `await` pattern, and **CORS** — the browser security rule that needs the server to say "this origin is allowed" before your JavaScript can read the response. About thirty lines of JavaScript, and a full loop from the browser to the API and back.

**Key concepts you can now name:**

- **DOM** — the live tree of your page; JavaScript reads and changes it
- **`querySelector` / `textContent` / `createElement` / `appendChild`** — the small set of commands that does most of the work
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
