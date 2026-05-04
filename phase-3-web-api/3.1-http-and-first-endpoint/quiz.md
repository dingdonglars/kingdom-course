# Quiz — Module 3.1

## 1. Which of these is the *verb* in `GET /kingdom HTTP/1.1`?

a. `kingdom`
b. `GET`
c. `HTTP/1.1`
d. `/kingdom`

## 2. What's an *idempotent* HTTP method?

a. One that's been deprecated
b. One where doing it twice has the same effect as doing it once. `GET`, `PUT`, `DELETE` are idempotent; `POST` typically isn't.
c. One that returns JSON
d. One that requires authentication

## 3. What does the `4xx` family of status codes mean?

a. Server's fault
b. Client sent something wrong (bad request, not found, unauthorized, forbidden)
c. Success
d. Redirect

## 4. What does `app.MapGet("/kingdom", () => ...)` do?

a. Sends a GET request
b. Registers a route — when an HTTP `GET /kingdom` arrives, run the lambda and serialise its return value to JSON
c. Creates a new endpoint URL
d. Configures logging

## 5. The lesson says "the API is a shell." What's that mean here?

a. ASP.NET runs in a shell
b. HTTP is just one more way to talk to the engine; the engine doesn't change. Same as Console (M1.10), Persistence (Block 4), now Web API.
c. The shell is the operating system
d. It's a metaphor with no real meaning