# Quiz — Módulo 5.5

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que usar chaves de string (`"Gold"`) em vez de um enum em Luau?

- **a.** Lua não tem enums — strings são o idioma padrão para conjuntos fechados, e o editor do Luau ainda verifica o tipo delas
- **b.** Strings são mais rápidas que enums no interpretador de Lua
- **c.** O Roblox exige chaves de string para qualquer valor guardado numa table
- **d.** Uma preferência de estilo sem diferença funcional

## 2. O que `task.wait(5)` faz?

- **a.** Bloqueia o jogo inteiro por cinco segundos, incluindo todo outro script
- **b.** Pausa *este script* por cinco segundos sem bloquear os outros — o Roblox roda cada script na própria coroutine
- **c.** Espera cinco eventos dispararem num RemoteEvent
- **d.** Dá uma dica ao runtime de que o script terminou

## 3. A lição diz "a engine não liga para o runtime em que ela está." Em quantos runtimes você provou isso até o fim deste módulo?

- **a.** Dois — console e Roblox
- **b.** Três — adicionando a API web no meio
- **c.** Cinco — console, arquivo com JSON e SQLite, API web, navegador e Roblox
- **d.** Dez ou mais, contando cada variação de shell

## 4. O `Kingdom.lua` em Luau é mais curto que o `Kingdom.cs` em C#. Por quê?

- **a.** Luau é uma linguagem mais poderosa que C# em todos os aspectos
- **b.** Luau tem menos cerimônia — sem namespaces, sem diretivas `using`, sem modificadores public/private, sem documentação XML. Mesma ideia, texto menor.
- **c.** A versão em Luau está incompleta e pula funcionalidades
- **d.** C# é prolixo por natureza; o texto mais curto do Luau não é realmente significativo

## 5. Por que o game loop deveria rodar no servidor, e não no cliente?

- **a.** O servidor é autoritativo — rodar o loop em cada cliente significa que cada jogador tem a própria versão do estado, o que é facilmente explorável. O servidor faz tick uma vez; os clientes veem o resultado pela replicação.
- **b.** Desempenho — o hardware do servidor é mais rápido que o de qualquer jogador
- **c.** Clientes não conseguem rodar loops de jeito nenhum no Roblox
- **d.** Tradição emprestada de tutoriais mais antigos do Roblox

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
