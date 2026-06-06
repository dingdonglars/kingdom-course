# Módulo 5.6 — O Mundo Visual

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o kingdom aparece no mundo. Uma grade de tiles no `Workspace`; clique em um tile, construa uma fazenda; um modelo 3D aparece por cima. O engine controla o que você vê — o mesmo kingdom que imprimia no console na Fase 1, agora aparecendo como Parts no Roblox.

> **Words to watch**
>
> - **Part** — o bloco 3D básico. Tem forma, posição, cor e material.
> - **Model** — uma pasta de Parts agrupadas para que se movam e selecionem como uma só.
> - **CFrame** — um valor do Roblox que combina posição e orientação. O que você define para mover e girar algo.
> - **`ClickDetector`** — um filho que você coloca dentro de uma Part para ela reagir a cliques do mouse.
> - **`Instance.new(class, parent)`** — criar um objeto da classe dada enquanto o jogo roda e definir seu pai.
> - **`Vector3` / `Color3`** — os tipos de vetor 3D e cor RGB.

---

## O padrão

Objetos visuais de jogo no Roblox são só dados — Parts no `Workspace`. Scripts do servidor as criam enquanto o jogo roda, e o Roblox *as replica* — copia para a tela de cada jogador — para que você não precise enviar nada manualmente. A receita para um kingdom clicável é curta:

1. Monte uma grade de tiles vazios, quando o place começa ou quando o jogador pede um.
2. Cada tile tem um `ClickDetector` que escuta cliques.
3. No clique, o servidor verifica se o jogador pode pagar uma fazenda (uma chamada ao engine) e cria uma Part em cima do tile.
4. Os recursos do kingdom fazem tick no servidor a cada poucos segundos, e a UI do jogador mostra o resultado.

## Criar um tile com código

```lua
local function makeTile(position: Vector3): Part
    local p = Instance.new("Part")
    p.Anchored = true
    p.Size = Vector3.new(8, 1, 8)
    p.Position = position
    p.Color = Color3.fromRGB(120, 180, 100)   -- verde grama
    p.Material = Enum.Material.Grass
    p.Parent = workspace
    return p
end

-- Faz uma grade 5x5
for x = 1, 5 do
    for z = 1, 5 do
        makeTile(Vector3.new(x * 10, 0, z * 10))
    end
end
```

Cinco linhas por tile mais o loop. O script roda uma vez quando o servidor começa e cada jogador conectado vê a grade aparecer.

## Tratamento de cliques

```lua
local function onTileClicked(tile: Part, player: Player)
    print(player.Name, "clicou em", tile.Position)
    -- chamada ao engine: verificar recursos, deduzir, criar modelo de fazenda
end

-- Anexar um ClickDetector a um tile
local detector = Instance.new("ClickDetector")
detector.Parent = tile
detector.MouseClick:Connect(function(player)
    onTileClicked(tile, player)
end)
```

`ClickDetector` funciona porque é filho de uma Part. A conexão é feita no servidor, então o handler roda no servidor — não precisa de RemoteEvent para cliques no mundo.

> **Um detalhe nesse código.** A função que você passa para `:Connect(...)` é uma *closure*. Quando o clique acontece depois, a função ainda lembra da variável `tile` do loop `for` ao redor dela. O Lua mantém essa variável para você. (Você vai ver isso em muito código de evento: você escreve o handler agora, e as variáveis que ele usa ainda estão lá quando ele rodar.)

## Criar um modelo de prédio

```lua
local function spawnFarm(tile: Part)
    local farm = Instance.new("Part")
    farm.Anchored = true
    farm.Size = Vector3.new(6, 4, 6)
    farm.Color = Color3.fromRGB(150, 100, 50)   -- marrom
    farm.Material = Enum.Material.Wood
    farm.Position = tile.Position + Vector3.new(0, 2.5, 0)   -- em cima do tile
    farm.Parent = workspace
end
```

Uma fazenda de verdade seria um `Model` feito de várias Parts, talvez usando um dos modelos gratuitos do Roblox do Toolbox (modelos 3D que você arrasta para dentro). Para aprender, uma caixa marrom é suficiente para significar "uma fazenda."

## Ligando engine e visuais

O fluxo:

