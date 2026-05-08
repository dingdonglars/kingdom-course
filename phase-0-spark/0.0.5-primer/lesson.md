# Module 0.0.5 — Primer: what's actually on your computer

Tools are installed and the two folders are on your disk. Before you write any code, here's a half-hour walk through the parts of the computer that the rest of the course will assume you understand. Folders and files. The terminal. What "running a command" actually means. How to read a path. None of this is programming yet — it's the floor under the programming. Get this clear and the next module (and every module after) is much less mysterious.

You'll have File Explorer and Windows Terminal open in two windows for most of this. Hands-on; nothing to install.

> **Words to watch**
>
> - **filesystem** — the tree of folders and files on your computer
> - **drive** — a top-level container in the filesystem; `C:\` is the drive Windows lives on
> - **path** — the full address of a file or folder, like `C:\code\kingdom\Program.cs`
> - **extension** — the part after the dot in a filename (`.md`, `.cs`, `.json`); tells programs what kind of content it is
> - **terminal** — a window where you type commands and the computer types back
> - **shell** — the program *inside* the terminal that interprets your commands; PowerShell is the default Windows shell
> - **process** — a program that's currently *alive* (running) on your computer
> - **working directory** — the folder a terminal is "currently sitting in"; commands you type usually act on this folder
> - **.NET SDK** — the toolkit you installed on Day 1 for compiling and running C# programs; `dotnet` is its headline command
> - **compiler** — a program that turns source code (your `.cs` files) into something the computer can actually execute

---

## Step 1 — Folders and files

Open **File Explorer** (Windows key + E). Click *This PC* in the left sidebar. You see drives — usually at least `C:` (where Windows lives) and maybe `D:`, `E:`, etc. Inside each drive is a tree of *folders*; inside folders are more folders or *files*. That's the whole filesystem.

Click into `C:\` (just `Local Disk (C:)` in the sidebar). Look around. You'll see folders like `Windows`, `Program Files`, `Users`. Don't poke at these — Windows lives in some of them and gets cranky if you delete things. They're just here so you know they exist.

Now click into `C:\code\` (the folder you made on Day 1). Inside, you should see exactly two folders: `kingdom-course/` (the course you cloned) and `kingdom/` (your own repo, currently empty).

Open `kingdom-course/`. Look around. *Folders inside folders inside folders.* That's the shape. Every file lives at the bottom of some folder tree; every folder has a parent (except the drive itself); navigating means going *up* (back toward the drive) or *down* (deeper into a subfolder).

Mental model: think of the filesystem as a tree turned on its head — the drive is the root at the top, branches are folders, leaves are files. Or, if you'd rather: think of folders like Roblox places — a place can contain other places and items; you walk into one to see what's inside.

> **The folder you're in matters.** Most things in this course act on *the folder you're currently in*. Run a command in the wrong folder, you get a confusing error. Whenever something doesn't work, your first check is *"am I in the right folder?"*

### Two folders, two roles

Your `C:\code\` has exactly two folders right now, and they're not the same kind of thing.

- `kingdom-course/` — **the course textbook**. Lessons, starter kit, glossary. You read from it; you never edit it. When the course updates, you pull the changes down with `git pull` from inside the folder. Think of it as a library book.
- `kingdom/` — **your repo**. Everything you write goes here. You commit, push, and one day a real PR review happens here. This is the folder with your name on it.

**The rule:** never edit anything inside `kingdom-course/`. Not the lessons, not the starter files, not a typo you spot. If something's wrong, post in `#help` — Lars fixes it in the source and you `git pull` the fix. Editing your local copy makes your next `git pull` fail with merge conflicts; small problem becomes a big confusing one fast.

**Working with both in VS Code.** Open each as its own window: *File → Open Folder* → `C:\code\kingdom`, then in a fresh VS Code window, *File → Open Folder* → `C:\code\kingdom-course`. Two VS Code icons in the taskbar, one per repo, title bar shows which is which. Don't merge them into one workspace — keeping them as two windows means you're physically less likely to type into the wrong one.

## Step 2 — File extensions: how programs know what you've got

Every file has a name and (usually) an *extension* — the bit after the dot. The extension tells programs what kind of content the file holds. It's not magic; it's a convention everyone agrees to.

