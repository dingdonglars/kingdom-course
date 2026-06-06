# Módulo 0.5 — Types Deep Dive + Naming Conventions

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você está escrevendo código há cerca de um mês agora — strings, números, booleanos — sem que a gente parasse para dar nomes a eles. Hoje a gente desacelera e nomeia os blocos de construção básicos. Seis tipos que você já usou, um novo (`DateTime`) e um pequeno recurso novo chamado *nullable*. O compilador pode ter começado a te avisar sobre nullable em torno do Módulo 0.1, se você notou os sublinhados. Esta é a lição onde esses sublinhados são explicados.

> **Words to watch**
>
> - **type** — que *tipo* de valor algo é (número, texto, verdadeiro/falso, data)
> - **`int`** — um número inteiro (sem ponto decimal); intervalo de cerca de ±2 bilhões
> - **`double`** — um número que *pode* ter um ponto decimal
> - **`bool`** — `true` ou `false`, nada mais
> - **`string`** — texto em `"aspas duplas"`
> - **`DateTime`** — uma data *e* uma hora, juntas
> - **cast** — converter um valor de um tipo para outro (às vezes com segurança, às vezes não)
> - **nullable** — um tipo que também pode guardar *nenhum valor* (`null`); escrito com `?` no final
> - **PascalCase / camelCase** — convenções de nome; vamos conhecer as duas hoje

---

## Passo 1 — crie um projeto e use os tipos

Crie um novo projeto de console:

```powershell
cd <your-repo-root>
dotnet new console -n TypesDemo
cd TypesDemo
```

Abra a nova pasta no VS Code. Abra `Program.cs`. Substitua por:

```csharp
// The five types you'll use most
int gold = 100;                       // whole number
double averagePopulation = 12.5;       // decimal
bool isAtWar = false;                  // true/false
string kingdomName = "Eldoria";        // text
DateTime founded = new DateTime(2024, 1, 15);

Console.WriteLine($"{kingdomName} (founded {founded:yyyy-MM-dd})");
Console.WriteLine($"  gold: {gold}");
Console.WriteLine($"  avg pop: {averagePopulation}");
Console.WriteLine($"  at war: {isAtWar}");
```

Rode:

```powershell
dotnet run
```

Você deve ver:

```
Eldoria (founded 2024-01-15)
  gold: 100
  avg pop: 12.5
  at war: False
```

Seis tipos em cinco linhas. **Cada um diz ao compilador que tipo de valor esperar**, e o compilador recusa qualquer outro. Tente `int gold = 3.5;` — não vai compilar. Tente `bool isAtWar = "false";` — também não vai compilar. *Esse é o ponto todo dos tipos.* O compilador pega o erro antes do programa rodar.

| Tipo | O que guarda | Exemplo |
|---|---|---|
| `int` | Número inteiro, ~±2 bilhões | `int gold = 100;` |
| `double` | Número decimal, grande intervalo, não exato para dinheiro | `double avg = 12.5;` |
| `bool` | `true` ou `false` | `bool ok = true;` |
| `string` | Texto | `string name = "Eldoria";` |
| `DateTime` | Data + hora | `new DateTime(2024, 1, 15)` |

## Passo 2 — casting

Às vezes você tem um valor de um tipo e precisa dele como outro. Isso é um **cast**.

Adicione ao seu programa:

```csharp
double exactlyHalfGold = gold / 2.0;        // 50.0 (note the .0)
int roundedToBank = (int)exactlyHalfGold;   // 50 (truncates)
Console.WriteLine($"  half: {exactlyHalfGold}, banked: {roundedToBank}");
```

O `(int)` é o cast — *"trate este `double` como um `int`."* Duas coisas a lembrar sobre matemática em C#, e as duas pegam iniciantes:

- `int / int` é *divisão inteira* — a parte decimal é descartada. `5 / 2` te dá `2`, não `2.5`. Se você quer `2.5`, pelo menos um lado deve ser `double`: `5 / 2.0` ou `5.0 / 2`.
- Fazer cast de `double` para `int` *trunca* — `(int)3.99` é `3`, não `4`. Descarta a parte decimal; não arredonda.

Essas duas vão te pegar em algum momento. Saber delas agora significa que você vai reconhecer o bug quando acontecer.

