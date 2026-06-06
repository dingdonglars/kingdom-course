# Módulo 3.1 — HTTP e o Seu Primeiro Endpoint

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

No fim de hoje, o seu reino vai estar em `http://localhost:5000/kingdom`. Você vai abrir essa URL no navegador e o reino vai aparecer como JSON. O navegador agora é só um dos vários clientes possíveis — o `curl` de um amigo, algum código JavaScript, o seu celular — todos falam a mesma língua e todos chegam até o mesmo motor. O console que você escreveu na Fase 1 era uma camada externa. Hoje você adiciona outra.

Vamos aprender HTTP em uma tela, e depois escrever um único endpoint ASP.NET que serve o seu reino pela rede. *ASP.NET Core* é o framework web da Microsoft — a parte do .NET que lida com HTTP. O estilo de **minimal API** é o jeito novo e leve de escrever endpoints: algumas linhas em vez de uma pasta inteira de controllers.

E veja bem: isso é a ideia de *motor e shell* da Fase 1, mais uma vez. A API web é só um novo shell. Os clientes fazem perguntas; ela faz as mesmas perguntas ao mesmo `Kingdom.Engine` que você tem desde sempre.

```text
   CLIENTES                      SERVIDOR (um novo shell)         MOTOR

   navegador / curl / celular
        |  --- GET /kingdom --->   +------------------+
        |       (uma requisição)   | Endpoint ASP.NET |--usa-->  Kingdom.Engine
        |  <-- 200 + JSON -------   +------------------+          (mesmas regras
        |       (uma resposta)                                      da Fase 1)
```

O console era um shell; a API é outro. Nenhum guarda as regras — eles só levam perguntas ao motor e trazem as respostas de volta.

> **Words to watch**
>
> - **HTTP** — Hypertext Transfer Protocol — a língua que clientes e servidores falam pela rede
> - **request** / **response** — uma mensagem do cliente para o servidor e a resposta que vem de volta
> - **verb (or method)** — `GET`, `POST`, `PUT`, `DELETE` — o que o cliente quer fazer
> - **status code** — o número de três dígitos que descreve como foi a requisição (200 OK, 404 Not Found, 500 Server Error)
> - **minimal API** — a sintaxe leve do ASP.NET Core para escrever endpoints (`app.MapGet(...)`)

---

## HTTP, em uma tela

Um cliente (um navegador, `curl`, um app mobile — qualquer coisa) envia uma **request**:

```
GET /kingdom HTTP/1.1
Host: localhost:5000
Accept: application/json
```

Um servidor responde com uma **response**:

```
HTTP/1.1 200 OK
Content-Type: application/json

{ "name": "Eldoria", "day": 11 }
```

Três partes que você precisa saber: o **verbo mais o caminho** (`GET /kingdom`), o **status code** (200, 404, 500), e o **body** (o JSON). O resto é detalhe que você aprende conforme precisar.

Os cinco verbos que você vai encontrar hoje e amanhã:

| Verbo | O que faz | Idempotente? |
|---|---|---|
| `GET` | lê dados | sim |
| `POST` | cria algo ou executa uma ação | não |
| `PUT` | substitui um recurso inteiro | sim |
| `PATCH` | atualização parcial | depende |
| `DELETE` | remove | sim |

*Idempotente* (i-dem-po-ten-te) significa que fazer a ação duas vezes tem o mesmo efeito que fazer uma vez. Isso importa quando a rede tem um problema e o cliente manda a requisição de novo — você não quer que uma segunda tentativa cobre um cartão de crédito duas vezes.

## Status codes comuns

| Código | Significado |
|---|---|
| 200 OK | sucesso |
| 201 Created | sucesso — uma coisa nova foi criada |
| 204 No Content | sucesso — não há body para retornar |
| 400 Bad Request | o cliente enviou dados ruins |
| 401 Unauthorized | o cliente não está conectado |
| 403 Forbidden | conectado mas não tem permissão |
| 404 Not Found | o recurso não existe |
| 500 Internal Server Error | o servidor travou |

