# Quiz — Módulo 4.4

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que dividir a UI em componentes?

- **a.** Exigido por todo framework de frontend moderno antes de você poder renderizar uma página
- **b.** Unidades reutilizáveis com um trabalho só; todo framework copia essa ideia em cima de funções simples
- **c.** Execução mais rápida em tempo de execução na engine de renderização do navegador
- **d.** Tradição da era inicial do jQuery no desenvolvimento frontend

## 2. O que `escapeHtml` previne?

- **a.** Erros de tipo no compilador TypeScript quando valores de string passam pelos templates
- **b.** XSS — Cross-Site Scripting; colar strings controladas pelo usuário no `innerHTML` deixa atacantes injetarem tags `<script>`
- **c.** Erros de CORS ao buscar dados de uma origem diferente da página atual
- **d.** Problemas de layout causados por caracteres especiais em nomes de classe CSS

## 3. O que é delegação de eventos?

- **a.** Escutar num elemento-pai os eventos que sobem (bubbling) de muitos filhos — um handler escala para milhares de itens
- **b.** Um padrão C# para disparar eventos de uma classe para outra no mesmo projeto
- **c.** Necessário para handlers de clique funcionarem em navegadores modernos desde a atualização de 2020
- **d.** O comportamento padrão do navegador quando nenhum ouvinte de evento foi anexado ainda

## 4. Por que os componentes desta lição são "só funções"?

- **a.** Uma restrição do framework — o Vite se recusa a compilar componentes que não são funções
- **b.** Para ensinar a ideia por baixo; React, Vue e Svelte todos adicionam detecção de mudança em cima do mesmo padrão `(dados) => UI`
- **c.** JavaScript puro não permite componentes baseados em classe nos navegadores modernos
- **d.** Funções executam mais rápido que classes na engine V8, por uma margem mensurável

## 5. Os componentes "renderiza-como-string" desta lição retornam strings de HTML. Qual é o custo?

- **a.** Mais lento para árvores grandes, porque cada renderização reconstrói a string e o navegador interpreta tudo de novo; tudo bem em pequena escala
- **b.** Eles não rodam de jeito nenhum sem um framework envolvendo eles primeiro
- **c.** Eles têm problemas de segurança de tipos com que o compilador não consegue ajudar
- **d.** Nenhum — renderizar-como-string é mais rápido e mais seguro que renderizar-como-DOM em todo caso

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
