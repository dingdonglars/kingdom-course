# Module 0.0 — Setup

By the end of today your computer is ready, your code folder is set up, and you're in Slack. You will install an editor, a compiler, a terminal, and git. You will make a GitHub account. You will create the two folders where every line of code this year will live. And you will join the chat workspace where Lars and you talk through the week. You do all of this with Lars next to you. From the next module on, you work on your own. This takes about two hours, often less.

## What you need first

- A Windows PC (you have this).
- **Your phone**, charged. You'll install Slack on it during Part 3.
- Lars sitting next to you. A few install screens are easier with someone who has done this before. The GitHub account, the Slack invite, and the phone install all go smoother with Lars in the room.
- About two hours with no interruptions.

> **After today — read the mentor protocol.** `MENTOR-PROTOCOL.md` is the agreement on how Lars and you run the year: the 20-minute rule, the four channels, the weekly sync, and how you can stop if you ever want to. You can find it in two places. It is on your computer at `C:\code\kingdom-course\MENTOR-PROTOCOL.md` (you copy it down in Part 2). It is also pinned in `#all-kingdom-hq` once you join Slack in Part 3. Read it during the first week and bring it to the first weekly sync.

---

## Part 1 — Tools (Lars walks you through this; about 50 minutes)

You install six tools, add one VS Code extension, and tell git who you are. Lars sits with you. Each install is mostly clicking *next, next, next*. None of them are hard.

Install **VS Code first**. That way the Git installer can set VS Code as your default editor instead of Vim. Vim is an old, tricky editor, and you do not want to meet it today.

1. **VS Code** — the editor you write code in for the next year. Download from <https://code.visualstudio.com/>. Default options. Make sure *"Add to PATH"* is ticked (it is by default).
2. **Git for Windows** — the tool every developer in the world uses to save their work. Download from <https://git-scm.com/download/win>. Use the default options **except one screen**: when the installer asks *"Choosing the default editor used by Git"*, pick **Use Visual Studio Code as Git's default editor**. It is in the dropdown list. VS Code is already installed from step 1, so it shows up there. Vim is the default the installer would pick on its own. You don't need it today.
3. **.NET SDK 10** — the toolkit for building C# programs. Download from <https://dotnet.microsoft.com/download/dotnet/10.0>. Default options.
4. **Node.js LTS** — you'll need this later, in Phase 4, for the browser version. Download from <https://nodejs.org/>. Pick the LTS version and use the default options.
5. **Windows Terminal** — a modern terminal app. It has better fonts, several tabs at once, and more. From the Start menu open **Microsoft Store**, search **Windows Terminal**, and click **Get**. On Windows 11 it is often already installed. Search for it first; if it is there, skip the install.
6. **A GitHub account** — your home on the internet for code. Sign up at <https://github.com>. You pick the username. It becomes part of who you are as a developer, so choose one you like.

### Verify the installs

Open **Windows Terminal** (search for it in the Start menu — pin it to the taskbar while you're there; you'll open it five times a day). It defaults to PowerShell. Type:

```powershell
git --version
dotnet --version
node --version
```

You should see three version numbers. If any of them say "not found", tell Lars.

### Add the C# Dev Kit extension to VS Code

An extension is a small add-on that gives VS Code extra powers. Without this one, your C# files (`.cs` files) have no code suggestions, no debugger, and no colours.

1. Open VS Code.
2. Click the **Extensions** icon in the left sidebar (four squares, fourth icon down). Shortcut: `Ctrl + Shift + X`.
3. Search **C# Dev Kit** (publisher: Microsoft).
4. Click **Install**. It brings the C# language server along with it. This takes about a minute.

When you open a `.cs` file later, you'll see colours, code suggestions, and a small *Run | Debug* link above `Main`. That all comes from this extension.

> **One habit worth learning now.** There's a single rule that makes running and debugging painless every time: open each program in *its own window* — one program, one window. It's one short page. Read `running-your-project.md` once now, and come back to it the first time *Run* or the debugger ever acts up.

### Tell git who you are

Every commit gets stamped with a name and an email. Set them once, for your whole computer, to match your GitHub account:

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

