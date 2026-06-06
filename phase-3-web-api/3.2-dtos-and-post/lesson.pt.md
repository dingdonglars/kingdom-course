# Módulo 3.2 — DTOs na Fronteira da API e `POST /kingdom/tick`

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Ontem o seu reino podia ser lido via HTTP. Hoje ele pode *mudar* via HTTP. Uma requisição `POST /kingdom/tick` avança um dia, e a resposta mostra o novo estado. O reino agora responde à rede do mesmo jeito que respondia ao teclado na Fase 1 — mesmo motor, mesmo método `AdvanceDay`, só um caller diferente pedindo por ele.

Enquanto estamos aqui, vamos deixar clara a regra que encontramos pela primeira vez na Fase 2: **os dados que você envia pela rede são um DTO, não o tipo do motor.** O motor retorna objetos com estado escondido e construtores que precisam de um `IRandom`. A API retorna tipos `record` pequenos que viram JSON sem problema. A mesma lição, pela segunda vez.

> **Words to watch**
>
> - **request DTO** — o layout dos dados que o cliente envia no body da requisição
> - **response DTO** — o layout dos dados que o servidor retorna no body da resposta
> - **`Results.Ok(...)` / `Results.NotFound()`** — helpers da minimal-API que deixam você escolher o status code você mesmo
> - **`[FromBody]`** — o atributo que diz *leia este parâmetro do JSON da requisição*; as minimal APIs adicionam isso para você em parâmetros do tipo record

---

## Por que DTOs de novo

A gente viu DTOs no Módulo 2.2 (persistência JSON). O mesmo raciocínio se aplica na fronteira da API, e importa ainda mais aqui. Os dados que você envia pela rede precisam:

- Virar JSON sem problema — sem construtores que precisam de `IRandom`, sem métodos virtuais
- Ficar *estáveis* mesmo quando o motor muda — adicionar um campo privado não deve quebrar um cliente
- Ficar *pequenos* — retorne só o que o cliente realmente precisa (isso manda menos bytes)
- Ficar *claros* — toda propriedade que o cliente pode ver deve estar lá de propósito

`KingdomSummary` (do Módulo 2.2) já funciona para `GET /kingdom`. Hoje adicionamos `TickResponse` para o novo endpoint.

## O que vem no starter

- **NOVO:** `Kingdom.Api/Dtos/TickResponse.cs`
- **MODIFICADO:** `Kingdom.Api/Program.cs` — adiciona `POST /kingdom/tick` e o parâmetro opcional de dias para avançar
- **NOVO:** `tests/Kingdom.Api.Tests/Endpoint_GET_Kingdom_Tests.cs` — smoke test; testes de integração reais vêm no Módulo 3.7

## Passo 1 — o DTO `TickResponse`

`Kingdom.Api/Dtos/TickResponse.cs`:

```csharp
namespace Kingdom.Api.Dtos;

/// <summary>O que a API retorna após um tick.</summary>
public record TickResponse(
    int DaysAdvanced,
    string KingdomName,
    int CurrentDay,
    int Gold, int Wood, int Stone, int Food
);
```

Um record pequeno e claro. O cliente sabe exatamente o que esperar — sem surpresas no JSON, e sem risco de um campo escondido do motor aparecer por acidente.

## Passo 2 — o endpoint `POST /kingdom/tick`

Substitua o `Kingdom.Api/Program.cs`:

```csharp
using Kingdom.Api.Dtos;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;
using Kingdom.Persistence;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IRandom rng = new SystemRandom(seed: 42);
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, new SystemClock());
kingdom.AddBuilding(new Farm("Main Farm"));
kingdom.AddBuilding(new Lumberyard("Eastern Lumberyard"));
kingdom.AddCitizen(new Citizen("Lyra"));
for (int i = 0; i < 10; i++) kingdom.AdvanceDay();

// GET /kingdom — lê o estado
app.MapGet("/kingdom", () => KingdomJsonStore.ToSummary(kingdom));

// POST /kingdom/tick — avança um ou mais dias; retorna o novo estado
app.MapPost("/kingdom/tick", (int? days) =>
{
    var n = Math.Clamp(days ?? 1, 1, 100);   // recusa zero ou números absurdos
    for (int i = 0; i < n; i++) kingdom.AdvanceDay();

    return Results.Ok(new TickResponse(
        DaysAdvanced: n,
        KingdomName: kingdom.Name,
        CurrentDay:  kingdom.Day,
        Gold:  kingdom.Resources.Get(Resource.Gold),
        Wood:  kingdom.Resources.Get(Resource.Wood),
        Stone: kingdom.Resources.Get(Resource.Stone),
        Food:  kingdom.Resources.Get(Resource.Food)));
});

app.Run();
```

As partes novas, devagar:

- **`(int? days)`** — os parâmetros da minimal-API são combinados por nome. `days` é opcional (o `?` deixa ser null), e o framework procura `?days=N` na query string. Para tipos maiores, os dados vêm do body da requisição via `[FromBody]`, que as minimal APIs adicionam para você em parâmetros do tipo record.
- **`Math.Clamp(days ?? 1, 1, 100)`** — verificando a entrada em uma linha. Se `days` é null, usa 1. Caso contrário mantém o valor entre 1 e 100. Isso bloqueia o tick de 1000 dias que congelaria o servidor por um minuto.
- **`Results.Ok(value)`** — retorna `200 OK` você mesmo, com `value` como o body. Tem também `Results.NotFound()`, `Results.BadRequest("msg")`, `Results.Created(uri, value)`, e alguns outros. Use esses quando quiser escolher o status code. Caso contrário, só dê `return value` e o framework escolhe 200 para você.

