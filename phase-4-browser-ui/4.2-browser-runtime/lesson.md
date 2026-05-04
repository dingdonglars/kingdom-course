# Module 4.2 — The Browser as a Runtime: DOM, DevTools, fetch

> **Hook:** today the page comes alive. JavaScript reads from the live `https://localhost:5xxx/kingdom` API and writes the result into the page. **Same engine across the wire as the one we built in Block 3.** Plus the browser's three-tab developer toolkit: Elements (the DOM), Console (run JS live), Network (see every request).

> **Words to watch**
> - **DOM** — Document Object Model — the in-memory tree of your page; mutate it from JS
> - **`document.querySelector`** — find an element by CSS selector
> - **`fetch(url)`** — modern way to make HTTP requests from the browser
> - **`async` / `await`** — JavaScript's way of writing async code that *reads* synchronous
> - **CORS** — Cross-Origin Resource Sharing — browser security around API calls

---

## DevTools, in 30 seconds

Open browser DevTools (F12 in most browsers). Three tabs you'll live in:

- **Elements** — the live DOM. Click any node; expand attributes; edit attributes inline.
- **Console** — run JavaScript live. Type `2 + 2`, hit Enter. Type `document.querySelector('h1')`, see the H1.
- **Network** — every HTTP request the page makes. Click any to see its headers, response body, timing.

**Open DevTools every time you work on a page.** Hides nothing.

## DOM basics

```js
// Find one element
const h1 = document.querySelector('h1');
h1.textContent = "Eldoria, the Brave";

// Find a list element by id
const dayEl = document.getElementById('day');
dayEl.textContent = "12";

// Build new elements
const ul = document.querySelector('#resources');
const li = document.createElement('li');
li.textContent = "Gold: 100";
ul.appendChild(li);
```

That's the entire vocabulary you need today: `querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`. **Every browser app does most of its work with these.**

## fetch + async

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

`fetch(url)` returns a *promise* — a value that arrives later. `await` unwraps it. Add `async` to the function so `await` is allowed.

**Async errors are a class of bug** — forgetting `await` returns a Promise object instead of the value, then you call `.json()` on it, then you call `.name` on `undefined`. **Read DevTools' console output religiously when working with `fetch`.**

## CORS heads-up

When your page (`http://localhost:5500`) calls your API (`https://localhost:5xxx`), the browser checks the API's `Access-Control-Allow-Origin` header. If your API doesn't say "this origin is allowed," **the browser refuses to even *show* the response to your JS** — security feature called CORS.

Two-line fix in `Kingdom.Api/Program.cs`:

```csharp
builder.Services.AddCors();
// ...
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
```

(For production, replace `AllowAnyOrigin` with the specific frontend origin.)

## Delta starter

- **MODIFIED:** `web/kingdom.js` — fetch + DOM update
- **MODIFIED:** `Kingdom.Api/Program.cs` — adds CORS

## `web/kingdom.js`

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

Open `web/index.html` in the browser. **The page loads, calls your API, and renders the live kingdom data.** First end-to-end browser-API call.

## Tinker

- Open DevTools → Network → reload. Find the `/kingdoms` request. Click it. Read the response panel. **Same JSON your `curl` saw in Block 5.**
- Forget the `await` (delete it once). Look at the console error. **That's the most common async bug.**
- Add `console.log(slots)` in your handler. **DevTools logs are the friendly debugger** for this kind of code.
- Try `fetch` to a wrong port. Notice the CORS error message vs network error message — different messages tell you different things.

## Name it

- **DOM** — the live tree of your page; manipulable from JS.
- **`document.querySelector(selector)`** — find element by CSS selector.
- **`fetch(url)`** — modern HTTP request from JS.
- **`async` / `await`** — write async code that *reads* synchronous.
- **CORS** — browser security around cross-origin requests; configure your API to allow your page.

## The rule of the through-line

> **The browser is just another shell.** Your engine + API don't change. The browser fetches JSON; the JS turns it into DOM. Same engine, fourth host.

## Quiz / challenge

Open `quiz.md`.

## Connect

Module 4.3 introduces **TypeScript** and the modern frontend toolchain (Vite). Types come back; build tooling brings hot-reload, modules, and the rest of professional JS dev experience.