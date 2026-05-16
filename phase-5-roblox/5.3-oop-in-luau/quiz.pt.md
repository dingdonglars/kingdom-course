# Quiz — Módulo 5.3

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é uma metatable?

- **a.** Uma table que define comportamento extra para *outra* table — o coração da receita de OOP-via-tables do Lua
- **b.** Um tipo de tabela de banco de dados indexada por metadados
- **c.** Uma table maior usada para guardar outras tables
- **d.** Uma palavra-chave reservada do Lua para declarar objetos

## 2. O que `__index = Building` faz numa metatable?

- **a.** Marca a table com um comentário para o editor
- **b.** Diz à metatable: quando uma chave não é encontrada na instância, procure ela em `Building` — é assim que os métodos são encontrados
- **c.** Constrói um índice das chaves da table para busca mais rápida
- **d.** Um pedaço de sintaxe que o Roblox exige em todo ModuleScript

## 3. Por que usar a sintaxe de dois-pontos `farm:upgrade()` em vez do ponto `farm.upgrade()`?

- **a.** Comportam-se de forma idêntica — os dois-pontos são uma preferência de estilo
- **b.** Os dois-pontos passam `self` implicitamente como primeiro argumento; o ponto não passa, então `farm.upgrade()` fica sem o `self` e lança erro
- **c.** Os dois-pontos são a sintaxe mais antiga e serão removidos numa versão futura do Luau
- **d.** Os dois-pontos são obrigatórios ao chamar uma função de dentro de uma classe

## 4. O que é um ModuleScript no Roblox?

- **a.** Um script que roda automaticamente quando o place começa
- **b.** Um script que define um módulo — ele não roda sozinho; outros scripts chamam `require` nele. Usado para código de engine, bibliotecas, tipos compartilhados.
- **c.** Um LocalScript com um nome diferente no Explorer
- **d.** Uma pasta para agrupar scripts relacionados

## 5. A lição diz "OOP não é palavra-chave `class`." O que isso quer dizer aqui?

- **a.** OOP é agrupar dados com métodos, mais herança. Lua faz isso com tables e metatables em vez de uma palavra-chave `class`. O padrão é o que vale, não a sintaxe.
- **b.** O C# faz orientação a objetos de forma incorreta em comparação à abordagem do Lua
- **c.** Lua não tem orientação a objetos de verdade, só tables
- **d.** Usar ou não OOP é uma preferência de estilo pessoal

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
