# Módulo 5.7 — Roblox DataStore: Persistência

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o kingdom fica salvo entre sessões de jogo. O Roblox oferece o **DataStoreService** — um armazenamento de chave/valor que só pode ser usado com código do servidor. Salvamos o snapshot do kingdom quando um jogador sai e carregamos de volta quando ele volta. Mesma ideia do arquivo JSON da Fase 2, com uma API diferente.

> **Words to watch**
>
> - **DataStore** — a persistência de chave/valor do Roblox. Gratuito até uma cota; um conjunto por place.
> - **`DataStoreService`** — a API. Só pode ser chamada de scripts do servidor.
> - **`SetAsync(key, value)` / `GetAsync(key)`** — as duas operações básicas.
> - **`UpdateAsync(key, fn)`** — atualização atômica; mais segura que ler-depois-escrever.
> - **`pcall`** — a chamada protegida do Lua, equivalente a um wrapper de try/catch.
> - **JSON via Lua** — `HttpService:JSONEncode` e `JSONDecode` para codificar tables em strings.

---

## DataStore em 30 segundos

```lua
local DataStoreService = game:GetService("DataStoreService")
local store = DataStoreService:GetDataStore("Kingdoms")

-- Escrever
store:SetAsync("player_12345", { day = 11, gold = 250 })

-- Ler
local data = store:GetAsync("player_12345")
print(data.day, data.gold)
```

Isso é quase toda a API para a maioria dos casos. As duas funções só funcionam no servidor — `DataStore` não carrega em um `LocalScript`.

`SetAsync` e `GetAsync` são assíncronas: enviam um pedido para os servidores do Roblox e esperam a resposta. Às vezes esse pedido falha por um momento, e a chamada lança um erro. Então jogos de verdade envolvem essas chamadas em `pcall`.

## Snapshot para JSON

Na maioria das vezes você não precisa converter nada — o DataStore pode guardar uma table Lua diretamente. Quando você precisa de uma string (por exemplo, para enviar por um RemoteEvent), o Roblox oferece JSON pelo `HttpService`:

```lua
local HttpService = game:GetService("HttpService")

-- Codificar
local json = HttpService:JSONEncode({ day = 11, name = "Eldoria" })
-- Decodificar
local table = HttpService:JSONDecode(json)
```

## O ciclo de salvar e carregar

```lua
local DataStoreService = game:GetService("DataStoreService")
local Players = game:GetService("Players")
local store = DataStoreService:GetDataStore("Kingdoms")

local kingdoms: { [number]: any } = {}   -- userId → kingdom em memória

local function key(player: Player): string
    return "player_" .. player.UserId
end

local function loadKingdom(player: Player)
    local snapshot = nil
    local ok, err = pcall(function()
        snapshot = store:GetAsync(key(player))
    end)
    if not ok then
        warn("Failed to load:", err)
        return Kingdom.new(player.Name .. "'s Kingdom")
    end
    if snapshot then
        return Kingdom.fromSnapshot(snapshot)
    end
    return Kingdom.new(player.Name .. "'s Kingdom")
end

local function saveKingdom(player: Player, kingdom: any)
    local snapshot = kingdom:toSnapshot()
    local ok, err = pcall(function()
        store:SetAsync(key(player), snapshot)
    end)
    if not ok then
        warn("Failed to save:", err)
    end
end

Players.PlayerAdded:Connect(function(player)
    kingdoms[player.UserId] = loadKingdom(player)
end)

Players.PlayerRemoving:Connect(function(player)
    if kingdoms[player.UserId] then
        saveKingdom(player, kingdoms[player.UserId])
        kingdoms[player.UserId] = nil
    end
end)

game:BindToClose(function()
    -- O servidor está encerrando — salve todos primeiro.
    for _, player in ipairs(Players:GetPlayers()) do
        if kingdoms[player.UserId] then
            saveKingdom(player, kingdoms[player.UserId])
        end
    end
end)
```

Cinco coisas que vale nomear:

- `PlayerAdded` — roda quando alguém entra; carregue o kingdom deles aqui.
- `PlayerRemoving` — roda quando alguém sai; salve o kingdom deles aqui.
- `BindToClose` — roda quando o servidor está encerrando; o Roblox dá cerca de trinta segundos para salvar tudo.
- `pcall` — o try/catch do Lua. Um erro do DataStore pode ser recuperado, então envolver a chamada deixa o script continuar se a rede falhar por um momento.
- `warn` — imprime no Output panel em amarelo; você pode vê-lo nos logs do servidor ao vivo além do Studio.

## Engine: salvar e carregar

`Kingdom:toSnapshot()` e `Kingdom.fromSnapshot(data)` precisam existir no engine Luau — mesma ideia que o `KingdomSnapshot` que você escreveu na Fase 2. `toSnapshot` transforma o kingdom em uma table simples que pode ser salva; `fromSnapshot` reconstrói um kingdom completo a partir dessa table. Um primeiro rascunho:

