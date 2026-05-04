# Module 0.0 — Setup + Roast-O-Matic

By the end of today you have a real program on your computer that prints roasts you wrote — and it's on the internet, in your own GitHub repo. Two hours from now you'll have done what most people think takes weeks. We're going to install some tools, write a tiny program, and push it where the world can see it.

## What you need first

- A Windows PC (you have this).
- About two hours uninterrupted.
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

## Part 2 — Your first repo (about 10 minutes; you drive)

A *repo* is short for repository — a folder of code that git tracks for you. GitHub stores a copy on the internet so you don't lose it.

1. Go to [github.com/new](https://github.com/new).
2. **Name your repo.** This is where your year of work lives. Pick a name you like (you can rename later, but most people don't).
3. **Set it Public.** Don't worry — there's nothing in it yet.
4. **Tick "Add a README file."**
5. Click **Create repository**.

Now copy the repo URL from GitHub. In PowerShell:

```powershell
cd $HOME
git clone <the-url-you-just-copied>
cd <your-repo-name>
```

You now have a folder on your PC that's connected to GitHub. Anything you commit and push here goes to the internet.

---

## Part 3 — Roast-O-Matic (about 75 minutes; you drive)

Inside your repo folder, run:

```powershell
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

Open the `RoastOMatic` folder in VS Code:

```powershell
code .
```

(If `code` isn't recognised, open VS Code by hand and use *File → Open Folder*.)

Open `Program.cs`. You'll see one line: `Console.WriteLine("Hello, World!");`. Replace it with the canonical demo (copy from `starter/Program.cs`):

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

## Part 4 — Push it to GitHub (about 10 minutes)

Back in PowerShell, in the `RoastOMatic` folder:

```powershell
cd ..
git add .
git commit -m "first roasts of my own"
git push
```

(If `git push` asks for credentials, follow the prompts. Lars helps.)

Now go to your repo on github.com in the browser. **Refresh.** Your code is there. It's on the internet.

---

## Part 5 — Join Slack, post your first win (about 15 minutes)

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

You installed the tools every working developer uses every day, made your own GitHub repo, wrote a program in C#, ran it on your machine, edited it to do something *you* wanted (your own roasts), pushed the result onto the internet, and posted your first win. Most of the year is exactly this — write code, run it, commit it, push it, share the win — just with bigger ideas.

**Key things you can now name:**

- a *repo* — a folder of code that git tracks, with a copy on GitHub
- *git commit* — saving a snapshot of your work
- *git push* — sending your snapshot up to GitHub
- *`dotnet run`* — build the C# code and run the result
- the four Slack channels — `#general`, `#wins`, `#help`, `#milestones`

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Welcome to programming. See you in Module 0.1.
