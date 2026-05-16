# Quiz — Módulo 3.1

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual parte de `GET /kingdom HTTP/1.1` é o *verbo*?

- **a.** `kingdom`
- **b.** `GET`
- **c.** `HTTP/1.1`
- **d.** `/kingdom`

## 2. O que significa um método HTTP ser *idempotente*?

- **a.** Fazer ele duas vezes tem o mesmo efeito que fazer uma vez — `GET`, `PUT`, `DELETE` são idempotentes; `POST` normalmente não é
- **b.** Ele retorna JSON em vez de texto puro
- **c.** Ele exige que o cliente esteja logado
- **d.** Ele foi substituído por um método mais novo

## 3. O que a família de códigos de status 4xx significa?

- **a.** A requisição teve sucesso
- **b.** O cliente enviou algo errado — requisição ruim, não encontrado, não autorizado, proibido
- **c.** O servidor quebrou ou bateu num problema dele mesmo
- **d.** O servidor está pedindo que o cliente seja redirecionado para outro lugar

## 4. O que `app.MapGet("/kingdom", () => ...)` realmente faz?

- **a.** Envia uma requisição `GET` para outro servidor
- **b.** Registra uma rota — quando um `GET /kingdom` chega, roda a lambda e transforma o valor de retorno dela em JSON
- **c.** Cria um endpoint de URL totalmente novo na internet pública
- **d.** Configura o registro de logs da aplicação

## 5. A lição diz que *a API é mais uma camada externa*. O que isso quer dizer aqui?

- **a.** O ASP.NET roda dentro de um shell do sistema operacional
- **b.** HTTP é só mais uma forma de conversar com a engine; a engine em si não muda. Igual ao console, igual à persistência, agora a API web.
- **c.** O shell é o sistema operacional em que o servidor roda
- **d.** É uma metáfora sem um sentido concreto

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
