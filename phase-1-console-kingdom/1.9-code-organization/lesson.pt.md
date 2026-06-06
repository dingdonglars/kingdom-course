# Módulo 1.9 — Code Organisation

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.8:
>
> 1. O engine recebe `IRandom` e `IClock` pelo construtor em vez de criá-los ele mesmo. Como isso se chama?
> 2. Como entregar um `IRandom` *fake* ao engine deixa um teste confiável?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.8** — a aula de hoje fica bem em cima dele. Leve qualquer coisa que ficou frágil para o sync semanal.

`Kingdom.Engine/` agora tem quatorze arquivos todos em uma pasta. Já está ficando difícil de ler. Hoje não adicionamos recursos — movemos coisas. Pastas por tópico, sub-namespaces, e uma passagem de nomes pequena. O reino *funciona* exatamente igual; o *código* fica muito mais fácil de ler.

Por que organizar *agora*, antes de o engine ficar enorme? Pelo mesmo motivo que você arruma a mesa antes de ela ficar enterrada: é rápido quando há pouco, e lento quando há muito. Uma regra útil: assim que uma pasta tem mais de uns sete arquivos, é hora de pensar em subpastas. Uma pasta plana está bem por um tempo, e então um dia não está mais.

> **Words to watch**
>
> - **sub-namespace** — `Kingdom.Engine.Events` é um sub-namespace de `Kingdom.Engine`. Mesmo projeto, um grupo separado dentro dele.
> - **using directive** — a linha `using Kingdom.Engine.Events;` que traz nomes de um namespace para o escopo
> - **folder** — organização pura; o compilador não liga para pastas, só para namespaces. A gente os alinha na mão, por convenção.
> - **aggregate root** — a classe que possui tudo o mais em um modelo. `Kingdom` possui seus prédios, cidadãos, recursos. *(Primeiro uso: esta aula.)*

---

## O novo layout

```
Kingdom.Engine/
├─ Kingdom.cs              ← the aggregate root stays at top level
├─ Kingdom.Engine.csproj
├─ Buildings/
│   ├─ Building.cs
│   ├─ Farm.cs
│   ├─ Lumberyard.cs
│   └─ Mine.cs
├─ Citizens/
│   └─ Citizen.cs
├─ Resources/
│   ├─ Resource.cs
│   └─ ResourceLedger.cs
├─ Events/
│   ├─ KingdomEvent.cs     (and the three subclass records)
│   └─ EventEngine.cs
└─ Infrastructure/
    ├─ IRandom.cs
    ├─ SystemRandom.cs
    ├─ IClock.cs
    └─ SystemClock.cs
```

`Kingdom.cs` fica no topo porque é o **aggregate root** — a classe que une tudo e possui os outros. (Você vai encontrar esse termo em conversas de design também: o aggregate root é o único ponto de entrada pelo qual todas as mudanças no modelo passam.) As cinco subpastas cada uma cobre uma área: prédios, cidadãos, recursos, eventos, e infraestrutura (o código de suporte entediante).

## Sub-namespaces

Cada subpasta recebe um sub-namespace combinando:

```csharp
// Buildings/Farm.cs
namespace Kingdom.Engine.Buildings;

public class Farm : Building { ... }
```

```csharp
// Events/EventEngine.cs
namespace Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;

public class EventEngine { ... }
```

Dois efeitos. Primeiro, **é mais fácil achar as coisas** — digitar `Kingdom.Engine.B` no editor imediatamente sugere `Buildings.Farm`, `Buildings.Lumberyard`, `Buildings.Mine`. Sem sub-namespaces, você veria os quatorze tipos de uma vez. Segundo, **mostra para que cada classe serve** — uma classe em `Kingdom.Engine.Infrastructure` está te dizendo *"esse é código de suporte, não as regras do reino."* Qualquer um lendo o engine sabe onde procurar o quê.

## O custo

Todo arquivo que usa um tipo de outro sub-namespace precisa de uma linha `using`. `EventEngine` agora precisa de `using Kingdom.Engine.Buildings;`, `using Kingdom.Engine.Citizens;`, e `using Kingdom.Engine.Infrastructure;`. São três novas linhas. Assuma esse custo quando seu engine tiver crescido para um tamanho médio; não divida um projeto de quatro arquivos cedo por nenhuma razão.

## Passo 1 — aplique o movimento

Duas opções, mesmo estado final.

**Opção A — copie do `starter/` deste módulo.** O starter deste módulo é uma *cópia completa* de toda a pasta, não só as partes mudadas. Substitua toda a sua pasta `Kingdom.Engine/` pela do `starter/Kingdom.Engine/`. Faça o mesmo para o projeto de testes (só as linhas `using` mudam lá).

**Opção B — faça o movimento você mesmo.** No seu editor, selecione cada arquivo, clique com o botão direito → Mover para pasta, e escolha a pasta alvo. Depois atualize a linha `namespace` de cada arquivo. Depois adicione linhas `using` onde necessário (o compilador te mostra com sublinhados vermelhos). Isso é próximo de como os movimentos acontecem em projetos de verdade, então é bom praticar.

De qualquer forma, não mude o que o código *faz*. Mesmos prédios, mesmos eventos, mesmos testes.

## Passo 2 — atualize o `Kingdom.cs`

`Kingdom.cs` (o arquivo) agora precisa de diretivas `using` porque os tipos que ele usa vivem em outro lugar:

```csharp
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Kingdom.Engine.Resources;

namespace Kingdom.Engine;

public class Kingdom { ... unchanged ... }
```

## Passo 3 — atualize o `Program.cs`

