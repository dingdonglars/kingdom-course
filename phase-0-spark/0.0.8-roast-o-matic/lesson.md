# Module 0.0.8 — Roast-O-Matic

By the end of this session you have a real program on your computer that prints roasts you wrote — and it's on the internet, in your own GitHub repo. About two hours. This is your first session on your own. The tools are installed (Module 0.0), you've read the primer (Module 0.0.5), and now you write the code.

Here's the plan for today. You copy the day-1 kit into your repo and push it to GitHub for the first time. You create a tiny C# project called Roast-O-Matic. You edit it to print roasts you wrote, and push that too. Then you post your first win, in `journal/wins.md` and in Slack `#wins`.

> **Words to watch**
>
> - **commit** — one labelled snapshot of your code, with a message
> - **push** — send commits up to GitHub
> - **Source Control panel** — VS Code's git interface (icon: branch with a fork)
> - **`dotnet new`** — create a new C# project
> - **`dotnet run`** — compile your code and run the result
> - **starter kit** — the base files (conventions, AI rules, an empty journal) that every kingdom repo has on day one

---

## Step 1 — Drop in the day-1 kit

Your `kingdom` repo is empty. Time to copy in the base files: the conventions, the AI rules (these wait for Claude later in the year), the empty journal, and all the files that later lessons will point you to. The course repo's `starter-template/` folder has them ready.

In Windows Terminal:

```powershell
Copy-Item -Recurse C:\code\kingdom-course\starter-template\* C:\code\kingdom\
```

That brings in `STANDARDS.md`, `CLAUDE.md` (the AI's rules — these don't do anything until Claude arrives at Module 3.9), `.claude/commands/` (slash commands, also waiting for Module 3.9), `journal/` (where your wins and notes live), `.github/PULL_REQUEST_TEMPLATE.md`, `.editorconfig`, `.gitignore`, and a starter `README.md` you'll make your own later.

### Commit the base files — your first time using the Source Control panel

Switch to VS Code (the kingdom folder is open from Module 0.0 Part 2). You do all the git steps from the **Source Control** panel:

1. Click the **Source Control** icon in the left sidebar — third icon down, looks like a branch with a fork. (Shortcut: `Ctrl + Shift + G` then `G`.)
2. You'll see a list of new files under *Changes*. Hover the word **Changes** and click the `+` icon to stage them all.
3. In the box at the top, type a commit message: *"day-1 kit"*.
4. Click the blue **checkmark** to commit. The commit is on your computer.
5. Click **Sync Changes** at the bottom of the panel (or the `...` menu → **Push**) to send it to GitHub. The first time, VS Code opens a browser window to sign in to GitHub. Click *Authorise Visual Studio Code*, then come back. VS Code remembers the login after that.

Refresh your repo on github.com. The base files are now on GitHub.

> **Or in the terminal:**
>
> ```powershell
> cd C:\code\kingdom
> git add .
> git commit -m "day-1 kit"
> git push
> ```
>
> The buttons in VS Code do exactly these four steps for you. Through the year you'll mostly use the buttons. The terminal is still handy for the times when typing is faster than clicking.

---

## Step 2 — Build Roast-O-Matic

Inside your `kingdom` repo, you'll create a small console project. A console project is a program that runs in the terminal and talks to you with text. Open Windows Terminal:

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

### Make it print roasts

Open `Program.cs` in VS Code (it should already show up in the file list on the left). You'll see one line: `Console.WriteLine("Hello, World!");`. Replace it with this (you can copy it from `starter/Program.cs` in the course repo):

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

A different roast each time. Run it three times. **You're running code that picks something at random.**

---

## Step 3 — Push the toy to GitHub

Time to send the roasts you wrote up to GitHub. You've already done this once (the day-1 kit in Step 1) — same panel, same five clicks.

In VS Code's Source Control panel:

