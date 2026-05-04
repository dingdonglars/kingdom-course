# How we work — Athos & Lars

> A two-way agreement on how we run the year together. This file lives in the course repo because both of us need to read it, not just Lars. Read it once now. Come back to it whenever something feels off.

## Why this exists

Twelve months is a long time. If we don't have a rhythm, we'll both drift — you'll feel stuck and not know who to ping; Lars will feel out of the loop and not know what to ask about. So we set the rhythm up front. Most of it is asynchronous (you write, Lars reads later) with one weekly sit-down. That's also how grown-up dev teams handle the exact same problem, so the pattern itself is one of the things you're learning.

The rest of this file is just the details.

## How to ask for help — the 20-minute rule

When something breaks or doesn't make sense, the rule is:

1. **Try yourself for 20 minutes.** Read the error. Search the lesson again. Re-read the bit of code that's misbehaving. Try one obvious thing.
2. **If you're still stuck, ask Claude.** Paste the error, say what you tried, ask the question.
3. **If Claude can't help either, ping Lars in Slack `#help`.** Show what you tried — the error, your attempt, the specific thing you want help with.

This isn't gatekeeping. It's the single most useful skill in this whole course. The version of you that asks a *good* question — error pasted, attempt named, specific question at the end — is the version of you who'll be hireable in a year. The version that types "halp my code is broken" is the version that's still googling at 11pm on a Friday.

A worked example. Bad ask:

> *"my save file isn't working"*

Good ask:

> *"I'm trying to load the kingdom save file on startup. Got this error:*
>
> ```
> JsonReaderException: Unexpected character encountered while parsing value
> ```
>
> *I checked — the file is empty. I deleted it and ran again, but now I get FileNotFoundException. I tried wrapping the load in try/catch and returning a fresh `Kingdom()` but it feels wrong. What's the standard pattern for 'load if exists, else start fresh'?"*

Same problem. Wildly different ease of helping. The good version names the error, names the attempt, asks a specific question. Build the muscle now.

**Language is free.** If you don't know an English word, ask. Anywhere, any time, no preface needed. Asking *"what does 'idiomatic' mean?"* is not stuck — it's normal. The course is in English on purpose; reaching for a definition is part of the deal, not a failure.

## The six ways we talk

There are six different kinds of conversation we have during the year. Knowing which one you're in keeps both of us sane.

**1. Wins.** When something works — the first kingdom that ran for thirty days without crashing, the first endpoint that returned real JSON, the first time you fixed a bug without help — drop a line in Slack `#wins`. Screenshot or output snippet, one sentence of context. Lars doesn't need to reply. The point is the trail. By month twelve, `#wins` is its own artefact and reading it back is the proof you grew.

**2. Async questions.** When you're stuck and the 20-minute rule says ping, post in Slack `#help`. Lars replies within 24–48 hours, usually faster. Don't break his work by texting him — `#help` is the channel. He'll see it. If something is on fire and 48 hours is too slow, see Tier 5 below.

**3. Milestone PR review.** Every milestone (M0 through M6) ends with a real Pull Request — same workflow professional teams use. You open the PR, link it in `#milestones`, Lars reviews it within 48–72 hours. Most of the review happens as PR comments. Some of it happens face-to-face (in person or video) — especially the bigger milestones where there's more to walk through.

**4. The weekly sync.** One scheduled sit-down per week, in person if we're together, video otherwise. This is where the bigger questions land — quiz answers you weren't sure about, design choices you want to talk through, things that need a whiteboard. You bring whatever's on your mind; Lars brings whatever he noticed. Calendar slot is fixed; if Lars has to move it, he tells you ahead, not after.

**5. Rescue.** For when you're *really* stuck — not "this test won't pass," but "I think I broke my repo and I'm scared to git anything." Knock on Lars's door (or call him). Same-day, best-effort. This should be rare by design — it's an emergency channel, not a default.

**6. Regroup.** Some weeks the code itself isn't the problem; the problem is *momentum*. You're tired, the lesson feels boring, the kingdom feels like a slog. Coffee, walk, "let's just chat" — no agenda. Easier to invoke than rescue. Doesn't have to be about the course at all. Best-effort within a few days.

## Both sides of the deal

This protocol works because we both hold up our ends.

**You commit to:**

- Using the 20-minute rule before pinging.
- Showing what you tried when you ask. Error pasted, attempt named, specific question.
- Opening a real PR for milestone reviews — not a casual *"I'm done, can you check?"*
- Showing up to weekly syncs prepared. Have a thing to demo, a question to raise, or a topic you want to talk through.

**Lars commits to:**

- Holding the weekly sync slot. If it has to move, he tells you ahead.
- Replying to `#help` and PR reviews inside the windows above. Even just *"saw it, will respond Saturday"* counts — silence doesn't.
- Not ghosting. If something goes sideways on Lars's side, he says so: *"slammed this week, sync moves to Tuesday."* Predictability beats availability.

## Slack — `kingdom-hq`

We use Slack, not WhatsApp, for the course. Slack lets us have separate channels for separate kinds of talk, so `#wins` doesn't drown out `#help`. The free tier is fine — 90 days of history, plenty for two people.

The four channels:

- **`#general`** — meta. Scheduling, plans, "I'm going to be away next Tuesday."
- **`#wins`** — Tier 1. Celebrate stuff. No reply needed.
- **`#help`** — Tier 2. Stuck-and-unstuck-yourself questions. Channel topic: *Show what you tried.*
- **`#milestones`** — Tier 3. Drop your PR link here, plus the GitHub Slack app posts updates.

If you can't decide which channel to use, use `#general`. Lars will move it.

## What every milestone ends with

Every milestone (M0 through M6) ends with the same three-step ritual. Don't skip it — it's small, takes ten minutes, and the *trail* it builds is more valuable than any single milestone.

**1. A `wins.md` entry.** In your repo, open `journal/wins.md` and write one paragraph in your own words. What did you build? What surprised you? What would you do differently? Free-form, dated, short. By M6 your `wins.md` is twelve months of you-as-a-developer in your own voice. Future-you will reread it.

**2. A `#wins` Slack post.** Link to the merged PR. One screenshot or output snippet. One-line caption.

**3. A before/after one-liner.** Pick the thing you couldn't do before this milestone and the thing you can do now, and put them in one sentence. Save it in `wins.md`. Example: *"Six weeks ago I'd never opened a terminal. Today I shipped a deterministic kingdom engine with thirty-five tests."* This is the line that matters most. Write it.

The ASCII trophies in the lessons are fun. The trail in `wins.md` is the *durable* layer underneath them.

## What every quiz ends with

Every lesson ends with a small quiz — five questions, multiple choice. They're for *you*, not for Lars. Their job is to surface what you half-understood so you can poke at it before it hardens into a wrong idea.

After taking the quiz, write your answers in `journal/quiz-notes.md`. One letter per question, one sentence saying *why* you picked that letter. Same habit as `wins.md`, just smaller cadence.

You don't get an answer key in the course repo (that's deliberate — answer keys live separately so the quiz is real). At the next weekly sync, Lars has the key in front of him and you walk through whichever ones you flagged. If a question is genuinely blocking you mid-week, ping `#help` like anything else — but most quiz questions can wait for the sync.

## Final note

This whole document is a plan, not a contract. If something isn't working — the 24-hour window is too long, `#help` is silent, the sync slot is wrong, the rituals feel like overhead — say so. We change it. The rhythm matters; the specific rules don't. The point is that we're both in the same year, with the same expectations, and neither of us is guessing what the other is up to.
