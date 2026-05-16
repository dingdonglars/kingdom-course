# Quiz — Bônus B3.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `git rebase` faz com os seus commits?

- **a.** Combina eles num único commit com uma mensagem nova
- **b.** Repete eles em cima de uma nova base, dando a eles novos SHAs
- **c.** Move o ponteiro de pai deles sem tocar no commit em si
- **d.** Apaga eles permanentemente e cria um commit novo

## 2. O que é um merge *fast-forward*?

- **a.** Um merge que roda mais rápido porque os branches não divergiram
- **b.** Um merge que pula a criação de um commit de merge; o ponteiro só desliza para frente
- **c.** Um merge que ignora conflitos e escolhe a versão do branch de origem
- **d.** Um merge que roda em segundo plano enquanto você continua trabalhando

## 3. Por que fazer rebase de um branch *compartilhado* é perigoso?

- **a.** Pode produzir conflitos de merge difíceis de resolver
- **b.** Outras pessoas que baixaram a versão antiga agora têm commits com SHAs diferentes dos do remoto
- **c.** Sobrescreve as mensagens de commit com texto padrão
- **d.** Faz a interface web do GitHub mostrar o histórico errado

## 4. Qual é a diferença entre `--force` e `--force-with-lease`?

- **a.** `--force` é para o repositório local, `--force-with-lease` é para o remoto
- **b.** `--force-with-lease` adiciona uma tag de backup antes do force-push
- **c.** `--force-with-lease` se recusa a fazer o push se o remoto se moveu desde o seu último fetch
- **d.** São apelidos para o mesmo comando

## 5. Quando o rebase é a escolha certa em vez do merge?

- **a.** Quando você quer um histórico linear e limpo no *seu próprio* branch de funcionalidade ainda sem push
- **b.** Quando você está juntando trabalho num branch compartilhado como o `main`
- **c.** Quando você quer que o histórico mostre honestamente o trabalho paralelo
- **d.** Quando você quer evitar mudar qualquer SHA de commit

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
