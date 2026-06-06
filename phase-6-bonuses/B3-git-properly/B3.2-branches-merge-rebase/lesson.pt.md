# Bônus B3.2 — Branches, merge, rebase

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Existem duas formas de combinar trabalho de branches diferentes. Elas produzem histórias diferentes. Ambas têm bons usos. As pessoas online discutem sem fim sobre qual preferir. Por ora, você só quer saber o que cada uma *faz* no DAG, para que você consiga escolher a que se encaixa na situação.

A versão curta: **`merge` mantém a história como ela aconteceu**; **`rebase` reescreve a história.** Nenhuma é errada. O tradeoff é entre *mostrar como o trabalho realmente aconteceu* e *uma história limpa e em linha reta.*

> **Words to watch**
>
> - **fast-forward** — um merge onde a ponta do branch alvo é um ancestor da origem; o git só move o ponteiro
> - **merge commit** — um novo commit com dois parents, juntando duas linhas divergentes
> - **rebase** — repetir os seus commits sobre uma nova base; reescreve os hashes deles
> - **force-push** — `git push --force` (ou `--force-with-lease`); envia uma história reescrita sobre o que está no remoto
> - **upstream** — o branch remoto que o seu branch local rastreia

---

## A configuração sobre a qual a gente vai raciocinar

Você começa em `main` no commit `C3`. Você cria um branch de feature `feature/farms`, faz dois commits nele (`C4`, `C5`), e enquanto isso alguém adiciona um commit ao `main` (`C6`).

```
                            +-------+
                            |  C5   |   ← feature/farms
                            +-------+
                                |
                                v
                            +-------+
                            |  C4   |
                            +-------+        +-------+
                                |             |  C6   |   ← main
                                v             +-------+
                            +-------+--------+    |
                            |  C3   |             v
                            +-------+--------+ ← (was main)
```

Você quer combinar o trabalho de `feature/farms` com `main`. Duas formas.

## Opção A — merge

```powershell
git switch main
git merge feature/farms
```

O git cria um novo commit, `M`, com *dois parents* (`C6` de `main`, `C5` de `feature/farms`). O DAG agora tem um layout de bifurcação e junção:

```
                            +-------+
                            |   M   |   ← main
                            +-------+
                              /     \
                             v       v
                        +-------+   +-------+
                        |  C5   |   |  C6   |
                        +-------+   +-------+
                            |           |
                            v           v
                        +-------+    +-------+
                        |  C4   |    |  C3   |
                        +-------+    +-------+
                            |           |
                            +-----+-----+
                                  v
                              (older)
```

`M` é um **merge commit**. Tem dois parents — o último commit em cada branch — e uma mensagem que o git escreve para você (ou que você escreve).

O que isso mantém: *a verdade de como o trabalho aconteceu.* Você trabalhou em dois branches ao mesmo tempo, e a história mostra isso. Os commits mantêm os seus SHAs originais.

O que custa: o gráfico em `git log --graph` fica mais cheio. Se vinte pessoas fazem merge de vinte branches por semana, fica difícil de ler.

### A exceção fast-forward

Se `main` *não tivesse* movido (ou seja, sem `C6`), o git faria um merge de **fast-forward** — sem merge commit, ele apenas desliza o ponteiro `main` até `C5`. Sem novo commit, sem bifurcação de dois parents. História em linha reta. O git faz isso por conta própria quando consegue; é o caso mais simples.

## Opção B — rebase

```powershell
git switch feature/farms
git rebase main
```

O git pega os seus commits (`C4`, `C5`), levanta eles, os *repete* em cima da ponta atual de `main` (`C6`), e joga fora os originais. Os commits repetidos recebem **novos SHAs** — eles são novos commits, mesmo que a mensagem e as mudanças sejam iguais.

```
                            +-------+
                            |  C5'  |   ← feature/farms
                            +-------+
                                |
                                v
                            +-------+
                            |  C4'  |
                            +-------+
                                |
                                v
                            +-------+
                            |  C6   |   ← main
                            +-------+
                                |
                                v
                            +-------+
                            |  C3   |
                            +-------+
```

A história agora é uma **linha reta**. Não há bifurcação e junção. `git log --graph` lê de cima para baixo como uma história.

O que isso dá: uma história limpa e em linha reta. Revisores adoram. Bisecting (encontrar qual commit quebrou algo) é mais simples.

O que custa: os originais sumiram. Os novos commits têm novos SHAs. Se você já tivesse empurrado `C4` e `C5` para o GitHub, e alguém tivesse puxado, o seu rebase agora se separou do deles — e você não consegue ver. Eles vão ter conflitos na próxima vez que puxarem. Esse é o perigo.

## No painel vs no terminal

Os dois movimentos existem no painel Source Control do VS Code: menu `...` → *Branch → Merge from* ou *Branch → Rebase from*, depois escolha o branch de origem. O painel lida com o caminho comum. Para movimentos avançados (rebase interativo, squash, fixup), vá para o terminal — é onde fica o conjunto completo de opções. A gente vai passar o resto da lição no CLI porque torna *o que realmente está acontecendo* mais fácil de ver.

## A regra geral

- **Faça rebase dos seus *próprios* branches que você ainda não empurrou** para arrumar antes de fazer merge. Seguro; só toca na sua cópia local.
- **Faça merge** quando o trabalho vai *para* um branch compartilhado (`main`). O merge commit é honesto sobre a junção.
- **Nunca faça rebase de branches compartilhados.** Se você empurrou, e outros podem ter puxado, trate a história como *congelada*.

