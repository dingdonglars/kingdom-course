# Bônus B3.1 — Git como um DAG (o modelo)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você tem digitado `git add`, `git commit`, `git push` por um ano. Os comandos funcionam; você escreveu um ano de código com eles. O que você provavelmente não fez foi olhar para *o que o git realmente armazena* por baixo. A lição de hoje é o modelo — a imagem que transforma os comandos em algo que você consegue raciocinar em vez de memorizar.

Por que isso importa: a maior parte da confusão com git vem de usar o git sem um modelo. As pessoas digitam comandos que viram outras digitarem, recebem um resultado que não esperavam, entram em pânico, e adicionam `--force` para fazer o pânico desaparecer. Com o modelo na sua cabeça, o git para de ser um mistério. A maioria dos momentos de "eu quebrei o git" viram *"ah, eu entendo o que aconteceu — e aqui está o movimento que conserta isso."*

> **Words to watch**
>
> - **commit** — um snapshot do seu projeto inteiro em um ponto no tempo, mais uma mensagem curta
> - **DAG** — *Directed Acyclic Graph*. Um monte de nós com flechas de mão única; sem ciclos. A história do git é um DAG de commits.
> - **HEAD** — o ponteiro do git para "onde você está agora"
> - **branch** — um ponteiro nomeado para um commit específico; avança à medida que você commita
> - **parent** — o commit no qual um novo commit foi baseado (a maioria dos commits tem um; merges têm dois)

---

## Passo 1 — o que um commit realmente armazena

Um commit não é um diff. É um *snapshot* do seu projeto inteiro — cada arquivo, cada pasta, exatamente como estavam quando você digitou `git commit`. O git armazena de forma inteligente (ele não copia de verdade arquivos que não mudaram), mas você pode pensar em cada commit como o *projeto inteiro* naquele momento.

Cada commit também carrega:

- Uma **message** — sua nota curta sobre o que mudou
- Um **author** + um **timestamp**
- Um ponteiro para o **parent commit** — o commit no qual ele foi baseado
- Um **SHA hash** único — o nome do git para aquele snapshot específico

O hash é a parte que surpreende as pessoas. Quando o git diz `commit 7a3f5b2…`, aquela longa string não é um ID aleatório — é uma impressão digital calculada a partir do conteúdo do commit. Mude um caractere em um arquivo e o hash muda também. O modelo de armazenamento inteiro do git é construído sobre essas impressões digitais. É por isso que dois repos conseguem comparar histórias sem precisar confiar um no outro.

## Passo 2 — branches como ponteiros

Um branch é uma coisa curta e simples: um *ponteiro* para um commit. Só isso. `main` é um nome apontando para um commit específico. `feature/farms` é um nome apontando para outro. O ponteiro avança quando você commita naquele branch.

Isso é a coisa mais útil para entender sobre o git. O branch *não é* uma série de commits; é um rótulo que *atualmente aponta para um*. Os commits atrás dele são acessíveis pela cadeia de parents, mas o branch em si é só o rótulo na ponta.

Isso significa que criar um branch é gratuito — você está apenas escrevendo um novo nome apontando para o mesmo commit. Deletar um branch é quase gratuito — você apaga o rótulo, e os commits permanecem (por um tempo; a gente vai conhecer `git gc` depois).

```
                                    ↑ main
                            +-------+
                            |  C3   |
                            +-------+
                                |
                                v
                            +-------+
                            |  C2   |
                            +-------+
                                |
                                v
                            +-------+
                            |  C1   |
                            +-------+
```

`main` aponta para `C3`. O parent de `C3` é `C2`. O parent de `C2` é `C1`. A cadeia *é* a história.

As flechas apontam de cada commit de volta para o seu parent. Essa direção é o motivo inteiro pelo qual o git é um grafo "directed" — cada commit sabe de onde veio.

Agora crie um branch de feature:

```
                                    ↑ main
                            +-------+
                            |  C3   |   ← feature/farms
                            +-------+
```

Tanto `main` quanto `feature/farms` apontam para `C3`. Nenhum commit foi duplicado. Você adicionou um rótulo.

Agora commite em `feature/farms`:

```
                            +-------+
                            |  C4   |   ← feature/farms
                            +-------+
                                |
                                v
                            +-------+
                            |  C3   |   ← main
                            +-------+
```

`feature/farms` avançou para `C4`. `main` ficou em `C3`. Os dois branches agora diferem por um commit.

Esta é a imagem de cada branch em cada repo, sempre. Uma vez que está claro na sua cabeça, a maioria dos comandos git para de ser um mistério.

