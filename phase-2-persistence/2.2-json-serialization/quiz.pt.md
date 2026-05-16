# Quiz — Módulo 2.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que introduzir um projeto `Kingdom.Persistence` separado em vez de colocar o código JSON em `Kingdom.Engine`?

- **a.** Para fazer a contagem de arquivos parecer mais impressionante nos commits
- **b.** Outros runtimes (Roblox, banco de dados) não vão usar JSON; manter isso separado deixa a engine livre de dependências indesejadas
- **c.** O .NET exige que o código JSON fique no próprio projeto
- **d.** É a única forma de passar o desafio do marco M3

## 2. O que é um DTO?

- **a.** Um pequeno record só-de-dados, feito sob medida para cruzar uma fronteira (disco, rede)
- **b.** Direct Type Override, um recurso da linguagem C#
- **c.** Default Type Output, um modo do serializador
- **d.** Uma palavra-chave para declarar classes de dados em C#

## 3. Por que `KingdomSummary` é um `record` em vez de uma `class`?

- **a.** Records rodam mais rápido que classes no nível do JIT
- **b.** Records te dão imutabilidade de graça e igualdade por valor, as duas ideais para DTOs
- **c.** Records são o único tipo que o serializador JSON aceita
- **d.** Pura preferência de estilo; qualquer um funcionaria de forma idêntica

## 4. O que `WriteIndented = true` faz?

- **a.** Deixa o JSON com várias linhas e legível; mude para `false` para uso em rede
- **b.** Valida o JSON antes de gravar ele no disco
- **c.** Adiciona números de linha como comentários na saída
- **d.** Criptografa a saída antes de salvar

## 5. Por que `EventLog` não está no record `KingdomSummary`?

- **a.** Esqueceram de incluir; estaria lá, de outra forma
- **b.** O resumo é limitado de propósito — só os campos de que precisamos; módulos posteriores constroem um retrato mais completo
- **c.** O serializador JSON não consegue lidar com listas de objetos
- **d.** EventLog é um campo privado e inalcançável de fora da engine

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
