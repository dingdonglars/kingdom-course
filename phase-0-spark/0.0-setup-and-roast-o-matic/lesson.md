# Module 0.0 — Setup + Roast-O-Matic

By the end of today you have a real program on your computer that prints roasts you wrote — and it's on the internet, in your own GitHub repo. Three or four hours from now you'll have done what most people think takes weeks. We're going to install some tools, set up the folder where your year of work lives, write a tiny program, and push it where the world can see it.

## What you need first

- A Windows PC (you have this).
- About three to four hours uninterrupted. (Could be less; could be more if downloads are slow.)
- Lars sitting next to you for the install part and the Claude setup. The fun starts after that.

> **Heads up — the *how-we-work-together* doc.** `MENTOR-PROTOCOL.md` is the two-way agreement on how Lars and you run the year (the 20-minute rule, the four channels, the weekly sync, the off-ramp). You'll read it during the first week — either from your cloned course repo (`C:\code\kingdom-course\MENTOR-PROTOCOL.md` after Part 2) or from the link pinned in Slack `#all-kingdom-hq` (after Part 7). Bring it to the first weekly sync.

---

## Part 1 — Tools (Lars walks you through this; about 70 minutes)

You install six tools, add one VS Code extension, set up a styled terminal prompt, and tell git who you are. Lars sits with you. Each install is mostly *next, next, next*. None of them are scary.

