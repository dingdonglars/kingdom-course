# Module 0.0 — Setup

By the end of this session your computer is ready, your code home is set up, and you're in Slack. Editor, compiler, terminal, git, a GitHub account, a styled prompt, the two folders where every line of code from this year will live, and the workspace where everything async happens. You do this with Lars beside you; everything from the next module on you do solo. About two and a half hours, often less.

## What you need first

- A Windows PC (you have this).
- **Your phone**, charged. You'll install Slack on it during Part 3.
- Lars sitting next to you. A few installer dialogs are easier with someone who's been here before; the GitHub account, the Slack invite, and the mobile-install sanity-check all pair better when the person paying for the year is in the room.
- About two and a half hours uninterrupted.

> **After today — read the mentor protocol.** `MENTOR-PROTOCOL.md` is the two-way agreement on how Lars and you run the year (the 20-minute rule, the four channels, the weekly sync, the off-ramp). Two ways to find it: `C:\code\kingdom-course\MENTOR-PROTOCOL.md` on your disk (cloned in Part 2), or pinned in `#all-kingdom-hq` once you join Slack in Part 3. Read it during the first week and bring it to the first weekly sync.

---

## Part 1 — Tools (Lars walks you through this; about 70 minutes)

You install six tools, add one VS Code extension, set up a styled terminal prompt, and tell git who you are. Lars sits with you. Each install is mostly *next, next, next*. None of them are scary.

Install **VS Code first** — that way the Git installer can set VS Code as the default editor instead of Vim, which you do not want to meet today.

1. **VS Code** — the editor you write code in for the next year. Download from <https://code.visualstudio.com/>. Default options. Make sure *"Add to PATH"* is ticked (it is by default).
2. **Git for Windows** — the tool every developer in the world uses to save their work. Download from <https://git-scm.com/download/win>. Default options **except one screen**: when the installer asks *"Choosing the default editor used by Git"*, pick **Use Visual Studio Code as Git's default editor** (it's in the dropdown — VS Code is already installed from step 1, so it shows up). Vim is the silent factory default; you don't need it today.
3. **.NET SDK 10** — the toolkit for building C# applications. Download from <https://dotnet.microsoft.com/download/dotnet/10.0>. Default options.
4. **Node.js LTS** — needed later (Phase 4) for the browser version. Download from <https://nodejs.org/>. LTS version, default options.
5. **Windows Terminal** — the modern terminal app. Better fonts, multiple tabs, transparency, the works. From the Start menu open **Microsoft Store**, search **Windows Terminal**, click **Get**. (On Windows 11 it's often pre-installed — search for it; if it's there, skip the install.)
6. **A GitHub account** — your home on the internet for code. Sign up at <https://github.com>. You pick the username; this becomes part of your developer identity.

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

1. Go to <https://www.nerdfonts.com/font-downloads>.
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
oh-my-posh init pwsh --config "$env:LOCALAPPDATA\Programs\oh-my-posh\themes\paradox.omp.json" | Invoke-Expression
```

That's the full path to a clean theme called *paradox*. Other tutorials online use a shorter `$env:POSH_THEMES_PATH/...` form — that env var doesn't always resolve in fresh shells right after install, so the full path here is bulletproof.

Open a fresh Windows Terminal tab. Your prompt is now coloured, shows the folder, and — once you're inside a git repo — the branch name plus a `±` mark when you have uncommitted changes. Try a few themes later via `Get-PoshThemes`; if you find one you prefer, swap `paradox.omp.json` in your `$PROFILE` for `<that-theme-name>.omp.json`.

> **If something looks wrong** — boxes instead of icons mean the font isn't actually being used (recheck the Settings step); a slow first prompt is normal (oh-my-posh warms up on first call); a red error from `notepad $PROFILE` usually means PowerShell's execution policy blocks scripts (run `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` once, type `Y`, and you're fine); if the prompt shows `CONFIG_NOT_FOUND`, the theme path didn't resolve — check the `$PROFILE` line spells the path exactly as above.

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

You're reading this on GitHub right now. From now on, you'll read lessons from your own machine — faster, available offline, and the same files include the starter kit you'll need in the next module. In Windows Terminal:

```powershell
cd C:\code
git clone https://github.com/dingdonglars/kingdom-course
```

You now have `C:\code\kingdom-course\` — every lesson, the glossary, and the starter kit, all on disk.

### Make YOUR repo on GitHub

This is the repo for *your* year of work — separate from the course repo. Go to <https://github.com/new>.

1. **Name your repo `kingdom`.** *(Lessons refer to `kingdom`; pick differently and just substitute.)*
2. **Set it Public.**
3. **Don't tick "Add a README file"** — the day-1 kit (in the next module) brings its own.
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

## Part 3 — Join Slack (Lars sits with you; about 25 minutes)

Slack is where everything async happens this year — wins, help requests, milestone PR links. Doing this part with Lars on the spot means the invite link is ready to paste, the mobile install gets sanity-checked, and the notifications get tuned together. From the moment this part finishes, when you're stuck, `#help` is where you go.

