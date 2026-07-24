# The Road Ahead — the whole course, lesson by lesson

> 🇧🇷 [Leia em português](./the-road-ahead.pt.md)

You just finished Phase 0. You can already write a small program, run it, and push it to GitHub. That's further than most people ever get. Now the real project starts — the **Kingdom** — and it grows a little every week for the rest of the year.

This page is the map. It shows every lesson ahead of you, one short line each: **what you'll make** and **what new idea it teaches you**. Not to memorise — just so the year isn't a mystery. When you finish a lesson and wonder "where is this going?", come back here and look.

> **A lot of the words below will look like nonsense right now.** *SQL, API, deploy, TypeScript, DataStore…* That's completely fine — it's supposed to. You don't learn these by reading a list; you learn them by using them, and each one shows up with a full explanation the day you need it. Just skim. This is the destination, not a test.

**How to read it:** don't read it all now. Read the phase you're in, glance at the next one, and move on. The far-off phases are here for the days you want to see how big the whole thing really is.

---

## The shape of the year

You build the **same game five times**, each time in a different place:

1. **In your terminal** (Phase 1) — text on a black screen.
2. **Saved to disk** (Phase 2) — so it remembers itself when you close it.
3. **On the internet** (Phase 3) — a real web address friends can visit.
4. **In a browser** (Phase 4) — a web page they click and play.
5. **On Roblox** (Phase 5) — where your friends already hang out.

The clever part: the **rules of the game** — how the kingdom grows, how a day passes, who wins — you write once, in Phase 1. After that you're not rewriting the game five times; you're moving the same rules into five different homes. Each phase ends with a **milestone** (M0–M6): a point where you have something real to show off, and a sit-down with Lars.

---

## Phase 0 — Spark Week + Foundations  *(you're finishing this)*
*Four little toys, then the language pieces that hold them together. Milestones M0 and M1.*

**Why this phase —** Get your hands making things before anyone explains the theory, and build the daily rhythm: write, run, commit, push. You meet the basics by *using* them, so the words mean something later.

**Where you stand after it —** No Kingdom yet — that's Phase 1. You'll have a repo with four little toys and the Inventory Tool, and the basics proven cold at the gate. Warmed up, ready to build the real thing.

- **0.0 Setup** — Install your tools and set up your workshop with Lars. Nothing to build yet — you're getting ready.
- **0.0.5 Primer** — A calm tour of what's actually on your computer: what a file is, what "running a program" really means. No code, so nothing feels like magic later.
- **0.0.8 Roast-O-Matic** — Your first real program: it prints funny insults you can aim at a friend. You make the computer say something *you* chose.
- **0.1 Tinker** — You take Roast-O-Matic and make it yours — new words, your own jokes. The lesson: code isn't fixed, you can poke it and it changes.
- **0.2 Number Guess** — A guessing game that talks back rudely when you're wrong. You learn how a program reacts differently depending on what you type.
- **0.3 Tiny Adventure** — A little three-room text adventure in a world you invent. You learn how a program moves between places and remembers where you are.
- **0.4 Polish + Ship** — You dress one toy up with ASCII art and colour, and make it save to a file so it's still there next time. Then you ship it. *(Milestone M0 — your Joke Toolbox.)*
- **0.5 Types + Naming** — You slow down and put names to what you've been using: numbers, words, true/false. You learn why good names make code easier to read.
- **0.6 Methods** — You learn to wrap a chunk of work under a name and reuse it — like a spell you can cast whenever you need it.
- **0.7 Collections + Inventory Tool** — You build the Inventory Tool, a little program that keeps a list of your stuff. You learn how code holds many things at once.
- **0.8 Errors + Debugging** — You learn what to do when the program breaks, and how to watch it run one step at a time with the debugger. *(Milestone M1 — the Inventory Tool.)*
- **0.9 · 0.9b · 0.9c Foundations checkpoints** — Three times, you rebuild a small program from a blank file with nobody helping — a tavern tab, a quest board, an arena. This proves the basics really stuck before the big project begins.

---

## Phase 1 — Console Kingdom  *(you start here next)*
*The first real Kingdom, running in your terminal. Milestone M2.*

**Why this phase —** Build the Kingdom's beating heart, and learn the one rule the whole year rests on: the rules of the game live apart from the screen.

**The solution after it —** One "house" (solution) with three rooms: the program you run, the rules engine, and the tests that guard them. A full, playable tycoon in your terminal, all tests green.