## Passo 3 — nullable e ativar o sistema de avisos

Esta é a parte nova. Abra `TypesDemo.csproj` no editor. Perto do topo, você vai ver:

```xml
<Nullable>enable</Nullable>
```

Essa uma linha ativa um recurso chamado **nullable reference types**. Com ele ativo, o compilador C# rastreia quais valores têm *permissão* de ser `null` (nenhum valor) e quais não têm. Depois ele te avisa quando você mistura os dois.

Adicione isso em `Program.cs`:

```csharp
string nickname = null;
```

Salve. A linha recebe um sublinhado amarelo. Passe o mouse, ou olhe o painel *Problems* na parte inferior do VS Code. Você vai ver:

> *Cannot convert null literal to non-nullable reference type.*

O compilador está dizendo: *"Você disse `string nickname` — isso significa uma `string`, não `null`. Se `nickname` pode ser `null`, declare como `string?`."*

Conserte:

```csharp
string? nickname = null;
```

O `?` torna o tipo **nullable** — *"isso pode ser uma `string`, ou pode ser `null`."* O sublinhado vai embora.

Agora a pergunta é: **quando um valor pode ser `null`, o que você faz com ele?** Três operadores ajudam aqui.

```csharp
string? nickname = null;

// 1. ?? — "use este se for null"
string display = nickname ?? "(no nickname)";
Console.WriteLine($"Hello, {display}");

// 2. ?. — "só chame .Length se não for null"
int? length = nickname?.Length;     // null, porque nickname é null
Console.WriteLine($"Nickname length: {length ?? -1}");

// 3. ! — "prometo que não é null; confie em mim"
nickname = "the Bold";
int trustedLength = nickname!.Length;   // funciona, porque acabamos de definir
Console.WriteLine($"Trusted length: {trustedLength}");
```

Leia cada um devagar. Você vai usar esses de novo e de novo durante o ano:

- **`??`** — *"me dê este valor de backup quando for `null`."* Mais útil quando você lê entrada do usuário ou carrega um arquivo.
- **`?.`** — *"não quebre se for `null`; só me dê `null` de volta."* Deixa você chamar um method após outro sem verificar `null` a cada passo.
- **`!`** — *"sei mais do que o compilador; isso não é realmente `null`."* Use raramente. Você está substituindo o compilador, e se estiver errado, o programa quebra.

### Por que os avisos aparecem só *agora*

Seus programas anteriores (Roast-O-Matic, o jogo de adivinhação, a aventura de texto) não mostravam esses avisos. Porque os arquivos `.csproj` deles dizem `<Nullable>disable</Nullable>` em vez de `enable`. Até hoje, essa ideia era demais para adicionar em cima de tudo mais, então deixamos os avisos desligados. De `TypesDemo` em diante, eles estão ligados. **A partir deste módulo, todos os seus projetos têm nullable ativado.** Os projetos iniciais do resto do curso já têm.

Se você tiver curiosidade, abra `RoastOMatic/RoastOMatic.csproj` e mude `disable` para `enable`. Salve. Abra `Program.cs`. Sublinhados amarelos aparecem. Nenhum deles é bug no seu código; são lugares onde o compilador não tem certeza se um valor pode ser `null`. Mude de volta para `disable` se quiser; é com você. O ponto é só *ver* o que a regra faz.

## Mexa um pouco

Tente `int gold = 3.5;` e leia o erro.

Tente imprimir `founded` sem a parte de formato: `Console.WriteLine($"founded: {founded}");`. Como ele aparece por padrão?

Tente `string name; Console.WriteLine(name);` — a variável é criada mas nunca recebe um valor. O compilador se recusa. *Por quê?*

Adicione um method `int? FindGold(string kingdomName)` que retorna `100` se o nome for `"Eldoria"` e `null` caso contrário. Chame duas vezes — uma com `"Eldoria"`, outra com `"Nowhere"`. Use `??` para imprimir `"none"` quando a resposta for `null`.

## Convenções de nome

Agora a segunda metade da lição de hoje. O C# tem convenções de nome rígidas, e todo desenvolvedor C# as segue. O motivo é que elas tornam o código fácil de prever. Se você vê `Kingdom`, sabe que é um tipo. Se vê `kingdom`, sabe que é uma variável. Se a primeira letra é maiúscula ou não, isso te diz algo.

