# Bônus B3.3 — A rede de segurança (reflog e recuperação)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

A coisa que ninguém te conta sobre o git: ele quase nunca perde o seu trabalho de verdade. Mesmo quando você rodou `git reset --hard` e viu commits desaparecerem do `git log`, eles ainda estão lá — o git apenas parou de apontar para eles. O motivo pelo qual a maior parte do pânico de "eu quebrei o git" termina sem trabalho perdido é um único comando: `git reflog`.

A lição de hoje é a rede de segurança. Para ser exato: como encontrar trabalho que parece ter sumido, como se recuperar de erros de `reset --hard`, e como construir o hábito que previne a maioria desses momentos. O hábito é uma frase — *leia o estado antes de agir*. Os comandos abaixo são como você lê o estado.

> **Words to watch**
>
> - **reflog** — o log local do git de onde cada branch e HEAD esteve
> - **dangling commit** — um commit para o qual nenhum branch ou tag aponta, mas ainda está no armazenamento do git
> - **garbage collection (`gc`)** — o git eventualmente deleta objetos verdadeiramente inacessíveis (padrão ~30 dias)
> - **`reset --hard`** — move um branch para um commit diferente e limpa a árvore de trabalho para combinar
> - **rescue** — recuperar trabalho que parece perdido

---

## O modelo: nada foi embora (ainda)

Quando você faz `git commit`, o git armazena o commit no seu banco de dados de objetos, nomeado pelo SHA dele. *Nada apaga aquele objeto diretamente.* Branches, tags, HEAD — todos esses são apenas ponteiros. Quando você "deleta" um commit movendo um branch para fora dele, o commit ainda existe no armazenamento do git. Ele só não é acessível por nenhum ponteiro que você normalmente vê.

`git reflog` é o log extra oculto de *onde os ponteiros costumavam estar*. Toda vez que HEAD se move — checkout, commit, reset, rebase, merge — o git registra o movimento. Cada entrada tem um SHA e uma descrição curta como *"HEAD@{3}: rebase: feature/farms"*.

Essa é a rede de segurança. Enquanto o reflog ainda tiver o SHA, você consegue trazer o commit de volta.

## Passo 1 — usando o reflog

Este módulo inteiro é **só CLI**. O painel Source Control não tem botão para `reflog`, `fsck`, ou os movimentos de resgate construídos sobre eles — esses são exatamente os casos onde o terminal é a ferramenta certa. (Você tem usado o painel o ano todo; esta é a lição onde você vai ficar feliz que o CLI também ainda está lá para você.)

Em qualquer repo, rode:

```powershell
git reflog
```

Você vai ver algo assim:

```
3a4f5b2 HEAD@{0}: commit: M2 close ritual
8c9d1e0 HEAD@{1}: commit: add Mine subclass
b2a3c4d HEAD@{2}: checkout: moving from main to feature/farms
...
```

Esse é o seu último centena ou mais de posições de HEAD, mais novo primeiro. Os números em `HEAD@{N}` significam quantos movimentos atrás, não quão antigo o commit é.

Você pode usar qualquer um desses SHAs como uma referência de commit:

```powershell
git show 8c9d1e0
git checkout 8c9d1e0
git branch rescue 8c9d1e0    # cria um branch apontando para aquele commit
```

## Passo 2 — o resgate canônico de "perdi trabalho"

Você rodou `git reset --hard HEAD~3`. Três commits de trabalho acabaram de desaparecer do `git log`. Pânico.

O resgate:

```powershell
git reflog
# procure a entrada logo antes do reset, ex: HEAD@{1}
git reset --hard HEAD@{1}
```

Só isso. Você moveu HEAD de volta para onde estava antes do reset. Os commits "perdidos" são acessíveis de novo.

Outras formas de fazer a mesma coisa:

```powershell
git reset --hard <SHA-do-reflog>
git switch -c rescue <SHA-do-reflog>     # se preferir criar um branch
git cherry-pick <SHA-do-reflog>          # se quiser só um dos commits de volta
```

O resgate funciona porque os commits nunca foram deletados de verdade — eles só ficaram *órfãos* (nenhum branch ou tag aponta para eles mais). `reflog` mostrou o SHA; todo o resto é só mover um ponteiro.

## Passo 3 — quando a rede de segurança acaba

O garbage collection do git (`git gc`) eventualmente deleta objetos que nada consegue alcançar. O padrão é 30–90 dias. Até então, mesmo commits que você "perdeu" semanas atrás ainda estão no armazenamento.

Depois que o `gc` roda, um commit inacessível foi embora de verdade. Se você acha que perdeu algo antigo, tente:

```powershell
git fsck --lost-found
```

Isso encontra commits dangling e os escreve em `.git/lost-found/`. É um resgate de último recurso; os conteúdos não têm rótulos, então você vai ter que olhar cada um.

## Passo 4 — a disciplina (a lição de verdade)

A maioria dos resgates não é necessária. Eles acontecem porque alguém digitou um comando sem ler em que estado estava antes.

Três coisas importam em qualquer momento:

1. **Onde está HEAD?** (`git status` responde isso — a primeira linha)
2. **O que está modificado, e o que está staged?** (`git status` de novo)
3. **Onde eu quero chegar?** (você, o humano, decide)

