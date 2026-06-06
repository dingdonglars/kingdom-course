# Módulo 3.0 — Lendo Código Antes de Escrever

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Todo módulo até agora foi sobre *escrever* código. Hoje é sobre *ler* código que outra pessoa escreveu. Antes de escrever o primeiro endpoint ASP.NET, você vai passar uns trinta minutos lendo dois endpoints reais e dizendo em voz alta o que eles fazem. O que você produz hoje é uma entrada no journal, não um programa. Esse é o módulo inteiro: ler, anotar, fazer commit das anotações.

Ler é uma habilidade que ninguém ensina, mas todo mundo precisa. Um desenvolvedor experiente numa equipe lê cerca de cinco vezes mais código do que escreve. Sabe qual arquivo abrir primeiro. Escaneia em vez de ler palavra por palavra. Nota quando algo está *faltando*. Ninguém nasce sabendo fazer isso. É um hábito que você constrói fazendo de propósito, toda semana, mesmo quando não está caçando um bug.

> **Words to watch**
>
> - **read-don't-write hour** — um tempo reservado só para ler código; sem editar
> - **call graph** — quem chama quem; como uma mudança num método afeta o restante do projeto
> - **smell** — ainda não é um bug; algo que te deixa desconfiado sem estar claramente quebrado

---

## Abrindo a fase — branch `phase-3`

