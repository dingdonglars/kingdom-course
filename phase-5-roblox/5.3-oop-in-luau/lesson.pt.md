# Módulo 5.3 — OOP em Luau (Tables-as-Classes, Metatables)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O Lua não tem classes. Ele tem tables e uma ferramenta extra chamada *metatables*, e isso é suficiente para criar classes na mão. Cada pedaço de código Lua que você vai ler usa alguma versão da mesma receita. Hoje portamos o `Building` e o `Farm` da Fase 1 para o Luau usando essa receita — mesma ideia, linguagem menor, um pouco mais de digitação.

No Módulo 1.1, uma classe era algo embutido: você escrevia `class Building { ... }` e o C# fazia o resto. O Luau não tem palavra-chave `class`. Então você constrói a *mesma ideia de blueprint-e-objetos* na mão, com duas partes que você já conhece — uma table para guardar os dados de um objeto, e uma metatable que diz "para os métodos, vai olhar ali":

```text
   um objeto Building                  a table "classe" Building
   (criado por Building.new)           (guarda os métodos compartilhados)
   +------------------+             +----------------------+
   | name  = "Farm"   |   __index   | new()                |
   | level = 1        | ----------> | upgrade()            |
   +------------------+   "não achou +| tick()               |
                          o método?  +----------------------+
                          olha aqui"
```

Cada objeto guarda o seu próprio `name` e `level`. Os métodos ficam uma vez só na table `Building`, e cada objeto os alcança pelo `__index`. Esse é o truque inteiro — é o mesmo "um blueprint, muitos objetos" do 1.1, só montado na mão.

> **Words to watch**
>
> - **metatable** — uma table que define comportamento extra para *outra* table (operadores, lookup, comparação).
> - **`__index`** — uma entrada da metatable que diz "se uma chave não for encontrada aqui, procura nessa outra table." É isso que transforma uma table em uma classe.
> - **sintaxe de método `:`** — `obj:method()` é atalho para `obj.method(obj)`. Os dois pontos são o que passa o `self`.
> - **`setmetatable(t, mt)`** — anexa uma metatable a uma table.
> - **module pattern** — retornar uma table de um `ModuleScript`; `require` a devolve. O sistema de importação do Roblox.

---

## A receita OOP-via-tables

O Lua tem um container (a table) e uma ferramenta extra (metatables). Todo pedaço de código Lua usa alguma versão desta receita para criar classes na mão.

```lua
-- A "classe" é só uma table.
local Building = {}
Building.__index = Building          -- métodos são buscados no Building

-- Construtor.
function Building.new(name: string)
    local self = setmetatable({}, Building)
    self.name = name
    self.level = 1
    return self
end

-- Método.
function Building:upgrade()
    self.level = self.level + 1
end

function Building:tick(ledger: any)
    -- padrão: nada
end

return Building
```

Leia linha por linha:

- `local Building = {}` — a table que representa a classe.
- `Building.__index = Building` — quando você busca uma chave em uma *instância* e não acha, cai no `Building` table. **É assim que os métodos são encontrados.**
- `Building.new(name)` é o construtor. Cria uma nova table, anexa `Building` como sua metatable, preenche os campos e a retorna.
- `function Building:upgrade()` — os dois pontos significam *parâmetro `self` implícito*. É equivalente a escrever `Building.upgrade = function(self) ... end`.
- `setmetatable(t, mt)` anexa `mt` como metatable de `t`.

Você vai escrever essa mesma receita para cada "classe" do seu engine. É um pouco longa, mas são sempre os mesmos passos. Pelo terceiro, seus dedos já vão saber.

## Herança

Uma subclasse é a mesma receita com um passo a mais: defina a classe pai como o `__index` da subclasse.

```lua
local Building = require(script.Parent.Building)

local Farm = setmetatable({}, { __index = Building })   -- herda
Farm.__index = Farm

function Farm.new(name: string)
    local self = Building.new(name)                     -- chama construtor do pai
    setmetatable(self, Farm)                            -- re-parenta a instância
    return self
end

function Farm:tick(ledger: any)
    ledger:add("Food", 5 * self.level)                  -- sobrescreve o tick padrão
end

return Farm
```

Um `Farm` "é um" `Building` porque o pai é definido como o `__index` do `Farm`, e as instâncias de `Farm` são re-parentadas no construtor. Quando o Lua busca um método, ele segue essa cadeia: verifica a instância, depois o `Farm`, depois o `Building`.

## Module pattern (específico do Roblox)

O Roblox tem três tipos de script:

