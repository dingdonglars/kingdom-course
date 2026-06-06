# Módulo 5.5 — Port do Engine: ResourceLedger, Citizen, Kingdom, Tick

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o engine faz tudo o que fazia na Fase 1 — mas em Luau, em um servidor Roblox. `ResourceLedger`, `Citizen`, o agregado `Kingdom`, o loop de tick dos dias. Mesma ideia, linguagem menor, um lugar diferente para rodar. Ao final deste módulo, o engine funciona da mesma forma nas duas versões.

> **Words to watch**
>
> - **port** — traduzir código para outra linguagem preservando o significado.
> - **`task.wait(seconds)`** — a função do Roblox que pausa este script por um tempo. Não bloqueia outros scripts.
> - **`game:GetService("RunService")`** — o serviço que dispara uma vez por frame; usado para game loops que precisam rodar na taxa de frames.
> - **coroutine** — uma função que pode se pausar e ser retomada depois. O Roblox roda cada script em uma dessas, que é por isso que `task.wait` não congela o place.

---

## O plano de port

Ler a versão em C# ao lado da versão em Luau é o jeito mais rápido de aprender a tradução:

| Fase 1 (C#) | Roblox (Luau) | Note |
| --- | --- | --- |
| `Resource.cs` (enum) | chaves de string (`"Gold"`, `"Wood"`, ...) | Lua não tem enums; strings são o padrão. |
| `ResourceLedger.cs` | `ResourceLedger.lua` (ModuleScript) | Mesmo get / add / spend / snapshot. |
| `Citizen.cs` | `Citizen.lua` (ModuleScript) | Igual. |
| `Building.cs` e subclasses | feito no Módulo 5.3 | Já portado. |
| `Kingdom.cs` (agregado) | `Kingdom.lua` (ModuleScript) | Dono das listas; chama `:tick(ledger)`. |
| `EventEngine.cs` | `EventEngine.lua` | Usa `math.random` do Lua, ou passe uma função injetada para manter os testes determinísticos. |
| Shells de console/arquivo | não se aplica | O game loop no servidor os substitui. |

## `ResourceLedger.lua`

```lua
local ResourceLedger = {}
ResourceLedger.__index = ResourceLedger

function ResourceLedger.new()
    local self = setmetatable({}, ResourceLedger)
    self.amounts = { Gold = 0, Wood = 0, Stone = 0, Food = 0 }
    return self
end

function ResourceLedger:get(resource: string): number
    return self.amounts[resource] or 0
end

function ResourceLedger:add(resource: string, amount: number)
    if amount < 0 then error("Use spend for negatives") end
    self.amounts[resource] = (self.amounts[resource] or 0) + amount
end

function ResourceLedger:spend(resource: string, amount: number): boolean
    if amount < 0 then error("Spend amount must be non-negative") end
    local have = self.amounts[resource] or 0
    if have < amount then return false end
    self.amounts[resource] = have - amount
    return true
end

function ResourceLedger:snapshot(): { [string]: number }
    local out = {}
    for k, v in pairs(self.amounts) do out[k] = v end
    return out
end

return ResourceLedger
```

Os mesmos cinco métodos da versão em C#. `error(...)` é o `throw` do Lua.

## `Citizen.lua`

```lua
local Citizen = {}
Citizen.__index = Citizen

function Citizen.new(name: string)
    local self = setmetatable({}, Citizen)
    self.name = name
    return self
end

return Citizen
```

Ainda menor que a versão em C# — só um nome. Corresponde ao `Citizen` mínimo da Fase 1.

## `Kingdom.lua`

```lua
local Building = require(script.Parent.Building)
local ResourceLedger = require(script.Parent.ResourceLedger)

local Kingdom = {}
Kingdom.__index = Kingdom

function Kingdom.new(name: string)
    local self = setmetatable({}, Kingdom)
    self.name = name
    self.day = 1
    self.buildings = {}
    self.citizens = {}
    self.resources = ResourceLedger.new()
    -- Recursos iniciais
    self.resources:add("Gold", 100)
    self.resources:add("Wood", 50)
    self.resources:add("Stone", 20)
    self.resources:add("Food", 30)
    return self
end

function Kingdom:addBuilding(b)
    table.insert(self.buildings, b)
end

function Kingdom:addCitizen(c)
    table.insert(self.citizens, c)
end

function Kingdom:advanceDay()
    -- Prédios produzem
    for _, b in ipairs(self.buildings) do
        b:tick(self.resources)
    end
    -- Cidadãos comem
    for _ in ipairs(self.citizens) do
        self.resources:spend("Food", 1)
    end
    self.day = self.day + 1
end

return Kingdom
```

O engine inteiro em um arquivo. Leia isso ao lado do `Kingdom.cs` em C# e você vai ver as mesmas linhas, só mais curtas.

## O game loop no servidor

```lua
-- ServerScriptService/Script

local Engine = game.ReplicatedStorage:WaitForChild("Engine")
local Kingdom = require(Engine.Kingdom)
local Farm = require(Engine.Farm)
local Mine = require(Engine.Mine)
local Citizen = require(Engine.Citizen)

local kingdom = Kingdom.new("Eldoria")
kingdom:addBuilding(Farm.new("Main Farm"))
kingdom:addBuilding(Mine.new("Old Mine"))
kingdom:addCitizen(Citizen.new("Lyra"))

-- Tick a cada 5 segundos (5 dias por minuto — lento, mas visível)
while true do
    task.wait(5)
    kingdom:advanceDay()
    print(string.format(
        "Day %d — Gold:%d Wood:%d Stone:%d Food:%d",
        kingdom.day,
        kingdom.resources:get("Gold"),
        kingdom.resources:get("Wood"),
        kingdom.resources:get("Stone"),
        kingdom.resources:get("Food")))
end
```

`task.wait(5)` pausa *este script* por cinco segundos. Não para outros scripts, porque o Roblox roda cada script em sua própria coroutine. Uma *coroutine* é uma função que pode se pausar e reiniciar depois. Enquanto um script está pausado, o Roblox troca silenciosamente para os outros. Vale a pena saber a palavra, porque você vai vê-la na documentação.

Para um jogo de verdade, o loop faria tick mais rápido (a cada segundo, ou a cada 0,1 segundo), e você usaria `RunService.Heartbeat:Connect(function(dt) ... end)` para acompanhar a taxa de frames. Para aprender, `task.wait` é suficiente.

## Mexa um pouco

Mude o tick para `task.wait(1)`. Os recursos mudam a cada segundo e o place fica muito mais vivo.

Adicione um terceiro prédio, `Lumberyard.new("Eastern Lumberyard")`, ao kingdom. Wood começa a subir.

Substitua o `while true do` por `RunService.Heartbeat:Connect(function() ... end)` e faça o tick a cada N frames. Mesmo engine, jeito diferente de temporizá-lo — um port pequeno e satisfatório.

Abra o `Kingdom.cs` em C# e o `Kingdom.lua` em Luau lado a lado em duas janelas. Leia de cima a baixo e perceba quantas linhas realmente mudaram.

## O que você acabou de fazer

Você pegou o engine que escreveu na Fase 1 e o traduziu para Luau em um servidor Roblox. `ResourceLedger`, `Citizen` e `Kingdom` são agora ModuleScripts em `ReplicatedStorage`. Um Script do servidor em `ServerScriptService` chama `kingdom:advanceDay()` a cada cinco segundos e imprime o resultado. A versão em Luau sai menor que a versão em C# porque o Luau precisa de menos código extra — sem namespaces, sem linhas `using`, sem rótulos public ou private. E aqui está a prova de toda a ideia: o engine não se importa com onde roda. **Cinco vezes agora: console, arquivo com JSON e SQLite, web API, browser, Roblox. Cinco lugares para rodar; um engine.** Esse é o ponto do curso inteiro, e você acabou de fazê-lo.

**Conceitos que você já sabe nomear:**

- *port* — traduzir para outra linguagem preservando o significado
- *chaves de string em vez de enums* — o jeito Lua de lidar com um conjunto fixo de valores
- *`task.wait(seconds)`* — pausa um script; deixa os outros continuar rodando
- *coroutine* — uma função que pode pausar e reiniciar; o Roblox roda scripts nessas
- *`RunService.Heartbeat`* — um evento que dispara todo frame; usado para game loops na taxa de frames

## Por sua conta

Hora de fechar o livro. Não role de volta para o código — prove para você mesmo, da sua própria cabeça, que o loop do servidor pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um novo Script no Studio. Sem olhar, escreva o loop eterno que:

1. Espera um segundo.
2. Avança o kingdom um dia.
3. Imprime o número do dia.

(Você pode usar um kingdom falso — uma table simples com um campo `day` — se não quiser montar o engine de verdade.) Clique em Play e veja o contador de dias subir no Output.

<details><summary>Travou? Abra aqui para conferir.</summary>

```lua
local kingdom = { day = 1 }

while true do
    task.wait(1)
    kingdom.day = kingdom.day + 1
    print("Day", kingdom.day)
end
```

`task.wait(1)` pausa só este script por um segundo; o restante do place continua rodando. Com o engine de verdade você chamaria `kingdom:advanceDay()` no lugar da linha `day + 1`. Pressione Stop para encerrar o loop.

</details>

## Palavras para adicionar ao glossário

- **port** — traduzir código de uma linguagem ou runtime para outro preservando o significado.
- **`task.wait`** — pausa o script atual por N segundos; outros scripts continuam rodando.
- **coroutine** — uma função que pode pausar e reiniciar depois; o Roblox roda cada script em uma.
- **`RunService.Heartbeat`** — um evento do Roblox que dispara todo frame; usado para game loops na taxa de frames.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.5 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.6 constrói **o mundo visual** — uma representação 3D do kingdom no `Workspace`. Clique em um tile, construa uma fazenda; o engine controla a aparência.
