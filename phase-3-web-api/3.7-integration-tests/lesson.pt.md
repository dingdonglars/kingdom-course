# Módulo 3.7 — Testes de Integração com `WebApplicationFactory<T>`

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o projeto de testes inicia a *API inteira* em memória, faz chamadas HTTP reais para ela, e verifica as respostas reais. Sem porta real, sem limpeza à mão — o framework roda o app dentro do processo de teste. Essa é a classe de teste que pega os bugs que os testes unitários não conseguem ver: os que acontecem onde duas partes se encontram. Um cabeçalho content-type errado. Uma rota que compila mas não combina. Um handler de auth configurado na ordem errada.

Até agora seus testes foram *testes unitários* — um método, uma verificação, e rodam em menos de um milissegundo. Testes de integração são a outra metade de uma suíte de testes saudável. Custam mais (cerca de 100ms cada, e mais configuração), mas se pagam na primeira vez que pegam um bug de rota renomeada antes de você fazer o deploy.

> **Words to watch**
>
> - **integration test** — um teste que exercita múltiplos componentes juntos (vs. um unit test, que testa um só)
> - **`WebApplicationFactory<TEntryPoint>`** — helper de teste do ASP.NET Core que inicia o app dentro do processo de teste; `TEntryPoint` é a sua classe `Program`
> - **`HttpClient`** — o teste recebe um cliente real apontando para o servidor em memória
> - **test fixture** — configuração compartilhada entre múltiplos testes; `IClassFixture<T>` no xUnit

---

## Por que testes de integração agora

Testes unitários verificam um método. Testes de integração verificam o caminho inteiro: HTTP → roteamento → handler → store → DB → resposta. Eles pegam os bugs que só acontecem onde duas partes se encontram — um content-type errado, um status code errado, configuração de auth faltando, ou JSON que não combina entre os docs e o código.

O custo é real: testes de integração são mais lentos (cerca de 100ms cada, contra menos de 1ms para testes unitários) e quebram com mais facilidade (uma mudança em qualquer camada pode afetá-los). Você não precisa de muitos: para a maioria das APIs, cinco a dez testes de integração cobrindo os caminhos mais importantes é suficiente. Mantenha-os em uma fixture e compartilhe o servidor entre os testes da classe.

## `WebApplicationFactory` — o que é

O ASP.NET Core vem com um pacote NuGet chamado `Microsoft.AspNetCore.Mvc.Testing`. Referencie-o no seu projeto de teste, e:

```csharp
var factory = new WebApplicationFactory<Program>();
var client = factory.CreateClient();
var response = await client.GetAsync("/kingdoms");
```

Três linhas e toda a API está rodando, dentro do processo de teste, sem porta. O `client` é um `HttpClient` real — faça qualquer requisição que o framework suporte, e você recebe uma resposta real de volta.

## O que vem no starter

- **MODIFICADO:** `tests/Kingdom.Api.Tests/Kingdom.Api.Tests.csproj` — adiciona `Microsoft.AspNetCore.Mvc.Testing`
- **NOVO:** `tests/Kingdom.Api.Tests/IntegrationFixture.cs` — `WebApplicationFactory` compartilhado mais limpeza do DB de teste
- **NOVO:** `tests/Kingdom.Api.Tests/Endpoints_Integration_Tests.cs`

## Passo 0 — instalar o pacote de teste

```powershell
cd tests/Kingdom.Api.Tests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Shouldly
dotnet add reference ..\..\Kingdom.Api\Kingdom.Api.csproj
```

`Program` precisa ser visível para o projeto de teste. A linha `public partial class Program { }` no fundo de `Kingdom.Api/Program.cs` o torna visível — o tipo agora existe na saída compilada do projeto de API com esse nome.

## Passo 1 — fixture

`IntegrationFixture.cs`:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace Kingdom.Api.Tests;

public class IntegrationFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Usa um DB temporário por fixture para cada execução começar limpa
        var dbPath = Path.Combine(Path.GetTempPath(), $"itest-{Guid.NewGuid():N}.db");
        builder.UseSetting("ConnectionStrings:KingdomDb", dbPath);
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Google:ClientId"] = "test-client-id",
                ["Google:ClientSecret"] = "test-client-secret"
            });
        });
    }
}
```

Auth Google real precisaria de um fluxo OAuth real, que não queremos dentro de um teste. Em vez disso, trocamos o esquema de auth por um **esquema de teste** que assina cada requisição como um usuário de teste fixo (os docs do ASP.NET cobrem `TestAuthHandler`). Para esta lição, nossos testes de integração vão chamar os endpoints que *não* precisam de login (`/`, `/openapi/v1.json`).

## Passo 2 — primeiro teste de integração

`Endpoints_Integration_Tests.cs`:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Kingdom.Api.Tests;

public class Endpoints_Integration_Tests : IClassFixture<IntegrationFixture>
{
    private readonly IntegrationFixture _factory;
    private readonly HttpClient _client;

    public Endpoints_Integration_Tests(IntegrationFixture f)
    {
        _factory = f;
        _client = f.CreateClient();
    }

    [Fact]
    public async Task OpenApi_Spec_IsServed()
    {
        var resp = await _client.GetAsync("/openapi/v1.json");
        resp.IsSuccessStatusCode.ShouldBeTrue();
        var json = await resp.Content.ReadAsStringAsync();
        json.ShouldContain("\"openapi\":");      // toda spec tem isso
        json.ShouldContain("/kingdoms");          // nossos endpoints estão registrados
    }

    [Fact]
    public async Task UnauthenticatedKingdomsList_Returns401()
    {
        var resp = await _client.GetAsync("/kingdoms");
        ((int)resp.StatusCode).ShouldBe(401);   // requer auth (Módulo 3.5)
    }

    [Fact]
    public async Task Login_RedirectsToGoogle()
    {
        // O challenge /login deve redirecionar para accounts.google.com.
        // Desabilite auto-redirect para poder inspecionar o próprio 302 em vez de segui-lo.
        using var noFollow = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var resp = await noFollow.GetAsync("/login");

        ((int)resp.StatusCode).ShouldBe(302);
        resp.Headers.Location!.ToString().ShouldStartWith("https://accounts.google.com");
    }
}
```