In `C:\code\kingdom-course\`, you'll see files with different extensions:

| Extension | What it means | Example |
|---|---|---|
| `.md` | Markdown — formatted plain text. Lessons are written in this. | `README.md` |
| `.cs` | C# source code. Your programs will live in these. | `Program.cs` |
| `.csproj` | C# project file — tells the compiler how to build the code. | `RoastOMatic.csproj` |
| `.json` | Structured data, used for config and saved games later. | `appsettings.json` |
| `.gitignore` | A file git reads to know which files to ignore. | `.gitignore` |

Two things worth knowing:

1. **The extension is part of the filename**, even though Windows often hides it. If you see `Program` in File Explorer, the actual filename on disk is probably `Program.cs`. To make Windows always show extensions: in File Explorer, click *View* → *Show* → *File name extensions*. Tick it. **Do that now.** Hidden extensions cause confusing bugs (you save `Program.txt` thinking you saved `Program.cs`).
2. **Renaming the extension doesn't change the contents.** Renaming `Program.cs` to `Program.txt` doesn't make it text — it's still C# code; you've just lied to whoever opens it. Tools that act based on extension will now treat it wrong.

## Step 3 — The terminal, and what PowerShell actually is

Open **Windows Terminal**. You see a prompt — `PS C:\Users\YourName>` or similar — followed by a blinking cursor waiting for you to type. The thing you're looking at is *the terminal* — a window where text goes in and text comes out.

But what's actually *processing* what you type? That's the **shell**. The shell is a program that runs *inside* the terminal, reads what you type, and turns it into actions. On Windows the default shell is **PowerShell**. On Mac and Linux it's typically **bash** or **zsh**. Different shells, same idea: type a command, it runs, you see output.

Think of it like a chat window for your computer. You type a question or instruction, the computer types back. Difference from a regular chat: the computer doesn't get personality, it gets very specific. Type the right command, you get the answer. Type a wrong one, you get an error message — usually one that tells you exactly what's missing.

Try this. In Windows Terminal, type:

```powershell
cd C:\code
```

Hit Enter. Your prompt changes — now it shows you're in `C:\code`. The command `cd` stands for *"change directory"* and it moved your *working directory* to `C:\code`. *Working directory* is just *"the folder this terminal is currently sitting in."*

Now type:

```powershell
ls
```

Hit Enter. You see a listing of what's in `C:\code\` — the same two folders File Explorer showed you in Step 1: `kingdom-course/` and `kingdom/`. `ls` is short for *"list"*. The terminal can see exactly what File Explorer can see; it just shows it as text instead of icons.

Type `cd kingdom-course` then `ls` again. You see the inside of the course repo — same files you saw in File Explorer. Two windows on the same data.

> **Why two ways to look at the same thing?** File Explorer is great for browsing and dragging things around. The terminal is great for *automation* — running tools, building code, doing the same thing on a hundred files at once. Most of programming is done in the terminal because most programming tools are command-line tools.

## Step 4 — What "running a command" actually means

This is the most important section in the primer.

When you type `cd C:\code` and hit Enter, PowerShell runs that command and finishes — instantly, basically. Your prompt comes back, ready for the next command. *That command is done.*

But some commands take time. Or never finish on their own. When you eventually type `dotnet run`, that command will:

1. Compile your C# code into a program.
2. Start that program.
3. Wait for the program to finish (which might be milliseconds, or might be forever if the program loops).

While `dotnet run` is *running*, the terminal looks busy — it's showing the program's output, but it isn't accepting new commands. The prompt is gone or frozen. **The program is alive, inside PowerShell, inside the terminal window.**

```
Windows Terminal  (the window you can see)
  └── PowerShell  (the shell inside the window)
       └── your program  (the process running inside PowerShell)
