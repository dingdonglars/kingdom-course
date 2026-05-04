# Quiz answers — Module 3.7

## 1. b
Unit = one method, one class, isolated. Integration = the seams between components — HTTP framework + your routing + your handler + your store + the DB. Each catches a different bug class. Use both; ratio is many unit, few integration.

## 2. b
`WebApplicationFactory<Program>` is the test helper that hosts your full app in-process. No real port — the test client speaks to the in-process server directly. Real routing, real DI, real auth pipeline.

## 3. b
The default `Program.cs` produced by `dotnet new` is a top-level program, which generates an *implicitly internal* `Program` class. Adding `public partial class Program { }` exposes it so the test project can reference it as a type parameter to `WebApplicationFactory<>`.

## 4. a
Integration tests are slower and brittler. Cover the seams (auth wiring, content-type negotiation, the most-used endpoint paths). Don't write one per behaviour — that's what unit tests are for. The right ratio is many unit, few integration.

## 5. a
Tests must be isolated from each other and from your dev DB. A unique temp DB per fixture run guarantees both: tests don't see each other's data, and your real `kingdoms.db` isn't disturbed.