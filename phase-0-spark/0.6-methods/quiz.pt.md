# Quiz — Módulo 0.6

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `void` significa como tipo de retorno?

- **a.** O método não recebe parâmetros e ignora qualquer coisa passada para ele
- **b.** O método roda mas não devolve nenhum valor para quem o chamou
- **c.** O método não está definido e vai falhar quando o programa rodar
- **d.** O método roda em segundo plano enquanto o resto do código continua

## 2. Dado `int Square(int x) => x * x;`, quanto é `Square(7)`?

- **a.** `7` — a entrada original é devolvida sem mudança
- **b.** `14` — a entrada dobrada, já que `=>` é açúcar sintático para soma
- **c.** `49` — o corpo `x * x` avaliado com `x = 7`
- **d.** Um erro de compilação — métodos não podem usar `*` diretamente

## 3. Dois métodos estáticos da mesma classe podem compartilhar o nome `Square`?

- **a.** Não — todo nome de método numa classe tem que ser único globalmente
- **b.** Sim, se eles recebem *tipos* de parâmetro diferentes (um par de sobrecarga)
- **c.** Sim, se eles recebem os mesmos parâmetros mas retornam tipos diferentes
- **d.** Sim, se eles têm *nomes* de parâmetro diferentes mas os mesmos tipos

## 4. Qual é a diferença entre um *parâmetro* e um *argumento*?

- **a.** Significam a mesma coisa em C# e são intercambiáveis
- **b.** Parâmetro é o nome na definição; argumento é o valor passado na chamada
- **c.** Parâmetro é o valor passado; argumento é o nome na definição
- **d.** Parâmetros são obrigatórios; argumentos são opcionais e podem ser pulados

## 5. `int AddGold(int a, int b) => a + b;` e `int AddGold(int a, int b) { return a + b; }` são a mesma coisa?

- **a.** Não — a forma com seta roda mais rápido porque pula a palavra-chave `return`
- **b.** Não — a forma com seta é só para propriedades, não para métodos
- **c.** Sim — a forma com seta é uma abreviação de corpo de expressão para a forma em bloco
- **d.** Não — elas parecem iguais mas compilam para código intermediário diferente

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
