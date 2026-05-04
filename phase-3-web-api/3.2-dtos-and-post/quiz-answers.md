# Quiz answers — Module 3.2

## 1. a
The wire is its own concern. The engine type has constructor needs (`IRandom`, `IClock`), private fields, navigation lists. JSON can serialise some of that, but at the cost of leaking implementation. A purpose-built DTO is small, explicit, and stable across engine changes.

## 2. b
`days ?? 1` — null-coalescing: if `days` is null, use 1. `Math.Clamp(value, min, max)` — force `value` into `[min..max]`. Together: a one-liner that makes the input safe.

## 3. b
`Results.Ok(...)` is the explicit form — same as `200 OK` with the value as body. Use the explicit forms when a single endpoint can return *different* status codes (e.g., 200 if found, 404 if not). For "always 200 OK with this object," returning the object directly is fine.

## 4. b
For primitive types, minimal API binds from the query string by default. Complex types (records, classes) bind from the body. You can be explicit with `[FromQuery]`, `[FromBody]`, `[FromRoute]` attributes if needed.

## 5. a
The shell is where untrusted input enters. The engine should be free to assume its inputs are sane — that's how it stays simple. Validate at the boundary: clamp, reject, return a 400 with a useful message.