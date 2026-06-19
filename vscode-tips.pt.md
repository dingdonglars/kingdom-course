# VS Code — dicas para começar

> Tente primeiro o `vscode-tips.md` em inglês. Use este aqui só quando uma palavra te travar.

> Uma lista curta de coisas que deixam o VS Code com cara de casa, não de um monte de menus. Dê uma olhada rápida depois do Módulo 0.0. Volte aqui sempre que algo parecer estranho.
>
> **Setup assumido abaixo:** Windows + teclado português (Brasil), layout ABNT2. Todos os atalhos usam as teclas que você realmente tem.

O VS Code é a sua ferramenta principal o ano inteiro. Como qualquer ferramenta, a diferença entre "mais ou menos funciona" e "parece uma extensão das suas mãos" são uns dez pequenos hábitos. Aqui estão eles.

## Uma observação sobre o teclado português

Seu teclado português (Brasil / ABNT2) é uma complicação pequena, mas real, para os padrões do VS Code — quase todo tutorial de programação assume um teclado US English. As diferenças que realmente importam para este curso:

| Símbolo | Teclado US | Seu teclado (português ABNT2) |
|---|---|---|
| `\` (backslash) | uma tecla, canto superior direito | Tecla `\|` dedicada, entre o Shift esquerdo e o `Z` |
| `[` | uma tecla | `AltGr + ´` (a tecla de acento agudo morto, à direita do `P`) |
| `]` | uma tecla | `AltGr + [` (a tecla de crase morta, duas à direita do `P`) |
| `{` | `Shift + [` | `AltGr + Shift + ´` |
| `}` | `Shift + ]` | `AltGr + Shift + [` |
| `` ` `` (backtick) | uma tecla, canto superior esquerdo | Tecla morta — aperte a tecla de crase morta sem Shift (duas à direita do `P`), depois `Space` |
| `~` (til) | `Shift + ` ` | Tecla morta — aperte `~` (à esquerda do `´`), depois `Space` |
| `@` | `Shift + 2` | `Shift + 2` (igual ao US) |
| `/` | uma tecla | Tecla `/?` dedicada, à direita do Shift direito, sem Shift |
| `?` | `Shift + /` | `Shift +` a mesma tecla `/?`, à direita do Shift direito |

Você vai escrever `{`, `}`, `[`, `]`, `\` e `` ` `` o tempo todo no código. As duas teclas mortas à direita do `P` (`´` e `[`) são a sua família de colchetes — `AltGr` as transforma em `[ ]`, `AltGr + Shift` em `{ }`. Pratique essas quatro combinações agora até seus dedos decorarem — isso elimina 90% do atrito "onde é que está aquele símbolo?" no primeiro mês.

Se uma combinação de teclas envolve `` ` `` (backtick) e a sua é uma tecla morta, o jeito mais fácil é apertar `` ` `` e logo em seguida apertar `Space` — isso te dá um único caractere de crase.

## Os quatro atalhos que você vai usar cem vezes por dia

Vale a pena decorar estes no primeiro dia. Todos os outros atalhos podem esperar.

| Atalho | O que faz |
|---|---|
| `Ctrl + P` | Abre qualquer arquivo pelo nome. É só começar a digitar. |
| `Ctrl + Shift + P` | Abre a **Command Palette** (a Paleta de Comandos) — todo comando do VS Code está aqui dentro. Esqueceu como fazer algo? Ctrl+Shift+P, digite umas letras. |
| Abrir o terminal | Abre o terminal embaixo. *(O padrão do VS Code é `Ctrl + ` `, mas no ABNT2 a crase é uma tecla morta, então o padrão pode não funcionar. Se não funcionar, veja "Deixe o atalho do terminal sensato" mais abaixo e troque para outra tecla — recomendado `Ctrl + Ç`.)* |
| `Ctrl + B` | Mostra ou esconde a árvore de arquivos à esquerda. Sensação de tela grande numa tela pequena. |

`Ctrl + Shift + P` é o atalho. Se você não lembrar de mais nenhum, lembre desse. Qualquer coisa que o VS Code faz, você acha digitando na Command Palette.

## Mais dois que compensam na hora

- **`Ctrl + /`** — liga ou desliga um comentário na linha onde o cursor está. Selecione várias linhas antes para comentar um bloco.
- **`F12`** — pula para onde uma coisa foi *definida*. Clique no nome de uma classe, aperte `F12`, e o VS Code te leva para o arquivo que a escreveu. `Alt + ←` te traz de volta. Esse parece mágica na primeira vez.

## Extensões para instalar

O VS Code é pequeno por padrão. Você adiciona o que precisa. Para este curso, instale estas agora:

1. **C# Dev Kit** (Microsoft) — a experiência principal de C#. Adiciona IntelliSense, depuração, executor de testes, tudo.
2. **C#** (Microsoft) — instalada automaticamente pelo Dev Kit; se não vier, instale na mão.
3. **GitLens** — histórico do git dentro do editor. Passe o mouse sobre qualquer linha e veja quem a escreveu e quando (vai ser tudo você por um tempo — tudo bem).
4. **Error Lens** — mostra os erros do compilador *na própria linha* do código quebrado, em vez de escondidos numa lista. Pega os erros segundos mais cedo.

Instale pela barra lateral de Extensões (Ctrl+Shift+X) ou digitando na Command Palette: *"Extensions: Install Extensions"*.

Não instale cinquenta extensões por empolgação. Cada uma é uma peça que se move. Adicione conforme o curso precisar.

> **Vai rodar e depurar um programa?** Tem um hábito que deixa isso tranquilo — abra cada programa na sua própria janela (um programa, uma janela). O passo a passo completo, incluindo o que fazer quando o *Run* ou o **F5** se comporta mal, está em `running-your-project.md`. Leia uma vez.

## Às vezes o editor vai te dar trabalho — leia o sublinhado

Três cores de sublinhado ondulado vivem embaixo do seu código:

- **Vermelho** — quebrado. Não compila. Conserte isso antes de qualquer outra coisa.
- **Amarelo / laranja** — aviso. Compila, mas a linguagem está desconfiada. Leia.
- **Azul / cinza** — sugestão. Opcional. Muitas vezes uma melhoria de estilo.

**Passe o mouse sobre o sublinhado.** Aparece uma dica explicando o que está errado. Leia antes de chutar. A maior parte da dor de quem está começando vem de pular a dica e chutar a esmo. A mensagem de erro quase sempre diz exatamente o que está errado — com palavras meio estranhas.

Se a explicação estiver densa demais, cole no Claude. *"O que esse erro de C# quer dizer: ..."* Esse é um uso bom e incentivado da IA.

## O git vive no painel Source Control

Você vai fazer quase todo o seu git de dentro do VS Code, não do terminal. O painel Source Control é o terceiro ícone de cima para baixo na barra lateral esquerda (parece um galho com uma bifurcação) — atalho de teclado `Ctrl + Shift + G G`.

O que você vai fazer aqui todo dia:

- **Stage** (preparar) arquivos — passe o mouse em *Changes* e clique no `+`, ou clique no `+` ao lado de um arquivo para preparar só aquele.
- **Commit** — digite uma mensagem na caixa, clique no **checkmark** azul.
- **Push** — clique em **Sync Changes** embaixo do painel.
- **Ler um diff** — clique em qualquer arquivo modificado em *Changes*; abre o diff lado a lado. É bem mais fácil que `git diff` para qualquer coisa além de poucas linhas.

Dois extras que vale instalar no dia 2 (ou no dia 1 se estiver animado):

- **GitLens** — adiciona o blame na linha ("editado por último por você, 3 semanas atrás") e uma visão Commit Graph que mostra seu DAG visualmente. O nível gratuito já basta. Instale pela barra lateral de Extensões.

O terminal fica para os movimentos que o painel não tem botão: `git reflog`, `git cherry-pick`, rebase interativo, `gh pr`, scripts, debug de CI. A gente vê esses no B3 se você fizer esse bônus.

## Quando algo quebra — os três movimentos de reset

Quando o próprio VS Code parece quebrado (sublinhados vermelhos em código que você não mexeu, o IntelliSense parou, arquivos parecem desatualizados), tente estes na ordem. Cada um é inofensivo.

1. **Reload Window.** Ctrl+Shift+P → *"Developer: Reload Window"*. Reinicia o cérebro do VS Code sem fechar o projeto. Resolve talvez metade dos estados estranhos.
2. **Reiniciar o language server do C#.** Ctrl+Shift+P → *".NET: Restart Language Server"*. Resolve a maior parte do resto. (A Microsoft sugere tentar o *"Reload Window"* acima primeiro, porque reiniciar só o language server às vezes perde informação do projeto.)
3. **Feche e reabra o VS Code.** O reinício nuclear completo. Quase sempre resolve os casos que sobraram.

Se ainda estiver quebrado depois dos três, *aí* é um problema de verdade e vale chamar no `#help`. Mas os três primeiros resolvem talvez 90% dos momentos "o VS Code está estranho".

## O terminal vive dentro do editor

O painel do terminal abre embaixo da janela com o atalho de cima (ou pelo menu *View → Terminal*). Mesmo shell, mesma pasta, sem Alt-Tab para uma janela separada. Rode `dotnet build`, `dotnet test`, `git status` ali mesmo.

Você pode abrir vários terminais (o ícone `+` no painel do terminal) — útil quando um está rodando um servidor e você quer continuar digitando em outro.

Você também pode redimensionar o painel arrastando a borda de cima. Se o terminal estiver ocupando tela demais, arraste para baixo. Se precisar ver mais saída, arraste para cima.

### Deixe o atalho do terminal sensato

Se o padrão `` Ctrl + ` `` não funciona para você (o problema da tecla morta acima), troque para algo que seus dedos alcançam de boa. Recomendação: `Ctrl + Ç` — seu teclado tem uma tecla `Ç` de verdade bem onde os teclados US põem o `;`, e nada no VS Code usa ela.

Como:

1. `Ctrl + Shift + P` → *"Preferences: Open Keyboard Shortcuts"*.
2. Busque: *"Toggle Terminal"*.
3. Clique no ícone de lápis ao lado do comando certo, aperte sua nova combinação, dê Enter.

O mesmo truque conserta qualquer outro atalho cujo padrão envolve uma tecla morta no seu layout.

### Garanta que o PowerShell é o shell padrão

O curso dá os comandos em PowerShell (o shell do Windows). Dentro do terminal do VS Code, veja a lista suspensa no canto superior direito do painel do terminal — deve dizer **PowerShell**. Se disser **Command Prompt** ou **Git Bash**, clique na setinha → *"Select Default Profile"* → **PowerShell**. Reinicie o terminal.

## Configurações que vale ligar logo

Abra as Configurações (Ctrl+,) e busque por estas. Todas são qualidade de vida:

- **Editor: Format On Save** → ligado. Salva uma versão arrumada de todo arquivo. Arquivos C# são formatados automaticamente pela extensão de C#; você não precisa pensar em indentação de novo.
- **Editor: Format On Type** → ligado. Arruma o espaçamento de uma linha no momento em que você termina ela — assim que você digita o `;` no fim de uma instrução, o VS Code alinha tudo para você. O Format On Save arruma o arquivo inteiro quando você salva; este aqui arruma cada linha enquanto você escreve, então o código fica certo o tempo todo, não só depois de salvar. (Vem desligado até você ligar.)
- **Editor: Word Wrap** → `on`. Linhas longas quebram na largura do editor em vez de forçar rolagem horizontal.
- **Files: Auto Save** → `afterDelay`. O VS Code salva seu arquivo um segundo depois que você para de digitar. Menos momentos "esqueci de salvar". Um detalhe que vale saber: esses salvamentos automáticos por tempo **pulam o passo do Format On Save** — o VS Code só arruma ao salvar quando *você* aperta `Ctrl + S` na mão. Então, se você ligou o Auto Save e ficou se perguntando por que salvar nunca limpa seu código, é por isso. É exatamente por isso que o Format On Type (acima) importa: ele arruma enquanto você digita, sem precisar salvar. Ligue esse e o espaçamento fica certo não importa como o arquivo seja salvo.
- **Editor: Tab Size** → `4`. Convenção do C#. (Algumas outras linguagens usam 2 — o VS Code consegue ter configuração por linguagem se um dia você se importar.)

Você pode mudar qualquer uma depois. São padrões que funcionam para quase todo mundo no começo.

### Se o seu C# ainda não está sendo arrumado de jeito nenhum

Se nem salvar nem digitar limpa o seu código, provavelmente o VS Code não foi avisado de *qual* ferramenta deve formatar C#. Você ajusta isso uma vez:

1. Abra qualquer arquivo `.cs`.
2. Clique com o botão direito em qualquer lugar do código → **Format Document With…**.
3. Escolha **Configure Default Formatter…** → escolha **C#** (a da Microsoft).

Depois disso, tanto o Format On Save quanto o Format On Type têm uma ferramenta para chamar, e vão começar a funcionar. Um jeito rápido de testar: estrague a indentação de uma linha de propósito, depois aperte `Ctrl + S`. Ela deve voltar para o lugar.

## Quando a interface do VS Code aparece em português

O VS Code detecta sozinho o idioma do seu Windows. Se ele estiver rodando em português e você preferir em inglês (para os menus baterem com todo tutorial e captura de tela deste curso):

1. `Ctrl + Shift + P` → *"Configure Display Language"*.
2. Escolha `en` (inglês). O VS Code vai pedir para instalar o pacote de idioma se precisar.
3. Reinicie o VS Code.

Se preferir deixar em português, tudo bem também — a ferramenta é sua. Mas as lições vão dizer "Open the Command Palette", não *"Abra a Paleta de Comandos"*, então esteja pronto para traduzir os nomes dos menus de cabeça.

## A coisa que todo mundo demorou demais para aprender

Quando algo não funciona do jeito que você espera, **leia o que está de fato na tela**. A mensagem de erro. A dica do sublinhado. O número da linha no stack trace. O nome do arquivo na trilha lá em cima.

O VS Code é *falante*. Ele te diz o que está errado, onde, e muitas vezes por quê. A maior parte da frustração no começo com o VS Code é frustração de ler rápido demais — passar batido pela resposta que estava bem ali.

Quando você travar, antes de chamar no `#help`, antes de perguntar ao Claude — vá devagar e leia o que está na tela. Duas vezes. O conserto está na leitura mais vezes do que não.
