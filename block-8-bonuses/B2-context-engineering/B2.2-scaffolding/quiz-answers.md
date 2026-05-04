# Quiz answers — B2.2

## 1. a
Scaffold files are persistent context. The AI reads them on session start (or you reference them); they shape every interaction without per-prompt repetition. Investment compounds.

## 2. a
Token economy. A 500-line ARCHITECTURE.md is read every session. The cost is paid forever; the hit rate on the extra info is small. Keep scaffolds *tight* — if you can't answer "why is this line in here?" delete it.

## 3. a
Example files give the AI a concrete anchor. *"Write a method like `examples/01-store-method.md`"* lands the output much closer than abstract conventions. Hand-pick 3-5 examples; reference them in scoped prompts.

## 4. a
A scaffold that lies actively hurts: the AI follows it confidently to wrong answers. Better no scaffold than wrong scaffold. The discipline: audit every block; delete or update anything stale.

## 5. a
The 5-line header makes the file self-orienting. Anyone opening it (AI or human) sees role + conventions + related files immediately. Cheap; high-leverage; works at every scale.