Antes de qualquer código (o porquê está no Módulo 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-3
```

Todo commit desta fase vai para `phase-3`. No Módulo 3.9 (fechamento do M4 + AI Unlock), você abre um pull request para mesclar de volta em `main`.

---

## Por que ler primeiro

A maioria das pessoas aprende assim: lê um tutorial, escreve algo, quebra, conserta. Isso funciona para aprender sintaxe. Mas não ensina *julgamento* — saber por que uma forma de organizar o código é melhor que outra. O julgamento vem de ler muito código e perguntar *"o que essa pessoa sabe que eu não sei?"*

Uma boa meta é uma hora de leitura por semana, de propósito, não só quando você está caçando um bug. Esse é o hábito que este módulo começa.

## O que você vai fazer hoje

Dois arquivos para ler. Não execute. Não edite. Só leia.

O primeiro é pequeno — uma API mínima de amostra pelo time do ASP.NET Core:

> [`https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs`](https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/samples/MvcSandbox/Program.cs)

O segundo é maior — uma API de TODO real por David Fowler, uma das pessoas que projetou o ASP.NET Core:

> [`https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs`](https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Program.cs)

Para cada arquivo, responda estas cinco perguntas no seu `journal/3.0-reading.md` (tem um modelo no fim desta lição). Uma ou duas frases cada. Escreva seu próprio raciocínio com suas palavras — não copie um resumo de outro lugar.

1. **O que este arquivo faz, em uma frase?**
2. **O que ele configura primeiro?** (`builder = WebApplication.CreateBuilder(args);` — o que essa linha realmente faz?)
3. **Liste os endpoints em ordem.** Para cada um: verbo HTTP, caminho, o que retorna.
4. **Encontre uma coisa que te surpreende.** Alguma sintaxe que você nunca viu, ou um padrão que você não reconhece. Anote o que você ainda não entende.
5. **Encontre uma coisa que você acha que entende. Escreva um resumo de uma frase.** Quando cobrirmos isso no próximo módulo, você vai verificar se estava certo.

> Não procure respostas enquanto lê. O ponto é *tentar* entender primeiro, depois *se verificar* depois. Palpites errados fazem parte de como você constrói o julgamento. Um palpite errado é onde a resposta certa vai caber quando você aprender.

## Teste do smell

Enquanto lê, também anote coisas que parecem um pouco erradas. Bases de código reais estão cheias dessas. Exemplos:

- Código que parece copiado e colado, onde um método ajudante teria feito o trabalho
- Nomes de variáveis que não dizem nada (`var thing = ...`)
- Métodos longos fazendo muitas coisas ao mesmo tempo
- Números mágicos — `if (x > 100)` — por que cem? De onde vem esse número?

Na maioria das vezes, com código real, a resposta é *"sim, não é perfeito, mas consertar custaria mais do que deixar como está."* Saber o que consertar e o que deixar é também parte do julgamento de um desenvolvedor experiente. Ler assim é como você aprende a notar.

## O que vem no starter

Este módulo é leitura mais uma entrada no journal. A pasta starter te dá:

- `journal/3.0-reading.md` — um modelo com as cinco perguntas para preencher para cada arquivo

Sem código. Sem testes. O que você guarda são as suas anotações escritas.

O modelo:

```markdown
# Module 3.0 — Reading code

## File 1: aspnetcore/MvcSandbox/Program.cs

1. One-sentence summary:
2. First setup line + what it does:
3. Endpoints (verb, path, returns):
4. Surprise:
5. What I think I understand:

## File 2: davidfowl/TodoApi/Program.cs

1. One-sentence summary:
2. First setup line + what it does:
3. Endpoints (verb, path, returns):
4. Surprise:
5. What I think I understand:

## Smells noticed (across both files)

-
-

## Total reading time
```

## Mexa um pouco

Escolha um terceiro arquivo do repositório `dotnet/aspnetcore` — qualquer coisa que chame sua atenção. Dê quinze minutos, não mais. O que quer que tenha lido, escreva uma frase sobre isso no journal.

Compare os dois arquivos que você leu. Como os estilos são diferentes? `MvcSandbox` é uma demo de uma tela. `TodoApi` é uma aplicação real. Os dois são escritos por pessoas que conhecem bem o framework. Nenhum dos dois está errado. O estilo muda com o trabalho, e notar isso já é um passo à frente.

**Faça commit** do seu `journal/3.0-reading.md` no seu repositório. *"Module 3.0 reading notes"* é uma boa mensagem. (Painel Source Control → preparar → commit → Sync. Ou no terminal: `git add . && git commit -m "Module 3.0 reading notes" && git push`.) Ler e anotar é trabalho de verdade, e um commit de verdade faz aparecer no seu git log. Daqui a três meses você vai rolar por esse commit e pensar *"foi nesse dia que comecei a ler o código de outras pessoas de propósito."*

## Suas anotações são o resultado

As anotações que você escreve hoje são o que você guarda deste módulo. Sessões posteriores vão apontar de volta para elas — *"compare a surpresa que você anotou com o que acabamos de aprender."* O journal percorre todo o curso. Trate-o como qualquer outro commit.

## O que você acabou de fazer

Você passou trinta minutos lendo dois arquivos ASP.NET Core reais escritos pelas pessoas que construíram o framework. Anotou o que viu, incluindo o que te surpreendeu e o que você tentou adivinhar. Sem código, sem testes, só anotações. Isso parece pequeno, mas começa um hábito que distingue desenvolvedores mais fortes: eles leem muito mais código do que escrevem, e leem de propósito. Ao fazer commit do seu `journal/3.0-reading.md` no repositório, você tratou a leitura como trabalho real que conta, não só um passo no caminho para escrever. No próximo módulo você vai conhecer os mesmos padrões por dentro, no seu próprio código.

**Conceitos que você já sabe nomear:**

- **read-don't-write hour** — tempo de leitura reservado de propósito
- **call graph** — seguir quem chama quem por um projeto
- **smell** — um padrão que te deixa desconfortável, não sempre um bug
- **as cinco perguntas** — um conjunto de perguntas que você pode reusar em qualquer arquivo novo

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — da sua própria cabeça, nomeie as cinco perguntas que você fez para cada arquivo que leu hoje. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Pegue uma folha em branco. Sem olhar:

1. Escreva todas as cinco perguntas que você responde para cada arquivo novo.
2. Depois diga por que cada uma te ajuda a entender código que você não escreveu.

<details><summary>Travou? Abra aqui para conferir.</summary>

As cinco perguntas:

1. **O que este arquivo faz, em uma frase?**
2. **O que ele configura primeiro?**
3. **Liste os endpoints em ordem** (verbo, caminho, o que retorna).
4. **Encontre uma coisa que te surpreende** — sintaxe ou padrão que você ainda não conhece.
5. **Encontre uma coisa que você acha que entende** — escreva um resumo de uma frase.

O ponto do conjunto: a pergunta 1 força a visão geral, a pergunta 2 acha a linha de partida, a pergunta 3 mapeia o trabalho real, a pergunta 4 marca o que aprender a seguir, e a pergunta 5 te dá algo para verificar depois. Você pode usar essa mesma lista em qualquer arquivo novo pelo resto da sua vida.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.0 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.0 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.1 começa a API de verdade: fundamentos do HTTP, a configuração de minimal API que você acabou de ler, e o seu primeiro endpoint servindo seu reino pela internet.
