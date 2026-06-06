# Module 0.0.5 — Primer: what's actually on your computer

The tools are installed and the two folders are on your computer. Before you write any code, here's a half-hour walk through the parts of the computer that the rest of the course expects you to understand. Folders and files. The terminal. What "running a command" really means. How to read a path. None of this is programming yet. It's what the programming sits on top of. Get this clear and the next module, and every module after, is much easier to follow.

For most of this you'll have File Explorer and Windows Terminal open in two windows. You do it by hand; there's nothing to install.

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

Open **File Explorer** (Windows key + E). Click *This PC* in the left sidebar. You see drives — usually at least `C:` (where Windows lives), and maybe `D:`, `E:`, and so on. Inside each drive is a tree of *folders*. Inside folders are more folders, or *files*. That is the whole filesystem.

Click into `C:\` (it's called `Local Disk (C:)` in the sidebar). Look around. You'll see folders like `Windows`, `Program Files`, and `Users`. Don't change these. Windows lives in some of them and breaks if you delete things. They're just here so you know they exist.

Now click into `C:\code\` (the folder you made on Day 1). Inside, you should see exactly two folders: `kingdom-course/` (the course you cloned) and `kingdom/` (your own repo, empty for now).

Open `kingdom-course/`. Look around. *Folders inside folders inside folders.* That's how it's laid out. Every file lives at the bottom of some folder tree. Every folder has a parent, except the drive itself. Moving around means going *up* (back toward the drive) or *down* (deeper into a folder).

Here's a way to picture it. Think of the filesystem as a tree turned upside down: the drive is the root at the top, the branches are folders, and the leaves are files. Or, if you prefer, think of folders like Roblox places. A place can hold other places and items, and you walk into one to see what's inside.

> **The folder you're in matters.** Most things in this course act on *the folder you're currently in*. Run a command in the wrong folder and you get a confusing error. Whenever something doesn't work, the first thing to check is *"am I in the right folder?"*

### Two folders, two roles

Your `C:\code\` has exactly two folders right now, and they do different jobs.

- `kingdom-course/` — **the course textbook**. Lessons, starter kit, glossary. You read from it; you never edit it. When the course gets an update, you pull the changes down with `git pull` from inside the folder. Think of it as a library book.
- `kingdom/` — **your repo**. Everything you write goes here. You commit, push, and one day a real pull-request review happens here. This is the folder with your name on it.

**The rule:** never edit anything inside `kingdom-course/`. Not the lessons, not the starter files, not even a typo you spot. If something's wrong, post in `#help`. Lars fixes it in the original and you `git pull` the fix. If you edit your own copy, your next `git pull` fails with a merge conflict. A small problem turns into a big, confusing one fast.

**Working with both in VS Code.** Open each one in its own window. Use *File → Open Folder* → `C:\code\kingdom`. Then, in a new VS Code window, use *File → Open Folder* → `C:\code\kingdom-course`. Now you have two VS Code icons in the taskbar, one per repo, and the title bar tells you which is which. Don't join them into one workspace. Keeping them as two windows makes it much harder to type into the wrong one by accident.

## Step 2 — File extensions: how programs know what you've got

Every file has a name and, usually, an *extension* — the part after the dot. The extension tells programs what kind of content the file holds. It's not magic. It's just a habit everyone agrees to follow.

