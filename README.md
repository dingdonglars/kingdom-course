# Kingdom — A Programming Course

```
   _  _____ _   _  ____ ____   ___  __  __
  | |/ /_ _| \ | |/ ___|  _ \ / _ \|  \/  |
  | ' / | ||  \| | |  _| | | | | | | |\/| |
  | . \ | || |\  | |_| | |_| | |_| | |  | |
  |_|\_\___|_| \_|\____|____/ \___/|_|  |_|
```

> 🇧🇷 [Leia em português](./README.pt.md)

A year from now, you'll send a Roblox link to your friends and they'll play your game. They'll sign in, build farms on a grid, watch their resources tick up, race each other on a leaderboard. The game world they're playing in — the rules of how it works, the way it grows, the bit that makes it *fun* — was written by you, in code, from scratch.

Before that Roblox version, you'll have built the same game four other ways: as a thing that runs in your terminal, as a thing that saves itself to a database so it survives between sessions, as a website your friends can visit on a real public URL with their Google sign-in, and as a game playable in any browser. **Five working versions of the same idea.** Each one teaches you a chunk of how real software gets built. By the time you reach the Roblox version, the engineering is no longer the hard part — it's just translation.

Here's what's quietly true about all of this. The skills aren't toys — they're what working developers actually use to build the software you use every day. By the end of the year, the work you've shipped could be enough to land you a junior developer job: small companies regularly hire on portfolio, and yours will be a real one. Or if you head into university or a technical course instead, you'll arrive with real ground already under your feet — which makes choosing the right path easier, and succeeding once you're on it more likely. Either way, you'll be choosing from a position of strength, not guessing.

This is what the year looks like.

---

## Who this is for

Someone fifteen-ish, no prior code experience, comfortable on a computer, plays games, has four to six hours a week. Specifically: you. The course assumes nothing about prior programming background. It does assume you'll show up, write things down, and ship the work even when it gets uncomfortable. It will get uncomfortable. That's the deal.

---

## What this needs from you

Twelve months is a long time. The course works because both of us show up.

**What you commit to:** showing up. Putting hours in. Reading what's on screen before pinging. Writing the things down even when you don't feel like it. Not bailing the first time it stings. None of this needs to be heroic — four to six hours a week, every week — but it needs to be *real*. The uncomfortable parts are the parts where the learning lives.

**What I commit to:** when you're in it, I'm in it. The weekly sync. PR reviews inside the windows. Replies in `#help` you can count on. But that depends on you being in it. If you stop showing up, I'll quietly stop too — not as punishment, just because it doesn't work the other way around. Predictability over availability, both ways.

The deal in one sentence: *engaged on both sides, or neither side.*

### If you want out

At the end of any phase, you can decide you're done. No harsh feelings, no failure — *"I gave it Phase 3 and that was enough"* is a complete answer. The course is built so each phase ends with something real you keep, not something you only get after twelve more months. If you reach a phase boundary and the year stops feeling like the right shape, say so. We close it cleanly.

---

## What you'll actually be able to do

The skills are not decorative. By the time you finish, you'll genuinely know how to:

- **Use git the way working developers use it** — commit, branch, push, read a diff, recover from a mess. Mostly through VS Code's Source Control panel (the daily surface), with the terminal under your fingers for the moves the panel doesn't have a button for. Same git that backs every software project on Earth.
- **Write tests, and trust them.** When you change something and the tests still pass, you actually know you didn't break anything. That's a different relationship to your code than "I think it works."
- **Use a real database** — write SQL by hand, then use a code library that talks to it for you. Same pattern every web app you've ever used relies on.
- **Build a web API** — your code, accessible by URL, called by other programs over the internet.
- **Deploy to the cloud** — a real public URL anyone can visit, with auto-deploys every time you push.
- **Work with an AI assistant the right way** — context-engineer your prompts, read the output critically, and explain every line you ship.
- **Read other people's code.** The most underrated skill. Most of programming is reading.

These transfer. You can use them on a job, in a side project, in school, anywhere. Most are exactly what a working junior developer does on day one.

---

## What you'll build, in order

The course is laid out as seven phases — five core, two bonus — building toward the Roblox finale.

**Phase 0 — Spark Week + Foundations.** Your first month. Four small toys (a roast generator, a guessing game, a tiny adventure, a polished little CLI tool). The point is to get used to the rhythm: write, run, commit, push, brag in `#wins`. By the end of the month you're comfortable with the keyboard moves.