1. Open the **Source Control** panel (`Ctrl + Shift + G G` if it isn't visible).
2. You'll see your changed files under *Changes* — including the new `RoastOMatic/` folder and its contents. Hover **Changes** → click `+` to stage all.
3. Commit message: *"first roasts of my own"*.
4. Click the blue checkmark to commit.
5. Click **Sync Changes** to push to GitHub.

Now go to your repo on github.com in the browser. **Refresh.** Your code is there. It's on the internet.

> **Or in the terminal:**
>
> ```powershell
> cd C:\code\kingdom
> git add .
> git commit -m "first roasts of my own"
> git push
> ```

---

## Step 4 — Post your first win

Slack has been ready since Module 0.0; time to use it. Your wins go in two places, and both matter.

**First, `journal/wins.md` in your repo.** Open the file (it came in with the day-1 kit). Add an entry under today's date:

```markdown
## YYYY-MM-DD — Module 0.0.8 — Roast-O-Matic shipped

First commit, first push, first program of my own. The repo is live.
```

Save. Commit. Push. (You know how — Source Control panel, message *"Module 0.0.8 wins entry"*, sync.) `wins.md` is your record inside the repo. You'll add a line to it at every milestone for the rest of the year.

**Then, in Slack `#wins`:**

> 🎉 Roast-O-Matic shipped — *(link to your GitHub repo)*

That's it. Lars probably won't reply, and that's the rule for `#wins` — no reply expected. The post is the record. By the end of the year, scrolling back through `#wins` shows the proof you did the work, next to `journal/wins.md` in your repo.

---

## Tinker

Now write your own roasts. About your friends. About the most annoying YouTuber you can think of. About your cat. Replace the three default roasts with five or six of your own.

Run it again. **Your code, doing what you told it to do.**

Try printing two roasts at once — call `Console.WriteLine(roast)` twice. Or pick two different ones from the array. What would that take? (Hint: call `random.Next(...)` again.)

---

## What you just did

You copied in the day-1 kit, made your first commit and your first push using VS Code's Source Control panel, created a real C# project with `dotnet new console`, ran it, replaced its single `Hello, World!` line with code that picks a roast at random, changed the roasts to ones you wrote, pushed it all to GitHub, and posted your first win — both in `journal/wins.md` and in `#wins`. Most of the year follows this same rhythm — write code, run it, commit it, push it, share the win — just with bigger ideas.

**Key concepts you can now name:**

- a *repo* — a folder of code that git tracks, with a copy on GitHub
- *commit* — saving a snapshot of your work, with a message
- *push* — sending your snapshots up to GitHub
- the **Source Control panel** in VS Code — the buttons that do `add`, `commit`, `push` for you
- *`dotnet run`* — build the C# code and run the result
- *`journal/wins.md`* — your record of milestones inside the repo, posted to `#wins` at the same time

## On your own

Time to put the book away. Don't scroll back up to the steps — prove to yourself, from your own head, that the two big moves stuck. No one marks this — it's just for you. It's the easiest way to spot what hasn't stuck yet, while it's still simple to fix. Getting stuck here is completely fine — that's exactly what it's for.

### 1. Make and run a new project, from memory

In Windows Terminal, go into your `kingdom` folder. Without looking:

1. Make a brand-new console project called `Hello`.
2. Move into it.
3. Run it.

You should see a line of text print out.

<details><summary>Stuck? Open this to check yourself.</summary>

```powershell
cd C:\code\kingdom
dotnet new console -n Hello
cd Hello
dotnet run
```

- `dotnet new console -n Hello` makes a `Hello` folder with a `Program.cs` and a `.csproj` inside.
- `cd Hello` moves into it.
- `dotnet run` builds the code and runs it. You should see `Hello, World!`.

</details>

### 2. Commit and push, from memory

You just made new files. From memory, no scrolling back:

1. Send them to GitHub using the Source Control panel — stage, commit, Sync.
2. Refresh your repo page on github.com and check the `Hello` folder is there.

<details><summary>Stuck? Open this to check yourself.</summary>

- Open the Source Control panel (`Ctrl + Shift + G` then `G`). The header should say `kingdom`.
- Hover the word *Changes* and click `+` to stage all the new files.
- Type a commit message in the box, like *"add hello project"*.
- Click the blue checkmark to commit.
- Click *Sync Changes* to push to GitHub.
- Refresh your repo on github.com — the `Hello` folder is now there.

</details>

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Welcome to programming. See you in Module 0.1 — Tinker, where Roast-O-Matic learns to ask for a name and roasts your friends by name.
