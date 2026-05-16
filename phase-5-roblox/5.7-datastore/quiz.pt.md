# Quiz — Módulo 5.7

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. De onde você pode chamar o DataStore?

- **a.** Só de scripts do servidor — o `DataStoreService` é só do servidor por design, por segurança
- **b.** De qualquer lugar — servidor, cliente, ModuleScripts chamados de qualquer um
- **c.** Só de LocalScripts — o DataStore é um armazenamento do lado do cliente
- **d.** Só de scripts no `ReplicatedStorage`

## 2. O que `pcall(fn)` faz?

- **a.** Chama a função de forma assíncrona, retornando um objeto parecido com uma promise
- **b.** A chamada protegida do Lua — retorna `(true, resultado)` se a função rodou limpa, `(false, erro)` se ela lançou erro
- **c.** Uma otimização de desempenho que compila a função antes de rodar
- **d.** Um invólucro obrigatório para qualquer chamada de DataStore, para evitar erros de sintaxe

## 3. Por que `BindToClose` é importante?

- **a.** Quando o servidor desliga (para manutenção, novo deploy ou migração), o Roblox te dá cerca de trinta segundos. `BindToClose` é a sua última chance de descarregar dados não salvos do jogador antes de o processo morrer.
- **b.** É uma decoração opcional que alguns desenvolvedores Roblox adicionam por estilo
- **c.** Obrigatório em todo script do place; o runtime avisa sem ele
- **d.** Uma dica de desempenho ao runtime sobre o tempo de fechamento esperado

## 4. Qual é a regra sobre a frequência de salvamento?

- **a.** Salve a cada tick — perder dados é inaceitável num jogo multiplayer
- **b.** Não salve a cada tick. A cota do DataStore e o custo por chamada, os dois, punem isso. Salve quando o jogador sai, mais a cada cinco minutos mais ou menos, por segurança.
- **c.** Salve uma vez por sessão, só no desligamento
- **d.** Salve quando for conveniente; cotas não se aplicam a cargas pequenas

## 5. A lição cita o Módulo 2.1, o Módulo 2.2, o Módulo 2.4, o Módulo 2.6 e o Módulo 5.7 como o mesmo padrão. Qual é ele?

- **a.** Tirar um retrato do estado da engine, gravar ele em algum lugar, ler ele de volta, reidratar. Mesma disciplina; meio diferente a cada vez.
- **b.** No fundo, são todos persistência baseada em SQL
- **c.** No fundo, são todos persistência baseada em JSON
- **d.** Eles não são realmente o mesmo — só o enquadramento da lição faz eles parecerem iguais

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