Build, rode, tente:

```powershell
dotnet build
dotnet run --project Kingdom.Api
# em outro terminal
curl -X POST http://localhost:5xxx/kingdom/tick
curl -X POST "http://localhost:5xxx/kingdom/tick?days=5"
curl http://localhost:5xxx/kingdom
```

Cada tick muda o estado. O `GET` mostra o novo dia.

## Passo 3 — o que o framework faz por você

Note tudo o que você *não* escreveu:

- Ler o JSON da requisição (nenhum aqui — `int?` é um número simples)
- Transformar a resposta em JSON (`TickResponse` vira JSON automaticamente)
- Definir o status code (`Results.Ok` vira 200; 404 se nenhuma rota combinar)
- Escolher o content type (a resposta recebe `application/json; charset=utf-8` de graça)
- Roteamento — `/kingdom/tick` acha o handler certo sozinho

Isso é o que um framework te dá em vez de escrever tudo do zero. Você escreve a parte especial do seu app, e o framework cuida da configuração repetitiva.

## Passo 4 — testes

Por agora, um primeiro teste simples que confirma que o projeto compila. Testes de integração reais com `WebApplicationFactory<Program>` chegam no Módulo 3.7.

`tests/Kingdom.Api.Tests/Endpoint_GET_Kingdom_Tests.cs`:

```csharp
using Kingdom.Api.Dtos;
using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoint_GET_Kingdom_Tests
{
    [Fact]
    public void TickResponse_Record_HasExpectedProperties()
    {
        // Verificação em tempo de compilação que o DTO corresponde ao que o cliente espera
        var tr = new TickResponse(1, "X", 2, 100, 50, 20, 30);
        tr.DaysAdvanced.ShouldBe(1);
        tr.CurrentDay.ShouldBe(2);
    }
}
```

Um teste de verdade chamaria `WebApplicationFactory<Program>().CreateClient()` e faria um POST para o endpoint. Guardamos isso para o Módulo 3.7 — há muita configuração nova para aprender de uma vez só.

## Mexa um pouco

Tente `?days=10000` e veja o clamp te manter seguro. Comente o clamp e tente de novo — o servidor ainda responde, mas a chamada demora um momento que você consegue sentir. É por isso que a gente usa o clamp.

Adicione um endpoint `GET /kingdom/buildings` retornando `kingdom.Buildings.Select(b => new { b.Name, Kind = b.GetType().Name, b.Level })`. O objeto anônimo vira JSON sem você fazer nada a mais.

Adicione `GET /healthz` retornando `Results.Ok("ok")`. Esse é o jeito comum de perguntar *"o servidor está vivo?"* — load balancers e ferramentas de monitoramento chamam um endpoint assim para checar que o programa ainda está rodando.

Tente `curl -i -X POST http://localhost:5xxx/kingdom/tick`. O `-i` mostra os cabeçalhos da resposta — `Content-Type: application/json` é definido para você, o cabeçalho `Date` é definido para você, e assim por diante.

## Verifique a entrada na fronteira

Entradas em que você não pode confiar chegam na camada externa. Limite-as, verifique-as, e rejeite os valores ruins — tudo *antes* que o motor as veja. O motor nunca deve ter que se proteger contra os erros do caller. É por isso que `Math.Clamp` fica no handler, não dentro de `AdvanceDay`.

## O que você acabou de fazer

O seu reino agora lê *e* escreve via HTTP. Você escreveu um DTO `TickResponse` — um record pequeno e claro feito para enviar pela rede — e um handler `POST /kingdom/tick` que avança o reino por um ou mais dias e retorna o novo estado. O handler verifica a entrada com `Math.Clamp` para que uma requisição de mil dias não possa congelar o servidor. Você também conheceu `Results.Ok(...)` e viu como o framework transforma objetos em JSON, define status codes, e adiciona cabeçalhos content-type sem você escrever uma linha para nada disso.

**Conceitos que você já sabe nomear:**

- **DTO na fronteira da API** — record pequeno e claro feito para enviar pela rede
- **`Results.Ok` / `NotFound` / `BadRequest`** — controla o status code a partir de um handler
- **parâmetro de query opcional** — `(int? days)` vira `?days=5` na URL
- **verificar a entrada na fronteira** — limite, verifique, rejeite antes do motor rodar

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — escreva um DTO de resposta pequeno da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo. Imagine que a API precisa retornar um prédio pela rede. Sem olhar:

1. Escreva um `record` chamado `BuildingResponse` com o `Name` do prédio, seu `Kind` (uma string como "Farm"), e seu `Level` (um número) — só o que um cliente precisa.
2. Faça o build do projeto para checar que compila.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
namespace Kingdom.Api.Dtos;

public record BuildingResponse(string Name, string Kind, int Level);
```

Uma linha curta resolve. Um DTO é só um `record` pequeno e claro feito para enviar pela rede — sem estado escondido do motor, sem construtores que precisam de `IRandom`. Vira JSON sem problema, e o cliente sabe exatamente o que vai receber.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.3 apresenta **vários endpoints com os status codes certos** — `POST /kingdoms` para criar um, `DELETE /kingdoms/{id}` para remover um, mais 404s e 400s quando algo não encaixa. Isso é CRUD via HTTP, o padrão por trás de toda API web já construída.
