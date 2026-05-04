# Quiz answers — Module 3.4

## 1. b
OpenAPI is a *spec format* — a standardised JSON document describing your API's contract. Tools (Scalar, Swagger UI, Postman, codegen) read it. Other developers read it. AI agents read it. One generator (`AddOpenApi() / MapOpenApi()`), infinite consumers.

## 2. b
Structured logging captures *named fields*, not just a rendered string. With Serilog/Application Insights/Seq, a query like `WHERE Id = 42` works directly. With string interpolation, you've thrown away the structure — you can only grep.

## 3. b
Scalar / Swagger UI render the OpenAPI spec into an interactive web page. Click an endpoint, fill parameters, hit "Try It," see the response live. Cuts the "what does this API even do" loop from 30 minutes to 10 seconds.

## 4. b
ASP.NET Core writes a lot of `Information`-level events ("starting host", "now listening", "request started", etc.). Without filtering, your own log messages get buried. Setting the framework category to `Warning` keeps your signal-to-noise ratio high.

## 5. b
A 50KB OpenAPI spec gives an AI everything it needs to write a correct client call: endpoints, parameters, expected response shapes, status codes. Without the spec, the AI either reads (tokens) or guesses (errors). OpenAPI is *AI-readable documentation*.