1. **Git for Windows** — the tool every developer in the world uses to save their work. [Download here](https://git-scm.com/download/win). Default options.
2. **VS Code** — the editor you write code in for the next year. [Download here](https://code.visualstudio.com/). Default options.
3. **.NET SDK 10** — the toolkit for building C# applications. [Download here](https://dotnet.microsoft.com/download/dotnet/10.0). Default options.
4. **Node.js LTS** — needed later (Phase 4) for the browser version. [Download here](https://nodejs.org/). LTS version, default options.
5. **Windows Terminal** — the modern terminal app. Better fonts, multiple tabs, transparency, the works. From the Start menu open **Microsoft Store**, search **Windows Terminal**, click **Get**. (On Windows 11 it's often pre-installed — search for it; if it's there, skip the install.)
6. **A GitHub account** — your home on the internet for code. [Sign up here](https://github.com). You pick the username; this becomes part of your developer identity.

### Verify the installs

Open **Windows Terminal** (search for it in the Start menu — pin it to the taskbar while you're there; you'll open it five times a day). It defaults to PowerShell. Type:

```powershell
git --version
dotnet --version
node --version
```

You should see three version numbers. If any of them say "not found", flag it for Lars.

### Add the C# Dev Kit extension to VS Code

Without this, `.cs` files have no IntelliSense, no debugger, no syntax colours.

1. Open VS Code.
2. Click the **Extensions** icon in the left sidebar (four squares, fourth icon down). Shortcut: `Ctrl + Shift + X`.
3. Search **C# Dev Kit** (publisher: Microsoft).
4. Click **Install**. It pulls in the C# language server alongside it. Takes about a minute.

When you open a `.cs` file later, you'll see colours, suggestions, and an inline *Run | Debug* link above `Main`. That's this extension.

### Make the terminal prompt show git status

Default PowerShell prompt is one line of useless: `PS C:\code\kingdom>`. Real developers run a *styled prompt* that shows the current git branch, whether you have uncommitted changes, and which folder you're in — all at a glance. This is **oh-my-posh**, a tiny add-on that makes the terminal genuinely useful.

**Install a font that has the icons.** Oh-my-posh uses small symbols (a branch icon, a folder icon, a `±` for unstaged changes) that come from a *Nerd Font*. Without one, you'll see boxes where icons should be.

1. Go to [nerdfonts.com/font-downloads](https://www.nerdfonts.com/font-downloads).
2. Find **CaskaydiaCove Nerd Font** (it's a tweaked version of Cascadia Code, the default Windows Terminal font). Download.
3. Unzip. Right-click the `CaskaydiaCoveNerdFont-Regular.ttf` file → **Install for all users**.

**Tell Windows Terminal to use it.** Open Windows Terminal → click the `∨` next to the tab bar → **Settings** → **Profiles → Defaults → Appearance** → **Font face** → **CaskaydiaCove Nerd Font** → **Save**.

**Install oh-my-posh.** In Windows Terminal:

```powershell
winget install JanDeDobbeleer.OhMyPosh -s winget
```

Close Windows Terminal and reopen it (so the new tool is on the path).

**Make oh-my-posh load every time you open the terminal.** Edit your PowerShell profile:

```powershell
notepad $PROFILE
```

If Notepad asks whether to create the file, say yes. Paste this single line, save, close:

```powershell
oh-my-posh init pwsh --config "$env:POSH_THEMES_PATH/atomic.omp.json" | Invoke-Expression
```

Open a fresh Windows Terminal tab. Your prompt is now coloured, shows the folder, and — once you're inside a git repo — the branch name plus a `±` mark when you have uncommitted changes. Try a few themes later via `Get-PoshThemes`; pick whatever you like.

> **If something looks wrong** — boxes instead of icons mean the font isn't actually being used (recheck the Settings step); a slow first prompt is normal (oh-my-posh warms up on first call); a red error from `notepad $PROFILE` usually means PowerShell's execution policy blocks scripts. Run `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` once, type `Y`, and you're fine.

### Tell git who you are

Every commit gets stamped with a name and email. Set them once, globally, to match your GitHub account:

```powershell
git config --global user.name "your-github-username"
git config --global user.email "the-email-you-used-on-github@example.com"
```

Use the same email you signed up to GitHub with — that's how GitHub links commits to your profile picture and contribution graph. Confirm with:

```powershell
git config --global user.name
git config --global user.email
```

You should see what you just typed echoed back.

---

## Part 2 — Set up your work folder (about 15 minutes; you drive)

Before any code, set up the folder where everything for this course lives.

> **What git really is, in five sentences.** Git is a tool that takes *snapshots* of your code — every time you commit, it saves the whole state of the folder under a label so you can come back to it later. The snapshots live on your computer (the *local* copy) and on GitHub (the *remote* copy); pushing copies new snapshots up, and you can pull them down on any other machine. A *commit* is one labelled snapshot with a short message saying what changed; a *push* is shipping those commits to the internet. You'll be doing dozens of these in the first week and they'll feel weird before they feel normal — that's fine. The big idea: your code is never just *the file as it is now*; it's the whole history of how it got there, and git remembers the whole thing.

### A home for your code

Pick one folder on your computer where all your course work will live. Don't put it under `Documents` (Windows tends to sync that to OneDrive, which interferes with git). The simplest, cleanest choice — open Windows Terminal and type:

```powershell
mkdir C:\code
```

From now on, anything related to this course goes inside `C:\code\`.

### Get the course onto your computer

You're reading this on GitHub right now. From now on, you'll read lessons from your own machine — faster, available offline, and the same files include the starter kit you'll need in a few minutes. In Windows Terminal:

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

> **From tomorrow on**, two ways to come back here: in VS Code, *File → Open Recent → kingdom*. Or in Windows Terminal: `cd C:\code\kingdom` then `code .` — the dot means *"this folder."* Either way, VS Code opens on the kingdom folder.

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

In Windows Terminal:

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
5. Click **Sync Changes** at the bottom of the panel (or the `...` menu → **Push**) to send it to GitHub. The first time, VS Code will pop a browser window to sign in to GitHub — *Authorise Visual Studio Code*, then come back. VS Code remembers the login from there on. Lars sits with you for this part.

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

Inside your `kingdom` repo, you'll create a small console project. Open Windows Terminal:

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

Save. Back in Windows Terminal:

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

## Part 6 — Set up Claude Code (Lars walks you through this; about 20 minutes)

Claude is your AI assistant for the year. The full rules are in `ai-tools.md` — read that *after* this part. The short version: pre-Phase-3 it helps with friction (confused error messages, git scrapes, environment setup); post-Unlock it can help write code under the rule that *you can explain every line you keep*.

Lars sits with you for this part — the Anthropic account he set up is on his card; the install is one step but the sign-in is two.

### 1. Install Claude Code

Claude Code is the CLI form — a chat that runs in your terminal, in the same folder as your repo, with your code in context. Install it:

```powershell
npm install -g @anthropic-ai/claude-code
```

Verify:

```powershell
claude --version
```

You should see a version number.

### 2. Create your Anthropic account, together with Lars

The account is in **your name and your email**; you pick the password and only you know it. Lars puts in the payment card — the subscription is on his card, but the account, the history, and the password belong to you.

In a browser, go to [console.anthropic.com](https://console.anthropic.com). Sign up with your email; pick a password; complete the email confirmation. Lars enters the card details for the subscription tier he picked. You're now the owner of an Anthropic account that Lars pays for.

Then, in Windows Terminal:

```powershell
cd C:\code\kingdom
claude
```

The first launch pops a browser to sign in to Anthropic. Sign in with the account you just created. After sign-in, the browser closes, the terminal shows a prompt, and Claude is alive in your kingdom folder.

### 3. Try one slash command

Type `/` in the Claude prompt. You'll see a list including `/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`. These came in with the day-1 kit (in `.claude/commands/`).

Pick one to try. *"explain to me what `git push` actually does, briefly"* — `/explain-this-concept` is the right shape for that. Read what comes back. Type `/exit` (or `Ctrl + C`) to leave.

That's enough for today. The full rules — what to ask, what not to ask, the three buckets — are in `ai-tools.md`. Read it tonight.

> **Why Claude and not Copilot or ChatGPT?** Claude is what Lars uses, so it's what Lars can guide you on. Copilot and Cursor and ChatGPT are fine tools too — once you know the patterns the course teaches with Claude, the patterns transfer. The full *why* is in `ai-tools.md`.

---

## Part 7 — Join Slack, post your first win (about 20 minutes)

Slack is where everything async happens this year — wins, help requests, milestone PR links. The Mentor Protocol depends on you being reachable, so Slack lives on both your laptop *and* your phone.

### 1. Accept the invite

Lars sends you a link to a workspace called **`kingdom-hq`**. Open it.

- Sign up with your **personal email** (not a school email — schools sometimes block external sign-ins).
- **Display name:** something simple and lowercase, like your first name. Lars sees this; it's a private workspace.
- Profile picture if you feel like it. A photo or an avatar — anything that makes it obvious it's you.

### 2. Install Slack on your laptop

The browser version works, but the desktop app is faster and gets notifications properly. [Download here](https://slack.com/downloads). Sign in with the same email. Pick the `kingdom-hq` workspace.

### 3. Install Slack on your phone

This one matters more than you'd think. Lars works async — he replies in `#help` *eventually*, not instantly. If Slack only lives on your laptop, you'll miss replies that land while you're away from it. App Store / Play Store → search Slack → install → sign in.

### 4. Tune notifications

Default Slack pings on every message. For two-person Slack that's about right, but a couple of channel-level adjustments help:

- `#wins` → notifications **on**. You'll want to know when Lars posts a win for *you* (it happens).
- `#help` → notifications **on**. When Lars replies, you want to see it fast.
- `#milestones` → notifications **on**. Same reason.
- `#all-kingdom-hq` → notifications **on mention only**. Scheduling chatter doesn't need a buzz.

On the phone: tap the workspace name → *Notifications* → tap each channel.

### 5. The four channels

| Channel | What it's for |
|---|---|
| `#all-kingdom-hq` | Scheduling, plans, anything that doesn't fit elsewhere. **MENTOR-PROTOCOL** is pinned in the channel — one click whenever you want to re-read the rules. |
| `#wins` | Things you shipped. One post per real win. No reply expected. |
| `#help` | Stuck? Ask here. The channel topic is *"Show what you tried."* The 20-minute rule applies. |
| `#milestones` | The big seven moments — M0 through M6 — and PR links |

### 6. Post your first win

Two places — both matter.

**First, `journal/wins.md` in your repo.** Open the file (it came in with the day-1 kit). Add an entry under today's date:

```markdown
## YYYY-MM-DD — M0.0 — Roast-O-Matic shipped

First commit, first push, first program of my own. The repo is live.
```

Save. Commit. Push. (You know how — Source Control panel, message *"M0.0 wins entry"*, sync.) `wins.md` is your in-repo trail; you'll add to it every milestone for the rest of the year.

**Then, in Slack `#wins`:**

> 🎉 Roast-O-Matic shipped — *(link to your GitHub repo)*

That's it. Lars probably won't reply — that's the rule for `#wins`, no reply expected. The post is the trail. By the end of the year, scrolling back through `#wins` is the proof you did the work, alongside `journal/wins.md` in your repo.

---

## What you just did

You installed the tools every working developer uses every day, told git who you are, set up a clean folder for your year of work, cloned the course onto your computer, made your own GitHub repo, dropped in the day-1 foundation, wrote a program in C#, ran it on your machine, edited it to do something *you* wanted (your own roasts), pushed it to the internet, met Claude Code in your terminal, joined Slack on laptop and phone, and posted your first win — both in `journal/wins.md` and in `#wins`. Most of the year is exactly this rhythm — write code, run it, commit it, push it, share the win — just with bigger ideas.

**Key concepts you can now name:**

- a *repo* — a folder of code that git tracks, with a copy on GitHub
- *commit* — saving a snapshot of your work, with a message
- *push* — sending your snapshots up to GitHub
- the **Source Control panel** in VS Code — the buttons that do `add`, `commit`, `push` for you
- *`dotnet run`* — build the C# code and run the result
- *Claude Code* — your AI assistant in the terminal; rules in `ai-tools.md`
- the four Slack channels — `#all-kingdom-hq`, `#wins`, `#help`, `#milestones`

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Welcome to programming. See you in Module 0.1.
