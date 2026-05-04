# Quiz — Module 2.2

## 1. Why introduce a separate `Kingdom.Persistence` project instead of putting JSON code in `Kingdom.Engine`?

a. To pad the file count
b. The engine should not depend on JSON. Other shells (Roblox, database) won't use JSON. Keeping JSON separate keeps the engine free of unwanted dependencies.
c. Required by .NET
d. To pass the M3 challenge

## 2. What's a DTO?

a. Data Transfer Object — a small data-only record purpose-built for crossing a boundary (disk, network)
b. Direct Type Override
c. Default Type Output
d. A C# keyword

## 3. Why is `KingdomSummary` a `record` instead of a `class`?

a. Records are faster
b. `record` gives auto value-equality, immutability, and JSON-friendliness — perfect for DTOs
c. Records are required for JSON
d. Style preference only

## 4. What does `WriteIndented = true` do?

a. Makes the JSON multi-line and human-readable; useful while debugging. Switch to `false` for compactness/network use.
b. Validates the JSON
c. Adds line numbers
d. Encrypts the output

## 5. Why isn't `EventLog` in the `KingdomSummary` record?

a. Forgot to add it
b. The summary is intentionally lossy — only the fields we care about for the demo. Future modules will design a fuller snapshot.
c. JSON can't serialise lists
d. EventLog is private