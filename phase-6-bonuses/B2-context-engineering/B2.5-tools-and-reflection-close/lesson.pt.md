# Bônus B2.5 — Comparando Ferramentas, depois o Fechamento

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Claude não é o único assistente de IA. Cursor, GitHub Copilot, Claude Code, Continue, Codeium — cada um se encaixa em um trabalho diferente. Hoje a gente nomeia as ferramentas por aí, fala sobre quando cada uma é boa, e depois você escreve a reflexão de uma página que fecha o B2 e o curso ao mesmo tempo.

O ponto de comparar ferramentas não é fazer você usar todas elas. O ponto é aprender os *tipos* — estilo chat, completamento inline, agêntico — para que quando uma nova ferramenta sair no ano que vem (e uma vai sair), você saiba de que tipo é e se vale tentar.

> **Words to watch**
>
> - **chat-style** — você fala, a IA responde; interface web ou terminal
> - **inline completion** — texto fantasma aparece enquanto você digita; Tab para aceitar (Copilot, Cursor, Codeium)
> - **agentic** — a IA escreve, edita e roda comandos em vários arquivos (Claude Code, modo Agent do Cursor)
> - **diff-based** — a IA propõe um diff; você aceita ou rejeita trechos
> - **MCP** *(em-cê-pê)* — Model Context Protocol; o padrão emergente da Anthropic para ferramentas com as quais as IAs conseguem se comunicar

---

## O mapa

| Ferramenta | Estilo | Melhor para |
| --- | --- | --- |
| **Claude (web/desktop)** | Chat | Raciocínio longo, planejamento de refatoração, conversas de design |
| **Claude Code (CLI)** | Terminal agêntico | Edições em vários arquivos, loop de teste e código, trabalho com consciência do projeto |
| **GitHub Copilot** | Inline mais chat | Completamentos dentro do IDE; ótimo em *"termine esta linha"* |
| **Cursor** | Editor (VS Code bifurcado) mais modo Agent | Reescritas completas, edição com consciência do repo, loop de agent forte |
| **Continue** | Plugin de IDE open-source | Modelos auto-hospedados, controle empresarial |
| **Codeium / Tabnine** | Completamento inline | Gratuito, leve, funciona em muitos editores |

Para um projeto de aprendizado como o seu, a stack recomendada:

- **Claude Code** para tarefas não triviais (planejamento, edições em vários arquivos, loops de teste)
- **GitHub Copilot** para completamentos pequenos dentro do IDE
- **Claude desktop** para conversa pura e explicação

Você não precisa de todos eles. Escolha um estilo chat e um inline. Isso é suficiente.

## Quando cada um é bom

Alguns exemplos de qual ferramenta se encaixa em qual tarefa:

- *"Help me think through how to structure the OAuth flow."* → **Claude desktop / Claude (chat)**
- *"Add `LoadRichest` to the EF store, with a test."* → **Claude Code** (agent)
- *"Finish this method I'm typing."* → **Copilot** (inline)
- *"Refactor this 200-line file into four components."* → **Cursor agent** ou **Claude Code**
- *"Translate this Spanish error message."* → **Claude desktop** (ou qualquer ferramenta estilo chat)

Quando você não tem certeza: estilo chat para pensar, inline para digitar, agêntico para fazer.

## Passo — escreva a reflexão

Abra `journal/B2-what-i-learned.md`. O template tem cinco perguntas. Escolha as que ressoam; você não precisa responder todas as cinco.

1. **O que a IA fez bem ao longo do ano?** Escolha uma tarefa para a qual você a usou repetidamente. O que fez funcionar?
2. **O que ela fez mal?** Escolha a pior saída que você aceitou. O que quebrou? O que você aprendeu com a quebra?
3. **Que mudança de fluxo de trabalho você fez?** O que é diferente sobre como você começa uma tarefa hoje vs. no mês um?
4. **Um conselho concreto para alguém começando no ano que vem?** No máximo três frases.
5. **Onde você vai usar IA mais no ano que vem? Onde menos?**

Mire em umas 500 palavras no total. Não é sobre escrita bonita — é sobre pensar em como você pensa (que a reflexão do Módulo 5.8 também tocou). Nomear o que você tem feito é o que transforma um conjunto de hábitos em uma habilidade que você pode levar para qualquer lugar.

## Passo — marcar o bônus

Quando sua reflexão estiver escrita e commitada, marque o bônus. Este aqui é só CLI — o painel não tem botão para tags:

```powershell
git tag b2-context-engineering-complete
git push origin b2-context-engineering-complete
```

## O que vem a seguir (fim genuíno do currículo)

Você terminou o M6 *e* os dois bônus. O curso acabou.

Algumas direções se você quiser, nenhuma obrigatória:

- **Comece um novo projeto.** Qualquer coisa. Suas ferramentas vêm com você.
- **Ajude em um projeto open-source.** Leia o README dele, escolha um `good-first-issue`, e aprenda como um codebase real é cuidado por pessoas que não o escreveram do zero.
- **Ensine alguém.** Escolha a lição que mais te ajudou e escreva para um amigo que está prestes a começar. Explicar algo é o seu próprio próximo nível.
- **Continue praticando.** Leia código, escreva código, leia sobre código. A habilidade continua crescendo com o tempo enquanto você continuar aparecendo.

## O que você acabou de fazer

Você fechou o B2 e o curso inteiro ao mesmo tempo. A reflexão de fechamento é o que você fica — quinhentas palavras sobre o que a IA fez bem, o que ela fez mal, como a forma que você trabalha mudou, e que conselho você daria ao iniciante do ano que vem. Cinco módulos de context engineering — frame, scaffolding, scoping, eval, ferramentas — e você nomeou o que já estava fazendo. A habilidade é sua agora; as ferramentas vêm e vão ao redor dela.

**Conceitos que você já sabe nomear:**

- **chat-style** — fala, a IA responde
- **inline completion** — texto fantasma no editor; Tab para aceitar
- **agentic** — edição de arquivos em vários passos mais uso de ferramentas
- **MCP** — Model Context Protocol; padrão emergente de comunicação com ferramentas
- **sua stack escolhida** — uma ferramenta estilo chat, uma inline, opcionalmente um agent

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória:

1. Nomeie os três *tipos* de ferramenta de IA.
2. Para cada tipo, diga em poucas palavras para que é melhor, e nomeie uma ferramenta daquele tipo.

O ponto não é os nomes das marcas — é os tipos, para que você consiga colocar qualquer nova ferramenta que sair no ano que vem.

<details><summary>Travou? Abra aqui para conferir.</summary>

Os três tipos:

1. **Chat-style** — você fala, a IA responde. Melhor para pensar: planejamento, explicação, conversa de design. Exemplo: Claude desktop.
2. **Inline completion** — texto fantasma enquanto você digita, Tab para aceitar. Melhor para digitar: terminar a linha em que você está. Exemplo: GitHub Copilot.
3. **Agentic** — a IA escreve, edita e roda comandos em vários arquivos. Melhor para fazer: edições em vários arquivos e loops de teste. Exemplo: Claude Code.

O atalho: chat-style para pensar, inline para digitar, agêntico para fazer.

</details>

## O fio condutor — pela última vez

> **A disciplina é sua. As ferramentas vêm e vão. O que você leva com você é a forma como você pensa.**
>
> Um ano. Cinco shells. Um engine. Duas linguagens. Dois bônus. **Você não é mais alguém que fez um curso de programação — você é um programador.**

## Fechamento

1. **Quiz** — abra o `quiz.md` — o último de todos. Anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B2.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B2.5 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.
