# Quiz — Módulo 0.3

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `new List<string>()` cria?

- **a.** Uma nova variável chamada `"List"` que contém uma única string
- **b.** Uma lista vazia pronta para conter strings
- **c.** Uma lista cujo primeiro item é a palavra literal `"string"`
- **d.** Um erro — listas não podem ser criadas com `new` em C#

## 2. O que `inventory.Add("knife")` faz com a lista?

- **a.** Substitui todos os itens da lista por `"knife"`
- **b.** Acrescenta `"knife"` ao final da lista, aumentando ela em um item
- **c.** Remove `"knife"` da lista, se ele estiver lá
- **d.** Retorna `true` se a lista já contém `"knife"`

## 3. O que `inventory.Contains("knife")` retorna?

- **a.** A posição de `"knife"` na lista (ou `-1` se não estiver)
- **b.** Uma cópia do item se for encontrado, senão uma string vazia
- **c.** `true` se `"knife"` está na lista, `false` se não está
- **d.** A lista inteira, com `"knife"` destacado

## 4. Por que a aventura é dividida em três métodos (`Hallway`, `Kitchen`, `Library`)?

- **a.** Porque três é a menor quantidade que o C# permite para um programa funcionar
- **b.** Para economizar memória — métodos ocupam menos RAM do que cadeias de `if`
- **c.** Porque a estrutura do jogo espelha a estrutura do código; cada método cuida de um cômodo
- **d.** O compilador se recusa a lidar com um único método com mais de cinquenta linhas

## 5. Quando `Hallway()` chama `Kitchen()`, onde o programa continua depois que `Kitchen()` termina?

- **a.** No comecinho do programa, reiniciando ele
- **b.** Dentro de `Hallway()`, na linha logo depois da chamada a `Kitchen()`
- **c.** Onde quer que `Hallway()` tenha sido chamado, pulando o resto de `Hallway`
- **d.** Depende do que o usuário digitou

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
