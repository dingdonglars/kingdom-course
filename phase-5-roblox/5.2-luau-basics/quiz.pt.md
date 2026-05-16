# Quiz — Módulo 5.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `local` faz em Luau?

- **a.** Declara uma variável no namespace global
- **b.** Declara uma variável com escopo limitado à função ou bloco dela — sem `local`, a variável vira global
- **c.** Importa um módulo do `ReplicatedStorage`
- **d.** Uma palavra reservada sem efeito na variável

## 2. Qual é o índice de array do primeiro elemento em Luau?

- **a.** Zero — igual a C# e JavaScript
- **b.** Um — Lua e Luau começam do índice um; a pegadinha diária para cabeças de C# e JavaScript
- **c.** Menos um, contando a partir do fim do array
- **d.** Zero ou um, dependendo da table

## 3. Qual é a sintaxe para juntar duas strings em Luau?

- **a.** `+` — igual a C#
- **b.** `..` — dois pontos; `+` daria um erro de número
- **c.** `&` — emprestado do BASIC
- **d.** Uma chamada de função embutida `concat()`

## 4. Qual é a diferença entre `ipairs` e `pairs`?

- **a.** `ipairs` percorre um array em ordem e para no primeiro nil; `pairs` percorre toda chave de um dicionário em ordem não especificada
- **b.** Nada — são apelidos para a mesma função
- **c.** `ipairs` é a forma mais antiga e agora está obsoleta
- **d.** `pairs` roda mais rápido em tables grandes

## 5. Por que o Luau adiciona anotações de tipo em cima do Lua puro?

- **a.** Elas dão ao Studio informação suficiente para sublinhar o tipo errado conforme você digita — mesmo valor do TypeScript, barato em tempo de execução
- **b.** São obrigatórias para qualquer script que roda no servidor
- **c.** Fazem o interpretador de Lua rodar visivelmente mais rápido em tempo de execução
- **d.** São uma preferência de estilo sem efeito funcional

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
