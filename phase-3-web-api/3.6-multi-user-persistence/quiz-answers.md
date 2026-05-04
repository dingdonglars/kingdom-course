# Quiz answers — Module 3.6

## 1. b
Mandatory parameters are guard rails. The store *cannot* be called without specifying which user — the compiler enforces it. If `ownerSub` were optional or read from "ambient context," forgetting to pass it becomes a security bug. Mandatory makes the bug a build error.

## 2. b
Authentication = *who are you*. Authorisation = *what are you allowed to do*. After OAuth, you know the user (auth*entication*). Then `WHERE OwnerSub = currentUser` decides whether they can see this kingdom (auth*orisation*).

## 3. b
`sub` (subject id) is the permanent, globally-unique identifier Google assigns. `email` is mutable — users change addresses. Always store `sub` as the canonical user reference; use `email`/`name` only for display.

## 4. a
The happy path ("user A loads user A's kingdom") would pass even if you forgot the scope. The test that actually checks security is "user B cannot load user A's kingdom." It's the test that catches the bug class real breaches come from.

## 5. b
Without an index, every list query (`WHERE OwnerSub = ?`) scans the whole table — fine for 10 rows, brutal for a million. With an index, the DB does a direct lookup. **Index every column you frequently filter or join on.**