## Passo 3 — HEAD

`HEAD` é o nome do git para *onde você está agora*. Geralmente aponta para um branch (que aponta para um commit). Quando você faz `git commit`, aqui está o que realmente acontece:

1. O git faz um novo commit, usando o commit do HEAD atual como parent
2. O git avança o branch para o qual HEAD aponta para o novo commit
3. O próprio HEAD continua apontando para aquele branch

Então o rótulo avança para você, por conta própria.

Se HEAD apontar direto para um commit em vez de um branch, você está em *detached HEAD state* — o jeito educado do git de dizer *"você está flutuando; nenhum branch está te seguindo, então qualquer coisa que você commitar aqui vai ser difícil de encontrar depois."* Não é quebrado; é só um estado. A correção é `git switch -c <novo-nome-de-branch>` para criar um branch de onde você está.

## Passo 4 — olhando o seu próprio DAG

Abra o seu repo kingdom. A visão que você quer é o **Commit Graph** do GitLens — `Ctrl + Shift + P` → *"GitLens: Show Commit Graph"*. Cada commit que você fez aparece como um nó, com links de parent desenhados entre eles e branches mostrados como raias coloridas. Aquela imagem, na tela, é o seu DAG real. Se o GitLens não estiver instalado ainda, instale (barra lateral Extensions → procure *GitLens*).

Escolha um commit no gráfico e clique — o painel do lado direito mostra a mensagem, o parent, o autor, e o diff contra o parent. Perceba que o diff não é o snapshot inteiro — é a *mudança*, calculada a partir do snapshot deste commit e do snapshot do parent. Você está olhando o modelo.

> **Ou no terminal:**
>
> ```powershell
> git log --oneline --graph --decorate --all   # o gráfico
> git show <hash>                               # o diff de um commit
> ```
>
> Mesma imagem, mesmos dados; o painel só desenha mais claramente. Muitos leitores do B3 preferem o CLI para uma olhada de perto — `git log --graph` é rápido, funciona em scripts, e funciona via SSH. Use os dois.

## Mexa um pouco

Rode `git cat-file -p HEAD`. Este é o objeto de commit armazenado bruto — autor, SHA do parent, SHA da tree, e mensagem. Por baixo, o git são *apenas arquivos de texto*.

Rode `git cat-file -p <tree-SHA>` (usando o tree SHA de cima). Trees listam os arquivos e pastas no snapshot. Vá um passo além com `git cat-file -p <file-blob-SHA>` e você vai ver o próprio conteúdo do arquivo. O modelo inteiro são só commits → trees → blobs, cada um nomeado pelo seu SHA.

## O que você acabou de fazer

Você olhou para o git não como um conjunto de comandos mas como uma *estrutura de dados* — um grafo de commits, com branches como rótulos que você consegue mover em cima. Você conheceu os quatro blocos de construção (commit, parent, branch, HEAD) e a regra de que um commit é um *snapshot, não um diff*. Você rodou `git log --graph` contra o seu próprio ano de trabalho e viu o seu DAG real. Esse é o modelo sobre o qual o resto do B3 se constrói.

**Conceitos que você já sabe nomear:**

- **DAG** — o grafo do git: commits com links de parent de mão única, sem ciclos
- **commit** — um snapshot rotulado do projeto inteiro
- **branch** — um ponteiro para um commit; o ponteiro, não a linha de história
- **HEAD** — o ponteiro *"você está aqui"* do git
- **detached HEAD** — HEAD apontando diretamente para um commit, não para um branch

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, siga esta história na sua cabeça: você começa com `main` apontando para o commit `C3`. Você cria um branch `feature/farms` (ele aponta para `C3` também). Você faz um commit em `feature/farms`. Agora responda:

1. Para onde `main` aponta?
2. Para onde `feature/farms` aponta?
3. Quantos commits foram copiados quando você criou o branch?

<details><summary>Travou? Abra aqui para conferir.</summary>

- `main` ainda aponta para **`C3`** — criar um branch não move `main`, e commitar em um branch *diferente* também não o move.
- `feature/farms` aponta para o **novo commit** (chame de `C4`), cujo parent é `C3`. Commitar avançou o ponteiro daquele branch.
- **Zero** commits foram copiados. Um branch é só um novo nome (um ponteiro) — criar um é gratuito.

Se você disse que `main` moveu, lembre-se: um commit só move o branch no qual HEAD está atualmente.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B3.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B3.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo B3.2 pega o modelo e aplica às duas operações que confundem mais as pessoas: `merge` e `rebase`. Com o DAG na sua cabeça, as duas finalmente fazem sentido.
