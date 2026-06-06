# Module 1.0 — Set Up the Kingdom

> Travou no inglês? Abra o `lesson.pt.md` — é esta mesma lição em português. Tente em inglês primeiro.

Phase 1 begins today. From here on, something changes: in Spark Week you built lots of small, separate programs — Roast-O-Matic, the Tavern Tab, and the rest. Each one stood alone. Now you start building **one** thing — the Kingdom — and you keep growing it all year. By the end of the course the same Kingdom will run in the terminal, save to disk, serve a web page, and live inside a Roblox game.

Today is a calm setup day. No game rules yet — that's the next module. Today you build the *home* the Kingdom lives in, learn how you'll open and run it from now on, and make it say hello. Short and steady.

### One thing, made of many parts

The Kingdom won't be one big file. It'll be several **projects** — one for the rules, one for the program you run, more later for saving and the web. To keep them together, they live inside a **solution**: one container that holds many projects.

Think of it like a house. The solution is the house; each project is a room. Today the house has one room (the program you run). Over the year you'll add more rooms — but it stays one house, and you keep it open in one window the whole time.

```text
   Kingdom solution  (the house — one file: Kingdom.slnx)
     │
     └─ Kingdom.Console   (the first room: the program you run)

        ... later: Kingdom.Engine (the rules), tests,
            then Web and Roblox rooms in their own phases
```

