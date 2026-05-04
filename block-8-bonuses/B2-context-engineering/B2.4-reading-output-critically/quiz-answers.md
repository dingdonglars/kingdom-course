# Quiz answers — B2.4

## 1. a
The 5 are the high-frequency failure modes. Invented APIs (call something that doesn't exist), unhappy paths (null/empty/edge), style drift (wrong patterns), over-help (did more than asked), swallowed errors (silent catches). Two-minute read; catches most AI-rot.

## 2. b
The AI confidently calls a method that doesn't exist. `db.Kingdoms.GetRichest()` — sounds right, doesn't exist. Grep the suspicious calls; if they're not in your code or libraries, push back.

## 3. a
`catch { return null; }` swallows everything — network errors, OOM, programming bugs. Your tests pass; production silently corrupts. Always: catch the *named* exception you can handle; rethrow the rest; log everything.

## 4. a
Over-help is scope creep. Either it's wrong (now you have to roll it back), or it's right but unauthorized (a decision you should have made). Both are bad. Stay on the scoped task; if more is needed, ask explicitly.

## 5. a
The viva test (post-gate, M3.9) is the practical eval. If you can explain every line of AI-generated code, you ship. If not, you ask the AI more questions until you can. Code you can't explain = code you can't maintain.