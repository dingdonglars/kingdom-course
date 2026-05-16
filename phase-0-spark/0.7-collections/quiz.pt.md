# Quiz — Módulo 0.7

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual é a diferença prática entre `List<T>` e `Dictionary<K, V>`?

- **a.** São a mesma coisa — sintaxe diferente, comportamento idêntico em tempo de execução
- **b.** `List<T>` é ordenada e indexada por posição inteira; `Dictionary<K, V>` é indexada por chave
- **c.** `List<T>` só guarda números; `Dictionary<K, V>` só guarda strings
- **d.** `List<T>` roda mais rápido em toda operação; `Dictionary<K, V>` é só para conjuntos minúsculos

## 2. O que `inventory.GetValueOrDefault("apple", 0)` faz?

- **a.** Adiciona `"apple"` ao dicionário com valor `0` e retorna `0`
- **b.** Retorna o valor de `"apple"` se ele for uma chave, senão retorna o valor reserva `0`
- **c.** Lança uma exceção quando `"apple"` ainda não é uma chave
- **d.** Retorna `0` independentemente de `"apple"` estar ou não no dicionário

## 3. O que `foreach (var x in list)` faz?

- **a.** Percorre a lista em ordem, colocando cada elemento em `x` um de cada vez
- **b.** Percorre a lista em ordem inversa, do último elemento para o primeiro
- **c.** Ordena a lista em ordem alfabética e depois percorre o resultado ordenado
- **d.** Modifica a lista, removendo cada item conforme visita ele

## 4. O que `inventory.OrderBy(kvp => kvp.Key)` devolve?

- **a.** O próprio dicionário, ordenado no lugar pela chave
- **b.** Uma nova sequência de pares chave-valor ordenada por chave (o original fica intacto)
- **c.** Uma lista só com as chaves, com os valores descartados
- **d.** Um erro de compilação — dicionários não podem ser ordenados com LINQ

## 5. Por que usar `Dictionary<string, int>` para a ferramenta de inventário em vez de `List<string>`?

- **a.** Dicionários sempre rodam mais rápido que listas, então são a escolha padrão
- **b.** Um dicionário acompanha as contagens naturalmente — o mesmo item adicionado duas vezes vira contagem 2
- **c.** Uma lista recusa duplicatas, então não consegue representar vários itens iguais
- **d.** São equivalentes para este caso; a escolha é puramente estilística

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
