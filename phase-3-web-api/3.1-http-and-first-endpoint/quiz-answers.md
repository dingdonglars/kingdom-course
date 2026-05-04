# Quiz answers — Module 3.1

## 1. b
The verb is the action word: `GET`, `POST`, `PUT`, `DELETE`, `PATCH`. Path is `/kingdom`. `HTTP/1.1` is the protocol version. The verb tells the server *what* the client wants to do.

## 2. b
Idempotent: same effect on retry. Doing `DELETE /thing/5` twice still leaves it deleted. Doing `GET /thing/5` twice still returns the same thing. `POST /things` typically *creates* — doing it twice creates two — so it's not idempotent. The distinction matters when networks blip and clients retry.

## 3. b
4xx = client sent something wrong (400 bad request, 401 not signed in, 403 forbidden, 404 not found). 5xx = server crashed (500 server error, 503 unavailable). The split helps debugging — *whose code is at fault?*

## 4. b
`MapGet` registers a route handler. When the framework receives a matching request, it runs the lambda. The return value is serialised to JSON automatically. (Other return-type rules apply for strings, status codes, IResult — covered later.)

## 5. b
The pattern repeats: build a Kingdom in memory, hand it to the shell to expose. Engine unchanged across console (Block 3), persistence (Block 4), API (Block 5). That's the engine-vs-shell discipline still paying off three blocks later.