Rode:

```powershell
dotnet test
```

Os testes de integração iniciam a API dentro do processo de teste, fazem chamadas HTTP reais, e verificam as respostas reais. Sem servidor web, sem porta, sem limpeza à mão.

## Passo 3 — trade-offs

Testes de integração:

- Mais lentos (cerca de 100ms cada)
- Pegam bugs que cruzam várias camadas (roteamento, JSON, configuração de auth)
- Você precisa de menos — cinco a dez cobre os caminhos mais importantes
- Devem dar o mesmo resultado toda vez — use DBs temporários e seeds fixos

Testes unitários:

- Rápidos (menos de um milissegundo)
- Pegam bugs de lógica em um lugar só
- Você precisa de muitos — cerca de um por comportamento
- Fácil de manter dando o mesmo resultado toda vez

Use os dois. Use testes unitários para cobrir muito terreno, e testes de integração para cobrir os lugares onde as partes se encontram.

## Mexa um pouco

Adicione um teste que faz POST `{"name":"Test"}` e verifica que a resposta é `Created` com um cabeçalho `Location:`. (Isso precisa do auth de teste configurado — veja os docs do ASP.NET para `TestAuthHandler`.)

Cronometre `dotnet test --logger "console;verbosity=detailed"`. Note que os testes de integração rodam visivelmente mais lentos que os testes unitários. O custo é real, mesmo sendo pequeno.

Tente renomear uma rota de `/kingdoms` para `/realms` na API. Rode os testes de integração — eles pegam o bug de rota renomeada imediatamente. Esse é o valor.

## O ponto principal

Teste os lugares onde as partes se encontram. Testes unitários provam que cada peça funciona sozinha. Testes de integração provam que as peças funcionam *juntas*. Pule os testes de integração, e você vai continuar recebendo a surpresa *"tudo parecia bem, mas quebrou em produção"*.

## O que você acabou de fazer

Você adicionou testes de integração que iniciam toda a API em memória e a chamam com um `HttpClient` real. Três linhas iniciam a API; cinco a dez testes cobrem os lugares mais importantes onde as partes se encontram — o endpoint OpenAPI, o guarda de login, e o redirecionamento para o Google para login. Você também viu o trade-off claramente: testes de integração rodam cerca de cem vezes mais lentos que testes unitários, então você mantém o número pequeno e deixa os testes unitários cobrir a maior parte do terreno. Juntos eles formam uma suíte de testes contra a qual você pode refatorar sem clicar por cada endpoint à mão.

**Conceitos que você já sabe nomear:**

- **integration test** — testa vários componentes funcionando juntos
- **`WebApplicationFactory<TEntryPoint>`** — inicia seu app dentro do processo de teste para testar
- **`IClassFixture<T>`** — o *compartilhe esta configuração cara entre testes da classe* do xUnit
- **test scheme** — auth falsa que assina cada requisição como um usuário de teste fixo
- **per-fixture temp DB** — cada execução recebe seu próprio DB isolado, gone depois da execução

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — escreva o formato de um teste de integração da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Escreva um `[Fact]` pequeno que inicia toda a API, chama `GET /openapi/v1.json`, e verifica que a resposta é um sucesso. Obter um cliente ativo precisa de três coisas — escreva sem olhar:

1. A factory.
2. O cliente.
3. A requisição.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
[Fact]
public async Task OpenApi_Spec_IsServed()
{
    var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();

    var resp = await client.GetAsync("/openapi/v1.json");

    resp.IsSuccessStatusCode.ShouldBeTrue();
}
```

`WebApplicationFactory<Program>` inicia toda a API dentro do processo de teste — sem porta real, sem servidor para iniciar à mão. `CreateClient()` te dá um `HttpClient` real apontando para ele. Depois você faz uma chamada HTTP normal e verifica a resposta real. Código de teste real mantém a factory numa fixture e a compartilha, já que iniciar o app é a parte lenta.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.7 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.8 é o último módulo de *conteúdo* da Fase 3: **deploy para Azure App Service mais CI/CD com GitHub Actions**. Depois disso, o Módulo 3.9 fecha a Fase 3 com o **AI Unlock** e o marco M4.
