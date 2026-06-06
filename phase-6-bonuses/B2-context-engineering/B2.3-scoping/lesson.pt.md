# Bônus B2.3 — Scoping (Enquadramento por Tarefa)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Scaffolding prepara o palco. Scoping é o que você diz *para esta tarefa específica.* A lição de ontem foi sobre os arquivos que a IA lê em cada sessão. A de hoje é sobre o enquadramento por prompt que transforma um pedido vago em um pedido claro. Scoping é a diferença entre *"escreva um método que encontra o reino com mais ouro"* e *"em `KingdomEfStore.cs`, adicione `LoadRichest(string ownerSub)` retornando um `KingdomSlotInfo?`, correspondendo ao estilo de projeção de `ListSlots`, com um teste em `SlotCrudTests.cs`."*

O primeiro prompt faz a IA adivinhar os seus limites. O segundo diz. Três minutos digitando o segundo economizam dez minutos limpando o que volta.

> **Words to watch**
>
> - **scoping** — definir o que está dentro e fora da tarefa da IA para este pedido específico
> - **non-goal** — algo que a IA *não* deve fazer
> - **trap** — uma armadilha conhecida para chamar atenção explicitamente
> - **success criterion** — como você vai julgar "feito"
> - **single example** — um trecho concreto de referência, na própria mensagem

---

## Por que scoping importa

Sem scoping, a IA adivinha os seus limites. Quando ela adivinha, ela tende a adivinhar *mais* — o que geralmente significa:

- Reescrever código próximo que você não pediu (muitas vezes mal).
- Adicionar recursos que você não pediu (*"I also added validation!"*).
- Explicar longamente quando você queria código.
- Escrever muito código quando você queria uma explicação.

Um prompt com scope para todos os quatro ao declarar o objetivo, os não-objetivos, o formato que você quer, e como você vai julgar "feito" de antemão.

## O template de scoping

Você já tem isso do Módulo 4.0 — fica como o slash command `/implementation-help` em `.claude/commands/implementation-help.md`. Os cinco campos que ele pede são:

```markdown
**Goal (one sentence):**

**File path and surrounding context:**

**Relevant existing code (paste 1-3 small snippets):**

**Conventions to follow:**

**What you should NOT do:**

**My understanding:** I'll be asked to explain each line you write.
```

O B2.3 faz uma atualização — adicione critérios de sucesso explícitos:

```markdown
**Done when:**
- The new method appears in `KingdomEfStore.cs` between `Delete` and `ListSlots`
- Existing tests still pass
- One new test exists in `tests/.../SlotCrudTests.cs`
- Output ends with the explanation prompt
```

Agora você consegue ler a resposta e marcar a lista. Um "feito" claro.

## Exemplo trabalhado

**Prompt ruim:**

> me: write me a method that finds the kingdom with the most gold for a given user

A IA inventa um tipo `Kingdom`, usa LINQ em uma `List<Kingdom>`, e ignora a sua store completamente. A saída não tem nada a ver com o seu projeto.

**Prompt melhor:**

> me: in `KingdomEfStore.cs`, add `LoadRichest(string ownerSub): KingdomSlotInfo?` that returns the user's slot with the highest gold (or null if no kingdoms). Match the projection style of `ListSlots`. Add one test in `SlotCrudTests.cs` that creates three kingdoms with different gold values and asserts the right id comes back. Don't load full Kingdom entities — project to summary inline.

A IA escreve o método certo, no estilo certo, no arquivo certo, com um teste de verdade. Três minutos digitando o prompt economizam dez minutos de limpeza.

## O fallback de três frases

Se o template completo parecer pesado para uma tarefa pequena, volte para três frases:

1. **Em `<arquivo>`, adicione `<assinatura do método>` que faz `<uma frase>`.**
2. **Combine o estilo de `<método existente ou exemplo>`.**
3. **Não faça `<armadilha conhecida>`.**

Três frases cobrem uns 80% dos pedidos pequenos. Use o template completo quando o pedido for maior que umas 30 linhas.

## Passo — escreva o seu próximo prompt assim

Escolha a próxima tarefa que você estava planejando pedir para a IA. Antes de enviar o prompt, escreva no formato de três frases. *Aí então* envie. Compare o que volta com o que você teria recebido da versão vaga.

Você vai perceber a diferença rapidamente. Essa é a lição inteira.

## Mexa um pouco

Pegue um prompt vago recente que você usou. Reescreva como um prompt com scope. Envie os dois — mesma tarefa, mesmo modelo, palavras diferentes. Compare as saídas lado a lado.

Escolha a pior saída que você teve no último mês. Descubra: foi scaffolding, scoping ou eval que falhou? Muitas vezes é scoping — o prompt não disse o que estava dentro ou fora, então a IA preencheu a lacuna com adivinhações.

Adicione mais dois arquivos de exemplo em `.claude/examples/`. Aponte para eles nos seus próximos cinco prompts com scope. Observe quantas vezes a IA combina com o exemplo.

## O que você acabou de fazer

Você conheceu o template de scoping (objetivo, não-objetivos, armadilhas, critérios de sucesso) e o fallback de três frases para pedidos menores. Você viu um exemplo trabalhado onde um prompt vago produziu saída sem relação e um prompt com scope produziu exatamente o método que você pediu. O ponto do scoping é simples: um prompt é um acordo, e a IA entrega de acordo com o acordo que você escreveu — acordo vago, entrega vaga.

**Conceitos que você já sabe nomear:**

- **scoping** — enquadramento por tarefa para este pedido específico
- **non-goal** — a linha explícita de "não faça isso"
- **trap** — uma armadilha conhecida chamada atenção de antemão
- **success criterion** — como você julga "feito"
- **o prompt de três frases** — fallback para pedidos pequenos

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, escreva um prompt com scope de memória. Escolha algo real que você poderia pedir para a IA — digamos, adicionar um método pequeno à sua store:

1. Escreva no formato de três frases, preenchendo os espaços para a sua própria tarefa.
2. Depois leia de volta — a IA teria que adivinhar alguma coisa?

<details><summary>Travou? Abra aqui para conferir.</summary>

O formato de três frases:

1. **Em `<arquivo>`, adicione `<assinatura do método>` que faz `<uma frase>`.**
2. **Combine o estilo de `<método existente ou exemplo>`.**
3. **Não faça `<armadilha conhecida>`.**

Um exemplo preenchido:

> Em `KingdomEfStore.cs`, adicione `LoadRichest(string ownerSub): KingdomSlotInfo?` que retorna o slot do usuário com mais ouro. Combine o estilo de projeção de `ListSlots`. Não carregue entidades Kingdom completas — projete para o resumo inline.

Se o seu prompt nomeia o arquivo, a assinatura, o estilo para combinar, e uma coisa para não fazer, a IA não tem nada para adivinhar.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B2.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B2.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B2.4 cobre **ler a saída com cuidado** — o passo eval. A outra metade de fazer isso bem: pegar o que escorrega por entre os dedos.
