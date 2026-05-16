# Quiz — Módulo 1.7

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é um `record` em C#?

- **a.** Um tipo de log usado para escrever entradas num arquivo
- **b.** Uma pequena classe de dados imutável — a igualdade compara os campos, o ToString imprime eles, ótima para eventos
- **c.** Uma palavra-chave reservada só para tipos de linha de banco de dados
- **d.** Um tipo de comentário usado para documentar classes

## 2. Por que `EventEngine` criar um `Random` diretamente com `new` é *ruim*?

- **a.** `Random` é lento em tempo de execução; a engine faz tick com frequência demais para usá-lo
- **b.** A engine agora é não-determinística e impossível de testar — você não consegue dizer "com o dado X, espere o evento Y"
- **c.** `Random` é obsoleto e o compilador emite avisos sobre ele
- **d.** Ele usa memória demais ao longo de jogos que rodam por muito tempo

## 3. O que o `_` em `_ => null` (o switch) significa?

- **a.** Uma variável de descarte que sinaliza que você não se importa com o valor
- **b.** O caso "qualquer outra coisa" — corresponde ao que não correspondeu acima
- **c.** As duas coisas acima; o sublinhado faz os dois papéis
- **d.** Um erro de digitação que deveria ter sido `default`

## 4. O que `1 when k.Citizens.Count > 0` significa dentro de um switch?

- **a.** Corresponde ao padrão `1`, mas só se `k.Citizens.Count > 0`. Senão, o próximo padrão é verificado.
- **b.** Sempre corresponde a `1`; a cláusula `when` é um comentário para humanos
- **c.** Um recurso só do C# 12 que foi removido em versões mais novas
- **d.** Um bug — `when` não é permitido dentro de expressões switch

## 5. Por que os testes em `EventLogTests.cs` são tão vagos (*"alguns eventos acontecem"*)?

- **a.** O autor estava com pressa e pulou os testes precisos
- **b.** A engine é não-determinística — não tem como fazer verificações precisas, só frouxas. O Módulo 1.8 introduz `IRandom` para corrigir isso.
- **c.** O xUnit não suporta verificações mais apertadas que isso
- **d.** O Shouldly só lida com correspondências aproximadas; checagens precisas precisam de outra biblioteca

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