Um fluxo de trabalho comum: você trabalha em `feature/farms`, faz rebase na sua própria máquina para arrumar os seus commits antes de abrir um PR, depois faz `merge` para `main` quando o PR é aprovado. Histórico principalmente em linha reta, e sem surpresas em branches compartilhados.

## Force-push, o perigoso

Às vezes você *precisa* sobrescrever o remoto — por exemplo, quando você fez rebase de um branch que já tinha empurrado. O comando é `git push --force`.

O custo: qualquer colega que tivesse puxado a versão antiga do branch agora tem commits na cópia local deles que não existem mais no remoto. O próximo `git pull` deles vai ficar estranho. Eles vão ter que saber rodar `git reset --hard origin/<branch>`.

A versão mais segura: `git push --force-with-lease`. Faz a mesma coisa, mas o git se recusa a empurrar se o remoto se moveu desde que você buscou a última vez — ou seja, se alguém mais empurrou enquanto isso. Essa é a versão para aprender.

> **Regra:** nunca faça force-push para `main` (ou qualquer branch compartilhado com o time). Faça force-push para os seus próprios branches de feature com moderação e avise (*"heads up, acabei de fazer rebase de `feature/farms`"*) se qualquer outra pessoa tiver puxando eles.

## Prática

Crie um repo sandbox:

```powershell
mkdir $HOME\git-sandbox
cd $HOME\git-sandbox
git init
"line 1" | Set-Content README.md
git add README.md
git commit -m "C1: initial"
"line 2" | Add-Content README.md
git commit -am "C2: add line 2"
git switch -c feature
"feature line" | Add-Content README.md
git commit -am "F1: feature work"
git switch main
"main line" | Add-Content README.md
git commit -am "C3: parallel main work"
```

Agora tente os dois:

**Tentativa de merge:**

```powershell
git merge feature
# resolva conflito se houver (edite README.md, depois git add + git commit)
git log --oneline --graph --all
```

Você vai ver o layout de bifurcação e junção com um merge commit.

**Redefina e tente rebase:**

```powershell
git reset --hard <C3-hash>          # volta para antes do merge
git switch feature
git rebase main
git log --oneline --graph --all
```

Agora linear. Os commits de feature têm novos SHAs.

Compare as duas saídas de `git log --graph` lado a lado. A primeira mantém o layout de dois-branches-ao-mesmo-tempo; a segunda faz parecer que o trabalho aconteceu um passo após o outro.

## Mexa um pouco

Tente `git rebase -i HEAD~3`. O rebase interativo abre um editor onde você consegue reordenar, fazer squash, ou editar os seus últimos três commits. Squash é o movimento que você vai usar mais — combine três commits pequenos em um só antes de fazer merge.

Leia o `git log --graph --all` do seu repo kingdom. Quantos merge commits tem? Quantos lugares onde dois branches se juntaram de volta? Essa é a imagem de como o ano realmente aconteceu.

Tente `git push --force-with-lease` no seu sandbox depois de um rebase. Perceba que funciona quando ninguém mais tocou no branch.

## O que você acabou de fazer

Você conheceu as duas formas de combinar a história do git — `merge` (mantém o layout de dois-branches-ao-mesmo-tempo com um merge commit) e `rebase` (repete os seus commits sobre uma nova base, produzindo uma história em linha reta com novos SHAs). Você aprendeu a regra geral: faça rebase dos seus próprios branches que não empurrou ainda; faça merge para branches compartilhados; nunca faça rebase de história compartilhada. Você conheceu force-push e o mais seguro `--force-with-lease`. Você rodou um sandbox onde viu a mesma situação de dois branches produzir dois layouts diferentes de DAG dependendo de qual você escolheu.

**Conceitos que você já sabe nomear:**

- **fast-forward merge** — sem merge commit necessário; ponteiro desliza para frente
- **merge commit** — um commit com dois parents que junta dois branches
- **rebase** — repete commits sobre uma nova base; novos SHAs, história linear
- **force-push** — sobrescreve o remoto; perigoso em branches compartilhados
- **`--force-with-lease`** — force-push mais seguro que se recusa se o remoto se moveu

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, responda três coisas de memória:

1. O que `merge` faz com a história, e o que `rebase` faz com ela?
2. Qual dos dois muda os SHAs dos commits?
3. A regra geral: qual você usa no seu próprio branch não empurrado, e qual para colocar trabalho em um branch compartilhado como `main`?

<details><summary>Travou? Abra aqui para conferir.</summary>

1. **`merge`** mantém a história como ela aconteceu — ele adiciona um merge commit com dois parents, então o gráfico mostra os dois branches se juntando. **`rebase`** reescreve a história — ele repete os seus commits sobre uma nova base, dando uma linha reta.
2. **`rebase`** muda os SHAs — os commits repetidos são novos commits, mesmo que a mensagem e as mudanças sejam iguais. `merge` mantém os SHAs originais.
3. **Rebase** do seu próprio branch que você ainda não empurrou (seguro — só local). **Merge** para um branch compartilhado como `main`. Nunca faça rebase de um branch que outros podem ter puxado.

Os dois movimentos estão no painel Source Control do VS Code (menu `...` → *Branch → Merge from* / *Rebase from*); o CLI (`git merge` / `git rebase`) está lá para os casos avançados como rebase interativo.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B3.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B3.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo B3.3 é a rede de segurança: `reflog`, recuperar trabalho que você pensou que tinha perdido, e os movimentos de resgate para os quais você chega quando algo parece errado. A regra é *leia o estado antes de agir*; o B3.3 transforma isso em comandos específicos.