- **1.0 Set Up the Kingdom** — You build the home the Kingdom will live in all year, and make it say hello. Today is setup — the workshop, not the game yet.
- **1.1 OOP Basics** — The first real kingdom appears on screen: buildings, citizens, resources. You learn to make your own *kinds of things* in code.
- **1.2 Engine vs Shell** — You split the kingdom's rules away from the screen. This is the biggest idea of the whole year: the rules don't care how you look at them.
- **1.3 Unit Testing Arrives** — You write code that checks your other code is right. From now on, when everything's green, you *know* you didn't break anything.
- **1.4 The Game Loop** — Days start to tick. The kingdom now changes over time — resources climb, things happen turn by turn.
- **1.5 Inheritance** — You make special kinds of buildings that share a base but each do their own thing. Less copying, more reuse.
- **1.6 LINQ** — You learn to ask the kingdom questions in one short line — "which building makes the most gold?" — and get the answer straight back.
- **1.7 Events and Randomness** — Random events hit the kingdom: good harvests, bandit raids. The game stops being predictable and starts being fun.
- **1.8 Interfaces + Fakes** — You learn a trick to test the random and time-based bits reliably, by swapping in a fake dice or a fake clock.
- **1.9 Code Organisation** — You tidy the growing project into sensible folders and files, so it stays easy to find your way around.
- **1.10 Polish + Rescue** — You polish it into a real, playable game: name your kingdom, play a 50-turn run, win or lose. *(Milestone M2 — Kingdom v1, Console.)*

---

## Phase 2 — Persistence
*Your kingdom learns to remember itself. Milestone M3.*

**Why this phase —** Make the Kingdom remember itself, so closing the program doesn't wipe it. You meet the three ways software stores things — a file, JSON, a database.

**The solution after it —** A new room added for saving (plus its own tests). The same terminal game, but now it saves and loads, with many separate save slots kept in a real database. Quit today, come back tomorrow — it's all still there.

- **2.1 File I/O** — Your kingdom writes itself to a file on disk for the first time. Close the program — the file's still there.
- **2.2 JSON** — You save the kingdom in a tidy, standard format that programs everywhere use. Save and load, cleanly.
- **2.3 Round-Trip Tests** — You prove that what you save comes back exactly the same when you load it. No quiet data loss.
- **2.4 SQL Primer** — You meet a database: a smarter store you can ask questions of. You learn its almost-plain-English query language.
- **2.5 JOINs** — You learn to pull related things together in one question — like matching each citizen to the building they work in.
- **2.6 EF Core** — You let a helper library do the talking to the database for you, straight from your classes. Less hand-written query, more just-works.
- **2.7 Migrations** — You learn to change the shape of your saved data safely, without throwing away what's already saved.
- **2.8 DB Tooling** — You get tools to peek inside the database and see your data with your own eyes.
- **2.9 Save Slots** — Many separate saved kingdoms, not just one. Like save slots in a real game.
- **2.10 Save-Slot UI** — A little menu to pick, load, and manage those saves from inside the program.
- **2.11 Names That Earn Their Keep** — A careful pass over every name in your code, making it read clearly. *(Milestone M3 — the kingdom survives a restart.)*

---

## Phase 3 — Web API on the Internet
*The same kingdom, now living at a real web address. Milestone M4 — and your AI assistant joins.*

**Why this phase —** Lift the Kingdom off your computer and onto the internet, where a friend can reach it by a link. Same rules; a new outer layer that answers requests from anywhere.

**The solution after it —** Another room — the web service — with its own tests. The Kingdom lives at a real public address, friends sign in with Google and each get their own, and every push updates the live site by itself. Your AI assistant has also joined the work.

- **3.0 Reading Code Before Writing It** — No writing today: you read two real pieces of code written by pros. Reading code is a skill, and you start it early.
- **3.1 HTTP + First Endpoint** — Your kingdom gets its first tiny door to the internet: a web address that answers when something calls it.
- **3.2 DTOs + POST** — You learn the safe way to send and receive data over the web, and add an address that advances the kingdom a turn.
- **3.3 Routing + Status Codes** — More web addresses, and the little codes that say "ok" or "not found". You can now manage many kingdoms over the web.
- **3.4 OpenAPI + Logging** — Your web service gets a self-made instruction page, and starts keeping a diary of what it does so you can spot problems.
- **3.5 OAuth (Google)** — Friends sign in with their real Google account — no passwords for you to handle. Proper, real sign-in.
- **3.6 Multi-User Persistence** — Every kingdom now belongs to a real signed-in person. Your friends each get their own.
- **3.7 Integration Tests** — You test the whole web server end to end, the way a real visitor would hit it — automatically.
- **3.8 Deploy + CI/CD** — Your kingdom goes live on the real internet, at a URL you can text. And every time you push, it updates itself.
- **3.9 M4 Close + the AI Unlock** — The milestone — and the day your AI assistant joins the work, under one rule: you can explain every line you keep. *(Milestone M4 — Live API.)*

