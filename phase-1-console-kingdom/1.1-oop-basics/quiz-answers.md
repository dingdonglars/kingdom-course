# Quiz answers — Module 1.1

## 1. b

A class is the *blueprint* — `class Building { ... }` describes what a building IS. An object (or *instance*) is a specific thing created from that blueprint with `new`: `new Building("Main Farm")` creates one Building object. You can have many Building objects, all from the same Building class.

## 2. a

`public int Level` declares a public property. `{ get; private set; }` says: anyone can read it, only code inside this class can write it. `= 1` is the default value. So outside code can read `building.Level` but cannot do `building.Level = 5;`. The class controls when `Level` changes (via `Upgrade()`).

## 3. b

`public Building(string name)` is the *constructor* — same name as the class. It runs once, when you say `new Building("Main Farm")`. Its job is usually to initialise the object's properties. In this case, it copies the `name` parameter into the `Name` property.

## 4. b

Encapsulation. Once a building is built, its name probably shouldn't change. By making `Name` read-only (only a `get`, no `set`), you prevent accidental mutation. If you change your mind later, you can add a `set`. The lesson: **default to read-only; open up only when you have a reason.**

## 5. b

An enum (`enum Resource { Gold, Wood, Stone, Food }`) defines a named set of allowed values. Anywhere you use `Resource`, the only valid values are `Resource.Gold`, `Resource.Wood`, etc. The compiler refuses anything else. This catches typos and invalid arguments at compile time, not runtime.