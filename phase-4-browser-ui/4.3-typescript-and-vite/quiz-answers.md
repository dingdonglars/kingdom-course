# Quiz answers — Module 4.3

## 1. b
TypeScript adds compile-time type-checking. `slot.naem` (typo) becomes a build error instead of a runtime undefined. The output is plain JS — the browser runs it normally. The win is at authoring time.

## 2. b
Vite is the modern default for frontend dev: blazing-fast hot reload, TypeScript out of the box, production-grade bundling. Replaced webpack as the default starter. `npm create vite@latest` is the one-line setup.

## 3. b
You edit a file, save, the browser reflects the change without a full reload — your state (form input, scroll position) survives. Multiplied across thousands of edits, this is hours saved.

## 4. b
JSON is untyped at runtime. The compiler has no way to verify the API actually returns the shape you declared. `as` is your assertion that it does. Production apps add a runtime validator (Zod, io-ts) at the boundary — same DTO discipline as C#.

## 5. a
Same rule, different language. C# uses DTOs at every boundary (M2.2, M3.2). TypeScript does the same with interfaces. The shape lives in `types.ts`; everything else stays honest. Skipping types at boundaries is how shape bugs slip into production.