```lua
function Kingdom:toSnapshot()
    return {
        name = self.name,
        day = self.day,
        gold = self.resources:get("Gold"),
        wood = self.resources:get("Wood"),
        stone = self.resources:get("Stone"),
        food = self.resources:get("Food"),
        buildings = (function()
            local out = {}
            for _, b in ipairs(self.buildings) do
                table.insert(out, { kind = getmetatable(b).__name, name = b.name, level = b.level })
            end
            return out
        end)(),
    }
end

function Kingdom.fromSnapshot(snap: any)
    -- (similar ao Kingdom.LoadFrom da Fase 2; implementação completa no starter)
end
```

Cada subclasse define `__name` na sua metatable — `Farm.__name = "Farm"`, `Mine.__name = "Mine"` — o que faz o mesmo trabalho que `b.GetType().Name` fazia no C#.

## Cotas do DataStore

O plano gratuito dá cerca de sessenta chamadas por minuto por servidor, com até quatro megabytes por chave. É mais que suficiente para um jogador de cada vez. Não salve a cada tick — salve quando o jogador sai, mais a cada cinco minutos ou mais para garantir.

Para um jogo de verdade e muito ocupado, você adicionaria três coisas:

- **Session locking** — impedir dois servidores de escrever a mesma chave ao mesmo tempo.
- **Backups** — guardar versões antigas caso um save fique corrompido.
- **Migration** — lidar com formatos de save antigos quando você muda o layout.

O Roblox tem bibliotecas da comunidade para isso — `ProfileService` e `Suphi DataStore` são as duas mais citadas — mas elas estão além do que esta lição cobre.

## Mexa um pouco

Teste no Studio com *Test → Local Server*. O DataStore fica **desligado** no Studio por padrão. Ligue-o em *Game Settings → Security → Enable Studio Access to API Services*.

Adicione um comando de chat (`/save`) que salva sob demanda, reinicie o servidor, entre de novo como o mesmo jogador e veja o mesmo kingdom voltar.

Perceba como isso é parecido com a tabela de migrations do EF Core da Fase 2: no DataStore, você adicionaria um campo `version` ao seu snapshot e trataria versões mais antigas quando carregar. Mesma ideia de migration; ferramenta diferente.

## O que você acabou de fazer

Você ensinou o kingdom a ficar salvo entre sessões no Roblox. `Players.PlayerAdded` carrega o snapshot do jogador do DataStore quando ele entra. `Players.PlayerRemoving` salva quando ele sai. `game:BindToClose` salva todos quando o próprio servidor encerra. `pcall` envolve as chamadas de rede, para que uma falha curta não pare o script inteiro. Os métodos `Kingdom:toSnapshot` e `Kingdom.fromSnapshot` ficam no próprio engine, igual ao design da Fase 2 — o engine sabe se descrever, e o código de salvar só move os dados. **O mesmo padrão cinco vezes agora: arquivo (Módulo 2.1), JSON (Módulo 2.2), SQLite (Módulo 2.4), EF Core (Módulo 2.6), DataStore (Módulo 5.7).** Como você guarda muda; o cuidado não muda.

**Conceitos que você já sabe nomear:**

- *DataStoreService* — o armazenamento de chave/valor embutido do Roblox, só para servidor
- *`SetAsync` / `GetAsync` / `UpdateAsync`* — as três operações básicas
- *`pcall`* — o wrapper de try/catch do Lua; mantém o script rodando se uma chamada falhar por um momento
- *`PlayerAdded` / `PlayerRemoving` / `BindToClose`* — eventos que rodam ao entrar, sair e encerrar
- *salvar e carregar* — o engine se descreve; o código de salvar move os dados

## Por sua conta

Hora de fechar o livro. Não role de volta para o código — prove para você mesmo, da sua própria cabeça, que as chamadas de salvar e carregar pegaram. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um novo Script no `ServerScriptService` (ligue o DataStore primeiro: *Game Settings → Security → Enable Studio Access to API Services*). Sem olhar:

1. Acesse um DataStore.
2. Salve uma table pequena sob uma chave.
3. Leia de volta e imprima um campo.
4. Clique em Play.

<details><summary>Travou? Abra aqui para conferir.</summary>

```lua
local DataStoreService = game:GetService("DataStoreService")
local store = DataStoreService:GetDataStore("Kingdoms")

store:SetAsync("player_12345", { day = 11, gold = 250 })

local data = store:GetAsync("player_12345")
print(data.day, data.gold)   -- 11   250
```

`GetDataStore` dá acesso a um store nomeado. `SetAsync` escreve; `GetAsync` lê. Ambos só funcionam no servidor. Em um jogo de verdade você envolve cada chamada em `pcall`, porque o pedido pode falhar por um momento e lançar um erro.

</details>

## Palavras para adicionar ao glossário

- **DataStore** — a persistência de chave/valor do Roblox; só para servidor.
- **`SetAsync` / `GetAsync` / `UpdateAsync`** — as operações básicas do DataStore.
- **`pcall`** — a chamada protegida do Lua; retorna um booleano de sucesso mais resultado ou erro.
- **`warn`** — log amarelo no Output; visível nos logs do servidor em produção.
- **`BindToClose`** — roda um último save quando o servidor encerra.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.7 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.8 é o último. **Publicar, M6, e a reflexão final do ano.** Seus amigos jogam o seu jogo. O curso fecha.
