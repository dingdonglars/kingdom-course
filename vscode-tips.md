# VS Code — startup tips

> A short list of things that make VS Code feel like home, not like a wall of menus. Skim this once after Module 0.0. Come back to it whenever something feels clunky.
>
> **Setup assumed below:** Windows + Portuguese (Brazil) keyboard layout (ABNT2). All shortcuts use the keys you actually have.

VS Code is your main tool for the whole year. Like any tool, the difference between "it kind of works" and "it feels like an extension of your hands" is about ten small habits. Here they are.

## A note on the Portuguese keyboard

Your Portuguese (Brazil / ABNT2) keyboard is a small but real complication for VS Code defaults — almost every coding tutorial assumes a US English keyboard. The differences that actually matter for this course:

| Symbol | US keyboard | Your keyboard (Portuguese ABNT2) |
|---|---|---|
| `\` (backslash) | one key, top right | `AltGr + Q` (or use the dedicated key right of the right-shift, which is `\` + `|`) |
| `[` | one key | `AltGr + 8` |
| `]` | one key | `AltGr + 9` |
| `{` | `Shift + [` | `AltGr + Shift + 8` (or simpler: hold `AltGr` and press `8` then `9` for `[]`, then type the contents) |
| `}` | `Shift + ]` | `AltGr + Shift + 9` |
| `` ` `` (backtick) | one key, top-left | dead key — press `` ` `` then space, OR `AltGr + ’` depending on your specific layout |
| `~` (tilde) | `Shift + ` ` (top-left) | dead key — press `~` then space |
| `@` | `Shift + 2` | `AltGr + 2` |
| `/` | one key | `Shift + 7` (the `/` and `7` share a key) |
| `?` | `Shift + /` | `Shift + W` location? — actually it's `Shift + Q` on ABNT2 |

You'll write `{`, `}`, `[`, `]`, `\`, and `` ` `` constantly in code. **The `AltGr` key (right of the spacebar) becomes your best friend.** Practise the four bracket combinations now until your fingers know them — it removes 90% of the "where is that symbol again?" friction in the first month.

If a key combination involves `` ` `` (backtick) and yours is a dead key, the easiest workaround is to press `` ` `` and then immediately press `Space` — that gives you a single backtick character.

## The four shortcuts you'll use a hundred times a day

These are worth memorising on day one. Every other shortcut can wait.

| Shortcut | What it does |
|---|---|
| `Ctrl + P` | Open any file by name. Just start typing. |
| `Ctrl + Shift + P` | Open the **Command Palette** — every command in VS Code is in here. Forgot how to do something? Ctrl+Shift+P, type a few letters. |
| `Ctrl + ’` (apostrophe key) | Open the terminal at the bottom. *(VS Code's default is `Ctrl + ` `, but on ABNT2 the backtick is a dead key — VS Code on Windows treats `Ctrl + ’` as the same shortcut. If neither works, see "Make the terminal shortcut sane" below.)* |
| `Ctrl + B` | Show or hide the file tree on the left. Big screen feel from a small screen. |

`Ctrl + Shift + P` is the one. If you remember nothing else, remember that one. Anything VS Code can do, you can find by typing into the Command Palette.

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

The terminal panel opens at the bottom of the window with the shortcut above (or via *View → Terminal* in the menu). Same shell, same folder, no Alt-Tab to a separate window. Run `dotnet build`, `dotnet test`, `git status` right there.

You can open multiple terminals (the `+` icon in the terminal panel) — handy when one is running a server and you want to keep typing in another.

You can also resize the panel by dragging its top edge. If the terminal is hogging too much screen, drag it down. If you need to see more output, drag it up.

### Make the terminal shortcut sane

If the default `` Ctrl + ` `` doesn't work for you (the dead-key problem above), rebind it to something your fingers can hit reliably. Recommendation: `Ctrl + Ç` — Athos's keyboard has a real `Ç` key right where US keyboards put `;`, and nothing in VS Code uses it.

How:

1. `Ctrl + Shift + P` → *"Preferences: Open Keyboard Shortcuts"*.
2. Search: *"Toggle Terminal"*.
3. Click the pencil icon next to the matching command, press your new combination, hit Enter.

The same trick fixes any other shortcut whose default involves a dead key on your layout.

### Make sure PowerShell is the default shell

The course gives commands in PowerShell (the Windows shell). Inside the VS Code terminal, check the dropdown at the top-right of the terminal panel — it should say **PowerShell**. If it says **Command Prompt** or **Git Bash**, click the dropdown arrow → *"Select Default Profile"* → **PowerShell**. Restart the terminal.

## Settings worth flipping early

Open Settings (Ctrl+,) and search for these. All of them are quality-of-life:

- **Editor: Format On Save** → on. Saves a tidied-up version of every file. C# files get auto-formatted by the C# extension; you don't have to think about indentation again.
- **Editor: Word Wrap** → `on`. Long lines wrap to the editor width instead of forcing horizontal scroll.
- **Files: Auto Save** → `afterDelay`. VS Code saves your file a second after you stop typing. Fewer "I forgot to save" moments.
- **Editor: Tab Size** → `4`. C# convention. (Some other languages use 2 — VS Code can do per-language settings if you ever care.)

You can change any of these later. They're defaults that work for almost everyone starting out.

## When VS Code's UI shows up in Portuguese

VS Code auto-detects your Windows display language. If it's running in Portuguese and you'd rather have it in English (so its menus match every tutorial and screenshot in this course):

1. `Ctrl + Shift + P` → *"Configure Display Language"*.
2. Pick `en` (English). VS Code will prompt you to install the language pack if needed.
3. Restart VS Code.

If you'd rather keep it in Portuguese, that's fine too — it's your tool. But the lessons will say "Open the Command Palette" not *"Abra a Paleta de Comandos"*, so be ready to translate menu names mentally.

## The thing it took everyone too long to learn

When something doesn't work the way you expect, **read what's actually on screen**. The error message. The squiggle tooltip. The line number in the stack trace. The filename in the breadcrumb at the top.

VS Code is *talkative*. It tells you what's wrong, where, and often why. Most of the early frustration with VS Code is reading-too-fast frustration — flicking past the answer that was right there.

When you're stuck, before pinging `#help`, before asking Claude — slow down and read what's on screen. Twice. The fix is in the read more often than not.
