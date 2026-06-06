# Using the debugger — a mini guide

> Travou no inglês? Abra o `using-the-debugger.pt.md` — é este mesmo guia em português. Tente em inglês primeiro.

> Read this once. Come back any time you want to watch your code run, or hunt down a bug.
>
> **Setup assumed:** Windows + VS Code (with C# Dev Kit), your project open.

A debugger lets you **pause your program on a line and look inside it** — see every variable, run one line at a time, and watch values change. It turns *"I think this is what happens"* into *"I can see exactly what happens."* It's the fastest way to understand code, and the fastest way to find a bug.

(You first met the debugger in Module 0.8. This page is the short reference to come back to.)

## Launch it

- Open the file you want to watch (for example `Program.cs`).
- Press **F5**. The program starts *under the debugger*.
- To run *without* pausing on breakpoints, press **Ctrl+F5**.

If F5 does nothing, or asks "which project?", that's a *which-folder / which-project* thing — not a debugger problem. See `running-your-project.md`.

## Breakpoints — where it pauses

A **breakpoint** is a line you mark so the program stops there.

- **Set one:** click in the narrow strip just left of a line number. A red dot appears. (Or put your cursor on the line and press **F9**.)
- **Remove it:** click the red dot again (or press F9 again).
- The program pauses **just before that line runs** — the line turns yellow, and it *hasn't happened yet*.
- Set as many as you like.

## The keys you'll use most

| Key | What it does |
|---|---|
| **F5** | Start debugging / **Continue** (run until the next breakpoint) |
| **Ctrl+F5** | Run **without** the debugger |
| **F9** | Add / remove a breakpoint on the current line |
| **F10** | **Step over** — run this line, move to the next one |
| **F11** | **Step into** — go *inside* the method this line calls |
| **Shift+F11** | **Step out** — finish this method and go back to whoever called it |
| **Shift+F5** | **Stop** debugging |
| **Ctrl+Shift+F5** | **Restart** from the beginning |

**Step over vs step into:** **F10** runs a method call in one go; **F11** follows it *inside*, line by line. Use F11 when you want to see what a method actually *does*; F10 when you trust it and just want to move past it.

## While it's paused — the panels

Press **Ctrl+Shift+D** to open the **Run and Debug** view on the left. It has four panels:

- **Variables** — every variable in view right now, with its value. Click the arrow next to an object (like `kingdom`) to open it up and see its fields, lists, and dictionaries inside.
- **Watch** — expressions *you* choose to keep an eye on. Click **+** and type something like `kingdom.Day` or `kingdom.Resources.Get(Resource.Food)`. It refreshes every time the program pauses. (Or right-click a variable → **Add to Watch**.)
- **Call Stack** — the chain of methods that led to here: who called whom. The top line is where you are now. Click any line below it to jump to that method and see *its* variables.
- **Breakpoints** — a list of all your breakpoints. Tick / untick to turn them on and off without deleting them.

**Tip:** while paused, just **hover your mouse over any variable in the code** to see its value in a little pop-up.

## Change a value while paused

You can edit the program *while it's stopped*:

- In the **Variables** panel, double-click a value, type a new one, and press **Enter**. The program continues with your new value.
- Example: pause inside the game loop, change `Day` to `49`, then continue — now you're testing what happens at day 50 without waiting for it.

You can also type expressions in the **Debug Console** (the panel at the bottom while paused): type `kingdom.Day` and press Enter to see it, or `kingdom.Resources.Get(Resource.Gold)` to check a number. You can even call methods from there.

## Pause only when it matters — conditional breakpoints

A normal breakpoint pauses *every* time. Sometimes you only care about one case.

- Right-click a red dot → **Edit Breakpoint…** → type a condition like `Day == 50`. Now it only pauses when that's true.
- Perfect when a bug appears on the 50th time round a loop, not the 1st.

(There's also a **Logpoint** — right-click the strip → *Add Logpoint…* — which **prints a message instead of pausing**. Handy when you want to watch without stopping the program.)

## A 60-second tour on your kingdom

Once you have a game loop (Module 1.4):

1. Put a breakpoint on the first line inside `AdvanceDay`.
2. **F5** — it pauses at the start of the first day.
3. Open **Variables**; find `Resources` and `Day`.
4. **F10** through the lines and watch `Food` drop and `Day` climb.
5. **F11** on `b.Tick(...)` to step *inside* a building's production.
6. **F5** to finish, or **Shift+F5** to stop.

## If F5 misbehaves

Nine times out of ten it's the wrong folder or project open, not your code. See `running-your-project.md`: in Phase 0 you open one program's folder; from Phase 1 you open the `kingdom-game` solution, and from Phase 3 you pick which project F5 starts.

## Cheat sheet

- **Pause here:** click the strip left of the line / **F9**
- **Go:** **F5**  ·  **Run, no debug:** **Ctrl+F5**  ·  **Stop:** **Shift+F5**
- **One line:** **F10**  ·  **Into a method:** **F11**  ·  **Back out:** **Shift+F11**
- **See values:** Variables panel · hover · Debug Console
- **Track an expression:** Watch (**+**)
- **Who called this:** Call Stack
- **Change a value:** double-click it in Variables
