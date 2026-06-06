# Módulo 5.4 — Especificidades do Roblox: Workspace, Servidor vs Cliente, RemoteEvents

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O Roblox não é só "Lua mais 3D." Ele foi construído para multiplayer desde o início. Todo place é dividido entre *um servidor* e *muitos clientes*. Hoje vamos nomear essa divisão, descobrir qual código fica onde e conhecer os **RemoteEvents** — a forma como clientes e servidor trocam mensagens entre si.

> **Words to watch**
>
> - **Workspace** — a cena 3D ao vivo. Cada parte, modelo e personagem fica aqui.
> - **Server** — roda uma vez nas máquinas do Roblox. Guarda o estado real do jogo. Cada place tem exatamente um.
> - **Client** — roda no dispositivo de cada jogador. Cada jogador conectado tem o seu próprio cliente.
> - **RemoteEvent** — uma mensagem de mão única e assíncrona entre servidor e cliente.
> - **RemoteFunction** — uma mensagem de pedido/resposta; quem chama espera uma resposta.
> - **Replication** — o sync embutido do Roblox que copia mudanças do `Workspace` do servidor para os clientes.

---

## Por que servidor vs cliente importa

Todo jogo Roblox é um jogo multiplayer, até os single-player. Ainda tem um servidor, só com um cliente conectado. Aqui está a regra que importa: **o estado real do jogo fica no servidor.** Tudo que afeta o jogo pertence ao servidor. O cliente é para mostrar o que o servidor diz.

Alguns exemplos para deixar a regra clara:

| Código | Onde roda | Por quê |
| --- | --- | --- |
| Tick do engine (recursos mudam) | Servidor | O servidor é a autoridade; código do cliente pode ser adulterado. |
| Renderização de UI | Cliente | Cada jogador vê uma visão diferente. |
| Efeitos de partícula | Cliente | Só cosmético; rodar cinquenta no servidor daria lag para todos. |
| Salvar e carregar (DataStore) | Servidor | A API do DataStore é só para servidor por design. |
| Detectar um clique do mouse | Cliente | O mouse pertence ao jogador. |
| Reagir a um clique do mouse | Servidor (via RemoteEvent) | O servidor valida e então aplica. |

A regra, em palavras simples: **o cliente mostra e pede; o servidor valida e aplica.**

## O que cada pasta faz

| Pasta | Fica em | Visível para |
| --- | --- | --- |
| `ServerScriptService` | Servidor | Só o servidor |
| `ServerStorage` | Servidor | Só o servidor (dados, blueprints) |
| `ReplicatedStorage` | Ambos | Servidor e clientes (módulos do engine ficam aqui) |
| `StarterPlayerScripts` | Cliente | Cada jogador quando entra |
| `StarterPack` | Ambos | Itens que cada jogador recebe na mochila ao aparecer |
| `Workspace` | Ambos | Replicado para todos os clientes |

`ReplicatedStorage` é a biblioteca compartilhada do place. ModuleScripts do engine ficam lá para que tanto scripts do servidor quanto scripts do cliente possam usar `require` neles.

## RemoteEvent — o link entre os dois lados

Quando um cliente precisa pedir ao servidor para fazer algo, os passos são sempre os mesmos:

1. O servidor tem um `RemoteEvent` em algum lugar em `ReplicatedStorage`.
2. O cliente dispara com `event:FireServer(args)`.
3. O servidor está conectado com `event.OnServerEvent:Connect(function(player, args) ... end)`.

No sentido contrário — servidor para cliente — usa `event:FireClient(player, args)` ou `event:FireAllClients(args)`, e o cliente escuta com `event.OnClientEvent`.

```lua
-- ReplicatedStorage/Events/TickRequest é um RemoteEvent inserido pelo Explorer do Studio.

-- Cliente (LocalScript em StarterPlayerScripts)
local event = game.ReplicatedStorage.Events.TickRequest
event:FireServer(5)   -- "avance 5 dias, por favor"

-- Servidor (Script em ServerScriptService)
local event = game.ReplicatedStorage.Events.TickRequest
event.OnServerEvent:Connect(function(player, days)
    -- valide days; aplique ao kingdom desse jogador; responda
    print(player.Name, "pediu", days, "dias")
end)
```