- **Script** — roda no servidor quando o place começa.
- **LocalScript** — roda no cliente (o dispositivo de um jogador) quando ele entra.
- **ModuleScript** — define um módulo. Não roda sozinho. Outro script chama `require` nele.

O código do engine fica em **ModuleScripts** em `ReplicatedStorage`, para que scripts do servidor e do cliente possam importá-lo. Todo arquivo de módulo tem o mesmo formato: declare uma table no topo, anexe funções a ela e retorne-a no final.

```lua
-- consumidor
local Building = require(game.ReplicatedStorage.Engine.Building)

local farm = Building.new("Main Farm")
farm:upgrade()
print(farm.level)          -- 2
```

`require` salva o módulo depois da primeira chamada. Chamá-lo cem vezes retorna a mesma table, não cem cópias. Então existe sempre apenas um de cada módulo.

## Mexa um pouco

Adicione um `Mine.lua` que copia o `Farm.lua` mas adiciona `"Stone"` em vez de `"Food"`. O padrão é o mesmo; você está praticando a receita.

Tente `farm.upgrade()` (com ponto em vez de dois pontos). Dá um erro, porque `self` está faltando — o ponto não o passa. O ponto versus os dois pontos é um dos dois ou três detalhes do Luau que pegam as pessoas no começo. Cometa esse erro de propósito para a regra ficar.

Imprima `getmetatable(farm)`. A saída é a table da classe `Farm`. Isso mostra que a metatable *é* a classe.

Adicione um método `Building:describe()` que retorna uma string. Sobrescreva-o no `Farm`. Chame `farm:describe()` e veja o Lua encontrar a versão do `Farm` primeiro. Se você apagar o override, ele cai de volta para a versão do `Building`.

## O que você acabou de fazer

Você conheceu a ideia de programação orientada a objetos do Lua. Não há palavra-chave `class` — é uma receita no lugar. Uma classe é uma table. Um método é uma função anexada a essa table. Uma instância é uma nova table cuja metatable aponta para a classe. `__index` é a regra que faz o lookup de métodos funcionar. Herança é a mesma receita com mais um passo. A receita inteira é sempre os mesmos passos, e todo pedaço de código Lua usa ela. Depois de escrever `Building` e `Farm`, você pode ler `Mine`, `Lumberyard` e as próximas vinte classes sem esforço. Você também conheceu os três tipos de script do Roblox: um Script que roda no servidor, um LocalScript que roda no dispositivo de um jogador e um ModuleScript que guarda código do engine que outros scripts usam com `require`.

**Conceitos que você já sabe nomear:**

- *metatable* — table de comportamento anexada a outra table
- *`__index`* — a regra de fallback-lookup que faz o dispatch de métodos funcionar
- *dois pontos vs ponto* — `:` passa `self` automaticamente; `.` não passa
- *ModuleScript* — a unidade de código importável do Roblox, retornada por `require`
- *module pattern* — declare uma table, anexe funções, retorne-a

## Por sua conta

Hora de fechar o livro. Não role de volta para a receita — prove para você mesmo, da sua própria cabeça, que ela pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um novo Script no Studio. Sem olhar, escreva uma classe pequena chamada `Tower`:

1. Um construtor `.new(name)` que define `name` e `level = 1`.
2. Um método `:upgrade()` que adiciona 1 ao nível.
3. Crie uma, chame `upgrade` duas vezes e imprima o nível. Clique em Play.

<details><summary>Travou? Abra aqui para conferir.</summary>

```lua
local Tower = {}
Tower.__index = Tower

function Tower.new(name: string)
    local self = setmetatable({}, Tower)
    self.name = name
    self.level = 1
    return self
end

function Tower:upgrade()
    self.level = self.level + 1
end

local t = Tower.new("North Tower")
t:upgrade()
t:upgrade()
print(t.level)   -- 3
```

Os três passos que fazem dela uma classe: `Tower.__index = Tower`, `setmetatable({}, Tower)` no construtor, e os dois pontos em `Tower:upgrade()` para passar `self`. Se você usou ponto (`t.upgrade()`), `self` está faltando e você recebe um erro.

</details>

## Palavras para adicionar ao glossário

- **metatable** — uma table anexada a outra table que define comportamento extra.
- **`__index`** — uma entrada da metatable que diz "se uma chave não for encontrada aqui, procura ali."
- **`setmetatable`** — a função que anexa uma metatable.
- **ModuleScript** — a unidade de código importável do Roblox; retornada por `require`.
- **module pattern** — declare uma table, anexe funções a ela, retorne-a.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.4 apresenta os **conceitos específicos do Roblox** — Workspace, RemoteEvents, servidor vs cliente. É o ambiente dentro do qual o seu engine roda.
