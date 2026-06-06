# Módulo 3.5 — OAuth (Sign In With Google)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje cada requisição pode saber *para quem* ela é. Seu amigo abre a URL, clica em Sign in with Google, e agora a API pode ligar cada save a uma pessoa real. Sem senhas que você precisa guardar, sem passos de confirmar email, sem preocupação tarde da noite sobre como fazer hash de senha com segurança — o Google faz a parte difícil.

Vamos configurar a menor versão correta de login: Sign In With Google mais um cookie de sessão. A API recebe um endpoint `/login` e um `/me`, e vamos marcar os endpoints de reino como precisando de login. No fim desta lição, um cliente que não está logado recebe um 401, e um cliente logado recebe a API completa.

Antes dos detalhes, aqui está toda a dança de uma olhada — três jogadores, passando uma pessoa entre eles:

```text
   [1] Você clica em "Sign in with Google"
            |
            v
   [2] Sua API manda seu navegador para o Google
            |
            v
   [3] Você faz login no Google   (Google cuida de senha + 2FA — nunca seu código)
            |
            v
   [4] Google te manda de volta para sua API com um código curto e de uso único
            |
            v
   [5] Sua API + seu segredo trocam silenciosamente esse código com o Google por um token
            |
            v
   [6] Sua API lê quem você é a partir do token e define um cookie

   Depois disso: seu navegador envia o cookie em cada requisição, então a API
   sabe quem você é — e sua API nunca lidou com uma senha.
```

Os seis passos numerados abaixo são só essa imagem, dita devagar.

> **Words to watch**
>
> - **OAuth 2.0** (oh-oth dois-ponto-zero) — o protocolo para "deixa *outro serviço* (Google) verificar o usuário; me diga *quem* eles são"
> - **OIDC** (OpenID Connect) — uma camada em cima do OAuth 2.0 especificamente para *identidade*, não só acesso
> - **client ID / client secret** — credenciais que *seu app* obtém do Google identificando-o como um app registrado
> - **claim** — um pedaço de informação de identidade que o Google afirma sobre o usuário — `email`, `name`, `sub` (id do sujeito)
> - **cookie auth** — após o login, o servidor define um cookie; o navegador o envia em cada requisição posterior

---

## Por que você nunca deve construir o próprio login

Construir um sistema de usuário+senha+reset-email+2FA *corretamente* é um projeto de seis meses. Até empresas grandes (LinkedIn, Yahoo) vazaram os dados de todos os usuários por causa de um único bug nesse tipo de código. **A primeira regra do login: não construa.** Use um provedor bem testado — Google, Microsoft, GitHub, Auth0, Clerk — e deixe eles carregar o risco.

Para um projeto de aprendizado, **Sign In With Google** é a resposta mais simples que vale a pena fazer:

