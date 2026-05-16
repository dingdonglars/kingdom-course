# Quiz — Módulo 0.8

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `try { A } catch (Exception ex) { B }` faz?

- **a.** Sempre roda `B` primeiro para preparar as coisas, e depois roda `A`
- **b.** Roda `A`; se `A` lançar uma exceção, `B` roda no lugar e a execução continua depois
- **c.** Roda `B` independentemente de `A` ter dado certo ou ter lançado um erro
- **d.** Roda `A` e quebra se qualquer coisa dentro de `A` lançar uma exceção

## 2. Por que capturar tipos específicos de exceção (`IOException`) antes de capturar a `Exception` genérica?

- **a.** O compilador C# se recusa a compilar código que captura `Exception` primeiro
- **b.** Capturas específicas rodam mais rápido que as genéricas em tempo de execução
- **c.** As capturas são testadas de cima para baixo; a específica primeiro deixa cada problema ter o seu próprio tratamento
- **d.** É uma preferência de estilo, sem impacto real no comportamento

## 3. O que é um breakpoint no depurador?

- **a.** Uma linha onde o programa com certeza vai quebrar em tempo de execução
- **b.** Uma linha marcada onde o depurador pausa a execução para você inspecionar o estado
- **c.** O fim de todo método, marcando onde o depurador naturalmente para
- **d.** Uma fronteira de `try`/`catch` que o depurador respeita automaticamente

## 4. No depurador do VS Code, o que `F10` (Step Over) faz?

- **a.** Roda a linha atual e passa para a próxima linha do mesmo método
- **b.** Mergulha no método chamado na linha atual, linha por linha
- **c.** Para o depurador e encerra o programa na hora
- **d.** Reinicia o programa do começo com as mesmas entradas

## 5. O que é a pilha de chamadas (call stack)?

- **a.** Uma pilha de mensagens de erro não lidas esperando para serem mostradas ao usuário
- **b.** A cadeia de chamadas de método que levou de `Main` até a linha atual
- **c.** Uma lista de todo breakpoint definido na sessão de depuração atual
- **d.** Um histórico de toda tecla que o usuário apertou desde que o programa começou

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
