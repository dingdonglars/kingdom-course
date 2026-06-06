# Bônus B3.4 — Ferramentas — git dentro do VS Code (e outros clientes)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você passou um ano usando o painel Source Control do VS Code como a sua ferramenta de git do dia a dia. A essa altura você também usou CLI o suficiente para saber quando cada um é a escolha certa. A lição de hoje vai mais fundo no painel e no GitLens (a extensão que você instalou lá no Módulo 1.6), e faz um tour pelos clientes GUI dedicados que as pessoas usam quando querem ainda mais.

O ponto principal: continue fazendo o que tem feito. O painel Source Control para o trabalho diário, o CLI para os movimentos que o painel não tem botão (`reflog`, `cherry-pick`, `rebase -i`, scripts, trabalho CI, trabalho em servidor). Chegue a um GUI dedicado (Fork, GitKraken, e assim por diante) só se você descobrir que quer um; muitos desenvolvedores experientes nunca o fazem.

> **Words to watch**
>
> - **Source Control panel** — a GUI git embutida do VS Code; ícone na barra lateral que parece um branch
> - **stage** — marcar uma mudança como parte do próximo commit (a "staging area" ou "index")
> - **hunk** — um bloco contíguo de linhas alteradas em um diff
> - **GitLens** — uma extensão popular do VS Code que adiciona blame inline e visões ricas de histórico
> - **dedicated GUI client** — um app standalone cujo único trabalho é git (Fork, GitKraken, SourceTree, etc.)

---

## Passo 1 — instalar (quase nada)

O próprio git já está instalado (você configurou no dia 1, Módulo 0.0). O VS Code já tem um painel Source Control — não há nada novo para instalar para o básico.

A única extensão que realmente vale ter: **GitLens**. Instale agora se ainda não o fez.

1. No VS Code, abra Extensions (`Ctrl + Shift + X`).
2. Procure por *"GitLens — Git supercharged"* do GitKraken.
3. Clique em **Install**.

GitLens adiciona blame inline em cada linha ("last edited by you, two months ago"), cartões de hover de commits detalhados, e uma visão de gráfico. O nível gratuito é suficiente para tudo que você vai fazer este ano.

## Passo 2 — abrindo o painel Source Control

O painel Source Control é o segundo ícone de cima da barra lateral esquerda do VS Code — parece um branch com um fork. Clique nele. (Ou aperte `Ctrl + Shift + G G`.)

Você vai ver três seções:

- **Changes** — arquivos modificados desde o último commit
- **Staged Changes** — arquivos enfileirados para o próximo commit (depois de `git add`)
- **Caixa de mensagem de commit** — onde você digita a mensagem e clica no checkmark para commitar

A ideia é exatamente o que você tem digitado no terminal:

| No terminal | No painel |
|---|---|
| `git status` | o que o painel mostra por padrão |
| `git add file.cs` | clique no `+` ao lado de um arquivo em *Changes* |
| `git add -p` (interativo) | clique em um arquivo → diff abre → clique no `+` ao lado de linhas específicas |
| `git commit -m "..."` | digite a mensagem na caixa, clique no checkmark |
| `git push` | menu `...` → Push (ou clique no ícone de sync na parte inferior) |

O melhor recurso é a terceira linha: **preparar (stage) linhas individuais**. Na visão de diff, cada hunk alterado tem um ícone `+` no gutter; clicar nele prepara só aquele hunk. Isso é bem mais rápido do que `git add -p` para dividir um commit bagunçado que você acidentalmente fez grande demais.

## Passo 3 — lendo diffs no painel

Clique em qualquer arquivo modificado em *Changes*. O VS Code abre um diff lado a lado: versão antiga à esquerda, nova à direita, linhas alteradas destacadas. Isso é bem mais fácil de ler do que `git diff` para qualquer coisa com mais de algumas linhas.

A mesma visão funciona em commits no histórico: com GitLens instalado, abra a visão de histórico do painel *Source Control* e clique em um commit. Ele mostra o diff para aquele commit, arquivo por arquivo. Útil para *"o que eu realmente mudei no commit de terça-feira passada?"*