> **Words to watch**
>
> - **solution** — one container that groups related projects so they build together. Its file ends in `.slnx`.
> - **project** — one buildable piece (a program or a library). A solution holds one or many.
> - **`.slnx`** — the solution file. (Older guides online show `.sln` — same idea, newer format. Don't worry if you see both.)
> - **branch** — a separate line of work in git. Phase 1's work goes on a branch called `phase-1`.
> - **build** — turn your code into something the computer can run.

---

## Step 1 — make a branch for Phase 1's work

Run this first, before anything else:

```powershell
cd C:\code\kingdom
git switch -c phase-1
```

You're now on a *branch* called `phase-1`. A branch is a separate line of work in git. From now until the end of Phase 1, your commits go onto `phase-1` instead of `main`. At Module 1.10 you'll bring all that work back into `main` through a **pull request** — the place where Lars reviews a whole phase before it joins `main`.

**If "branch" and "pull request" feel unclear right now, that's expected** — it's the first time you've met them. What you actually do is small: one command at the start of the phase (now), and open a pull request on github.com at the end (Module 1.10 walks you through it). The idea stops feeling fuzzy after a few weeks of using it. Module 1.8 comes back to branches once you've lived with one for a while.

Confirm you're on it:

```powershell
git status
```

The first line should say *"On branch phase-1"*.

## Step 2 — give the Kingdom its own home folder

Your `kingdom` repo already holds your Spark Week sketches. To keep the Kingdom clean and separate from them, it gets its own folder:

```powershell
mkdir kingdom-game
cd kingdom-game
```

Everything you build for the rest of the year lives in here. Your old toys stay where they are, out of the way.

## Step 3 — create the solution and your first project

Three commands:

```powershell
dotnet new sln -n Kingdom
dotnet new console -n Kingdom.Console
dotnet sln add Kingdom.Console
```

Line by line:

- `dotnet new sln -n Kingdom` makes the **solution** — a file called `Kingdom.slnx`. That's the house. Right now it's empty.
- `dotnet new console -n Kingdom.Console` makes your first **project**: a console program in a folder called `Kingdom.Console`. This is the program you'll run.
- `dotnet sln add Kingdom.Console` puts the project *into* the solution — moves the room into the house.

Your folder now looks like this:

```text
C:\code\kingdom\kingdom-game\
├─ Kingdom.slnx              <- the solution (the house)
└─ Kingdom.Console\          <- your first project (the program)
   ├─ Kingdom.Console.csproj
   └─ Program.cs             <- one line of code, prints "Hello, World!"
```

## Step 4 — open it as your window (and keep it open all year)

**File → Open Folder…** → pick `C:\code\kingdom\kingdom-game` → Open.

This is the window you keep open for the **whole rest of the course**. You don't hop in and out of project folders the way you did in Spark Week — the Kingdom is one thing, so you open it once and stay there. The title bar should say **kingdom-game**.

> **Your git still works.** Even though the window is the `kingdom-game` folder, VS Code looks *upward* and finds your `kingdom` repo, so the Source Control panel still says **`kingdom`** and Sync pushes as normal. Your `journal/` notes are up in the repo, and the Source Control panel sees them fine.

The full how-to-run guide — and what to do if Run or the debugger ever misbehaves — lives in `running-your-project.md`. Read it once; come back to it whenever something's odd.

## Step 5 — run it

Open a terminal (*View → Terminal*) and run:

```powershell
dotnet run --project Kingdom.Console
```

You should see:

```text
Hello, World!
```

That's the whole point of today: the house is built, one room is in it, and it runs. The `--project Kingdom.Console` part names *which* room to run. You'll write that same command (sometimes naming a different project) all year, so it's worth getting used to now.

To **debug** instead, open `Kingdom.Console/Program.cs`, click to the left of a line number to drop a red breakpoint, and press **F5**. Right now there's only one program that can run, so F5 knows exactly what to start.

## Tinker

Open `Kingdom.Console/Program.cs`. It's one line: `Console.WriteLine("Hello, World!");`. Change the text to `Console.WriteLine("Eldoria awakes.");` and run again. The program is yours to change.

Open `Kingdom.slnx` in the editor. It's a short piece of XML that just lists the projects in the solution. Right now it lists one. You'll watch this file grow as you add rooms.

## What you just did

You set up the home your Kingdom will live in all year. You made a `phase-1` branch for the phase's work, gave the Kingdom its own `kingdom-game` folder so your Spark Week toys don't clutter it, and created a **solution** (`Kingdom.slnx`) with one **project** inside it (`Kingdom.Console`). You opened that folder as your one window, ran the program with `dotnet run --project Kingdom.Console`, and saw it print. No game logic yet — that starts next module — but the workshop is ready.

**Key concepts you can now name:**

- **solution** — one container (`.slnx`) that groups projects so they build together
- **project** — one buildable piece; a solution holds one or many
- **branch** — a separate line of git work; Phase 1 lives on `phase-1`
- **one solution, one window** — open `kingdom-game` once and stay there all year
- **`dotnet run --project <name>`** — run a named project from the solution

## On your own

Time to put the book away. Don't scroll back up — prove to yourself, from your own head, that the setup stuck. No one marks this — it's just for you.

Without looking, answer out loud or on paper:

1. What's the difference between a **solution** and a **project**?
2. Which folder do you open in VS Code to work on the Kingdom — and how is that different from how you opened your Spark Week toys?
3. What command runs the program, and what does the `--project` part do?

<details><summary>Stuck? Open this to check yourself.</summary>

- A **solution** is the container (the house, the `.slnx` file); a **project** is one buildable piece inside it (a room). One solution can hold many projects.
- You open the **`kingdom-game` folder** — the whole Kingdom — as one window, and keep it open all year. In Spark Week each toy was separate, so you opened *each toy's own folder*. The Kingdom is one thing, so it's one window.
- `dotnet run --project Kingdom.Console` runs it. `--project Kingdom.Console` names *which* project in the solution to run — useful once the solution holds more than one runnable program.

If any of that felt shaky, glance back at the step before moving on.

</details>

## Wrap up

1. **Progress** — one line in `journal/progress.md`: `Module 1.0 — Set Up the Kingdom — DATE — made the solution and ran Hello. Learnt: one sentence.`
2. **Commit and push** — stage your work, commit message `Module 1.0 done`, Sync. (There's no quiz for a setup day.)
3. **Post in `#wins`** — one line: the Kingdom has its home and it runs. Add the commit URL.

Module 0.1 covers the why and the panel/CLI steps if you need a refresher on committing.

## Next

Module 1.1 fills the empty program with the first real code: four classes — `Resource`, `Building`, `Citizen`, `Kingdom` — that build a tiny medieval kingdom and print it to the terminal. The workshop is ready; now you start building.
