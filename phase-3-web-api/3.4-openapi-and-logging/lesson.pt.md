# Módulo 3.4 — OpenAPI e Logging Estruturado

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

A API funciona, mas agora a única forma de outra pessoa aprender o que ela faz é ler o seu código-fonte. Hoje a API ganha dois recursos que serviços reais têm. **OpenAPI** escreve a documentação para você — visite `/swagger` (ou `/scalar/v1`) e uma página web deixa qualquer pessoa explorar os endpoints, enviar requisições de teste, e ver respostas reais. **Logging** escreve um registro organizado de cada requisição — útil para consertar bugs agora, e útil daqui a seis meses quando algo der errado em produção e você tiver que descobrir o que aconteceu.

Os dois recursos são baratos de adicionar hoje e muito caros de adicionar depois, quando a API estiver no ar e algo estiver quebrado. A gente os adiciona agora enquanto a API ainda é pequena o suficiente para entender do começo ao fim.

> **Words to watch**
>
> - **OpenAPI** — um formato padrão de descrição para APIs HTTP (antes chamado Swagger; os dois nomes ainda aparecem)
> - **Swagger UI / Scalar** — páginas HTML interativas geradas a partir de uma spec OpenAPI
> - **structured logging** — entradas de log com campos nomeados, não só strings de texto
> - **`ILogger<T>`** — a interface de logger do ASP.NET Core; você pede como parâmetro e o framework fornece

---

## Por que OpenAPI

Sem OpenAPI, a única forma de aprender sua API é ler o código-fonte ou o seu README. Cada equipe de cliente tem que adivinhar como funciona. Eles adivinham errado, e a chamada errada vai parar em produção.

Com OpenAPI, há uma descrição que uma máquina pode ler em `/openapi/v1.json`. A Swagger UI em `/swagger` mostra cada endpoint: seus parâmetros, o layout da requisição e da resposta, e os status codes que pode retornar. Qualquer pessoa pode chamar sua API em dez segundos sem ler uma linha de C#.

No .NET 9 e posteriores, OpenAPI é uma linha de cada lado: `builder.Services.AddOpenApi();` mais `app.MapOpenApi();`.

## Por que logging estruturado

`Console.WriteLine($"User {userId} did action {action}")` é logging de *string*. A entrada de log é só texto sem estrutura — para encontrar "todas as ações do usuário 42," você tem que procurar no arquivo à mão.

Logging **estruturado**:

```csharp
_log.LogInformation("User {UserId} did action {Action}", userId, action);
```

A entrada de log agora tem campos nomeados (a forma exata depende de onde os logs vão): `{ "msg": "User did action", "UserId": 42, "Action": "tick" }`. Para encontrar toda ação do usuário 42, você roda uma consulta: `WHERE UserId = 42`. Com dez mil entradas de log, essa é a diferença entre horas de busca e alguns segundos.

`ILogger<T>` é o jeito padrão de fazer isso. Peça como parâmetro, depois chame `LogInformation`, `LogWarning`, `LogError`. O framework e o lugar onde você manda os logs (console, arquivo, Application Insights, Seq) cuidam do resto.

## O que vem no starter

- **MODIFICADO:** `Kingdom.Api/Kingdom.Api.csproj` — adiciona o pacote `Microsoft.AspNetCore.OpenApi`
- **MODIFICADO:** `Kingdom.Api/Program.cs` — adiciona registro do OpenAPI mais o endpoint, e injeção de `ILogger` em dois handlers
- As saídas de log no console e debug já estão configuradas por padrão nos templates do ASP.NET Core.

## Passo 0 — instalar o OpenAPI

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.OpenApi
```

## Passo 1 — configurar o OpenAPI

No `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();              // <-- adiciona serviços OpenAPI

var app = builder.Build();

app.MapOpenApi();                           // <-- expõe /openapi/v1.json
```

Rode, depois visite `http://localhost:5xxx/openapi/v1.json`. Essa é toda a sua API, descrita como JSON.

Para uma página clicável, adicione **Scalar** — uma alternativa mais nova e leve à Swagger UI mais antiga:

```powershell
dotnet add package Scalar.AspNetCore
```

```csharp
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();            // serve /scalar/v1
```

Visite `http://localhost:5xxx/scalar/v1`. Você recebe docs em que pode clicar. Escolha qualquer endpoint, preencha os parâmetros, e clique em Try It.

O pacote mais antigo `Swashbuckle.AspNetCore` mostra a Swagger UI clássica em `/swagger`. Os dois funcionam; escolha o que você preferir.

## Passo 2 — adicionar `ILogger`

Num handler de minimal-API, você pode obter um `ILogger<Program>` (ou qualquer tipo de classe) só adicionando como parâmetro — o container de injeção de dependência o fornece de graça:

