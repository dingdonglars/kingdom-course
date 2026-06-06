# Bônus B2.2 — Scaffolding (Contexto Persistente)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Scaffolding são os *arquivos* e *trechos* que tornam cada conversa com IA melhor, por conta própria. Você os configura uma vez, e eles pagam de volta toda vez que você fala com a IA a partir daquele dia em diante. Você já tem um bom scaffold — você tem adicionado a ele desde que o `ARCHITECTURE.md` apareceu no Módulo 4.0. A lição de hoje é sobre verificar o que você tem, apertar isso, e adicionar algumas peças novas que valem o custo.

A ideia é simples. Cada arquivo de scaffold é um texto que a IA lê em cada sessão. Quanto mais barato for o texto (curto, atual, correto), melhor a saída. Enrolação prejudica duas vezes — você paga tokens por ela, e a IA tem que ler além dela para chegar nas partes úteis.

> **Words to watch**
>
> - **scaffold file** — um doc de nível de projeto que a IA lê no início de uma sessão
> - **`CLAUDE.md` / `AGENTS.md`** — arquivos de convenção para ferramentas de IA lerem primeiro
> - **example file** — um trecho curado de *"olhe aqui para o padrão"*
> - **type file** — uma fonte única de verdade para formas de dados (DTOs, tipos, schemas)
> - **architecture doc** — uma página única descrevendo seus projetos e como os dados fluem entre eles

---

## Passo 1 — auditar o que você tem

Abra o seu repo. Você deve ver a maioria desses:

- `CLAUDE.md` (raiz) — carregado automaticamente pelo Claude Code; importa `CLAUDE.md`
- `STANDARDS.md` — convenções de código, nomes e arquivos
- `CLAUDE.md` — regras específicas para IA (flag de modo mais comportamentos)
- `ai-tools.md` — notas voltadas ao aprendiz sobre ferramentas de IA
- `.claude/commands/` — slash commands que você pode digitar no Claude Code (`/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`, `/implementation-help`, `/lesson-review`, `/milestone-review`)
- `GLOSSARY.md` — vocabulário específico do projeto
- `ARCHITECTURE.md` — projetos e fluxo de dados (adicionado no Módulo 4.0)

Isso já é um scaffold de verdade. A verificação não é *"você tem os arquivos?"* — você tem. A verificação é *"eles valem o custo?"*

Para cada arquivo, faça três perguntas:

1. **Ele responde a primeira pergunta mais provável da IA?** (*"Que convenções este projeto usa?"* → STANDARDS. *"Como os projetos estão conectados?"* → ARCHITECTURE.) Se um arquivo não responde uma pergunta real, é só preenchimento.
2. **É curto o suficiente para ser contexto barato?** Cada linha é lida em cada sessão. ARCHITECTURE.md deve ter 30–60 linhas; STANDARDS.md umas 100. Qualquer coisa a mais é demais.
3. **Está atual?** Um arquivo de scaffold que está errado desperdiça tokens *e* manda a IA com confiança para a resposta errada. Atualize depois de cada fase.

## Passo 2 — adicionar arquivos de exemplo

Um hábito que vale adotar: manter arquivos de exemplo pequenos e escolhidos a dedo que a IA pode olhar.

Crie `.claude/examples/01-store-method.md`:

````markdown
# Example: a store method

Pattern used in `KingdomEfStore`:

```csharp
public IReadOnlyList<KingdomSlotInfo> ListSlots(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    return ctx.Kingdoms
        .AsNoTracking()
        .Where(k => k.OwnerSub == ownerSub)
        .OrderBy(k => k.Id)
        .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
        .ToList();
}
```

Notes:
- `ownerSub` is required for security
- `using var ctx` for disposal
- `AsNoTracking` for read-only queries
- Project to a small DTO with `.Select`
````

Quando você pedir para a IA escrever um método *parecido*, aponte para este arquivo: *"match the style of `examples/01-store-method.md`."* A saída combina com o seu padrão imediatamente. Você gastou dez minutos fazendo um exemplo; vai reutilizá-lo dezenas de vezes.