> **What git really is, in a few sentences.** Git is a tool that takes *snapshots* of your code. Every time you commit, it saves the whole state of the folder under a label, so you can come back to it later. The snapshots live in two places: on your computer (the *local* copy) and on GitHub (the *remote* copy). Pushing copies new snapshots up to GitHub. You can pull them back down onto any other computer. A *commit* is one labelled snapshot with a short message saying what changed. A *push* sends those commits to the internet. You'll do dozens of these in the first week. They feel strange before they feel normal, and that is fine. The big idea is this: your code is never just *the file as it is now*. It is the whole history of how it got there, and git remembers all of it.

### A home for your code

Pick one folder on your computer where all your course work will live. Don't put it inside `Documents`. Windows often copies that folder to OneDrive, and OneDrive gets in the way of git. The simplest, cleanest choice is a folder at the top of your `C:` drive. Open Windows Terminal and type:

```powershell
mkdir C:\code
```

From now on, anything related to this course goes inside `C:\code\`.

### Get the course onto your computer

You're reading this on GitHub right now. From now on, you'll read lessons from your own computer. That is faster, it works without internet, and the same download includes the starter kit you'll need in the next module. In Windows Terminal:

```powershell
cd C:\code
git clone https://github.com/dingdonglars/kingdom-course
```

You now have `C:\code\kingdom-course\` on your computer: every lesson, the glossary, and the starter kit.

### Make YOUR repo on GitHub

This is the repo for *your* year of work. It is separate from the course repo. Go to <https://github.com/new>.

1. **Name your repo `kingdom`.** *(The lessons all say `kingdom`. If you pick a different name, just use your name wherever a lesson says `kingdom`.)*
2. **Set it Public.**
3. **Don't tick "Add a README file".** The day-1 kit, in the next module, brings its own.
4. Click **Create repository**.

Copy the repo URL from the green **Code** button.

### Add Lars as a collaborator

Later in the year, Lars will review your pull requests. To let him do that, give him access to the repo right now while you're still on github.com. On your repo's page, click **Settings** (top-right of the repo, in the row of tabs), then **Collaborators** (left sidebar), then **Add people**. Type `dingdonglars`, pick him from the dropdown, choose **Read** access, and click **Add to repository**.

Lars gets an email invite and accepts it on his side. After that he can review your pull requests and give them the official *Approve* badge. Read access lets him look and review only. He can't push or merge anything into your repo.

### Clone YOUR repo into `C:\code\`

In VS Code:

1. Open VS Code.
2. `Ctrl + Shift + P` to open the Command Palette → type *"Git: Clone"* → Enter.
3. Paste the repo URL.
4. **When the file picker opens, go to `C:\code\` and click _Select Repository Location_.** The picker often starts in your home folder or `Documents`. Make sure the path bar shows `C:\code\` before you click. Otherwise the repo ends up in the wrong place.
5. When VS Code asks *"Would you like to open the cloned repository?"*, click **Open**.

You now have `C:\code\kingdom\` on your PC, connected to GitHub, open in VS Code.

> **From tomorrow on**, there are two ways to come back here. In VS Code: *File → Open Recent → kingdom*. Or in Windows Terminal: type `cd C:\code\kingdom`, then `code .` — the dot means *"this folder."* Either way, VS Code opens on the kingdom folder.

> **Or in the terminal.** You can also clone from PowerShell instead. This is useful when you're away from VS Code, or following a tutorial that uses the terminal:
>
> ```powershell
> cd C:\code
> git clone <the-url-you-just-copied>
> cd kingdom
> ```
>
> Both ways do the same thing. Through the year you'll mostly use VS Code's git buttons. The terminal is still there for the times when it's faster.

---

## Part 3 — Join Slack (Lars sits with you; about 25 minutes)

Slack is where Lars and you talk through the year when you're not in the same room: your wins, your help requests, and your milestone links. Doing this part with Lars right here means the invite link is ready to paste, the phone install gets checked, and you set the notifications together. From the moment this part finishes, `#help` is where you go when you're stuck.

### Accept the invite

Lars sends you a link to a workspace called **`kingdom-hq`**. Open it.

- Sign up with your **personal email**, not a school email. Schools sometimes block sign-ins from outside.
- **Display name:** something simple and lowercase, like your first name. Lars sees this. It's a private workspace.
- Add a profile picture if you feel like it. A photo or an avatar is fine — anything that makes it clear it's you.

