# Quiz answers — B2.1

## 1. a
The four-step frame: prompt (your ask) → context (everything else the AI sees) → output (what it returns) → eval (your judgement). Three of the four are engineerable; the middle generation step is the one you can't directly steer.

## 2. a
Generic-tutorial output (no scaffolding), invented APIs (calls non-existent methods), drift from style (uses different patterns than your codebase). Naming them is half the cure — once you know the failure modes, you spot them in seconds.

## 3. a
Generation is a black box. Prompt, context, scaffolding, eval — yours. Optimise the parts you control instead of trying to control the model's output via tricks.

## 4. b
Scaffolding is the persistent background that ships with every interaction (ARCHITECTURE.md, STANDARDS, type files). Scoping is the per-task framing (the goal of *this* request + the traps to avoid). Both matter; they're different surfaces.

## 5. a
Eval is *your* call: does this fit my project? Does it follow our conventions? The AI doesn't know your bar — you do. Skipping the eval step is how AI-rot enters the codebase.