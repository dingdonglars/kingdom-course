# Quiz — Módulo 1.3

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual é a diferença entre `[Fact]` e `[Theory]`?

- **a.** São a mesma coisa; a palavra-chave que você usa é só preferência de estilo
- **b.** `[Fact]` é um teste só; `[Theory]` roda a mesma lógica com entradas diferentes vindas de `[InlineData]`
- **c.** `[Fact]` é para coisas verdadeiras; `[Theory]` é para coisas que você suspeita que podem ser verdadeiras
- **d.** `[Theory]` é uma sintaxe antiga que não roda mais no xUnit moderno

## 2. O que `b.Level.ShouldBe(2)` faz se `b.Level` for `5`?

- **a.** Define `b.Level` como `2` para o teste poder continuar
- **b.** Retorna `false` para a próxima verificação decidir o que fazer
- **c.** Lança uma exceção de asserção, e o executor de testes reporta uma falha
- **d.** Registra um aviso e o teste passa mesmo assim

## 3. Qual é a estrutura convencional de um teste de unidade?

- **a.** Setup / Teardown / Assert
- **b.** Arrange / Act / Assert
- **c.** Build / Test / Deploy
- **d.** Try / Catch / Finally

## 4. Por que testar a engine e não o console?

- **a.** O console é pequeno demais para valer o esforço de testar
- **b.** O código da engine guarda as regras; testá-lo uma vez verifica elas em todo shell que vier depois
- **c.** O console não pode ser testado — não tem como capturar a saída dele
- **d.** É uma escolha de estilo, sem nenhum motivo real por trás

## 5. O que o nome de teste `Spend_WhenInsufficient_ReturnsFalse` te diz?

- **a.** Nada — o nome é só para humanos
- **b.** Método (`Spend`), cenário (fundos insuficientes), comportamento esperado (retorna false). A convenção do `STANDARDS.md`.
- **c.** Que o teste vai gastar alguma coisa durante a execução
- **d.** Que alguém chamado "Spend" escreveu o teste

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