Para uma imagem do DAG inteiro, GitLens adiciona uma visão **Commit Graph** (`Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*). É o mais próximo da imagem que você desenhou no B3.1 — branches, commits, e links de parent, tudo na tela. Quando você se pegar rodando `git log --graph --all` mais de duas vezes em uma sessão, mude para a visão de gráfico.

## Passo 4 — quando voltar para o CLI

O painel lida com uns 90% dos movimentos diários. O CLI é mais rápido para:

- **Qualquer coisa em um script** — comandos git em scripts PowerShell, pipelines CI, e assim por diante.
- **Comandos que o painel não mostra** — `git rebase -i`, `git cherry-pick`, `git reflog`, `git fsck`. O painel não tem botão para todo comando; o CLI tem todos eles.
- **Trabalhando via SSH ou no WSL** — você pode não ter o VS Code lá.
- **Quando algo está confuso** — às vezes o painel esconde o que está acontecendo de verdade; largar para o CLI e ler `git status` é mais rápido do que adivinhar o que a GUI está fazendo.

O hábito é: *use os dois, e incline para o que for mais rápido para o movimento que você está fazendo*. Você não precisa escolher um.

## Passo 5 — clientes GUI dedicados (o tour rápido)

Alguns desenvolvedores preferem um único app dedicado para git. Os dois mais populares:

### Fork

[**Fork**](https://git-fork.com/) — rápido, nativo, gratuito para uso pessoal ($50 único para uso comercial). Gráfico visual limpo, rebase interativo fácil, diff lado a lado, terminal embutido. Amado por quem quer uma ferramenta focada que "só faz git." Vale uma olhada se você descobrir que quer mais do que o painel do VS Code oferece.

### GitKraken

[**GitKraken**](https://www.gitkraken.com/) — focado em gráficos; o seu recurso principal é o gráfico de commits. Gratuito para uso não comercial; assinatura para recursos completos. Polido, com opiniões fortes sobre como você deve trabalhar. Alguns acham pesado demais; outros adoram.

Outros nomes que você vai ver: **SourceTree** (gratuito, Atlassian, ficando antigo), **GitHub Desktop** (gratuito, específico para GitHub, mantido simples de propósito), **Sublime Merge** (pago, rápido, do pessoal do Sublime Text). Cada um tem seus fãs.

**A visão honesta:** *a maioria dos desenvolvedores que trabalham na área usa o CLI mais o painel git do editor e nunca toca em um cliente dedicado.* É bom conhecê-los; você não precisa de um. Tente um se você for curioso; volte para o VS Code se não ajudar. O curso não vem com nenhum deles; se você quiser tentar o Fork, instale e veja como parece por uma semana.

## Mexa um pouco

Abra a visão Commit Graph do GitLens no seu repo kingdom. Clique em commits aleatórios e leia o que mudou. Esta é a regra de "leia mais código do que você escreve" aplicada à sua própria história.

No painel Source Control, faça um commit bagunçado de propósito: edite duas coisas não relacionadas em um arquivo, depois prepare (stage) como dois *commits separados* usando os botões `+` linha a linha. Este é o movimento que mantém os commits fáceis de ler quando você tem feito os bagunçados.

Instale o Fork (gratuito para uso pessoal). Abra o seu repo kingdom nele. Tente uma coisa no Fork que você normalmente faria no CLI — digamos, um rebase interativo. Veja se parece mais rápido ou mais lento para você. Desinstale se você não acabar gostando.

## O que você acabou de fazer

Você fechou o B3 conectando o modelo na sua cabeça com as ferramentas na tela que o mostram. O painel Source Control do VS Code lida com uns 90% dos movimentos diários de git e já está instalado; GitLens adiciona blame inline e uma visão de gráfico que combina com o DAG que você desenhou no B3.1. Você aprendeu quando voltar para o CLI (scripts, comandos ocultos, depurar a própria GUI) e conheceu os clientes dedicados (Fork, GitKraken, e outros) como ferramentas *opcionais* — a maioria dos desenvolvedores que trabalham na área nunca chega neles. Você também conheceu a regra de fechamento do bônus: incline para a ferramenta que for mais rápida para o movimento que você está fazendo. CLI e GUI não são uma escolha entre duas coisas; eles são um par que você usa junto.

**Conceitos que você já sabe nomear:**

- **Painel Source Control do VS Code** — sua GUI principal diária de git, sem instalação necessária
- **GitLens** — a única extensão VS Code que ganha o seu lugar para git
- **preparar (stage) linhas individuais** — o melhor recurso do painel para commits arrumados
- **o fallback para o CLI** — scripts, comandos ocultos, quando a GUI confunde
- **clientes GUI dedicados** — Fork, GitKraken, etc.; opcionais, muitas vezes pulável

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória:

1. Para cada um destes comandos de terminal, diga o que você clica no painel Source Control do VS Code para fazer a mesma coisa: `git status`, `git add file.cs`, `git commit -m "..."`, `git push`.
2. Depois nomeie o melhor recurso do painel — o que bate o terminal para arrumar um commit bagunçado.

<details><summary>Travou? Abra aqui para conferir.</summary>

| No terminal | No painel |
|---|---|
| `git status` | o que o painel mostra por padrão (a lista *Changes*) |
| `git add file.cs` | clique no `+` ao lado do arquivo em *Changes* |
| `git commit -m "..."` | digite a mensagem na caixa, clique no checkmark |
| `git push` | menu `...` → Push (ou o ícone de sync na parte inferior) |

O melhor recurso: **preparar (stage) linhas individuais** — na visão de diff, clique no `+` ao lado de um único bloco de mudança (hunk) para preparar só aquela parte. Divide um commit grande demais mais rápido do que `git add -p`.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B3.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B3.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

---

## Bônus B3 — fechamento

O B3 está feito. Você foi de digitar comandos git que tinha meio aprendido para pensar sobre o git como um *DAG de commits com ponteiros móveis*. Você nomeou o tradeoff entre merge e rebase. Você conheceu a rede de segurança — `reflog` e os commits que nunca foram perdidos de verdade. Você conectou o modelo às ferramentas na tela que o mostram. Os comandos que você tem digitado o ano todo não mudaram; o que mudou é a imagem que eles pintam na sua cabeça quando você os digita.

Marque o bônus como completo. **Este aqui é só CLI — o painel não tem botão para tags:**

```powershell
git tag b3-git-properly-complete
git push origin b3-git-properly-complete
```

Não tem PR, não tem revisão — o B3 é um bônus que fica por conta própria. A lição vive nos seus hábitos a partir de agora: *leia o estado antes de agir; o DAG são só commits e ponteiros; reflog é a rede de segurança; a GUI é uma visão do modelo, não o modelo em si.*
