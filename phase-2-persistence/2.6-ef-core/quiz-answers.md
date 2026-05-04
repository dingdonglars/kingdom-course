# Quiz answers — Module 2.6

## 1. a
ORM = Object-Relational Mapper. It bridges two worlds: classes (with properties, inheritance, references) and tables (with columns, primary keys, foreign keys). EF Core, Dapper (lighter), Hibernate (Java), and SQLAlchemy (Python) are all ORMs.

## 2. b
EF expects simple POCO entities — public properties with `get; set;`, parameterless constructor, no funny business. The engine class has `IRandom`/`IClock` constructor parameters and private fields. Trying to make EF deal with that is more pain than just adding a thin entity layer.

## 3. b
EF stages changes in memory (`Add`, `Remove`, modifying tracked entities). `SaveChanges()` flushes everything in a single database transaction — INSERTs, UPDATEs, DELETEs all together. Either all succeed or none do (rollback on error).

## 4. b
By default EF doesn't follow navigation properties — fetching a kingdom doesn't fetch its buildings. `Include` says "also fetch this related collection." Without it, `entity.Buildings` would be empty.

## 5. a
Three-layer mapping is the textbook persistence pattern. Each layer has a single concern: engine = domain logic, entity = wire shape, database = storage. Replacing any one layer (different ORM, different DB) is mechanical instead of structural.