```

Three layers. The window contains the shell; the shell contains the running program. **If you close the window, you kill PowerShell, and PowerShell kills the program with it.** That's how "running" works on a computer — running things live inside other running things, and the chain has to stay alive.

This is why, on Day 1, the rule was *don't close PowerShell while a command is running*. Closing the window mid-command is like ripping the power cord out of a microwave mid-cycle: whatever was happening stops, half-cooked. Sometimes that's fine. Sometimes it leaves a mess.

**The right way to stop a running program:**

- If the program finishes on its own — great, it just ends, you get your prompt back.
- If you want to stop it early, hit `Ctrl + C`. That sends a *"please stop"* signal. The program usually stops, cleanly, and gives you your prompt back.
- *Then* you can close the window if you want. The window is just the container; closing it after stopping the program is fine.

The vocabulary worth knowing: a *running program* is called a **process**. Your computer has hundreds of processes alive right now (Windows itself, Chrome, the terminal, every game and app you have open). Each one is a thing the computer is keeping alive. When a process ends, it disappears.

> **Save vs run is not the same thing.** Saving a `.cs` file writes the code to disk — that's permanent until you delete it. Running it (`dotnet run`) starts a process that's alive only while the command is alive. *Saving the code does not run it; running it does not save anything new to disk.* The two are completely separate moves. People mix them up at first; you won't, now.

## Step 5 — The .NET SDK: your C# toolkit

That `dotnet run` command from the previous step. What's `dotnet`? Where did it come from?

When you installed the **.NET SDK** on Day 1, what actually landed on your computer was a *toolkit* — a bundle of programs, libraries, and built-in rules that together turn C# code into runnable software. *SDK* stands for *Software Development Kit*; lots of platforms have one (the iOS SDK, the Android SDK, the Roblox-Studio-as-an-SDK). They all mean roughly the same thing: *"here's everything you need to build software for this platform, in one install."*

`dotnet` is the headline command that fronts the SDK. Three flavours of it you'll meet often:

- `dotnet --version` — print the installed SDK version (you used this on Day 1 to confirm install).
- `dotnet new console -n MyApp` — create a new C# console-application project called `MyApp`. This makes a folder, drops in a `Program.cs` and a `MyApp.csproj`, and you have something buildable.
- `dotnet run` — *compile* the C# code in the current project, then *run* the result. Two steps in one command.

What's *inside* the SDK that makes `dotnet run` work?

- The **compiler** — a program that turns your `.cs` source files into a `.dll` or `.exe` your computer can execute. C# is not read line-by-line at runtime; it gets compiled to a binary form first.
- The **runtime** — the engine that loads and runs the compiled output. Even after compilation, C# programs need this engine to work. The SDK includes one.
- **Built-in libraries** — pre-written code your programs can use without installing anything extra. When you call `Console.WriteLine` in the next module, that method lives in a built-in library that came with the SDK.
- The **NuGet** client — when you need a *third-party* library (not built in), `dotnet` pulls it from a public registry called NuGet and wires it in. You won't use this for a while; just know the word.

So when you run `dotnet --version` and see `10.0.x`, you're confirming **.NET 10's SDK is installed and ready to compile and run C# code**. That's what the toolbox under your hood holds.

## Step 6 — Paths: addresses for files

A **path** is the full address of a file or folder. `C:\code\kingdom\Program.cs` is a path. Read left to right, it's a tour from the drive down to the file:

- `C:` — the drive (Windows's main drive)
- `\code\` — a folder under the drive root
- `\kingdom\` — a folder inside `code`
- `\Program.cs` — the file inside `kingdom`

The `\` (backslash) is Windows's separator between folders. On Mac and Linux it's `/` (forward slash). Some Windows tools accept both. Stick with `\` and you'll never be wrong on this OS.

Two flavours of path you'll see all year:

**Absolute paths** start at a drive root: `C:\code\kingdom\Program.cs`. They work no matter where the terminal is currently sitting — every part of the address is spelled out.

**Relative paths** start from the working directory. If your terminal is in `C:\code\kingdom\`, then `Program.cs` is a relative path that means "the `Program.cs` file in this folder." Likewise `RoastOMatic\Program.cs` means "the `Program.cs` inside the `RoastOMatic` folder inside this folder."

Two shortcuts that show up everywhere:

- `.` (single dot) means *"this folder"* — the current working directory. `code .` opens VS Code on this folder. `git add .` stages every change in this folder.
- `..` (two dots) means *"the parent folder"* — one level up. `cd ..` moves the terminal up one level. From `C:\code\kingdom\`, `cd ..` lands you in `C:\code\`.

Combine them: from `C:\code\kingdom\`, the path `..\kingdom-course\STANDARDS.md` means *"go up to `code`, then into `kingdom-course`, then to `STANDARDS.md`."* Reading paths gets quick once you've stared at a few.

Try this in Windows Terminal:

```powershell
cd C:\code\kingdom-course
ls
cd ..
ls
cd kingdom
ls
```

Three `ls` outputs, three different folders, all reached by walking the path tree. You're now navigating like a developer.

## Step 7 — Keeping order: why `C:\code\`, why not `Documents`

You put your year of work under `C:\code\`. Why not under `Documents`?

Because `Documents` is special on Windows. Two reasons it bites you:

1. **OneDrive often syncs `Documents` automatically.** OneDrive is fine for Word files; it's *terrible* for code. It locks files mid-save, copies half-written commits, sometimes silently renames things. Git and OneDrive fight; git loses. You don't want that fight.
2. **It mixes domains.** `Documents` ends up holding school PDFs, screenshots, downloaded forms, a thesis your cousin wrote in 2019. Code in there gets lost. Code in `C:\code\` is *only* code. You always know where to look.

The "keeping order" habit is small but pays off all year:

- **One root folder for code.** That's `C:\code\`. Everything code-related goes inside it.
- **One folder per project, directly under the root.** `C:\code\kingdom\`, `C:\code\kingdom-course\`, plus future ones. Don't nest projects inside each other.
- **Project folders match repo names.** Your repo on GitHub is `kingdom`; the folder on disk is `kingdom`. Same name, no surprises.
- **Don't move folders around manually after git is involved.** Once a folder is a git repo, treat the path like part of its identity. If you really need to move it, `git status` should be clean first, then move.

These aren't laws — they're conventions that prevent specific kinds of confusion. Adopt them now and you skip a class of *"why isn't this working"* moments.

---

## Tinker

Open **File Explorer** and **Windows Terminal** side by side on the screen.

In File Explorer, navigate to `C:\code\kingdom-course\`. In Windows Terminal, type `cd C:\code\kingdom-course` and `ls`. Notice the same files appear in both. Same folder, two views.

In File Explorer, double-click into `phase-0-spark/`. In Windows Terminal, type `cd phase-0-spark` and `ls`. Same place again, same files.

Try going up: in File Explorer, click the *back* arrow or `phase-0-spark` in the breadcrumbs. In Windows Terminal, `cd ..`. Confirm both went up to `kingdom-course`.

Now this: in Windows Terminal, type `code .` (the dot — *this folder*). VS Code opens on `kingdom-course`. **The same `kingdom-course` you've been looking at in File Explorer and listing in PowerShell.** Three windows on the same folder, three different ways of looking at it.

That's the whole picture. Folders and files on disk; one or more programs (File Explorer, PowerShell, VS Code) showing them in different shapes; commands acting on whatever folder the program happens to be sitting in.

---

## What you just did

You walked the filesystem. You saw `C:\` and the tree of folders inside it; you understood that File Explorer and PowerShell are two views on the same underlying disk. You met file extensions and turned on *"show extensions"* in File Explorer so they stop being hidden. You learned that the terminal is a window, the shell (PowerShell on Windows) runs inside it, and the programs you launch run inside the shell — three layers, kill one and you kill what's inside it. You read absolute and relative paths, used `cd` and `..` to navigate, and adopted the conventions that keep `C:\code\` an orderly working folder for the year. Nothing here is "code" yet — but every line of code you write will live in this filesystem, run in this shell, and follow these path rules.

**Key concepts you can now name:**

- *filesystem* — the tree of folders and files; drives at the top, files at the leaves
- *path* — the full address of a file; absolute starts at the drive, relative starts at the working directory
- *terminal vs shell* — the window vs the program inside it that runs commands; PowerShell is Windows's default shell
- *process* — a program that's currently alive; closing the terminal kills processes inside it
- *.NET SDK* — the toolkit `dotnet` fronts; compiler + runtime + libraries + NuGet client, all installed Day 1

## Quiz

This is your first quiz, so a quick word on how these work — once, here. Future lessons close with the standard one-line pointer.

**Quizzes are self-checks, not tests.** Five or six multiple-choice questions, no time limit, no marks. The point is *you* checking yourself — did the lesson sink in or not? A quiz that goes badly isn't a problem; it just shows the bits worth bringing up at the next weekly sync.

**Where you write your answers.** Normally, answers go in `journal/quiz-notes.md` inside your kingdom repo. Small thing about this first time: that file isn't in your repo yet — you'll copy it in during the next module (`0.0.8`), as part of the day-1 kit. So for this one quiz: write your answers anywhere fast — a notes app, paper, your phone, doesn't matter. Once you've copied the kit in during `0.0.8`, move them into `journal/quiz-notes.md` properly. From the next quiz on, you write straight into that file.

**The format is short.** One letter for each question, plus one quick sentence about *why* you picked it. You can see how it looks at the top of the template file — open `C:\code\kingdom-course\starter-template\journal\quiz-notes.md` if you want a peek. The *why* matters more than the letter. *"I picked b because..."* is the bit you'll re-read months later and actually get something out of. The letter alone — useless after a week.

**Don't write answers in `quiz.md` itself.** Keep that file clean. You might want to come back in a month or two and try the quiz again — a quiz with the answers already on it isn't really a quiz any more.

**If you're stuck on a question.** Try for twenty minutes. Re-read the part of the lesson the question is about. If it still won't come together, post in Slack `#help` (it's been live since Module 0.0). The 20-minute rule from `MENTOR-PROTOCOL.md` is for exactly this — there's a difference between *trying-and-getting-somewhere* and *staring-at-it-going-in-circles*.

**Then bring whichever question you were least sure about to the next weekly sync.** That's the rhythm: read the lesson, do the quiz, write the answers, talk through the shaky bits at the sync. Every module from here on follows this.

Open `quiz.md` now.

## Next

You now know what's underneath the lessons. **Module 0.0.8 — Roast-O-Matic** is your first solo session — you'll write a real program, commit it, push it, and post your first win. See you there.
