# Quiz — Módulo 1.6

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `.Where(b => b.Level > 1)` retorna?

- **a.** Um `bool` dizendo se algum item correspondeu ao predicado
- **b.** Uma nova sequência contendo só os itens cujo `Level > 1`
- **c.** O primeiro item com `Level > 1`, ou `null` se não houver nenhum
- **d.** Sempre `null`; `Where` é um método de efeito colateral

## 2. Qual é a diferença entre `.First(...)` e `.FirstOrDefault(...)`?

- **a.** Os dois se comportam de forma idêntica; o segundo nome é só um apelido obsoleto
- **b.** `First` lança erro se nenhum item corresponder; `FirstOrDefault` retorna `default(T)` (ex.: `null`, `0`)
- **c.** `First` é mais rápido em listas grandes; `FirstOrDefault` é mais lento
- **d.** `FirstOrDefault` não aceita um predicado; `First` aceita

## 3. O que é um *predicado*?

- **a.** Uma classe que guarda regras sobre uma coleção
- **b.** Uma função que retorna `bool` — o tipo de função que `Where` e `Any` aceitam
- **c.** Uma palavra-chave do C# reservada para expressões de consulta
- **d.** Um namespace que contém os métodos do LINQ

## 4. A sintaxe `b => b.Level > 1` é chamada de...

- **a.** Expressão lambda — uma função escrita em linha, sem precisar de nome
- **b.** Restrição genérica que limita quais tipos podem ser passados
- **c.** Classe anônima com um método nela
- **d.** Método assíncrono declarado com a sintaxe de seta

## 5. Por que `.OfType<Farm>()` costuma ser melhor que `.Where(b => b is Farm)`?

- **a.** Têm o mesmo resultado, mas `OfType<Farm>` devolve os itens já com o tipo `Farm`, então você pode usar membros só-de-Farm sem um cast
- **b.** `OfType<Farm>` é mais rápido em tempo de execução; `Where(b is Farm)` percorre a lista duas vezes
- **c.** `Where` é obsoleto e deve ser evitado
- **d.** Eles produzem resultados diferentes — `OfType<Farm>` inclui subclasses de Farm; `Where(b is Farm)` não

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