In `C:\code\kingdom-course\`, you'll see files with different extensions:

| Extension | What it means | Example |
|---|---|---|
| `.md` | Markdown — formatted plain text. Lessons are written in this. | `README.md` |
| `.cs` | C# source code. Your programs will live in these. | `Program.cs` |
| `.csproj` | C# project file — tells the compiler how to build the code. | `RoastOMatic.csproj` |
| `.json` | Structured data, used for config and saved games later. | `appsettings.json` |
| `.gitignore` | A file git reads to know which files to ignore. | `.gitignore` |

Two things worth knowing:

1. **The extension is part of the filename**, even though Windows often hides it. If you see `Program` in File Explorer, the real filename on disk is probably `Program.cs`. To make Windows always show extensions: in File Explorer, click *View* → *Show* → *File name extensions*, and tick it. **Do that now.** Hidden extensions cause confusing bugs. For example, you save `Program.txt` while thinking you saved `Program.cs`.
2. **Renaming the extension doesn't change what's inside.** If you rename `Program.cs` to `Program.txt`, it doesn't turn into text. It's still C# code; the name now says something untrue about it. Any tool that decides what to do based on the extension will now handle it the wrong way.

## Step 3 — The terminal, and what PowerShell actually is

Open **Windows Terminal**. You see a prompt — `PS C:\Users\YourName>` or something like it — and then a blinking cursor, waiting for you to type. What you're looking at is *the terminal*: a window where text goes in and text comes out.

But what actually *reads and runs* what you type? That's the **shell**. The shell is a program that runs *inside* the terminal. It reads what you type and turns it into actions. On Windows the default shell is **PowerShell**. On Mac and Linux it's usually **bash** or **zsh**. Different shells, same idea: you type a command, it runs, you see the output.

Think of it like a chat window for your computer. You type a question or an instruction, and the computer types back. The difference from a normal chat: the computer has no personality, and it needs you to be exact. Type the right command and you get the answer. Type a wrong one and you get an error message — usually one that tells you exactly what's missing.

Try this. In Windows Terminal, type:

```powershell
cd C:\code
```

Press Enter. Your prompt changes — it now shows you're in `C:\code`. The command `cd` stands for *"change directory"*, and it moved your *working directory* to `C:\code`. *Working directory* just means *"the folder this terminal is currently in."*

Now type:

```powershell
ls
```

Press Enter. You see a list of what's in `C:\code\` — the same two folders File Explorer showed you in Step 1: `kingdom-course/` and `kingdom/`. `ls` is short for *"list"*. The terminal can see exactly what File Explorer can see. It just shows it as text instead of icons.

Type `cd kingdom-course`, then `ls` again. You see what's inside the course repo — the same files you saw in File Explorer. Two windows looking at the same thing.

> **Why two ways to look at the same thing?** File Explorer is great for browsing and dragging things around. The terminal is great for getting the computer to do work for you: running tools, building code, doing the same thing to a hundred files at once. Most programming is done in the terminal, because most programming tools are run by typing commands.

## Step 4 — What "running a command" actually means

This is the most important section in the primer.

When you type `cd C:\code` and press Enter, PowerShell runs that command and finishes almost instantly. Your prompt comes back, ready for the next command. *That command is done.*

But some commands take time. Some never finish on their own. When you later type `dotnet run`, that command will:

1. Turn your C# code into a program. (Turning code into a program is called *compiling*.)
2. Start that program.
3. Wait for the program to finish. That might take a few milliseconds, or it might never end if the program loops forever.

While `dotnet run` is *running*, the terminal looks busy. It's showing the program's output, but it won't take new commands. The prompt is gone or frozen. **The program is running, inside PowerShell, inside the terminal window.**

```
Windows Terminal  (the window you can see)
  └── PowerShell  (the shell inside the window)
       └── your program  (the process running inside PowerShell)
