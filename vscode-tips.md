# VS Code — startup tips

> A short list of things that make VS Code feel like home, not like a wall of menus. Skim this once after Module 0.0. Come back to it whenever something feels clunky.

VS Code is your main tool for the whole year. Like any tool, the difference between "it kind of works" and "it feels like an extension of your hands" is about ten small habits. Here they are.

## The four shortcuts you'll use a hundred times a day

These are worth memorising on day one. Every other shortcut can wait.

| Shortcut | What it does |
|---|---|
| `Ctrl + P` | Open any file by name. Just start typing. |
| `Ctrl + Shift + P` | Open the **Command Palette** — every command in VS Code is in here. Forgot how to do something? Ctrl+Shift+P, type a few letters. |
| `Ctrl + ` ` | Open the terminal at the bottom (the backtick key, top-left of the keyboard) |
| `Ctrl + B` | Show or hide the file tree on the left. Big screen feel from a small screen. |

`Ctrl + Shift + P` is the one. If you remember nothing else, remember that one. Anything VS Code can do, you can find by typing into the Command Palette.

> **On a Mac:** swap `Ctrl` for `Cmd` everywhere on this page.

## Two more that pay back instantly

- **`Ctrl + /`** — toggle a comment on the line your cursor is on. Highlight several lines first to comment a block.
- **`F12`** — jump to where a thing is *defined*. Click on a class name, hit `F12`, and VS Code takes you to the file that wrote it. `Alt + ←` jumps you back. This one feels like magic the first time.

## Extensions to install

VS Code is small by default. You add what you need. For this course, install these now:

1. **C# Dev Kit** (Microsoft) — the main C# experience. Adds IntelliSense, debugging, test runner, the works.
2. **C#** (Microsoft) — installed automatically by Dev Kit; if it isn't, install it manually.
3. **GitLens** — git history right inside the editor. Hover over any line and see who wrote it and when (it'll all be you for a while — that's fine).
4. **Error Lens** — shows compiler errors *inline*, on the same line as the broken code, instead of buried in a list. Catches mistakes seconds earlier.

Install via the Extensions sidebar (Ctrl+Shift+X) or by typing into the Command Palette: *"Extensions: Install Extensions"*.

Don't install fifty extensions out of excitement. Each one is a moving part. Add them as the course needs them.

## The editor will fight you sometimes — read the squiggle

Three colours of squiggle live under your code:

- **Red** — broken. Won't compile. Fix this before you do anything else.
- **Yellow / orange** — warning. Compiles, but the language is suspicious. Read it.
- **Blue / grey** — suggestion. Optional. Often a stylistic improvement.

**Hover the squiggle.** A little tooltip appears explaining what's wrong. Read it before guessing. Most beginner pain comes from skipping the tooltip and guessing wildly. The error message is almost always saying exactly what's wrong — in slightly weird words.

If the explanation is too dense, paste it into Claude. *"What does this C# error mean: ..."* That's a fine, encouraged use of AI.

## When stuff breaks — the three reset moves

When VS Code itself feels broken (red squiggles on code you didn't change, IntelliSense stopped working, files look stale), try these in order. Each is harmless.

1. **Reload Window.** Ctrl+Shift+P → *"Developer: Reload Window"*. Restarts VS Code's brain without closing the project. Fixes maybe half of weird states.
2. **Restart the C# language server.** Ctrl+Shift+P → *"OmniSharp: Restart"* or *".NET: Restart Language Server"*. Fixes most of the rest.
3. **Close and reopen VS Code.** The full nuclear restart. Almost always fixes the residual cases.

If it's still broken after all three, *then* it's a real problem and worth pinging `#help`. But the first three resolve maybe 90% of "VS Code is being weird" moments.

## The terminal lives inside the editor

`` Ctrl + ` `` opens a terminal at the bottom of the window. Same shell, same folder, no Alt-Tab to a separate window. Run `dotnet build`, `dotnet test`, `git status` right there.

You can open multiple terminals (the `+` icon in the terminal panel) — handy when one is running a server and you want to keep typing in another.

You can also resize the panel by dragging its top edge. If the terminal is hogging too much screen, drag it down. If you need to see more output, drag it up.

## Settings worth flipping early

Open Settings (Ctrl+,) and search for these. All of them are quality-of-life:

- **Editor: Format On Save** → on. Saves a tidied-up version of every file. C# files get auto-formatted by the C# extension; you don't have to think about indentation again.
- **Editor: Word Wrap** → `on`. Long lines wrap to the editor width instead of forcing horizontal scroll.
- **Files: Auto Save** → `afterDelay`. VS Code saves your file a second after you stop typing. Fewer "I forgot to save" moments.
- **Editor: Tab Size** → `4`. C# convention. (Some other languages use 2 — VS Code can do per-language settings if you ever care.)

You can change any of these later. They're defaults that work for almost everyone starting out.

## The thing it took everyone too long to learn

When something doesn't work the way you expect, **read what's actually on screen**. The error message. The squiggle tooltip. The line number in the stack trace. The filename in the breadcrumb at the top.

VS Code is *talkative*. It tells you what's wrong, where, and often why. Most of the early frustration with VS Code is reading-too-fast frustration — flicking past the answer that was right there.

When you're stuck, before pinging `#help`, before asking Claude — slow down and read what's on screen. Twice. The fix is in the read more often than not.