Se você não consegue nomear essas três coisas, *não rode o próximo comando ainda*. Leia primeiro. A maioria das histórias de "o git comeu meu trabalho" são na verdade *"eu não sabia de verdade em que estado estava, rodei algo, entrei em pânico, depois rodei outra coisa."*

A regra do resgate: **leia o estado antes de agir.** Para ser exato:

- Antes de qualquer tipo de `reset`: rode `git status` e `git log --oneline -10`. Certifique-se de entender quais commits você está prestes a passar por cima.
- Antes de `force-push`: rode `git log @{upstream}..HEAD` (commits que você tem e o remoto não) e `git log HEAD..@{upstream}` (commits que o remoto tem que você sobrescreveria).
- Antes de um `rebase` de um branch empurrado: pergunte *"alguém mais está puxando isso?"* Se sim, não faça rebase.
- Antes de `clean -f` ou `git restore .`: saiba que você está jogando fora mudanças não commitadas. Não há reflog para essas.

As duas formas reais de perder trabalho são: mudanças não commitadas (nenhum reflog cobre elas) e force-push sobre o seu próprio trabalho (raro; você tem que realmente tentar). Tudo o mais consegue ser recuperado.

## Prática

No seu repo sandbox (ou qualquer repo onde você não se importa de experimentar):

1. Faça três commits.
2. `git reset --hard HEAD~3`. Confirme com `git log --oneline` que você voltou três commits.
3. `git reflog`. Confirme que os três commits ainda estão listados.
4. `git reset --hard HEAD@{1}`. Confirme com `git log --oneline` que os três commits voltaram.

Agora tente a variante cherry-pick:

5. `git reset --hard HEAD~3` de novo.
6. Do `git reflog`, escolha o SHA de um dos três commits (não o mais recente).
7. `git cherry-pick <esse-SHA>`. As mudanças daquele único commit são reaplicadas em cima do HEAD.

Fazer isso algumas vezes constrói o hábito de pensar *"ah, foi só um movimento de ponteiro; consigo colocar de volta."*

## Mexa um pouco

Rode `git fsck --lost-found` no seu repo kingdom. Provavelmente nada — mas se houver qualquer coisa em `.git/lost-found/`, esse é um commit dangling antigo, ainda recuperável até o próximo `gc`.

Rode `git gc --prune=now --aggressive` em um repo de *teste* (não o kingdom). Perceba que depois do `gc`, commits dangling foram embora de verdade. É por isso que a rede de segurança tem um limite de tempo.

Configure um alias útil:

```powershell
git config --global alias.last "log -1 HEAD --stat"
git config --global alias.tree "log --oneline --graph --decorate --all"
```

Agora `git last` e `git tree` são formas curtas para os comandos longos. Aliases economizam tempo de digitação ao longo do ano.

## O que você acabou de fazer

Você conheceu `git reflog` — o log extra oculto do histórico do HEAD que é também a rede de segurança do git. Você aprendeu que commits "perdidos" não são deletados de verdade até que o `gc` rode dias ou semanas depois, então a maioria dos resgates são movimentos de ponteiro, não recuperação de dados. Você trabalhou o resgate padrão: desfaz um `reset --hard` lendo o reflog e resetando de novo. Você nomeou o hábito que previne a maioria dos resgates: leia o estado antes de agir — `git status` e `git log` antes de qualquer movimento que muda a história.

**Conceitos que você já sabe nomear:**

- **reflog** — o log do git de onde HEAD esteve; a rede de segurança
- **dangling commit** — um commit que nenhum ponteiro alcança mas o git ainda armazena
- **`gc`** — eventual deleção de objetos verdadeiramente inacessíveis (~30 dias padrão)
- **a regra do resgate** — leia o estado (`git status`, `git log`) antes de agir
- **as duas formas reais de perder trabalho** — mudanças não commitadas; force-push sobre o seu próprio trabalho

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, passe pelo resgate de memória. Você acabou de rodar `git reset --hard HEAD~3` e três commits desapareceram do `git log`:

1. Qual comando mostra onde HEAD costumava estar?
2. Qual comando te coloca de volta?
3. Depois diga *por que* isso funciona — onde estavam esses commits o tempo todo?

<details><summary>Travou? Abra aqui para conferir.</summary>

```powershell
git reflog
# encontre a entrada de logo antes do reset, ex: HEAD@{1}
git reset --hard HEAD@{1}
```

- `git reflog` é o log oculto de onde HEAD esteve; ainda lista os commits "perdidos" com os SHAs deles.
- `git reset --hard HEAD@{1}` move HEAD de volta para onde estava antes do reset.

Por que funciona: os commits nunca foram deletados. `reset --hard` só moveu um *ponteiro* para fora deles. Eles ficam no armazenamento do git até que `gc` rode (uns 30 dias depois), então o resgate é só um movimento de ponteiro, não recuperação de dados.

Este módulo é só CLI — o painel Source Control não tem botão para `reflog`.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B3.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B3.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo B3.4 fecha o B3 com as *ferramentas* ao redor do git — o painel Source Control do VS Code (a sua GUI principal do dia a dia), mais um tour curto pelos clientes GUI dedicados (Fork, GitKraken). O modelo está na sua cabeça; as ferramentas só o mostram para você na tela.