### Install Slack on your laptop

The browser version works, but the desktop app is faster and handles notifications properly. Download from <https://slack.com/downloads>. Sign in with the same email. Pick the `kingdom-hq` workspace.

### Install Slack on your phone

This matters more than you'd think. Lars does not reply right away. He answers in `#help` when he can, not the second you ask. If Slack only lives on your laptop, you'll miss replies that come in while you're away from it. App Store or Play Store, search Slack, install, sign in.

### Set up notifications

Out of the box, Slack pings you on every message. For a two-person Slack that's about right, but a few changes per channel help:

- `#wins` → notifications **on**. You'll want to know when Lars posts a win for *you* (it happens).
- `#help` → notifications **on**. When Lars replies, you want to see it fast.
- `#milestones` → notifications **on**. Same reason.
- `#all-kingdom-hq` → notifications **on mention only**. Scheduling messages don't need a buzz.

On the phone: tap the workspace name, then *Notifications*, then tap each channel.

### The four channels

| Channel | What it's for |
|---|---|
| `#all-kingdom-hq` | Scheduling, plans, and anything that doesn't fit elsewhere. **MENTOR-PROTOCOL** is pinned here — one click whenever you want to read the rules again. |
| `#wins` | Things you finished and shipped. One post per real win. No reply expected. |
| `#help` | Stuck? Ask here. The channel topic is *"Show what you tried."* The 20-minute rule applies. |
| `#milestones` | The big seven moments — M0 through M6 — and your milestone links. |

You won't post anything in `#wins` today. That comes at the end of the next coding module, once Roast-O-Matic exists and there's something real to celebrate. For now, Lars walks you to the pinned **MENTOR-PROTOCOL** message in `#all-kingdom-hq` so you know where it is.

---

## What you just did

You installed six tools (VS Code, Git, .NET 10, Node.js, Windows Terminal, and a GitHub account). You added the C# Dev Kit extension. You told git who you are. You picked a clean folder for the year's work. You copied the course onto your computer. You created your own `kingdom` repo on GitHub and cloned it into VS Code. And you joined the `kingdom-hq` Slack workspace on both laptop and phone. None of those steps were *coding*. They set up your workshop. Every working developer started by doing the same kinds of things, and you got through them in one afternoon with Lars next to you.

**Key concepts you can now name:**

- *repo* — a folder of code that git tracks, with a copy on GitHub
- *clone* — copy a remote repo onto your computer
- *PATH* — the list of folders Windows looks in when you type a command in the terminal
- *Source Control panel* — the buttons in VS Code that run git for you (you'll first use these in 0.0.8)
- *GitHub account* — your home on the internet for code
- the four Slack channels — `#all-kingdom-hq`, `#wins`, `#help`, `#milestones`

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that you can find your way back to your work tomorrow. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

From memory, get back to your work two ways:

1. Close VS Code completely, then open it again and return to your `kingdom` folder.
2. Now do the same from Windows Terminal: move into the folder, then open it in VS Code from there.

Both ways should land you on the same `C:\code\kingdom` folder.

<details><summary>Stuck? Open this to check yourself.</summary>

- In VS Code: *File → Open Recent → kingdom*. VS Code opens on the `kingdom` folder.
- In Windows Terminal:
  ```powershell
  cd C:\code\kingdom
  code .
  ```
  The `.` means "this folder", so VS Code opens on `kingdom`.
- Either way, the VS Code title bar should show `kingdom`, and you should see your files in the left sidebar.

</details>

## Wrap up

No quiz today — the proof of work is on the screen. Your terminal shows three version numbers; `C:\code\` has two folders inside (`kingdom-course` and `kingdom`); VS Code is open on the empty kingdom folder; `kingdom-hq` is open in your Slack desktop app and on your phone.

## Next

Two short lessons before you write any code. **Module 0.0.5 — Primer** is a read-on-your-own chapter about what's actually on your computer: folders, files, what a terminal really does, what *running* a program means, and how to read a path. Then **Module 0.0.8 — Roast-O-Matic** is your first session on your own: first program, first commit, first push, first `#wins` post.

See you in 0.0.5.
