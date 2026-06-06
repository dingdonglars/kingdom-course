# Bônus B2.1 — O que é Context Engineering

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você tem feito context engineering desde o AI Unlock no Módulo 4.0. Provavelmente não chamava assim. Hoje a gente dá um nome a isso. No final do B2, toda vez que você usar uma ferramenta de IA você vai ver quatro partes que pode mudar — em vez de uma caixa preta para a qual você faz perguntas e torce.

A habilidade funciona muito como edição. A maior parte do valor está em dois lugares: as escolhas que você faz *antes* de escrever o primeiro prompt, e a forma como você lê a resposta *depois* de a IA escrever. A parte do meio — a IA realmente escrevendo — é a parte que você não controla. O ponto inteiro do context engineering é gastar o seu esforço nas partes que você controla.

> **Words to watch**
>
> - **context engineering** — a prática deliberada de escolher o que a IA vê antes de ela responder
> - **prompt** — a instrução em si; uma parte do contexto
> - **scaffolding** — contexto de fundo persistente (arquivos do projeto que a IA consegue ler)
> - **scoping** — enquadramento por tarefa — o que está dentro e fora para *este* pedido
> - **eval** — o seu passo de julgamento; a saída se encaixa no seu projeto?

---

## Por que ter um bônus próprio para isso

Você desbloqueou a ajuda de implementação de IA no Módulo 4.0. Quando você chega aqui no final da Fase 5, foram meses de prática — em código, conteúdo, depuração, até em escrever entradas no #wins. O B2 reúne o que você aprendeu até agora e afina isso. Dar um nome à habilidade te torna melhor nela.

O AI Unlock no Módulo 4.0 definiu a regra: você escreve a primeira versão sozinho, depois traz a IA para a segunda. O B2 é sobre *como* você traz a IA — o que você entrega a ela, e como você lê o que vem de volta.

## O frame de quatro passos

Toda vez que você usa uma ferramenta de IA, existem quatro passos:

1. **Prompt** — o que você pede, de forma precisa. A sua mensagem.
2. **Context** — tudo mais que a IA vê: o prompt do sistema, os arquivos do projeto que ela consegue ler, turnos anteriores no chat, e as ferramentas dela.
3. **Output** — o que volta do modelo.
4. **Eval** — o seu julgamento sobre se a saída está certa *para o seu projeto*.

Você consegue mudar três deles. O quarto — a escrita de verdade — você não consegue. Então a regra é gastar o seu esforço nos três que você controla: o prompt, o contexto e o eval.

## Onde cada módulo B2 se encaixa

Os cinco módulos do B2 afinam cada parte do frame:

- **B2.1** (este aqui) — o frame em si, e o que conta como contexto
- **B2.2** — scaffolding (o fundo que fica por aí — `ARCHITECTURE.md`, `STANDARDS.md`, exemplos)
- **B2.3** — scoping (por tarefa: objetivo, não-objetivos, armadilhas, critérios de sucesso)
- **B2.4** — ler a saída com cuidado (o passo eval — APIs inventadas, casos de borda perdidos, desvio de estilo)
- **B2.5** — comparar ferramentas de IA, depois a reflexão de fechamento

## Três modos de falha que valem nomear

Quando a saída da IA é ruim, ela costuma ser ruim de um entre três jeitos. Saber os três nomes é metade do remédio — uma vez que você consegue identificá-los em segundos, você para de aceitá-los.

1. **Saída genérica de tutorial.** A IA não viu o seu projeto, então deu o que funciona em livros didáticos em vez do que se encaixa no seu código. A solução é mais scaffolding (B2.2 cobre isso).
2. **APIs inventadas.** A IA chama métodos que não existem com confiança. A solução é ler cada linha e fazer grep das que parecem erradas (B2.4 cobre isso).
3. **Desvio de estilo.** Sua codebase usa um padrão; a IA te dá um diferente. A solução é apontar para um exemplo existente no seu scoping (*"match the style of `KingdomEfStore.cs`"* — B2.3 cobre isso).

Esses três cobrem quase toda saída ruim de IA que você vai ter. O resto do B2 transforma cada um em um hábito.

## Mexa um pouco

Olhe de volta para três conversas com IA da última semana ou duas. Para cada uma, avalie a saída como boa, razoável ou ruim. Para as ruins, pergunte: *que contexto estava faltando?* Era scaffolding (a IA não sabia do seu projeto), scoping (você não disse o que estava dentro ou fora), ou eval (você aceitou algo que não devia)?

Escolha uma tarefa pequena — algo de cinco linhas. Tente duas vezes. Uma sem contexto (só o objetivo em uma frase), e uma com a receita completa do Módulo 4.0. Compare as duas saídas. A diferença entre elas é o valor do context engineering.

Leia seu `CLAUDE.md` do começo ao fim. Perceba com que frequência a IA foi guiada por ele sem você fazer nada por prompt. O arquivo está fazendo context engineering para você em segundo plano.

## O que você acabou de fazer

Você conheceu o frame de quatro passos — prompt, context, output, eval — e a regra que vem com ele: três dos quatro são seus para mudar, um não é, então gaste seu esforço nos três que você controla. Você também conheceu os três jeitos mais comuns que a saída da IA fica ruim (saída genérica de tutorial, APIs inventadas, desvio de estilo) que cobrem quase toda saída ruim de IA. O resto do B2 transforma esse frame em hábitos específicos.

**Conceitos que você já sabe nomear:**

- **context engineering** — escolher o que a IA vê antes de ela responder
- **o frame de quatro passos** — prompt, context, output, eval
- **scaffolding vs scoping** — fundo persistente versus enquadramento por tarefa
- **eval** — o seu julgamento sobre se a saída se encaixa no seu projeto
- **os três modos de falha** — genérico-de-tutorial, APIs inventadas, desvio de estilo

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória:

1. Nomeie todos os quatro passos do frame.
2. Marque quais três você consegue mudar e qual um você não consegue.
3. Diga a regra que segue disso.

<details><summary>Travou? Abra aqui para conferir.</summary>

Os quatro passos:

1. **Prompt** — o que você pede. (Você controla isso.)
2. **Context** — tudo mais que a IA vê: prompt do sistema, arquivos do projeto, turnos anteriores, ferramentas. (Você controla isso.)
3. **Output** — o que volta do modelo. (Você *não* controla isso.)
4. **Eval** — o seu julgamento sobre se a saída se encaixa no seu projeto. (Você controla isso.)

A regra: gaste seu esforço nos três que você controla — prompt, context e eval — não no que você não consegue.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B2.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B2.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B2.2 vai mais fundo no **scaffolding** — os arquivos de fundo que mantêm cada conversa com IA no seu estilo sem você reexplicar toda vez.