```lua
local Kingdom = require(...)
local kingdom = Kingdom.new("Eldoria")
local tileToBuilding = {}    -- mapeia Part do tile → referência do Building no engine

local function tileClicked(tile, player)
    if tileToBuilding[tile] then return end          -- já construído
    if not kingdom.resources:spend("Wood", 10) then  -- o jogador pode pagar?
        print(player.Name, "não tem recursos para uma fazenda")
        return
    end
    local farm = Farm.new("Farm @" .. tile.Position.X .. "," .. tile.Position.Z)
    kingdom:addBuilding(farm)
    spawnFarm(tile)
    tileToBuilding[tile] = farm
    print("Fazenda construída; total agora:", #kingdom.buildings)
end
```

O engine e os visuais ficam em sincronia pelo mapa `tileToBuilding`. **O engine é o único registro verdadeiro.** O que você vê é uma cópia do estado do engine. Se o engine não tem um prédio, o mundo não deve mostrar um.

## Mexa um pouco

Faça os tiles brilhar quando o mouse passa por cima, escutando `ClickDetector.MouseHoverEnter`. Esse é um feedback normal que avisa ao jogador que algo é clicável.

Adicione uma instância `Sound` em `Workspace` e chame `sound:Play()` em cada clique. Agora você tem efeitos sonoros também.

Substitua a caixa marrom por um modelo gratuito do Toolbox (`View → Toolbox`, busque "farm"). Arraste para dentro, coloque em `ServerStorage.Templates` e use `:Clone()` em cada criação em vez de `Instance.new("Part")`.

Adicione um `BillboardGui` que mostra os totais de recursos atuais flutuando acima do kingdom. É uma UI posicionada no mundo 3D.

## O que você acabou de fazer

Você conectou o engine a um mundo 3D. Código do servidor montou uma grade de cinco por cinco tiles no `Workspace`, deu a cada tile um `ClickDetector` e construiu fazendas no clique — tudo rodando pelos métodos `:spend` e `:addBuilding` do engine. O mapa `tileToBuilding` manteve o engine e o mundo visível em sincronia, com o engine no comando. Esta é a lição: o engine é o único registro verdadeiro; o que você vê é uma cópia dele. O mesmo engine que imprimia no console, que gravava JSON, que servia HTTP, que desenhava a página do browser — agora criando Parts no Workspace.

**Conceitos que você já sabe nomear:**

- *Part* — o bloco 3D básico no Roblox; tamanho, posição, cor, material
- *Model* — uma pasta de Parts que selecionam e se movem como uma só
- *`Instance.new(class, parent)`* — criar um objeto enquanto o jogo roda e posicioná-lo
- *`ClickDetector`* — filho de uma Part que dispara eventos de clique no servidor
- *o engine é o único registro verdadeiro* — o que você vê segue ele, nunca o contrário

## Por sua conta

Hora de fechar o livro. Não role de volta para o código — prove para você mesmo, da sua própria cabeça, que criar uma Part pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um novo Script no `ServerScriptService`. Sem olhar, escreva as linhas que criam uma Part com código:

1. Crie ela.
2. Defina o tamanho.
3. Defina a posição.
4. Defina a cor.
5. Coloque no `workspace` como pai.

Clique em Play e procure no Viewport.

<details><summary>Travou? Abra aqui para conferir.</summary>

```lua
local p = Instance.new("Part")
p.Anchored = true
p.Size = Vector3.new(8, 1, 8)
p.Position = Vector3.new(0, 5, 0)
p.Color = Color3.fromRGB(120, 180, 100)
p.Parent = workspace
```

A Part aparece no Viewport e no Explorer sob `Workspace`. A última linha, `p.Parent = workspace`, é a que realmente a coloca no mundo — sem ela, a Part existe mas ninguém vê. `Anchored = true` impede que ela caia.

</details>

## Palavras para adicionar ao glossário

- **Part** — o bloco 3D básico no Roblox.
- **Model** — uma pasta de Parts agrupadas.
- **CFrame** — um valor do Roblox que combina posição e orientação.
- **`Instance.new`** — criar um objeto de uma dada classe enquanto o jogo roda.
- **`ClickDetector`** — filho de uma Part que dispara eventos de clique no servidor.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.6 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.7 apresenta o **Roblox DataStore** — o jeito embutido do Roblox de salvar dados do jogador entre sessões. Salve o kingdom; carregue-o de volta na próxima visita.