### Accept the invite

Lars sends you a link to a workspace called **`kingdom-hq`**. Open it.

- Sign up with your **personal email** (not a school email — schools sometimes block external sign-ins).
- **Display name:** something simple and lowercase, like your first name. Lars sees this; it's a private workspace.
- Profile picture if you feel like it. A photo or an avatar — anything that makes it obvious it's you.

### Install Slack on your laptop

The browser version works, but the desktop app is faster and gets notifications properly. Download from <https://slack.com/downloads>. Sign in with the same email. Pick the `kingdom-hq` workspace.

### Install Slack on your phone

This one matters more than you'd think. Lars works async — he replies in `#help` *eventually*, not instantly. If Slack only lives on your laptop, you'll miss replies that come in while you're away from it. App Store / Play Store → search Slack → install → sign in.

### Tune notifications

Default Slack pings on every message. For two-person Slack that's about right, but a couple of channel-level adjustments help:

- `#wins` → notifications **on**. You'll want to know when Lars posts a win for *you* (it happens).
- `#help` → notifications **on**. When Lars replies, you want to see it fast.
- `#milestones` → notifications **on**. Same reason.
- `#all-kingdom-hq` → notifications **on mention only**. Scheduling chatter doesn't need a buzz.

On the phone: tap the workspace name → *Notifications* → tap each channel.

### The four channels

| Channel | What it's for |
|---|---|
| `#all-kingdom-hq` | Scheduling, plans, anything that doesn't fit elsewhere. **MENTOR-PROTOCOL** is pinned in the channel — one click whenever you want to re-read the rules. |
| `#wins` | Things you shipped. One post per real win. No reply expected. |
| `#help` | Stuck? Ask here. The channel topic is *"Show what you tried."* The 20-minute rule applies. |
| `#milestones` | The big seven moments — M0 through M6 — and PR links |

You won't post anything in `#wins` today — that comes at the end of the next coding module, once Roast-O-Matic exists and there's something real to celebrate. For now: Lars walks you to the pinned **MENTOR-PROTOCOL** message in `#all-kingdom-hq` so you know where it is.

---

## What you just did

You installed six tools (VS Code, Git, .NET 10, Node.js, Windows Terminal, a GitHub account), added the C# Dev Kit extension, set up a styled terminal prompt, told git who you are, picked a clean folder for the year's work, cloned the course onto your machine, created your own `kingdom` repo on GitHub, cloned that repo into VS Code, and joined the `kingdom-hq` Slack workspace on both laptop and phone. None of those steps are *coding* — they're the **workshop floor**. Every working developer started with the same set of moves; you covered them in one afternoon with Lars next to you.

**Key concepts you can now name:**

- *repo* — a folder of code that git tracks, with a copy on GitHub
- *clone* — copy a remote repo onto your computer
- *PATH* — the list of folders Windows searches when you type a command in the terminal
- *Source Control panel* — the buttons in VS Code that drive git (you'll first use these in 0.0.8)
- *GitHub account* — your home on the internet for code
- the four Slack channels — `#all-kingdom-hq`, `#wins`, `#help`, `#milestones`

## Wrap up

No quiz today — the proof of work is on the screen. Your terminal shows three version numbers; `C:\code\` has two folders inside (`kingdom-course` and `kingdom`); VS Code is open on the empty kingdom folder; `kingdom-hq` is open in your Slack desktop app and on your phone.

## Next

Two short lessons before you write any code. **Module 0.0.5 — Primer** is a self-study chapter on what's actually on your computer: folders, files, what a terminal really does, what *running* a program means, and how to read a path. Then **Module 0.0.8 — Roast-O-Matic** is your first solo session — first program, first commit, first push, first `#wins` post.

See you in 0.0.5.