---

## Phase 4 — Browser Kingdom
*That web address becomes a page your friends click and play. Milestone M5.*

**Why this phase —** Turn that address into something people actually *play* — a page they click, not a command they type. You learn what the browser really does underneath.

**The solution after it —** The house gains a front — a browser page (its own folder, with tests). Friends open a link and play in a browser tab, talking to the same web service and the same engine you wrote in Phase 1.

- **4.0 Context Engineering Unlock** — You learn to set up what the AI can see before it answers, so its help actually fits your project.
- **4.1 HTML & CSS** — The building blocks of any web page: the content, and how it looks.
- **4.2 Browser as a Runtime** — You learn what the browser really is under the hood, and get your first page talking to your live kingdom.
- **4.3 TypeScript + Vite** — A friendlier, safer way to write browser code, plus the modern tools that make building fast.
- **4.4 Componentised UI** — You build the screen out of reusable pieces — the idea every big framework is built on.
- **4.5 Vitest** — Tests, but for the browser side now. Same safety net, new place.
- **4.6 Deploy the Frontend** — The playable web page goes live on the internet, next to your API.
- **4.7 M5 Close + Reflection** — The milestone — plus a quiet look back at your very first code, with new eyes. *(Milestone M5 — Browser-Playable.)*

---

## Phase 5 — Roblox Kingdom  *(the finale)*
*Your game, on Roblox, where your friends already play. Milestone M6.*

**Why this phase —** Put the game where your friends already spend their evenings — Roblox — by carrying the same engine into one more language.

**The solution after it —** Beside everything else now sits a Roblox game, its engine rewritten in Roblox's language but following the exact same rules — and published, so anyone with the link can play. The whole point of the year, proven.

- **5.1 Roblox Studio Tour** — You open Roblox Studio and learn your way around the editor where the game gets built.
- **5.2 Luau Basics** — The basics of Roblox's language. It's a cousin of what you already know, so this goes fast.
- **5.3 OOP in Luau** — You learn how Roblox makes "kinds of things" — a different route to the same idea you met in Phase 1.
- **5.4 Roblox Specifics** — How Roblox splits the server from each player, and how they talk. The rules of multiplayer.
- **5.5 Engine Port** — Your Phase 1 engine crosses into Luau and starts running inside Roblox. Same rules, new home.
- **5.6 The Visual World** — The kingdom appears as real 3D blocks in the world, spawned from your code.
- **5.7 DataStore** — Roblox's own way of saving. The kingdom survives between play sessions again.
- **5.8 Publish + M6 Close** — You publish the game. You send the link. Your friends play. *(Final milestone, M6 — Roblox-Published.)*

---

## Phase 6 — Bonuses  *(optional, whenever you like)*
*Three short extras, each its own small brag. Best picked up after the finale.*

**Why this phase —** Optional extras that sharpen tools you've used all year. Each is a small, standalone win.

**After it —** The core Kingdom doesn't change — these are skills, not new rooms. You come out able to swap an entire database in three lines, steer an AI assistant on purpose, and use git with the model in your head, not just the commands.

- **B1 — Swap the Database** — You swap your database for a completely different one in three lines, and watch every test stay green — proof the engine-vs-shell idea from Phase 1 really works. Then you meet the professional database tool.
- **B2 — Context Engineering** — You go deeper on working with your AI assistant: how to frame it, feed it the right context, and read its answers with a careful eye.
- **B3 — Git, properly** — You open up git for real — what it actually stores under all the commands you've typed all year, and how to rescue work you thought was lost.

---

## And at the very end

After the Roblox game is live, there's a printed **diploma** with your name on it. Not a badge, not a sticker — a real page that names the engineering you actually did across the year: the commits, the tests, the reviews, the deploys, five working versions of one idea. That's the thing you walk away with.

That's the whole road. You don't have to see the end from here — you just have to take the next lesson. Let's build.
