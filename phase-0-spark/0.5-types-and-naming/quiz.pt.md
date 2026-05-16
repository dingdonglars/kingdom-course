# Quiz — Módulo 0.5

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual valor `int gold = 5 / 2;` coloca em `gold`?

- **a.** `2.5` (o resultado real da divisão)
- **b.** `2` (a divisão inteira descarta a parte fracionária)
- **c.** `3` (o resultado arredondado para o inteiro mais próximo)
- **d.** Um erro de compilação — `5 / 2` não é uma expressão int válida

## 2. O que `(int)3.99` te dá?

- **a.** `3` — o cast trunca em direção ao zero
- **b.** `4` — o cast arredonda para o inteiro mais próximo
- **c.** Um erro de compilação — doubles não podem ser convertidos para ints
- **d.** `3.99` — converter um double para ele mesmo não faz nada

## 3. Qual destas linhas compila?

- **a.** `int x = "5";` — atribuir a string `"5"` a um int
- **b.** `bool ready = "true";` — atribuir a string `"true"` a um bool
- **c.** `string name = 42;` — atribuir o int `42` a uma string
- **d.** `string? nickname = null;` — declarar uma string anulável definida como null

## 4. Por convenção, como uma classe pública deve ser nomeada?

- **a.** `myClass` — camelCase, inicial minúscula
- **b.** `MyClass` — PascalCase, inicial maiúscula
- **c.** `_myClass` — sublinhado no início, inicial minúscula
- **d.** `MY_CLASS` — maiúsculas com sublinhados entre as palavras

## 5. Por que `_camelCase` (com o sublinhado no início) é usado para campos privados?

- **a.** É uma tradição da Microsoft sem nenhum motivo prático por trás
- **b.** Marca eles como diferentes de variáveis locais e parâmetros num relance
- **c.** Faz com que sejam de propósito mais difíceis de ler, para não serem usados sem cuidado
- **d.** É uma exigência de sintaxe — o compilador precisa do sublinhado

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
