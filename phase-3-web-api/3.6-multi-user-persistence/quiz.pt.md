# Quiz — Módulo 3.6

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que *todo* método da store recebe `ownerSub` como parâmetro?

- **a.** Para deixar a API mais verbosa para fins de documentação
- **b.** Para que quem chamar e esquecer dele receba um erro de compilação em vez de um bug de segurança. Parâmetros obrigatórios são barreiras de proteção.
- **c.** Porque o rastreador do EF Core exige um identificador por usuário em toda chamada
- **d.** Para deixar os testes de unidade mais simples de escrever

## 2. Qual é a diferença entre autenticação e autorização?

- **a.** São grafias diferentes para exatamente a mesma ideia
- **b.** A autenticação responde *quem é você* (login). A autorização responde *o que você tem permissão de fazer* (ex.: acessar este reino).
- **c.** A autorização sempre roda antes da autenticação no ASP.NET
- **d.** Só a autenticação é necessária; a autorização é um enche-linguiça opcional

## 3. Por que preferir `sub` em vez de `email` para identificar o usuário no banco de dados?

- **a.** `sub` é mais curto e economiza alguns bytes por linha
- **b.** `email` pode mudar; `sub` (o id do sujeito) é permanente e globalmente único
- **c.** `email` não aparece no conjunto de claims que o Google retorna
- **d.** É uma tradição sem motivo real por trás

## 4. Por que o teste *entre usuários* (*carregar o reino de outro usuário lança um erro*) é o mais importante?

- **a.** Ele pega a classe de bug em que as invasões do mundo real de fato acontecem. O teste do caminho feliz não perceberia se você esquecesse a cláusula `WHERE OwnerSub = ?`.
- **b.** Ele roda visivelmente mais rápido que os outros testes
- **c.** É exigido pelo xUnit em projetos multiusuário
- **d.** Ele não é especialmente importante em comparação aos outros

## 5. Por que `HasIndex(k => k.OwnerSub)` importa?

- **a.** O EF Core se recusa a compilar sem um índice em toda coluna consultável
- **b.** Toda consulta de listagem é `WHERE OwnerSub = ?`. Sem um índice, o banco varre a tabela inteira — lento conforme os dados crescem. Com um índice, é uma busca direta.
- **c.** Ele habilita criptografia na coluna em repouso
- **d.** É puramente uma preferência de estilo, sem efeito em tempo de execução

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