Três a cinco exemplos cobrem a maior parte do que você vai pedir. Não tente cobrir tudo — escolha os padrões que você se encontra explicando de novo e de novo.

## Passo 3 — manter arquivos de tipo em um lugar só

Quando os seus tipos de dados ficam em um único lugar, a IA consegue encontrá-los e para de adivinhar. Em projetos TypeScript, um único `types.ts` faz isso para você. Em C#, a mesma ideia é manter os DTOs em uma pasta (`Dtos/`) sem nenhum outro código nela. A IA lê a pasta, aprende os seus formatos de dados, e para de inventar campos.

Isso não é um arquivo novo que você escreve — é um hábito sobre *onde* você coloca as coisas. Quando os DTOs estão todos em um lugar, a IA os usa corretamente.

## Passo 4 — adicionar um cabeçalho "você está aqui" em arquivos longos

Alguns times colocam um comentário curto no topo de arquivos longos:

```csharp
// File: KingdomEfStore.cs
// Role: EF Core implementation of the kingdom store; CRUD plus slot listing
// Conventions: every method takes string ownerSub first; returns IReadOnly* for read methods
// See also: KingdomEntity, KingdomDbContext, ARCHITECTURE.md
```

Cinco linhas, escritas uma vez, ajudando qualquer um — humano ou IA — que abre o arquivo no meio de uma tarefa. Sem elas, o leitor tem que rolar e descobrir. Com elas, o leitor sabe o papel antes de ler o primeiro método.

Isso é opcional, mas útil para arquivos acima de 200 linhas ou arquivos que fazem várias coisas diferentes.

## Mexa um pouco

Verifique o seu `ARCHITECTURE.md` agora. Abra ele. Está atual? Se qualquer coisa lá estiver desatualizada desde o Módulo 4.0, conserte uma linha. *"Um scaffold desatualizado é pior do que nenhum scaffold"* — esse é o hábito que previne isso.

Adicione uma pasta `.claude/examples/` com dois ou três trechos escolhidos a dedo. Escolha padrões que você explicou mais de uma vez.

Tente um prompt de verdade com e sem o arquivo de exemplo. Compare as saídas. A diferença é o valor do exemplo.

## O que você acabou de fazer

Você verificou o seu scaffold (o contexto de fundo que a IA lê em cada sessão), apertou quaisquer arquivos que estavam desatualizados ou com enrolação, e adicionou duas peças novas: uma pasta `examples/` com trechos escolhidos a dedo, e (opcionalmente) um cabeçalho curto no topo de arquivos mais longos. Cada peça paga de volta para sempre — uma vez que está no repo, cada conversa futura com IA se beneficia sem você fazer nada por prompt. Uns 20 minutos de trabalho; paga de volta pela vida toda do projeto.

**Conceitos que você já sabe nomear:**

- **scaffold file** — doc do projeto que a IA lê no início da sessão
- **example file** — trecho curado mostrando o seu padrão
- **type file** — fonte única de verdade para formas de dados
- **cabeçalho "você está aqui"** — comentário de orientação no topo do arquivo
- **scaffold desatualizado é pior do que nenhum** — verifique e atualize em cada fase

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, rode a auditoria em um arquivo real. Escolha um arquivo de scaffold no seu repo (digamos `ARCHITECTURE.md` ou `STANDARDS.md`), e de memória:

1. Faça as três perguntas que decidem se um arquivo de scaffold vale o custo.
2. Depois responda-as para aquele arquivo.

<details><summary>Travou? Abra aqui para conferir.</summary>

As três perguntas:

1. **Ele responde a primeira pergunta mais provável da IA?** Se não responder uma pergunta real, é só preenchimento.
2. **É curto o suficiente para ser contexto barato?** Cada linha é lida em cada sessão, então mantenha curto.
3. **Está atual?** Um arquivo de scaffold errado desperdiça tokens *e* manda a IA com confiança para a resposta errada — scaffold desatualizado é pior do que nenhum.

Se o arquivo que você escolheu falhou na pergunta 3, conserte uma linha agora.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B2.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B2.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B2.3 cobre **scoping** — enquadramento por tarefa. A outra metade do context engineering: o que você diz *para este único pedido* além do scaffolding.
