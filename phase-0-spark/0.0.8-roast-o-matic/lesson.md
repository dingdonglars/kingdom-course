# Module 0.0.8 — Roast-O-Matic

By the end of this session you have a real program on your computer that prints roasts you wrote — and it's on the internet, in your own GitHub repo. Two hours, give or take. This is your first solo session: tools are installed (Module 0.0), you've read the primer (Module 0.0.5), now you write the code.

The shape of today: drop the day-1 kit into your repo, push it to GitHub for the first time, scaffold a tiny C# project called Roast-O-Matic, edit it to print roasts you wrote, push that, then post your first win in `journal/wins.md` and Slack `#wins`.

> **Words to watch**
>
> - **commit** — one labelled snapshot of your code, with a message
> - **push** — send commits up to GitHub
> - **Source Control panel** — VS Code's git interface (icon: branch with a fork)
> - **`dotnet new`** — create a new C# project
> - **`dotnet run`** — compile your code and run the result
> - **starter kit** — the foundation files (conventions, AI rules, journal skeleton) that every kingdom repo has on day one

---

## Step 1 — Drop in the day-1 kit

Your `kingdom` repo is empty. Time to drop in the foundation — the conventions, the AI rules, the journal skeleton, all the files lessons will reference later. The course repo's `starter-template/` folder has them ready.

In Windows Terminal:

```powershell
Copy-Item -Recurse C:\code\kingdom-course\starter-template\* C:\code\kingdom\
```

That brings in `STANDARDS.md`, `CLAUDE.md` (the AI rules), `.claude/commands/` (your slash commands), `journal/` (where wins and reflections live), `.github/PULL_REQUEST_TEMPLATE.md`, `.editorconfig`, `.gitignore`, and a starter `README.md` you'll personalise later.

### Commit the foundation — your first time using the Source Control panel

Switch to VS Code (the kingdom folder is open from M0.0 Part 2). The git workflow lives in the **Source Control** panel:

1. Click the **Source Control** icon in the left sidebar — third icon down, looks like a branch with a fork. (Shortcut: `Ctrl + Shift + G` then `G`.)
2. You'll see a list of new files under *Changes*. Hover the word **Changes** and click the `+` icon to stage them all.
3. In the box at the top, type a commit message: *"day-1 kit"*.
4. Click the blue **checkmark** to commit. The commit is on your computer.
5. Click **Sync Changes** at the bottom of the panel (or the `...` menu → **Push**) to send it to GitHub. The first time, VS Code will pop a browser window to sign in to GitHub — *Authorise Visual Studio Code*, then come back. VS Code remembers the login from there on.

Refresh your repo on github.com — the foundation is now live.

> **Or in the terminal:**
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

## Step 2 — Build Roast-O-Matic

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

### Make it print roasts

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

---

## Step 3 — Push the toy to GitHub

Time to ship the roasts you wrote. You've already done this once (the day-1 kit in Step 1) — same panel, same five clicks.

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

Slack has been up since M0.0; time to use it. Two places your wins live, both matter.

**First, `journal/wins.md` in your repo.** Open the file (it came in with the day-1 kit). Add an entry under today's date:

```markdown
## YYYY-MM-DD — M0.0.8 — Roast-O-Matic shipped

First commit, first push, first program of my own. The repo is live.
```

Save. Commit. Push. (You know how — Source Control panel, message *"M0.0.8 wins entry"*, sync.) `wins.md` is your in-repo trail; you'll add to it every milestone for the rest of the year.

**Then, in Slack `#wins`:**

> 🎉 Roast-O-Matic shipped — *(link to your GitHub repo)*

That's it. Lars probably won't reply — that's the rule for `#wins`, no reply expected. The post is the trail. By the end of the year, scrolling back through `#wins` is the proof you did the work, alongside `journal/wins.md` in your repo.

---

## Tinker

Now write your own roasts. About your friends. About the most annoying YouTuber you can think of. About your cat. Replace the three default roasts with five or six of your own.

Run it again. **Your code, doing what you told it.**

Try printing two roasts at once — call `Console.WriteLine(roast)` twice. Or pick two different ones from the array — what does that take? (Hint: call `random.Next(...)` again.)

---

## What you just did

You dropped in the day-1 kit, made your first commit and your first push using VS Code's Source Control panel, scaffolded a real C# project with `dotnet new console`, ran it, replaced its single `Hello, World!` line with code that picks a roast at random, swapped the roasts for ones you wrote, pushed the lot to GitHub, and posted your first win — both in `journal/wins.md` and in `#wins`. Most of the year is exactly this rhythm — write code, run it, commit it, push it, share the win — just with bigger ideas.

**Key concepts you can now name:**

- a *repo* — a folder of code that git tracks, with a copy on GitHub
- *commit* — saving a snapshot of your work, with a message
- *push* — sending your snapshots up to GitHub
- the **Source Control panel** in VS Code — the buttons that do `add`, `commit`, `push` for you
- *`dotnet run`* — build the C# code and run the result
- *`journal/wins.md`* — your in-repo trail of milestones, posted to `#wins` in parallel

## Quiz

Open `quiz.md`. When you're done, jot your answers and a sentence of reasoning in `journal/quiz-notes.md` — same layout as the entries that came before. Bring whichever you're least sure about to the next weekly sync.

## Next

Welcome to programming. See you in Module 0.1 — Tinker, where Roast-O-Matic learns to ask for a name and roasts your friends by name.