`Program.cs` adiciona um conjunto mais amplo de `using`s (ou, para mantê-lo mais limpo, os traz com *global usings* — veja "Mexa um pouco"):

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;

IRandom rng = new SystemRandom(seed: 42);
IClock clock = new SystemClock();
var kingdom = new Kingdom.Engine.Kingdom("Eldoria", rng, clock);
kingdom.AddBuilding(new Farm("Main Farm"));
// ... rest unchanged
```

## Passo 4 — atualize os arquivos de teste

Cada arquivo de teste recebe o `using` certo para os tipos que usa. Por exemplo, `EventEngineTests.cs`:

```csharp
using FakeItEasy;
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Events;
using Kingdom.Engine.Infrastructure;
using Shouldly;
```

Muitos usings. Esse é o custo da estrutura. O benefício é que o bloco `using` de cada arquivo agora funciona como um mapa — ele diz *"esse código usa prédios, cidadãos, eventos, e infraestrutura."*

## Passo 5 — verifique

Nada mudou sobre o que o código faz. Rode:

```powershell
dotnet build
dotnet test
```

Você ainda deve ver 35 passando — igual a antes. Se qualquer coisa falhar, significa que você moveu algo errado, não que o código em si quebrou. Leia o erro; geralmente é um `using` faltando.

## Mexa um pouco

**Global usings.** Adicione um arquivo `Kingdom.Engine/GlobalUsings.cs` com:

```csharp
global using Kingdom.Engine.Buildings;
global using Kingdom.Engine.Citizens;
global using Kingdom.Engine.Resources;
global using Kingdom.Engine.Infrastructure;
global using Kingdom.Engine.Events;
```

Agora os arquivos dentro de `Kingdom.Engine` não precisam repetir esses usings — eles se aplicam em todo lugar. Menos bagunça. Use isso com cuidado, porém: global usings escondem de onde cada tipo vem. São bons para sub-namespaces próprios do projeto, mas não bons para bibliotecas externas.

Tente mover `Kingdom.cs` *para dentro de* `Buildings/`. O compilador fica bem, mas é um erro claro — `Kingdom` não é um prédio. Mova de volta. O layout de pastas deve combinar com o modelo.

Torne `EventEngine` `internal` em vez de `public`. O compilador fica feliz, porque `EventEngine` só é usado de dentro do engine. Agora o código fora do engine (o console, os testes) não pode usá-lo diretamente. Isso é encapsulation no nível de projeto inteiro — tipos `internal` são visíveis só dentro do mesmo projeto.

Tente tornar o campo `_eventEngine` em `Kingdom` `protected` em vez de `private`. O compilador aceita, mas agora qualquer classe que herda de `Kingdom` pode alcançá-lo. Mantenha-o `private` — `Kingdom` não tem subclasses hoje, e não vai ter.

## A linha-guia

A linha-guia neste módulo: **agrupe pastas pelo que o código é sobre, não pelo tipo de arquivo que é**. Mantenha `Farm`, `Lumberyard`, `Mine`, `Building` juntos (todos "prédios"), em vez de colocar `Farm.cs`, `FarmTests.cs`, `FarmDocs.md` juntos. O leitor se importa com a *ideia* primeiro. Você vai ver essa mesma regra usada em escala maior na Fase 3 (`Server/Controllers/`, `Server/Services/`) e Fase 4 (`Web/Components/Buildings/`, `Web/Components/Citizens/`).

## O que você acabou de fazer

Você moveu quatorze arquivos para cinco subpastas, deu a cada subpasta um sub-namespace combinando, e adicionou as linhas `using` para tipos usados entre elas. Nada sobre o reino mudou, mas muito sobre como o código se lê mudou. Você conheceu a ideia do **aggregate root** (a classe no topo que possui o modelo — Kingdom, aqui) e o modificador de acesso **internal** (visível só dentro do mesmo projeto). Todos os trinta e cinco testes ainda passaram no fim, que era a única prova que importava.

**Conceitos que você já sabe nomear:**

- **sub-namespace** — mesmo projeto, caminho de namespace mais profundo
- **folder = namespace** — convenção, alinhada na mão
- **`global using`** — diretiva aplicada a todo arquivo do projeto
- **`internal` vs `public`** — visível para o projeto vs visível em todo lugar
- **aggregate root** — classe do topo que possui o modelo

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: uma pasta e seu sub-namespace se alinham na mão. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem olhar, faça isso de memória no seu engine:

1. Crie uma nova subpasta chamada `Trade/`.
2. Adicione um arquivo de classe vazio `Caravan.cs` dentro dela.
3. Escreva a linha `namespace` certa no topo desse arquivo para que combine com a pasta.
4. De volta em `Kingdom.cs`, imagine a linha nova que você precisaria se `Kingdom` quisesse usar `Caravan`.
5. Faça build para verificar que o arquivo compila.

<details><summary>Travou? Abra aqui para conferir.</summary>

- O arquivo em `Trade/` recebe o namespace que combina com a pasta:

  ```csharp
  namespace Kingdom.Engine.Trade;

  public class Caravan { }
  ```

- Para que `Kingdom.cs` use `Caravan`, ele precisaria de uma linha `using`: `using Kingdom.Engine.Trade;`. O compilador não liga para pastas — só namespaces. A gente alinha os dois na mão, por convenção, para que o layout seja fácil de ler.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.9 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.9 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.10 é o módulo de **polish + resgate de repositório** — README, comentários, passagem de nomes, mais o fluxo de trabalho *"se seu repositório está destruído, resgate-o de um estado conhecido-bom"*. M2 fecha lá.