A regra mais importante ao tratar um RemoteEvent: **o servidor não deve confiar em nenhuma mensagem do cliente.** Verifique a entrada primeiro — `days` é um número razoável? Esse jogador é dono do kingdom que está tentando mudar? — antes de fazer qualquer coisa com ela. É o mesmo cuidado que você usou nos endpoints da API na Fase 3.

## Replication

Quando você muda uma Part no `Workspace` com código do servidor, o Roblox automaticamente copia a mudança para cada cliente conectado. Mova um tijolo no servidor e todos os jogadores veem ele se mover. **Você não escreve o código de rede — o Roblox faz isso.**

A outra metade da regra: uma mudança que um *cliente* faz no `Workspace` não é copiada. Ela é visível só naquele cliente. Então se você quer uma mudança que todos vejam, envie-a pelo servidor com um RemoteEvent.

## Mexa um pouco

Inicie uma sessão de teste no Studio com dois clientes simulados em *Test → Local Server → 2 players*. Rode um script que dispara um RemoteEvent de um cliente e veja o servidor registrar isso para os dois. A mensagem e a resposta aparecem no Output de cada janela.

Tente mudar uma Part no `Workspace` com um LocalScript — mova-a para uma posição diferente, mude a cor. Os outros clientes não veem a mudança. Mova o mesmo código para um Script em `ServerScriptService` e a mudança aparece em todo lugar.

Tente enviar uma mensagem enorme com `FireServer` — uma string de dez megabytes. O Roblox bloqueia você com um erro claro. RemoteEvents são feitos para mensagens pequenas. Arquivos grandes são enviados de outra forma.

## O que você acabou de fazer

Você conheceu a divisão multiplayer que o restante da Fase 5 usa. Um servidor, muitos clientes. O servidor guarda o estado real; o cliente só mostra. RemoteEvents são como os dois lados trocam mensagens. Um RemoteEvent vai em um sentido (`FireServer`, `FireClient`). Um RemoteFunction envia um pedido e espera uma resposta. Você também conheceu a replication — o Roblox copiando as mudanças do `Workspace` do servidor para os clientes por conta própria. É por isso que a maioria dos jogos multiplayer não precisa escrever nenhum código de rede. Lembre-se dessa regra nos próximos quatro módulos: o cliente mostra e pede; o servidor verifica e aplica.

**Conceitos que você já sabe nomear:**

- *servidor vs cliente* — um servidor guarda o estado real; um cliente por jogador
- *RemoteEvent* — uma mensagem de mão única entre os dois lados
- *replication* — o Roblox copiando as mudanças do `Workspace` do servidor para os clientes
- *`ReplicatedStorage`* — biblioteca compartilhada; os dois lados podem usar `require` de módulos aqui
- *a regra do multiplayer* — o cliente mostra e pede; o servidor verifica e aplica

## Por sua conta

Hora de fechar o livro. Não role de volta para o código — prove para você mesmo, da sua própria cabeça, que os dois lados pegaram. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem olhar:

1. Complete essa frase em voz alta: "O cliente ___ e ___; o servidor ___ e ___."
2. Escreva as duas linhas que enviam uma mensagem de um cliente para um servidor — o cliente dispara o evento, o servidor escuta.

<details><summary>Travou? Abra aqui para conferir.</summary>

A regra: **o cliente mostra e pede; o servidor verifica e aplica.** O estado real fica no servidor.

As duas linhas, com um RemoteEvent que já está em `ReplicatedStorage`:

```lua
-- Cliente
event:FireServer(5)

-- Servidor
event.OnServerEvent:Connect(function(player, days)
    print(player.Name, "pediu", days, "dias")
end)
```

A linha do servidor sempre recebe `player` primeiro, e depois o que o cliente enviou. E o servidor deve verificar esse valor antes de usá-lo — nunca confie em uma mensagem do cliente.

</details>

## Palavras para adicionar ao glossário

- **server** — o lado que guarda o estado real do jogo; um por place Roblox.
- **client** — o lado de um jogador; um por jogador conectado.
- **RemoteEvent** — uma mensagem de mão única entre servidor e cliente.
- **RemoteFunction** — uma mensagem de pedido-e-resposta entre servidor e cliente.
- **replication** — o Roblox copiando as mudanças do `Workspace` do servidor para os clientes.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.5 começa o **port do engine** de verdade — `ResourceLedger`, `Citizen`, `Kingdom` — traduzidos para Luau, rodando no servidor. Você vai usar o teste do Módulo 5.3 para verificar que ainda funciona.