- Uma biblioteca no lado .NET: `Microsoft.AspNetCore.Authentication.Google`
- Uma configuração de cinco minutos no [Google Cloud Console](https://console.cloud.google.com/) para registrar seu app e obter suas chaves
- Os usuários clicam em um botão, e o Google cuida de email, senha, dois fatores, e recuperação de conta — nada disso é o seu código
- Você recebe de volta um **claim**: *"este é o usuário `sub=12345`, email `lyra@gmail.com`"*

## O fluxo OAuth, em seis passos

1. **Usuário clica em Sign in with Google** no seu site.
2. **Navegador manda o usuário para o Google** com `?client_id=SEU_ID&redirect_uri=SUA_URL&scope=email+profile`.
3. **Usuário faz login no Google** (ou já está logado).
4. **Google manda o usuário de volta para sua URL** com `?code=AUTHCODE`.
5. **Seu servidor troca o código por um ID token** — um JWT (um token assinado que guarda os claims; pronuncia como a letra J + WT). Isso acontece no servidor, usando seu client *secret*. O navegador do usuário nunca vê o secret.
6. **Seu servidor lê os claims** e define um cookie de auth. O usuário agora está logado pelo tempo que o cookie durar.

Os passos 2 ao 5 são feitos para você pelo middleware de autenticação Google do ASP.NET Core. Você escreve quase nenhum código de login você mesmo — instala o pacote, configura, e protege os endpoints com `.RequireAuthorization()`.

## O que vem no starter

A configuração do OAuth tem um lado manual (Google Cloud Console) e um lado de código (pacotes NuGet mais configuração no `Program.cs`).

- **MODIFICADO:** `Kingdom.Api/Kingdom.Api.csproj` — adiciona `Microsoft.AspNetCore.Authentication.Google` mais `Microsoft.AspNetCore.Authentication.Cookies`
- **MODIFICADO:** `Kingdom.Api/Program.cs` — configura auth Google, cookies, e `.RequireAuthorization()` nos endpoints `/kingdoms/*`
- **NOVO:** `Kingdom.Api/appsettings.Development.json` — placeholder para `Google:ClientId` e `Google:ClientSecret`
- **NOVO:** `journal/3.5-google-setup.md` — suas anotações da configuração no Google Cloud Console

A maior parte deste módulo é a configuração no Google Cloud Console, que você faz à mão uma vez.

## Passo 0 — Google Cloud Console

No navegador:

1. Vá para [console.cloud.google.com](https://console.cloud.google.com).
2. Crie um novo projeto (ex: `kingdom-api`).
3. APIs & Services → OAuth consent screen → External → preencha o formulário. Nome do app = "Kingdom"; email de suporte = o seu. Pule a etapa de escopos por agora.
4. APIs & Services → Credentials → Create credentials → OAuth client ID:
   - Tipo de aplicação: Web application
   - URI de redirecionamento autorizado: `https://localhost:7XXX/signin-google` (você vai ver a porta real em `dotnet run --project Kingdom.Api`)
5. Salve. **Copie o Client ID e o Client Secret imediatamente** — o secret não será mostrado para você de novo.

Anote isso em `journal/3.5-google-setup.md`. Mantenha privado — nunca faça commit de secrets!

## Passo 1 — instalar os pacotes

```powershell
cd Kingdom.Api
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.Cookies
```

## Passo 2 — guarde os secrets *fora* do repositório

**Nunca** faça commit de `ClientId` e `ClientSecret` no git. Use `dotnet user-secrets` para desenvolvimento local:

```powershell
cd Kingdom.Api
dotnet user-secrets init
dotnet user-secrets set "Google:ClientId" "SEU_ID.apps.googleusercontent.com"
dotnet user-secrets set "Google:ClientSecret" "SEU_SECRET"
```

Os secrets são salvos em `%APPDATA%/Microsoft/UserSecrets/<id>/secrets.json` — fora do repositório, só na sua máquina. O ASP.NET Core os carrega para você em desenvolvimento.

Para deploy, as variáveis de ambiente `Google__ClientId` e `Google__ClientSecret` funcionam da mesma forma (Azure App Service, ambientes de contêiner, e assim por diante).

> **Cuidado — duas classes com o mesmo nome.** `Microsoft.AspNetCore.Authentication` tem sua própria classe `SystemClock`, e ela tem o mesmo nome que a nossa `Kingdom.Engine.Infrastructure.SystemClock`. Quando você escreve `new SystemClock()` neste arquivo, o compilador C# vê os dois nomes e não consegue dizer qual você quer, então para com um erro. Escreva o nome completo da nossa: `new Kingdom.Engine.Infrastructure.SystemClock()`. É o mesmo tipo de problema que o `global::Kingdom.Engine.Kingdom` do Módulo 1.4.

## Passo 3 — configurar auth no `Program.cs`

```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Somos uma API JSON. O cookie auth por padrão *redireciona* o navegador para
        // uma página de login em falha de auth (302 para /Account/Login). Isso é certo
        // para um site MVC; para uma API, cada requisição não autorizada deve retornar
        // um 401 limpo, não um redirecionamento para um caminho que não existe.
        // Substitua os dois eventos.
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId     = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Login + logout
app.MapGet("/login", () => Results.Challenge(
    new AuthenticationProperties { RedirectUri = "/" },
    [GoogleDefaults.AuthenticationScheme]));

app.MapPost("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok();
});

app.MapGet("/me", (HttpContext ctx) =>
{
    if (ctx.User.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();
    return Results.Ok(new
    {
        // O handler Google do ASP.NET mapeia cada claim do Google para a constante
        // `ClaimTypes` correspondente. Tente o nome curto primeiro, depois o mapeado —
        // versões diferentes de middleware expõem chaves diferentes.
        Email = ctx.User.FindFirst("email")?.Value ?? ctx.User.FindFirst(ClaimTypes.Email)?.Value,
        Name  = ctx.User.FindFirst("name")?.Value  ?? ctx.User.FindFirst(ClaimTypes.Name)?.Value,
        Sub   = ctx.User.FindFirst("sub")?.Value   ?? ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
    });
});

// ... restante dos endpoints /kingdoms do Módulo 3.4 ...
// Adicione .RequireAuthorization() em cada um:
// group.MapGet("/", () => store.ListSlots()).RequireAuthorization();
// (etc.)
```

As partes novas, devagar:

- **`AddAuthentication(...).AddCookie().AddGoogle(...)`** — configura dois esquemas: cookies para "lembra o usuário entre requisições," e Google para "deixa eu passar o login de verdade para o Google."
- **`Results.Challenge(...)`** — inicia o fluxo OAuth. O framework manda o usuário para o Google por você.
- **`ctx.User.FindFirst("email")`** — lê claims do cookie. O usuário está agora identificado e pronto para usar.
- **`.RequireAuthorization()`** — um guarda no endpoint. Sem um cookie de auth válido, o framework retorna `401 Unauthorized` antes do seu handler nem rodar.

## Passo 4 — tente

```powershell
dotnet run --project Kingdom.Api
```

Visite `https://localhost:7XXX/login`. O Google assume → você faz login → você é mandado de volta para `/`. O cookie agora está definido.

Visite `https://localhost:7XXX/me`. Você recebe JSON: `{ "Email": "...", "Name": "...", "Sub": "..." }`.

Visite `https://localhost:7XXX/kingdoms` — funciona, porque o cookie está lá. Agora tente numa janela privada — `401 Unauthorized`, porque não há cookie.

## Mexa um pouco

Faça login. Abra as ferramentas de desenvolvimento do navegador → Application → Cookies. Você vai ver uma entrada `.AspNetCore.Cookies` com um valor longo e embaralhado. Esse é o cookie de auth.

POST `/logout` (use Postman ou `curl -X POST -b cookies.txt`). O cookie é apagado. `/me` agora retorna 401.

Tente uma política de autorização personalizada. Adicione `.RequireAuthorization("Admin")` em algum lugar, e defina a política em `AddAuthorization` para exigir um claim específico — por exemplo, `email == "você@gmail.com"`.

**Não faça deploy sem HTTPS.** Cookie auth sobre HTTP simples não é seguro — alguém na rede pode ler o cookie. Para desenvolvimento local, `https://localhost:...` já está configurado; em produção, o App Service cuida disso para você.

## O ponto principal

Duas regras, lado a lado: **nunca faça commit de secrets, nunca construa seu próprio login.** Juntas elas param cerca de metade de todos os problemas reais de segurança. Use um provedor. Use user-secrets ou variáveis de ambiente. Nunca coloque secrets no repositório. Esses dois hábitos são fáceis de seguir e protegem muito. São o que separa um projeto hobby de um que você pode deixar outras pessoas usarem com segurança.

## O que você acabou de fazer

Você adicionou login real à sua API sem escrever uma única linha que lida com senhas. Cinco linhas de configuração `AddAuthentication / AddCookie / AddGoogle` conectaram o fluxo OAuth completo do Google ao seu app. Seus endpoints agora podem dizer *qual conta Google* enviou cada requisição, pelo claim `sub`. Os secrets ficam fora do repositório — em `dotnet user-secrets` para desenvolvimento, e variáveis de ambiente para produção. Você também viu seu segundo exemplo de duas classes com o mesmo nome: `Microsoft.AspNetCore.Authentication.SystemClock` e `Kingdom.Engine.Infrastructure.SystemClock` parecem idênticas para o compilador a menos que você escreva o nome completo da sua. O mesmo tipo de problema que o `global::Kingdom.Engine.Kingdom` no Módulo 1.4.

**Conceitos que você já sabe nomear:**

- **OAuth 2.0** — protocolo para passar verificação de identidade para outro serviço
- **OIDC** — a camada de identidade em cima do OAuth
- **claim** — chave/valor afirmado pelo provedor de identidade sobre o usuário
- **cookie auth** — servidor define um cookie de sessão; navegador o envia em cada requisição
- **`.RequireAuthorization()`** — guarda de endpoint que retorna 401 se o cookie está faltando
- **`dotnet user-secrets`** — armazenamento de secrets só local; nunca no repositório

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — explique o fluxo de login da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

No papel:

1. Descreva o que acontece quando um usuário clica em "Sign in with Google", desde o clique até o momento em que está logado — só a ordem: quem manda o usuário para onde, e onde o secret é usado.
2. Depois escreva as duas regras de segurança que este módulo repetiu.

<details><summary>Travou? Abra aqui para conferir.</summary>

O fluxo, em ordem:

1. Usuário clica em **Sign in with Google** no seu site.
2. O navegador manda o usuário para o **Google** (com seu client id, sua URL de redirecionamento, e os escopos que você quer).
3. O usuário faz login no Google (ou já está logado).
4. O Google manda o usuário de volta para **sua URL** com um código de uso único.
5. **Seu servidor** troca esse código por um ID token, usando seu client *secret*. O navegador nunca vê o secret.
6. Seu servidor lê os claims (`email`, `name`, `sub`) e define um cookie de auth. Agora o usuário está logado.

As duas regras: **nunca construa seu próprio login** (use um provedor), e **nunca faça commit de secrets** (use `dotnet user-secrets` para desenvolvimento, variáveis de ambiente para produção). Os passos 2 ao 5 são feitos para você pelo middleware Google do ASP.NET Core — você principalmente só configura e adiciona `.RequireAuthorization()` nos endpoints que precisam de um usuário logado.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.5 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.6 pega o *usuário logado* e vincula cada reino que ele cria ao `sub` dele (id de usuário Google). Múltiplos usuários, cada um com seus próprios reinos, consultáveis. Persistência multi-usuário de verdade.
