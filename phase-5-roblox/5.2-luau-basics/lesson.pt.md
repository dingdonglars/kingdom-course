# Módulo 5.2 — Básicos do Luau

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Luau é a linguagem que o Roblox usa. É uma versão do Lua com tipos opcionais por cima. É menor que C# e mais simples que JavaScript, mas é feita dos mesmos blocos: variáveis, funções, condicionais, loops e um único container chamado *table*. Hoje vemos como a sintaxe é diferente. No próximo módulo você usa Luau para criar classes.

Nenhuma das *ideias* de hoje é nova. Variáveis, funções, `if`, loops, um container de chave-e-valor — você conheceu tudo isso na Fase 0 e usou todas as semanas desde então. O Luau tem as mesmas peças; só soletradas de outra forma e com tudo em um único container em vez de listas e dicionários separados. Então leia hoje como uma *tradução*, não como um recomeço. Você já pensa nesses formatos — está aprendendo as palavras do Roblox para eles.

> **Words to watch**
>
> - **Luau** — a variante de Lua do Roblox. Adiciona tipos opcionais e alguns ajustes de desempenho.
> - **table** — o container universal do Lua. Arrays, dicionários e objetos são todos tables.
> - **`local`** — declara uma variável com escopo no bloco. Sem `local`, a variável é global, o que quase nunca é o que você quer.
> - **`function ... end`** — sintaxe de função. `end` fecha o corpo em vez de `}`.
> - **type annotation** — a parte `: number` de `local x: number = 10`. Opcional, mas recomendada.

---

## C# e Luau lado a lado

O jeito mais rápido de aprender uma segunda linguagem é ler uma tabela de tradução. As ideias são as mesmas que você já conhece. Só as palavras mudaram.

| C# | Luau | Note |
| --- | --- | --- |
| `int x = 10;` | `local x = 10` | `local` mantém a variável com escopo. Sem ele, a variável vira global. |
| `int x = 10;` (com tipo) | `local x: number = 10` | O tipo é opcional, mas recomendado. |
| `void Print(int x)` | `function Print(x: number)` | O corpo da função termina com `end`. |
| `if (x > 0) { ... }` | `if x > 0 then ... end` | Sem chaves. `then` abre, `end` fecha. |
| `for (int i = 0; i < 10; i++)` | `for i = 1, 10 do ... end` | Começa em 1. `do` abre, `end` fecha. |
| `foreach (var item in list)` | `for _, item in ipairs(list) do ... end` | `ipairs` para arrays, `pairs` para dicionários. |
| `// comment` | `-- comment` | Dois traços. |
| `/* block */` | `--[[ block ]]` | Colchetes duplos para comentários em bloco. |
| `string s = "x" + y;` | `local s = "x" .. y` | `..` para concatenar strings. `+` daria erro de número. |
| `null` | `nil` | O valor "sem valor". |
| `var d = new Dictionary<string,int>();` | `local d = {}` | Tables fazem dois trabalhos. |
| `var l = new List<string>();` | `local l = {}` | Mesmo literal. Array ou dicionário é uma diferença de uso, não de tipo. |

O Luau tem mais ou menos metade da sintaxe do C#. Também é uma linguagem muito menor para rodar, por isso fica rápido mesmo quando o jogo faz tick a cada segundo.

## Tables — o único container

O Luau tem exatamente um container embutido, e ele se chama table. O mesmo valor funciona como array, como dicionário, ou como os dois ao mesmo tempo.

```lua
-- Estilo array
local resources = {"Gold", "Wood", "Stone"}
print(resources[1])   -- "Gold"  (começa em 1!)

-- Estilo dicionário
local kingdom = { name = "Eldoria", day = 11 }
print(kingdom.name)            -- "Eldoria"
print(kingdom["name"])         -- mesma coisa, sintaxe diferente

-- Misto
local mixed = { "first", "second", name = "test" }
```

A coisa que mais pega quem vem do C# ou do JavaScript é que **arrays começam no índice 1**, não no 0. Os seus dedos vão querer digitar `for i = 0, n - 1`, e isso não dá nada. Todo loop em Luau quer `for i = 1, n`. Quando você se acostumar com isso, o resto da linguagem é fácil.

## Funções

```lua
local function greet(name: string)
    print("Hello, " .. name)
end

greet("Lyra")
```

`local` mantém o nome da função dentro do próprio arquivo. Sem ele, a função vira global, o que significa que todo script do place pode vê-la. Um place inteiro do Roblox compartilha um espaço global, então isso causa bugs difíceis de achar. Sempre escreva `local`.

O Lua deixa uma função retornar mais de um valor, sem sintaxe especial. `function f() return 1, 2 end` retorna dois valores, e `local a, b = f()` lê os dois. Uma função sem nome parece `local f = function(x) return x * 2 end`.

## Condicionais

```lua
if score > 100 then
    print("High score!")
elseif score > 50 then
    print("OK")
else
    print("Try again")
end
```