Você vai lembrar desses usando-os. A diferença mais importante é entre 4xx (culpa do cliente) e 5xx (culpa do servidor). O resto é detalhe.

## O que vem no starter

Um novo projeto — `Kingdom.Api` — mais um projeto de teste placeholder. Os projetos anteriores não mudam.

- **NOVO:** `Kingdom.Api/` (minimal API do ASP.NET Core)
- **NOVO:** `Kingdom.Api/Program.cs` — configura o host e o primeiro endpoint
- **MODIFICADO:** `Kingdom.slnx` — adiciona o novo projeto à solução
- **NOVO:** `tests/Kingdom.Api.Tests/` — placeholder; os testes de integração reais vêm no Módulo 3.7

Para este módulo a API tem um endpoint: `GET /kingdom` retorna um `KingdomSummary`.

(*Endpoint* é uma URL que a API responde, como `/kingdom`. Cada endpoint faz um trabalho.)

## Passo 0 — criar o projeto

```powershell
dotnet new web -n Kingdom.Api
dotnet add Kingdom.Api reference Kingdom.Engine
dotnet add Kingdom.Api reference Kingdom.Persistence
dotnet sln Kingdom.slnx add Kingdom.Api
```

`dotnet new web` cria um projeto de minimal API. O `Program.cs` que ele faz para você já tem uma linha `MapGet("/", () => "Hello World!")` — você pode rodar agora e visitar <http://localhost:5000> para ver funcionando.

## Passo 1 — seu primeiro endpoint de reino

Substitua o `Kingdom.Api/Program.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Constrói um reino em memória para a demo.
// (O Módulo 3.5 muda isso para um reino por requisição, persistido.)
IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

// GET /kingdom — retorna o resumo como JSON
app.MapGet("/kingdom", () => KingdomJsonStore.ToSummary(kingdom));

app.Run();
```

Três linhas para ir devagar:

- **`WebApplication.CreateBuilder(args)`** — o ponto de partida. Configura o hosting, logging, e de onde vêm as configurações. Te devolve um *builder*. Você chama `Build()` nele para obter o `app` de verdade.
- **`app.MapGet("/kingdom", () => ...)`** — configura uma rota. Quando uma requisição `GET /kingdom` chegar, execute esse lambda. O que quer que ele retorne é transformado em JSON para você.
- **`app.Run()`** — inicia o servidor. Essa linha fica rodando até você parar o programa.

Essa é toda a configuração. Sem XML, nada mais para instalar. Rode:

```powershell
dotnet run --project Kingdom.Api
```

Você vai ver `Now listening on: http://localhost:5xxx` no console. Abra a URL no navegador. O reino aparece como JSON.

> **Agora são dois projetos que rodam — escolhendo o que o F5 inicia.** Durante as Fases 1 e 2, o `Kingdom.Console` era a *única* coisa que podia rodar, então o **F5** simplesmente sabia o que iniciar. Agora o `Kingdom.Api` é um segundo projeto executável na solution, e o F5 precisa que você diga qual deles você quer. Dois jeitos:
>
> - **Pelo terminal (o que o curso usa):** `dotnet run --project Kingdom.Api` nomeia o projeto direto — nunca há dúvida. É por isso que todo comando de rodar nomeia o projeto. Use `--project Kingdom.Console` para rodar o console antigo.
> - **Para depurar com F5:** `Ctrl+Shift+P` → **"Select C# Startup Project"** → escolha `Kingdom.Api` (ou `Kingdom.Console`). Depois disso, o F5 inicia o que você escolheu. Rode o comando de novo quando quiser trocar.

## Passo 2 — tente um cliente de verdade

Abra outro terminal:

```powershell
curl http://localhost:5xxx/kingdom
```

O mesmo JSON. O navegador não importa, o `curl` não importa — todo cliente fala HTTP. *`curl`* (pronuncia como "curl" mesmo) é uma ferramenta de linha de comando que envia requisições HTTP e imprime a resposta para você.

Tente um caminho que não existe:

```powershell
curl -i http://localhost:5xxx/nonsense
```

