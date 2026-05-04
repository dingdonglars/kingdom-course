# Module 0.0 — Setup + Roast-O-Matic

By the end of today you have a real program on your computer that prints roasts you wrote — and it's on the internet, in your own GitHub repo. About three hours from now you'll have done what most people think takes weeks. We're going to install some tools, set up the folder where your year of work lives, write a tiny program, and push it where the world can see it.

## What you need first

- A Windows PC (you have this).
- About three hours uninterrupted. (Could be a bit less; could be a bit more if downloads are slow.)
- Lars sitting next to you for the install part. The fun starts after that.

---

## Part 1 — Tools (Lars walks you through this; about 45 minutes)

You install five things. Lars sits with you. Each install is mostly *next, next, next*. None of them are scary.

1. **Git for Windows** — the tool every developer in the world uses to save their work. [Download here](https://git-scm.com/download/win). Default options.
2. **VS Code** — the editor you write code in for the next year. [Download here](https://code.visualstudio.com/). Default options.
3. **.NET SDK 10** — the toolkit for building C# applications. [Download here](https://dotnet.microsoft.com/download/dotnet/10.0). Default options.
4. **Node.js LTS** — needed later (Phase 4) for the browser version. [Download here](https://nodejs.org/). LTS version, default options.
5. **A GitHub account** — your home on the internet for code. [Sign up here](https://github.com). You pick the username; this becomes part of your developer identity.

After everything's installed, open PowerShell (search "PowerShell" in the Start menu) and type:

```powershell
git --version
dotnet --version
node --version
```

You should see three version numbers. If any of them say "not found", flag it for Lars.

---

## Part 2 — Set up your work folder (about 15 minutes; you drive)

Before any code, set up the folder where everything for this course lives.

> **What git really is, in five sentences.** Git is a tool that takes *snapshots* of your code — every time you commit, it saves the whole state of the folder under a label so you can come back to it later. The snapshots live on your computer (the *local* copy) and on GitHub (the *remote* copy); pushing copies new snapshots up, and you can pull them down on any other machine. A *commit* is one labelled snapshot with a short message saying what changed; a *push* is shipping those commits to the internet. You'll be doing dozens of these in the first week and they'll feel weird before they feel normal — that's fine. The big idea: your code is never just *the file as it is now*; it's the whole history of how it got there, and git remembers the whole thing.

### A home for your code

Pick one folder on your computer where all your course work will live. Don't put it under `Documents` (Windows tends to sync that to OneDrive, which interferes with git). The simplest, cleanest choice — open PowerShell and type:

```powershell
mkdir C:\code
```

From now on, anything related to this course goes inside `C:\code\`.

### Get the course onto your computer

You're reading this on GitHub right now. From now on, you'll read lessons from your own machine — faster, available offline, and the same files include the starter kit you'll need in a few minutes. In PowerShell:

```powershell
cd C:\code
git clone https://github.com/dingdonglars/kingdom-course
```

You now have `C:\code\kingdom-course\` — every lesson, the glossary, and the starter kit, all on disk.

### Make YOUR repo on GitHub

This is the repo for *your* year of work — separate from the course repo. Go to [github.com/new](https://github.com/new).

1. **Name your repo `kingdom`.** *(Lessons refer to `kingdom`; pick differently and just substitute.)*
2. **Set it Public.**
3. **Don't tick "Add a README file"** — we'll bring in the starter README from the day-1 kit in Part 3.
4. Click **Create repository**.

Copy the repo URL from the green **Code** button.

### Clone YOUR repo into `C:\code\`

In VS Code:

1. Open VS Code.
2. `Ctrl + Shift + P` to open the Command Palette → type *"Git: Clone"* → Enter.
3. Paste the repo URL.
4. **When the file picker opens, navigate to `C:\code\` and click _Select Repository Location_.** The picker often defaults to your home folder or `Documents` — make sure the path bar shows `C:\code\` before you click. Otherwise the repo lands in the wrong place.
5. When VS Code asks *"Would you like to open the cloned repository?"*, click **Open**.

You now have `C:\code\kingdom\` on your PC, connected to GitHub, open in VS Code.

> **From tomorrow on**, two ways to come back here: in VS Code, *File → Open Recent → kingdom*. Or in PowerShell: `cd C:\code\kingdom` then `code .` — the dot means *"this folder."* Either way, VS Code opens on the kingdom folder.

> **Same move, in the terminal.** If you ever want to do this from PowerShell instead — useful when you're away from VS Code, or following a tutorial that assumes the terminal:
>
> ```powershell
> cd C:\code
> git clone <the-url-you-just-copied>
> cd kingdom
> ```
>
> Both ways do the same thing. Through the year you'll mostly use VS Code's git buttons; the terminal stays available for when it's faster.

---

## Part 3 — Drop in the day-1 kit (about 10 minutes)

Your `kingdom` repo is empty. Time to drop in the foundation — the conventions, the AI rules, the journal skeleton, all the files lessons will reference later. The course repo's `starter-template/` folder has them ready.

### Copy the starter files into your repo

In PowerShell:

```powershell
Copy-Item -Recurse C:\code\kingdom-course\starter-template\* C:\code\kingdom\
```

That brings in `STANDARDS.md`, `CLAUDE.md` (the AI rules), `.claude/commands/` (your slash commands), `journal/` (where wins and reflections live), `.github/PULL_REQUEST_TEMPLATE.md`, `.editorconfig`, `.gitignore`, and a starter `README.md` you'll personalise later.

### Commit the foundation — your first time using the Source Control panel

Switch back to VS Code (the kingdom folder is still open from Part 2). The git workflow lives in the **Source Control** panel:

1. Click the **Source Control** icon in the left sidebar — third icon down, looks like a branch with a fork. (Shortcut: `Ctrl + Shift + G` then `G`.)
2. You'll see a list of new files under *Changes*. Hover the word **Changes** and click the `+` icon to stage them all.
3. In the box at the top, type a commit message: *"day-1 kit"*.
4. Click the blue **checkmark** to commit. The commit is on your computer.
5. Click **Sync Changes** at the bottom of the panel (or the `...` menu → **Push**) to send it to GitHub. (If GitHub asks you to sign in, follow the prompts. Lars helps.)

Refresh your repo on github.com — the foundation is now live.

> **Same move, in the terminal:**
>
> ```powershell
> cd C:\code\kingdom
> git add .
> git commit -m "day-1 kit"
> git push
> ```
>
> The buttons in VS Code do exactly these four steps for you. Through the year you'll mostly use the buttons; the terminal stays useful for when something is faster typed than clicked.

---

## Part 4 — Roast-O-Matic (about 60 minutes; you drive)

Inside your `kingdom` repo, you'll create a small console project. Open PowerShell:

```powershell
cd C:\code\kingdom
dotnet new console -n RoastOMatic
cd RoastOMatic
dotnet run
```

You should see:

```
Hello, World!
```

**You just ran your first program.** That output came from real code on your computer.

### Do it — make it print roasts

Open `Program.cs` in VS Code (it should already be visible in the file tree on the left). You'll see one line: `Console.WriteLine("Hello, World!");`. Replace it with the standard demo (copy from `starter/Program.cs` in the course repo):

```csharp
string[] roasts = {
    "Your password is 'password' and we both know it.",
    "Your favorite Roblox game called. It wants its lag back.",
    "I'd insult your code, but you haven't written any yet.",
};

var random = new Random();
var roast = roasts[random.Next(roasts.Length)];
Console.WriteLine(roast);
```

Save. Back in PowerShell:

```powershell
dotnet run
```

A different roast each time. Run it three times. **You're calling code that picks something randomly.**

### Tinker — make the roasts your own

Now write your own roasts. About your friends. About the most annoying YouTuber you can think of. About your cat. Replace the three default roasts with five or six of your own.

Run it again. **Your code, doing what you told it.**

### Connect — what just happened

You wrote a program. It runs on your computer. It does what you decided. The rest of this course is the same thing, scaled up. Every program — every game, every website, every app — is *somebody told the computer what to do, and the computer did it*.

Six weeks from now, you'll laugh at how simple this seemed. Right now: it should feel like magic.

---

## Part 5 — Push the toy to GitHub (about 10 minutes)

Time to ship the roasts you wrote. You've already done this once (the day-1 kit in Part 3) — same panel, same five clicks.

In VS Code's Source Control panel:

1. Open the **Source Control** panel (`Ctrl + Shift + G G` if it isn't visible).
2. You'll see your changed files under *Changes* — including the new `RoastOMatic/` folder and its contents. Hover **Changes** → click `+` to stage all.
3. Commit message: *"first roasts of my own"*.
4. Click the blue checkmark to commit.
5. Click **Sync Changes** to push to GitHub.

Now go to your repo on github.com in the browser. **Refresh.** Your code is there. It's on the internet.

> **Same move, in the terminal:**
>
> ```powershell
> cd C:\code\kingdom
> git add .
> git commit -m "first roasts of my own"
> git push
> ```

---

## Part 6 — Join Slack, post your first win (about 15 minutes)

Lars sends you an invite link to a Slack workspace called **`kingdom-hq`**. Open it, accept, set a display name, add a profile picture if you feel like it.

There are four channels:

| Channel | What it's for |
|---|---|
| `#general` | Scheduling, plans, anything that doesn't fit elsewhere |
| `#wins` | Things you shipped. One post per real win. No reply expected. |
| `#help` | Stuck? Ask here. The channel topic is *"Show what you tried."* |
| `#milestones` | The big seven moments — M0 through M6 |

Now do the very first one. In `#wins`, post:

> 🎉 Roast-O-Matic shipped — *(link to your GitHub repo)*

That's it. Lars probably won't reply (that's the rule for `#wins` — no reply expected). The post is the trail. By the end of the year, scrolling back through `#wins` is the proof you did the work.

---

## What you just did

You installed the tools every working developer uses every day, set up a clean folder for your year of work, cloned the course onto your computer, made your own GitHub repo, dropped in the day-1 foundation, wrote a program in C#, ran it on your machine, edited it to do something *you* wanted (your own roasts), pushed it to the internet, and posted your first win. Most of the year is exactly this rhythm — write code, run it, commit it, push it, share the win — just with bigger ideas.

**Key concepts you can now name:**

- a *repo* — a folder of code that git tracks, with a copy on GitHub
- *commit* — saving a snapshot of your work, with a message
- *push* — sending your snapshots up to GitHub
- the **Source Control panel** in VS Code — the buttons that do `add`, `commit`, `push` for you
- *`dotnet run`* — build the C# code and run the result
- the four Slack channels — `#general`, `#wins`, `#help`, `#milestones`

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Welcome to programming. See you in Module 0.1.