```csharp
group.MapPost("/", (CreateKingdomRequest req, ILogger<Program> log) =>
{
    if (string.IsNullOrWhiteSpace(req.Name))
    {
        log.LogWarning("CreateKingdom called with empty name from {RemoteIP}",
            "(unknown)");   // O Módulo 3.6 vai injetar o IP real
        return Results.BadRequest(new { error = "Name is required." });
    }

    var k = new Kingdom.Engine.Kingdom(req.Name.Trim(), rng, clock);
    var id = store.Save(k);
    log.LogInformation("Created kingdom {KingdomId} '{KingdomName}'", id, k.Name);
    return Results.Created($"/kingdoms/{id}", new KingdomCreated(id, k.Name));
});
```

Duas chamadas de log — um warning, um information. Rode, crie um reino, veja o console:

```
warn: Program[0]
      CreateKingdom called with empty name from (unknown)
info: Program[0]
      Created kingdom 1 'Eldoria'
```

O `{KingdomId}` e `{KingdomName}` são *espaços reservados*. Ferramentas como Serilog, Application Insights, ou Seq os salvam como campos nomeados, em vez de só soltá-los numa linha de texto. Em produção, essa é a diferença entre procurar arquivos de log à mão e rodar uma consulta.

Níveis, do menos para o mais grave:

- `LogTrace` — muito detalhado, pequenos detalhes
- `LogDebug` — só para desenvolvimento
- `LogInformation` — eventos normais
- `LogWarning` — algo está errado, mas o app continua
- `LogError` — algo falhou, e o usuário pode ver
- `LogCritical` — tudo está quebrado

## Passo 3 — configuração

`appsettings.json` (já no projeto):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

`Microsoft.AspNetCore` está em `Warning` para as mensagens do próprio framework não enterrarem as suas. Ajuste como quiser.

## Mexa um pouco

Visite `/scalar/v1`, clique em `POST /kingdoms`, clique em Try It, preencha `{ "name": "Test" }`, e veja o 201 e o cabeçalho `Location` voltarem. Qualquer pessoa pode fazer isso — incluindo você daqui a seis meses, quando já tiver esquecido o layout exato da API.

Adicione `app.UseSerilogRequestLogging();` depois de instalar `Serilog.AspNetCore`. Cada requisição agora recebe uma única linha de log com o método, o caminho, o status, e quanto tempo demorou. Muita visibilidade para uma linha só.

Defina `LogLevel.Debug` para um handler. Veja quanto mais ele imprime. Depois note isso: quando `LogLevel.Default = Information`, as suas chamadas `LogDebug` não aparecem. O filtro de nível roda *antes* da mensagem ser construída.

Adicione a versão da minimal-API de `[ProducesResponseType(StatusCodes.Status201Created)]` — `Produces<T>(...)` — para a descrição OpenAPI listar os status codes que cada endpoint pode retornar.

## O ponto principal

Deixe sua API fácil de ler e fácil de observar desde o primeiro dia. OpenAPI descreve o *layout* de cada endpoint. Logging estruturado registra o que realmente *acontece*. Os dois são baratos de adicionar cedo e muito difíceis de adicionar depois, quando a API estiver no ar e algo estiver quebrado.

## O que você acabou de fazer

Você transformou sua API de *um conjunto de endpoints funcionando* para *um serviço documentado e fácil de observar*. Três novas linhas adicionaram a descrição OpenAPI, a Scalar UI, e logs estruturados aos seus handlers. Qualquer pessoa pode agora abrir `/scalar/v1` e explorar sua API sem ler uma linha de C#. Cada requisição que você trata deixa um registro com campos nomeados que ferramentas de log reais podem consultar. Essas são duas habilidades que o mercado levou anos para consolidar, e você as tem desde o início da vida da sua API.

**Conceitos que você já sabe nomear:**

- **OpenAPI** — o formato spec descrevendo uma API em JSON ou YAML
- **Swagger UI / Scalar** — docs HTML interativos gerados a partir dessa spec
- **`AddOpenApi` / `MapOpenApi`** — o gerador OpenAPI embutido do .NET, sem pacote de terceiros
- **`ILogger<T>`** — um logger que o framework fornece; o tipo `T` nomeia a classe chamadora, usado para filtragem
- **structured logging** — entradas de log com campos nomeados, consultáveis depois

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — escreva uma linha de log estruturado da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Imagine que você tem um logger chamado `log` e dois valores: um `int id` e uma `string name`. Sem olhar:

1. Escreva uma chamada `LogInformation` que registra "deletou um reino" *e* mantém o id e o nome como campos nomeados (não texto colado na frase).
2. Depois diga, em uma frase, por que a forma de campo nomeado ganha do `Console.WriteLine`.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
log.LogInformation("Deleted kingdom {KingdomId} '{KingdomName}'", id, name);
```

O `{KingdomId}` e `{KingdomName}` são espaços reservados, e os valores vêm depois como argumentos. Uma ferramenta de log os salva como campos nomeados, para você poder rodar depois uma consulta como `WHERE KingdomId = 42` em vez de procurar num arquivo de texto à mão. Com dez mil linhas de log, essa é a diferença entre segundos e horas.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.5 apresenta **OAuth via Google** — deixando usuários fazer login. Auth do mundo real é um tema grande, então fazemos a versão menor que ainda é correta (Google Sign-In mais cookie auth). Depois do 3.5, cada requisição pode saber *qual usuário* ela é.
