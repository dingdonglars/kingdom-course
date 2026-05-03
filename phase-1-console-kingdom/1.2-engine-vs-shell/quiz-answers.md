# Quiz answers — Module 1.2

## 1. b

A class library is a project that compiles to a `.dll` (a "dynamic link library"). It has no `Main` method and isn't directly runnable; other projects use it. A console app has a `Main` method and compiles to an executable `.exe`. The two project types are created with different commands: `dotnet new classlib` vs `dotnet new console`.

## 2. b

The console depends on the engine — the console knows about `Building`, `Resource`, etc. The engine does NOT know about the console. This direction matters: it lets the engine be reused with other shells (web, mobile, Roblox).

## 3. b

The engine is meant to be runtime-agnostic. `Console.WriteLine` ties the code to the console runtime — it wouldn't work (or would be silently no-op) in a web server, in Roblox, in a unit test process. By keeping IO out of the engine, you keep the engine portable.

## 4. b

`<ProjectReference Include="...">` is the .csproj way of saying "I depend on this project." The build system uses it to compile the engine first, then the console with the engine's `.dll` available.

## 5. b

A class library project (`<Project Sdk="Microsoft.NET.Sdk">` with no `<OutputType>`) defaults to `Library`, producing a `.dll`. To make a runnable `.exe`, add `<OutputType>Exe</OutputType>` to the `<PropertyGroup>`.