**Phase 1 — Console Kingdom.** The first real version of the Kingdom — buildings, citizens, resources, days that tick. Everything happens in your terminal. Fully tested. By the end you have a real engine running, and you've learned the rule the rest of the course rests on: the *engine* (the rules of the kingdom) is separate from the *shell* (the way you interact with it).

**Phase 2 — Persistence.** Your kingdom learns to remember itself across runs. First as a plain text file, then as JSON, then as a real SQL database with multiple save slots. By the end of this phase a player can quit the program and come back tomorrow to find the kingdom exactly where they left it.

**Phase 3 — Web API.** The same kingdom, now living at a real internet URL. Friends sign in with their Google account. Your code, hosted on a real cloud platform, redeploying every time you push. Halfway through this phase you reach the **AI Unlock** moment — the point where the rules around AI-assisted code expand. You earn that.

**Phase 4 — Browser Kingdom.** Your friends open a webpage, see your kingdom, click around, play. You learn HTML, CSS, JavaScript, and TypeScript along the way. The same engine that ran in the terminal in Phase 1 now powers a browser experience.

**Phase 5 — Roblox Kingdom.** *(Optional finale.)* You port the engine again — this time to Luau, Roblox's language — and publish a place your friends can play *on Roblox itself*. The reason this is optional and not skippable: by the time you finish Phase 4 you've earned everything the course set out to teach. Phase 5 is the brag.

**Phase 6 — Bonuses.** Three short bonuses if you want them. One swaps your database for a totally different one in three lines, just to feel how clean the engine/shell separation actually is. One goes deep on working with an AI assistant — the named skill of *context engineering*. One goes properly into git — the model underneath the commands you've been typing all year.

---

## How a typical week works

Most weeks you'll do one or two **modules**. A module is a single self-contained lesson — read, do, quiz, commit. There's a folder for each. Inside, you find a `lesson.md` (the teaching), a `starter/` folder (the code skeleton you're building from), and a `quiz.md` (a few questions to lock in what just landed).

Every few weeks you cross a **milestone** (M0 through M6). A milestone is the *brag-worthy* point — the place where you have something real to show off. Each milestone ends with a wins-log entry, a Slack post, and a sit-down review with Lars.

The rhythm is the rhythm of working developers. Picking it up now means you don't have to pick it up later under pressure.

---

## Where to start

Open `phase-0-spark/0.0-setup-and-roast-o-matic/lesson.md`. That's day one. About three hours from now you'll have shipped your first program to GitHub and posted your first `#wins`.

A few other docs you'll want to know exist, but don't have to read in order:

- **`MENTOR-PROTOCOL.md`** — how Lars and you work together: when to ping, how to ask, what to expect back. Read this before module 0.0.
- **`ai-tools.md`** — how to use Claude (your AI assistant) during the course.
- **`vscode-tips.md`** — the ten habits that turn VS Code from "wall of menus" into a tool you can fly. Skim once after Module 0.0.
- **`STYLE.md`** — what every lesson looks like, so you know what to expect.
- **`STANDARDS.md`** — the code, naming, and PR conventions used throughout.
- **`ENGLISH-NOTES.md`** — why the course is in English, and the small scaffolding that helps when a word stalls you.
- **`GLOSSARY.md`** — every term the course teaches, in alphabetical order.

---

## Where to ask for help

**Lars** is your mentor. The full *how we work together* protocol lives in `MENTOR-PROTOCOL.md` at the root of this repo — it describes when to ping him, how to ask, and what to expect back. Read it before module 0.0.

**Claude** is your AI assistant. The first half of the course, Claude helps with friction (a confused error message, a git mess). After the AI Unlock, Claude can also help you write code — under the rule that *you have to be able to explain every line you keep*. The full story is in `ai-tools.md`.

---

## A note on what's not in this repo

The **reference Kingdom** — Lars's working version of the project, written by him as a finished example — lives in a separate repo. **Don't open it until your phase is done.** The point isn't to copy his version; it's to write yours, then compare. Your phase only counts as finished when you've built it yourself and *then* peeked.

Your **own work** lives in your own repo, which you'll create on day one from `starter-template/`.

---

Welcome to the Kingdom. Let's build something.
