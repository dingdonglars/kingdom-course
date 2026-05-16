# Quiz — Módulo 1.4

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que a palavra-chave `virtual` em `Building.Tick` faz?

- **a.** Marca ele como falso ou ainda-não-implementado
- **b.** Permite que subclasses substituam ele pela própria versão delas
- **c.** Faz ele rodar numa máquina virtual
- **d.** Torna ele assíncrono

## 2. Por que `AdvanceDay()` retorna `void` em vez de um novo reino?

- **a.** O C# proíbe retornar uma classe personalizada de um método
- **b.** Ele muda o reino no lugar — um efeito colateral
- **c.** O compilador recusa `Kingdom` como tipo de retorno
- **d.** Retornar qualquer coisa de um método de tick seria um erro de sintaxe

## 3. Em `AdvanceDay()`, por que os prédios fazem tick *antes* dos cidadãos comerem?

- **a.** Ordem alfabética — *buildings* vem antes de *citizens*
- **b.** Desempenho — os laços dos prédios são mais rápidos
- **c.** A ordem importa: as fazendas de hoje produzem antes de os cidadãos comerem hoje
- **d.** Eles fazem tick ao mesmo tempo, então a ordem não importa

## 4. O que é um *efeito colateral*?

- **a.** Qualquer coisa que imprime no console
- **b.** Um método que muda o estado em vez de retornar um valor
- **c.** Um bug não intencional no seu código
- **d.** Um método sem tipo de retorno

## 5. Por que o `[Fact] Spend_NoFood_DoesNotCrash` existe?

- **a.** Para verificar que a engine lida com "sem comida, cidadãos ainda fazem tick"
- **b.** Para encher a contagem de testes
- **c.** Para testar só o valor de retorno de `Spend`
- **d.** Para verificar que `Add` funciona depois de um `Spend` que falhou

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