| O quê | Estilo | Exemplo |
|---|---|---|
| Tipo / classe / record | **PascalCase** | `Building`, `Kingdom`, `MyClass` |
| Method / propriedade | **PascalCase** | `Console.WriteLine`, `kingdom.Name` |
| Variável local / parâmetro | **camelCase** | `goldCount`, `firstName` |
| Campo privado | **`_camelCase`** | `_health`, `_random` (com um sublinhado) |
| Interface | **`I` + PascalCase** | `IBuilding`, `IRandom` |
| Constante | **PascalCase** | `const int MaxBuildings = 50;` |
| Method assíncrono | adiciona sufixo `Async` | `SaveAsync`, `LoadAsync` |

O conjunto completo está em `STANDARDS.md` no topo do currículo. O VS Code (com a extensão C# Dev Kit) marca qualquer quebra dessas regras enquanto você digita — um sublinhado amarelo, com a sugestão na caixa de hover. **Use sugestões de código e a ferramenta de renomear (pressione `F2` em um nome para renomeá-lo em todo lugar de uma vez). Siga as convenções, não as contrarie.**

As duas que você mais vai usar este ano:

- **Nomes de tipos — `PascalCase`.** Primeira letra maiúscula, sem sublinhados, sem espaços. `Building`, `ResourceLedger`, `KingdomDbContext`.
- **Variáveis — `camelCase`.** Primeira letra minúscula, depois maiúsculas no começo de cada palavra. `goldCount`, `firstName`, `currentDay`.

Copie o estilo que você vê no código ao seu redor. Quando não tiver certeza, o seu **IDE** (*Integrated Development Environment* — o editor onde você escreve código; neste curso, é o VS Code) te avisa com um sublinhado vermelho abaixo de um nome com as maiúsculas erradas.

## O que você acabou de fazer

Você aprendeu os seis tipos que vai usar pelo resto do ano — `int`, `double`, `bool`, `string`, `DateTime`, mais a forma *nullable* como `string?`. Conheceu duas regras de matemática em C# que pegam iniciantes (divisão inteira e casts que truncam). Ativou os avisos de nullable no seu projeto, viu o que os sublinhados significam e aprendeu três operadores (`??`, `?.`, `!`) para trabalhar com valores que podem estar faltando. E conheceu as convenções de nome que todo desenvolvedor C# segue — `PascalCase` para tipos, `camelCase` para variáveis. Seis palavras novas, um recurso novo do compilador e um conjunto de regras de nome que moldam cada linha de C# que você vai escrever daqui para frente.

**Conceitos que você já sabe nomear:**

- os cinco tipos do dia a dia — `int`, `double`, `bool`, `string`, `DateTime`
- *cast* — mudar um valor de um tipo para outro com `(type)value`
- *nullable* — `string?` permite `null`; `??`, `?.`, `!` funcionam com isso
- *PascalCase* e *camelCase* — nomes de tipos vs nomes de variáveis

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a grande ideia pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar:

1. Crie uma variável que pode guardar `null` — uma string nullable — e defina-a como `null`.
2. Imprima-a, mas use `??` para imprimir `"unknown"` quando for `null`. Rode; deve imprimir `unknown`.
3. Mude o valor para uma palavra de verdade e rode de novo — agora deve imprimir a palavra.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
string? title = null;
Console.WriteLine(title ?? "unknown");

title = "the Brave";
Console.WriteLine(title ?? "unknown");
```

- O `?` em `string?` torna o tipo nullable — pode guardar uma string, ou `null`.
- `??` significa "use este valor de backup quando o lado esquerdo for `null`." Então a primeira linha imprime `unknown`, a segunda imprime `the Brave`.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote as suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 0.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare os dois arquivos, mensagem do commit `Module 0.5 done`, Sync.
4. **Poste em `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI se você precisar relembrar. Traga as respostas do quiz das quais você estiver menos certo para o próximo sync semanal.

## Próximo

O Módulo 0.6 apresenta os **methods** — pedaços de código com nome que você chama pelo nome. Você tem chamado methods desde o dia 1 (`Console.WriteLine` é um method). Agora você vai escrever os seus próprios.
