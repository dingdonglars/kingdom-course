# Module 4.2 — The Browser as a Runtime: DOM, DevTools, fetch

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Today the page starts working. JavaScript reads from the live `https://localhost:5xxx/kingdoms` API and writes the result into the page. The same engine that's been running since Phase 1, the same JSON your `curl` saw in Phase 3 — now arriving in a browser tab and showing up as HTML you can see. You'll also meet the browser's three developer tabs: Elements (the DOM), Console (run JavaScript live), and Network (every request).

"The browser as a runtime" is the idea in the title, and it just means: the browser is a place your code *runs*, not only a place that shows pages. Your JavaScript runs there, reaches out to your API, and edits the page live. Here's the whole round-trip — and notice it ends at the same engine you wrote in Phase 1:

```text
   THE BROWSER  (the runtime — your JavaScript runs here)

     index.html + your JS
          |   fetch("/kingdoms")
          v
     your API  --uses-->  Kingdom.Engine   (Phase 1 rules, still unchanged)
          |
          |   JSON comes back
          v
     your JS writes it into the page (the DOM)  -->  you see the kingdom
```

Console (Phase 1), API (Phase 3), now browser (Phase 4) — three shells, one engine. Today you wire up the last hop: JSON into a page you can look at.

> **Words to watch**
>
> - **DOM** — Document Object Model. The in-memory tree of your page; JavaScript reads and changes it.
> - **`document.querySelector`** — find one element by CSS selector.
> - **`fetch(url)`** — modern way to make HTTP requests from the browser.
> - **`async` / `await`** — JavaScript's way of writing async code that *reads* synchronous.
> - **CORS** — Cross-Origin Resource Sharing — pronounced *"kors"*. Browser security around API calls. First time we name it; once-per-term explanation lives below.

---

## New language: JavaScript — and why

You've written C# all year. Today a second language turns up, and it's fair to ask why you can't just keep using the one you know.

The reason is simple: **a web browser runs only one programming language — JavaScript.** Not C#, not Python. If you want a page to *do* anything — react to a click, load data, change its text — that code has to be JavaScript. It's the language the browser was built to speak, and there's no way around it. (Your C# is still here: it's the API the page talks to. The browser half just has to be JavaScript.)

The good news: you already know how to program, and JavaScript is built from the same parts you've used all year — variables, `if`, loops, functions, objects. You're not starting over. You're learning new words for ideas you already own. Here's the quick translation:

| C# | JavaScript | Note |
| --- | --- | --- |
| `string name = "Eldoria";` | `const name = "Eldoria";` | `const` = can't reassign later; `let` = can. No type written. |
| `int day = 11;` | `let day = 11;` | JavaScript doesn't make you write the type. |
| `Console.WriteLine(x);` | `console.log(x);` | Print — here, to the DevTools console. |
| `void Greet(string n) {}` | `function greet(n) {}` | No return type, no parameter types. |
| `if (x == 5)` | `if (x === 5)` | **Three** equals signs in JavaScript (see below). |
| `foreach (var b in list)` | `for (const b of list)` | `of`, not `in`. |
| `var k = new Kingdom();` | `const k = { name: "Eldoria", day: 11 };` | A plain object is just `{ key: value }` — which is also exactly what JSON is. |

Two differences catch every C# person, so meet them now:

1. **No types written down, and nothing checks your code before it runs.** You don't declare `int` or `string`, and the browser just runs whatever you wrote. A typo like `slot.naem` doesn't error — it quietly gives you `undefined`, and you find out later when something looks wrong. (Module 4.3 adds TypeScript to put that safety net back.)
2. **Use `===`, not `==`.** Always three equals signs to compare. JavaScript's two-equals version has surprising rules that lead to confusing bugs; just avoid it.

That's enough to read today's code. You'll be comfortable faster than you expect — the shapes are ones you already think in.

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

## On your own

Time to put the book away. Don't scroll back up to the steps. From your own head, write and run this (the DevTools Console is a quick place to try it):

1. An `async` function that takes a `url`.
2. Inside it, `await fetch(url)` to get the response.
3. A second `await` on `response.json()` to read the body as JSON.
4. Return the data — then call the function so it actually runs.

No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

<details><summary>Stuck? Open this to check yourself.</summary>

```js
async function load(url) {
  const response = await fetch(url);
  if (!response.ok) {
    console.error("Request failed:", response.status);
    return;
  }
  const data = await response.json();
  return data;
}
```

The thing to get right: `async` on the function, `await` before `fetch`, and a second `await` before `.json()`. Forget either `await` and you end up working with a Promise instead of the value — that is the most common bug here.

</details>

## Wrap up

1. **Quiz** — open `quiz.md`, jot your answers in `journal/quiz-notes.md`.
2. **Progress** — one line in `journal/progress.md`: `Module 4.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit and push** — stage both files, commit message `Module 4.2 done`, Sync.
4. **Post in `#wins`** — one line about today, plus the URL of the commit.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher. Bring quiz answers you're least sure about to the next weekly sync.

## Next

Module 4.3 introduces TypeScript and Vite. Types come back; build tooling brings hot-reload, modules, and the rest of the modern frontend dev experience.