O `-i` diz ao `curl` para mostrar também os cabeçalhos da resposta. Você vai ver `HTTP/1.1 404 Not Found`. Isso é o ASP.NET lidando com um caminho que ele não reconhece, sem nenhum código extra da sua parte.

## Passo 3 — teste o endpoint

`tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` já existe como placeholder. Adicione um primeiro teste simples que confirma que o projeto compila e o endpoint compila:

```csharp
namespace Kingdom.Api.Tests;

public class SmokeTests
{
    [Fact]
    public void Api_Project_Compiles()
    {
        // O fato de esta classe de teste compilar prova que o projeto de API está referenciado corretamente.
        Assert.True(true);
    }
}
```

O Módulo 3.7 traz testes de integração HTTP reais com `WebApplicationFactory<Program>`.

## Mexa um pouco

Abra sua URL de uma mensagem de chat. Um amigo pode abrir também se você configurar um túnel com `ngrok http 5000`. Não faça o deploy ainda — sem login, qualquer um poderia mudar seus dados.

Adicione um segundo endpoint: `app.MapGet("/", () => "Welcome, traveller.");`. Visite `/` no navegador.

Adicione `app.MapGet("/buildings", () => kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level }));`. O `new { ... }` é um *objeto anônimo* — um objeto rápido sem nome, útil para uma resposta de API de uma vez só.

Pare o servidor com Ctrl+C e tente a URL de novo. O navegador mostra "Connection refused." É assim que você sabe que o seu próprio programa em execução era o que servia a página. Quando não está rodando, nada responde.

## A mesma ideia, de novo

A API é mais uma camada externa. Mesmo motor, uma quarta forma de chegar nele. O `Program.cs` lê as entradas (requisições HTTP), chama o motor e o código de persistência, e envia as saídas (respostas HTTP). O motor em si não mudou nada.

Essa é a terceira camada externa agora: console (Fase 1), persistência (Fase 2 — em certo sentido), web API (esta fase). Ainda o mesmo motor por dentro.

## O que você acabou de fazer

O seu reino agora é acessível via HTTP. Você criou um novo projeto, escreveu uma única linha `MapGet("/kingdom", ...)`, e abriu o seu motor para qualquer cliente no mundo que fale HTTP. Você também viu o modelo de requisição-resposta em forma real — verbo, caminho, status code, body — e viu o ASP.NET lidar com as partes chatas (roteamento 404, transformar seu objeto em JSON, cabeçalhos content-type) sem uma linha de código da sua parte. O motor que você escreveu na Fase 1 não mudou nada; só a camada externa é nova. Um endpoint no ar, muitos mais por vir.

**Conceitos que você já sabe nomear:**

- **HTTP** — verbo mais caminho mais body, com um status code na resposta
- **status codes** — 2xx sucesso, 4xx culpa do cliente, 5xx culpa do servidor
- **`MapGet` / `MapPost` / etc.** — o jeito da minimal-API de registrar rotas
- **`WebApplication.CreateBuilder`** — o ponto de entrada que configura hosting e logging
- **idempotente** — fazer duas vezes tem o mesmo efeito que fazer uma vez

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — escreva um endpoint pequeno da sua própria cabeça e veja ele responder no navegador. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

No seu `Program.cs`, adicione um novo endpoint `GET /hello` que retorna o texto `"Hello, kingdom"`. Um app web pequeno precisa de três coisas — escreva sem olhar:

1. Construa o app.
2. Mapeie a rota.
3. Execute o app.

Depois rode e abra `http://localhost:5xxx/hello` no navegador.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hello", () => "Hello, kingdom");

app.Run();
```

O navegador mostra `Hello, kingdom`. `CreateBuilder` te dá um builder, `Build()` transforma em app, `MapGet` diz "quando uma requisição GET chegar em `/hello`, execute isso", e `Run()` inicia o servidor e fica escutando.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.2 apresenta **DTOs na fronteira da API** — o mesmo padrão da persistência JSON, aplicado à rede. Vamos adicionar um endpoint `POST /kingdom/tick` e ver o reino avançar via HTTP.