```

Three layers. The window holds the shell; the shell holds the running program. **If you close the window, you stop PowerShell, and that stops the program with it.** That's how "running" works on a computer: running things live inside other running things, and the whole chain has to stay open.

This is why, on Day 1, the rule was *don't close PowerShell while a command is running*. Closing the window in the middle of a command is like pulling the power cord out of a microwave halfway through: whatever was happening just stops, half done. Sometimes that's fine. Sometimes it leaves a mess.

**The right way to stop a running program:**

- If the program finishes on its own, great. It just ends, and you get your prompt back.
- If you want to stop it early, press `Ctrl + C`. That sends a *"please stop"* signal. The program usually stops cleanly and gives you your prompt back.
- *Then* you can close the window if you want. The window is just the container. Closing it after the program has stopped is fine.

One word worth knowing: a *running program* is called a **process**. Your computer has hundreds of processes running right now — Windows itself, Chrome, the terminal, every game and app you have open. Each one is something the computer is keeping going. When a process ends, it disappears.

> **Saving and running are not the same thing.** Saving a `.cs` file writes the code to disk, and it stays there until you delete it. Running it (`dotnet run`) starts a process that exists only while the command is running. *Saving the code does not run it. Running it does not save anything new to disk.* They are two completely separate actions. People mix them up at first; you won't now.

## Step 5 — The .NET SDK: your C# toolkit

Let's look at that `dotnet run` command from the last step. What is `dotnet`? Where does it come from?

When you installed the **.NET SDK** on Day 1, what you really added to your computer was a *toolkit*: a set of programs, libraries, and built-in rules that together turn C# code into a working program. *SDK* stands for *Software Development Kit*. Lots of platforms have one — the iOS SDK, the Android SDK, Roblox Studio as a kind of SDK. They all mean roughly the same thing: *"here is everything you need to build software for this platform, in one install."*

`dotnet` is the main command for the SDK. Three forms of it you'll use often:

- `dotnet --version` — print the installed SDK version (you used this on Day 1 to check the install).
- `dotnet new console -n MyApp` — create a new C# console-application project called `MyApp`. This makes a folder, puts a `Program.cs` and a `MyApp.csproj` inside it, and now you have something you can build.
- `dotnet run` — *compile* the C# code in the current project, then *run* the result. Two steps in one command.

What's *inside* the SDK that makes `dotnet run` work?

- The **compiler** — a program that turns your `.cs` source files into a `.dll` or `.exe` that your computer can run. C# is not read line by line as it runs; it gets compiled into a form the computer understands first.
- The **runtime** — the engine that loads and runs the compiled result. Even after compiling, C# programs need this engine to work. The SDK includes one.
- **Built-in libraries** — code that comes ready-made, so your programs can use it without installing anything extra. When you call `Console.WriteLine` in the next module, that method lives in a built-in library that came with the SDK.
- The **NuGet** tool — when you need a library that someone else wrote and that isn't built in, `dotnet` downloads it from a public store called NuGet and adds it to your project. You won't use this for a while; just know the word.

So when you run `dotnet --version` and see `10.0.x`, you're confirming that **.NET 10's SDK is installed and ready to compile and run C# code**. That's what's inside the toolkit.

## Step 6 — Paths: addresses for files

A **path** is the full address of a file or folder. `C:\code\kingdom\Program.cs` is a path. Read left to right, it's a tour from the drive down to the file:

- `C:` — the drive (Windows's main drive)
- `\code\` — a folder under the drive root
- `\kingdom\` — a folder inside `code`
- `\Program.cs` — the file inside `kingdom`

The `\` (backslash) is what Windows uses to separate folders. On Mac and Linux it's `/` (forward slash). Some Windows tools accept both. Stick with `\` and you'll always be right on Windows.

Two kinds of path you'll see all year:

**Absolute paths** start at a drive root, like `C:\code\kingdom\Program.cs`. They work no matter which folder the terminal is in, because every part of the address is written out.

**Relative paths** start from the working directory. If your terminal is in `C:\code\kingdom\`, then `Program.cs` is a relative path meaning "the `Program.cs` file in this folder." In the same way, `RoastOMatic\Program.cs` means "the `Program.cs` inside the `RoastOMatic` folder inside this folder."

Two shortcuts that show up everywhere:

- `.` (single dot) means *"this folder"* — the current working directory. `code .` opens VS Code on this folder. `git add .` stages every change in this folder.
- `..` (two dots) means *"the parent folder"* — one level up. `cd ..` moves the terminal up one level. From `C:\code\kingdom\`, `cd ..` takes you to `C:\code\`.

Put them together: from `C:\code\kingdom\`, the path `..\kingdom-course\STANDARDS.md` means *"go up to `code`, then into `kingdom-course`, then to `STANDARDS.md`."* Reading paths gets quick once you've read a few.

Try this in Windows Terminal:

```powershell
cd C:\code\kingdom-course
ls
cd ..
ls
cd kingdom
ls
```

Three `ls` outputs, three different folders, all reached by walking through the path tree. You're now finding your way around the folders the way a developer does.

## Step 7 — Keeping order: why `C:\code\`, why not `Documents`

You put your year of work under `C:\code\`. Why not under `Documents`?

Because `Documents` is a special folder on Windows. Two reasons it causes trouble:

1. **OneDrive often copies `Documents` to the cloud on its own.** OneDrive is fine for Word files, but it's *terrible* for code. It locks files while you're saving, copies half-finished commits, and sometimes renames things without telling you. OneDrive and git get in each other's way, and git is the one that loses. You don't want that.
2. **It mixes everything together.** `Documents` ends up holding school PDFs, screenshots, downloaded forms, and a report your cousin wrote in 2019. Code gets lost in there. Code in `C:\code\` is *only* code, so you always know where to look.

Keeping things tidy is a small habit, but it helps all year:

- **One main folder for code.** That's `C:\code\`. Everything to do with code goes inside it.
- **One folder per project, directly inside that folder.** `C:\code\kingdom\`, `C:\code\kingdom-course\`, and others later. Don't put one project folder inside another.
- **Project folders match repo names.** Your repo on GitHub is `kingdom`, and the folder on disk is `kingdom`. Same name, no surprises.
- **Don't move folders by hand once git is involved.** Once a folder is a git repo, treat its path as part of what it is. If you really need to move it, first make sure `git status` is clean, then move it.

These aren't rules you'll be tested on. They're habits that prevent certain kinds of confusion. Start them now and you'll avoid a whole set of *"why isn't this working"* moments.

---

## Tinker

Open **File Explorer** and **Windows Terminal** side by side on the screen.

In File Explorer, go to `C:\code\kingdom-course\`. In Windows Terminal, type `cd C:\code\kingdom-course` and `ls`. Notice that the same files appear in both. One folder, two views.

In File Explorer, double-click into `phase-0-spark/`. In Windows Terminal, type `cd phase-0-spark` and `ls`. Same place again, same files.

Now try going up. In File Explorer, click the *back* arrow, or click `phase-0-spark` in the path bar at the top. In Windows Terminal, type `cd ..`. Check that both went up to `kingdom-course`.

Now this: in Windows Terminal, type `code .` (with the dot, which means *this folder*). VS Code opens on `kingdom-course`. **It's the same `kingdom-course` you've been looking at in File Explorer and listing in PowerShell.** Three windows on the same folder, three different ways of looking at it.

That's the whole picture. Folders and files sit on the disk. One or more programs (File Explorer, PowerShell, VS Code) show them in different ways. Commands act on whichever folder the program happens to be in.

---

## What you just did

You walked through the filesystem. You saw `C:\` and the tree of folders inside it, and you learned that File Explorer and PowerShell are two views of the same disk. You met file extensions and turned on *"show extensions"* in File Explorer so they stop being hidden. You learned that the terminal is a window, the shell (PowerShell on Windows) runs inside it, and the programs you start run inside the shell — three layers, where stopping one stops everything inside it. You read absolute and relative paths, used `cd` and `..` to move around, and picked up the habits that keep `C:\code\` a tidy work folder for the year. None of this is "code" yet, but every line of code you write will live in this filesystem, run in this shell, and follow these path rules.

**Key concepts you can now name:**

- *filesystem* — the tree of folders and files; drives at the top, files at the leaves
- *path* — the full address of a file; absolute starts at the drive, relative starts at the working directory
- *terminal vs shell* — the window vs the program inside it that runs commands; PowerShell is Windows's default shell
- *process* — a program that is currently running; closing the terminal stops the processes inside it
- *.NET SDK* — the toolkit you run with `dotnet`; compiler plus runtime plus libraries plus NuGet, all installed Day 1

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that you can move around the folders. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

Open Windows Terminal. Without looking:

1. Go into `C:\code` and list what's there.
2. Go into `kingdom-course` and list again.
3. Go back up one level to `C:\code`.

Watch how the prompt changes each time — it always shows the folder you're currently in.

<details><summary>Stuck? Open this to check yourself.</summary>

```powershell
cd C:\code
ls
cd kingdom-course
ls
cd ..
```

- After `cd C:\code`, the prompt shows `C:\code`.
- The first `ls` shows two folders: `kingdom-course` and `kingdom`.
- The second `ls` shows the files inside `kingdom-course`.
- `cd ..` means "go up one level", so the prompt is back to `C:\code`. (`..` is the parent folder; `.` is this folder.)

</details>

## Quiz

This is your first quiz, so here's a quick word on how these work — just once, here. Later lessons end with the standard one-line pointer.

**Quizzes are self-checks, not tests.** Five or six multiple-choice questions, no time limit, no marks. The point is for *you* to check yourself: did the lesson make sense or not? A quiz that goes badly isn't a problem. It just shows you which parts are worth bringing up at the next weekly sync.

**Where you write your answers.** Normally, your answers go in `journal/quiz-notes.md` inside your kingdom repo. One small thing about this first time: that file isn't in your repo yet. You'll copy it in during the next module (`0.0.8`), as part of the day-1 kit. So for this one quiz, write your answers anywhere quick — a notes app, paper, your phone, it doesn't matter. Once you've copied the kit in during `0.0.8`, move them into `journal/quiz-notes.md` properly. From the next quiz on, you write straight into that file.

**The format is short.** One letter for each question, plus one quick sentence about *why* you picked it. You can see how it looks at the top of the template file — open `C:\code\kingdom-course\starter-template\journal\quiz-notes.md` if you want a look. The *why* matters more than the letter. *"I picked b because..."* is the part you'll re-read months later and actually learn something from. The letter on its own is no help after a week.

**Don't write answers in `quiz.md` itself.** Keep that file clean. You might want to come back in a month or two and try the quiz again, and a quiz with the answers already on it isn't really a quiz any more.

**If you're stuck on a question.** Try for twenty minutes. Re-read the part of the lesson the question is about. If it still doesn't come together, post in Slack `#help` (it's been open since Module 0.0). The 20-minute rule in `MENTOR-PROTOCOL.md` is for exactly this. There's a difference between trying and slowly getting somewhere, and just staring at it with no progress.

**Then bring whichever question you were least sure about to the next weekly sync.** That's the rhythm: read the lesson, do the quiz, write your answers, talk through the parts you're unsure about at the sync. Every module from here on works this way.

**There's a Portuguese copy if the English blocks you.** Every quiz has a `quiz.pt.md` next to the `quiz.md` — the same quiz, in Portuguese. Here's how to use it: always try the English `quiz.md` first. Only open `quiz.pt.md` when an English *word* stops you. It's there to help with the language, not to skip the thinking.

Open `quiz.md` now.

## Next

You now know what sits underneath the lessons. **Module 0.0.8 — Roast-O-Matic** is your first session on your own. You'll write a real program, commit it, push it, and post your first win. See you there.