`then` vem depois da condição. `end` fecha o `if` inteiro. Sem parênteses ao redor da condição — são permitidos, mas não são necessários.

## Loops

```lua
-- Numérico
for i = 1, 10 do
    print(i)
end

-- Passo de 2
for i = 1, 10, 2 do
    print(i)         -- 1, 3, 5, 7, 9
end

-- Percorrer um array
for index, value in ipairs(resources) do
    print(index, value)
end

-- Percorrer um dicionário
for key, value in pairs(kingdom) do
    print(key, value)
end

-- While
while x < 10 do
    x = x + 1
end
```

Dois helpers valem a pena aprender. `ipairs` percorre um array em ordem e para no primeiro `nil`. `pairs` percorre todas as chaves de um dicionário, mas a ordem não é fixa. Se você escolher o errado, vai pular entradas ou recebê-las numa ordem estranha.

## O loop de debug com `print`

O Output panel no Studio é o que você vai usar para depurar na maior parte do tempo. Existe um depurador de verdade também — *menu Debug → Breakpoints* — e você vai conhecê-lo quando precisar de verdade. Por enquanto, `print(x)` é suficiente.

## Type annotations — a parte do Luau

O Lua puro não tem tipos. O Luau adiciona tipos opcionais por cima:

```lua
local function add(a: number, b: number): number
    return a + b
end

local kingdom: { name: string, day: number } = { name = "X", day = 1 }
```

O Studio verifica os tipos enquanto você digita. Passe uma string onde você prometeu um número, e a linha fica com um sublinhado vermelho. Isso funciona como a verificação do TypeScript: avisa no editor, mas não para o código enquanto ele roda. É uma boa ideia para qualquer função com dois ou mais parâmetros.

## Mexa um pouco

Abra o Studio, insira um Script no `ServerScriptService` e cole o pequeno playground do arquivo starter `luau-basics.lua`. Clique em Play e leia o Output panel.

Cometa um erro de propósito: `local x: number = "hello"`. O Studio sublinha. Passe o mouse sobre o sublinhado vermelho para ler o erro.

`#resources` dá o tamanho de uma table usada como array. Imprima isso.

`table.insert(resources, "Food")` adiciona um item no final. `table.remove(resources, 1)` remove o primeiro. A biblioteca padrão tem as operações de array que você esperaria, mas elas são funções no módulo `table`. Você não as chama diretamente no array.

## O que você acabou de fazer

Você percorreu a sintaxe de uma segunda linguagem e viu que a maior parte já é familiar. Variáveis, funções, condicionais, loops — mesmas ideias, palavras-chave diferentes. O único container do Luau, a table, faz o trabalho que o C# divide entre `List`, `Dictionary` e uma classe com campos. A única coisa que pega as pessoas no primeiro dia ou dois é que arrays começam no índice 1. Quando você se acostumar com isso, dá para ler a maioria do código Luau sem desacelerar. A sintaxe leva uns trinta minutos para ler. O resto vem de digitar algumas centenas de linhas ao longo dos próximos seis módulos.

**Conceitos que você já sabe nomear:**

- *`local`* — declara uma variável com escopo; sem ele, você criou um global
- *table* — o único container do Luau; array, dicionário e objeto em um só
- *arrays que começam em 1* — a coisa que mais pega quem vem do C# e do JavaScript
- *`..`* — operador de concatenação de strings; `+` é só para números
- *type annotations* — dicas opcionais que o Studio verifica enquanto você digita

## Por sua conta

Hora de fechar o livro. Não role de volta para o código — prove para você mesmo, da sua própria cabeça, que a sintaxe pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um novo Script no Studio. Sem olhar:

1. Crie uma table com três nomes de recursos.
2. Percorra ela e imprima cada item. (Cuidado com o índice — arrays em Luau começam em 1.)
3. Clique em Play.

<details><summary>Travou? Abra aqui para conferir.</summary>

```lua
local resources = {"Gold", "Wood", "Stone"}

for index, value in ipairs(resources) do
    print(index, value)
end
```

O Output panel mostra `1 Gold`, `2 Wood`, `3 Stone`. O primeiro índice é `1`, não `0`. Se você escreveu `for i = 0, ...` não veio nada para o índice 0, porque não há nenhum item lá.

</details>

## Palavras para adicionar ao glossário

- **Luau** — a variante de Lua do Roblox, com tipos opcionais.
- **table** — o container universal do Lua; arrays, dicionários e objetos são todos tables.
- **`local`** — declara uma variável com escopo no bloco.
- **`ipairs` / `pairs`** — iteração de array versus iteração de dicionário.
- **`nil`** — o valor "sem valor" do Lua.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.3 constrói **OOP em Luau** — classes via tables e metatables. O mesmo `Building` e `Farm` que você escreveu na Fase 1, em uma linguagem menor com uma receita diferente.
