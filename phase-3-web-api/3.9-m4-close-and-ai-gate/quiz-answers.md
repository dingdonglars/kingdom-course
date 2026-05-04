# Quiz answers — Module 3.9

## 1. b
The Gate is at M4 — the end of Block 5. Until then, AI is friction-only (git, env, error explanations). After, AI can help with implementation, with the explanation rule. The timing is intentional: 6 months of doing-it-yourself first, *then* the AI as a power tool.

## 2. b
You must be able to explain every line you merge. AI-generated code you can't explain is *AI-rot* — it works today, breaks tomorrow, and you can't fix it because you didn't build the mental model. Explanation as merge gate is the discipline that keeps you in charge.

## 3. b
The single line in `ai-context/CLAUDE.md` is the contract. Every AI agent in the project reads it on session start. Flipping the flag changes behavior across all tools (Claude Code, Copilot, Cursor) without per-tool config.

## 4. b
The PR description names the bot, lists which lines came from where, and flags anything you're unsure about. `/milestone-review` reads this section to seed the viva — the mentor asks you to explain a random AI-written line. Honesty is rewarded; hidden AI use is not.

## 5. a
Power tools amplify both skill and mistakes. The discipline accumulated through Blocks 1-5 — separation of concerns, deterministic tests, the rename party, multi-user safety, the explanation rule — is what lets the AI multiply your output without multiplying your bugs.