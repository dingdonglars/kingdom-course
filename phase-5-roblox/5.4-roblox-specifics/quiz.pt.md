# Quiz — Módulo 5.4

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Onde fica o estado autoritativo do jogo no Roblox?

- **a.** Em cada cliente — todo jogador tem a própria cópia
- **b.** No servidor — um por place; clientes podem mandar mensagens, mas o servidor valida e aplica elas
- **c.** No `Workspace`, replicado pelo Roblox entre todo mundo
- **d.** Em qualquer um dos lados, dependendo da preferência do desenvolvedor

## 2. O que é um RemoteEvent?

- **a.** Uma mensagem assíncrona de mão única entre cliente e servidor — a fiação que deixa as duas metades de um place multiplayer conversarem
- **b.** Um evento no estilo C# reaproveitado pelo runtime do Roblox
- **c.** Uma ferramenta de log que registra eventos no painel Output
- **d.** A versão do Roblox de async/await numa thread única

## 3. Por que o servidor trata as mensagens do cliente como não confiáveis?

- **a.** Clientes podem ser adulterados — trapaças e exploits modificam o código local. Valide tudo no lado do servidor; nunca confie na entrada bruta do cliente.
- **b.** Desempenho — a validação no servidor é mais rápida que no cliente
- **c.** Exigido pelo Roblox; o runtime lança erro se você pular a verificação
- **d.** Tradição emprestada de engines multiplayer mais antigas

## 4. Para que serve o `ReplicatedStorage`?

- **a.** Guardar dados só do servidor, que os clientes nunca deveriam ver
- **b.** Guardar recursos só do cliente, que cada jogador baixa ao entrar
- **c.** Guardar código e objetos compartilhados entre servidor e clientes — ModuleScripts da engine e RemoteEvents ficam aqui
- **d.** Guardar parts que foram replicadas de algum outro lugar

## 5. Por que "o servidor é a autoridade, o cliente é a apresentação" é a regra?

- **a.** Places multiplayer precisam de exatamente uma fonte da verdade, e ela tem que ser o servidor. Os clientes renderizam o resultado. Sem essa regra, jogadores veem estados diferentes e acusam uns aos outros de lag.
- **b.** É uma otimização de desempenho; o servidor é sempre mais rápido
- **c.** Tradição emprestada de documentação mais antiga do Roblox
- **d.** Exigido pelo runtime do .NET que sustenta o Roblox

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
