# Quiz answers — Module 4.2

## 1. b
The DOM is the in-memory tree the browser builds from your HTML. JavaScript can read and mutate it: add nodes, change text, swap classes. Updates are immediately reflected on the page.

## 2. b
`fetch(url)` returns a Promise; `await` pauses your async function until the Promise resolves to a `Response`. You then `await response.json()` to parse the body. Two awaits is the standard pattern.

## 3. b
`await` is a syntax marker for the compiler to rewrite the function into a suspending state machine. It can only appear inside `async` functions. Top-level `await` works in modules but not in plain script tags.

## 4. b
Browsers refuse to expose responses from origins the server didn't allow. If `https://api.example.com` doesn't say `Access-Control-Allow-Origin: https://your-frontend.com`, your JS sees an error instead of the body — even if the request itself succeeded. CORS prevents one site from silently scraping another.

## 5. b
Three tabs: Elements (live DOM, click anything), Console (run JS, see logs), Network (every request, headers, timing). Together they show you what's *really* happening. Closing them is willful blindness.