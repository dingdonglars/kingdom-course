# Módulo 5.1 — Tour pelo Roblox Studio

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Roblox é uma das maiores plataformas de jogos do mundo, e seus amigos já estão lá. O **Roblox Studio** é o editor para criar jogos nela. É gratuito e baixa em alguns minutos. Hoje vamos instalar, abrir o mundo padrão, clicar nos painéis e rodar o primeiro script. Ainda não tem código de verdade. O objetivo é aprender onde fica cada coisa, para que os próximos sete módulos não percam os primeiros dez minutos com isso.

> **Words to watch**
>
> - **Roblox Studio** — o editor. Windows ou Mac, gratuito.
> - **Place** — um mundo Roblox. O que você publica.
> - **Workspace** — a cena 3D ao vivo. Tudo que é visível e tocável.
> - **Explorer** — a árvore de objetos do Studio. Mostra cada objeto do place.
> - **Properties panel** — o inspetor do objeto selecionado.
> - **Output panel** — o console do Studio. `print(...)` e erros aparecem aqui.

---

## Abertura de fase — branch `phase-5`

Antes de qualquer código (o Módulo 1.1 explica o porquê):

```powershell
cd C:\code\kingdom
git switch -c phase-5
```

Todos os commits desta fase vão para `phase-5`. No Módulo 5.8 (o fechamento M6, o último), você abre um pull request para trazer tudo de volta para `main`.

---

## Passo 1 — instalar

Acesse <https://create.roblox.com/> e clique em *Start Creating*. A página pede para você instalar o Roblox Studio (cerca de 150 MB). Faça login com a sua conta Roblox, ou crie uma. Quando terminar a instalação, clique duas vezes em *Studio* para abrir.

## Passo 2 — abrir o Baseplate

Quando o Studio abre, você escolhe um template. Escolha **Baseplate** — o mais simples, uma placa verde plana flutuando no ar. Depois de abrir, você verá quatro áreas na tela:

- O **Viewport** no centro — o mundo 3D, com a placa verde.
- O **Explorer** à direita — uma árvore de cada objeto do place: `Workspace`, `Players`, `Lighting`, `ReplicatedStorage`, `ServerScriptService`, e mais alguns.
- O **Properties panel** abaixo do Explorer — quando você clica em um objeto, os campos dele aparecem aqui: Name, Size, Material, Color.
- O **Output panel** na parte de baixo — o console do Studio. Se não estiver visível, abra em *View → Output*.

Abra o Output panel antes de fazer qualquer outra coisa. Erros e chamadas de `print` aparecem lá, e você vai querer ele visível no momento em que algo rodar.

## Passo 3 — o seu primeiro script

No Explorer, clique com o botão direito em `ServerScriptService` e escolha *Insert Object → Script*. Uma nova aba abre com uma linha:

```lua
print("Hello world!")
```

Clique no botão verde **Play** no topo do Studio. O Output panel mostra:

```
Hello world!
```

Esse é o teste de uma linha que confirma que tudo funciona. O Studio roda o código em uma sessão simulada. Nada é publicado e nada é enviado. É só um teste local do que aconteceria se um jogador entrasse. Clique em *Stop* depois de ver o resultado.

## O que mora onde (aprenda isso)

Os places do Roblox têm muitas pastas. Estas são as que importam nos próximos sete módulos:

- **`Workspace`** — as partes e modelos 3D que o jogador vê e toca.
- **`Players`** — a lista ao vivo de cada jogador conectado.
- **`ServerScriptService`** — scripts do servidor. Rodam uma vez no servidor quando o place começa. Os jogadores não podem vê-los.
- **`ReplicatedStorage`** — compartilhado entre servidor e cliente. Módulos do engine ficam aqui para os dois lados poderem usá-los.
- **`StarterPlayerScripts`** — scripts que rodam no dispositivo de cada jogador quando ele entra.
- **`StarterPack`** — itens que cada jogador recebe na mochila quando aparece no jogo.

A divisão entre servidor e cliente é a maior ideia do Roblox, e ela tem o seu próprio módulo (5.4). Por hoje, você só precisa saber uma coisa: scripts em `ServerScriptService` rodam no servidor.

## Atalhos do Studio que você vai usar todo dia

| Atalho | O que faz |
| --- | --- |
| Botão direito em objeto do Explorer → Insert | Adiciona um objeto filho |
| F (com uma Part selecionada no Viewport) | Foca a câmera nela |
| Botão direito seguro + WASD + mouse | Voa a câmera |
| Ctrl+S | Salva o place localmente |
| Botão Play | Testa dentro do Studio |
| Botão Stop | Termina o teste |

## Mexa um pouco

Insira uma Part no Workspace pelo Explorer. Arraste-a pela placa verde. Abra o Properties panel e mude o `Size`, a `Color` e o `Material`, depois observe a Part mudar no Viewport. Clique com o botão direito na Part e escolha *Group as Model*. Um Model é só uma pasta de Parts agrupadas. É útil quando você tem um prédio feito de vinte peças e quer mover todas de uma vez.

Pressione F5 para rodar um teste de jogador. Um personagem aparece e você pode andar com WASD. Enquanto o teste está rodando, digite `print("test")` diretamente na linha de comando do Output panel na parte de baixo e pressione Enter.

A maioria dos tutoriais de Roblox espera que você saiba onde estão esses quatro painéis. Passe dez minutos clicando até as palavras *Viewport, Explorer, Properties, Output* ficarem familiares.

## Publicar fica para depois

`File → Publish to Roblox` é como um place vira uma URL que seus amigos podem visitar. Não vamos clicar esse botão hoje. O Módulo 5.8 é o módulo de publicação. Por enquanto, um rascunho local salvo já é suficiente.

## O que você acabou de fazer

Você instalou o Roblox Studio e rodou o primeiro script nele. Você conheceu os quatro painéis que a maioria dos tutoriais de Roblox espera que você saiba — Viewport, Explorer, Properties, Output — e viu um script de uma linha imprimir no console. Você também conheceu as pastas que os próximos sete módulos vão usar: `Workspace` para a cena 3D, `ServerScriptService` para código do servidor, `ReplicatedStorage` para código que os dois lados podem ler, e mais algumas. Hoje não precisou de Luau de verdade. Isso começa no próximo módulo. O ponto de hoje era fazer o Studio parecer familiar antes das lições pedirem trabalho de verdade.

**Conceitos que você já sabe nomear:**

- *Roblox Studio* — o editor gratuito para places do Roblox
- *os quatro painéis* — Viewport, Explorer, Properties, Output
- *`ServerScriptService`* — onde os scripts do servidor ficam
- *`ReplicatedStorage`* — lar de código compartilhado entre servidor e cliente
- *Place* — um mundo Roblox; o que é publicado

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra o Studio com o Baseplate. Sem olhar:

1. Adicione um Script ao `ServerScriptService`.
2. Faça ele imprimir uma palavra e clique em Play.
3. Depois aponte para cada um dos quatro painéis e diga o nome em voz alta.

<details><summary>Travou? Abra aqui para conferir.</summary>

Você deve ter feito tudo isso:

- Clicou com o botão direito em `ServerScriptService` no Explorer e escolheu *Insert Object → Script*.
- Digitou algo como `print("Kingdom")` no novo Script.
- Pressionou o botão verde **Play** e viu `Kingdom` aparecer no Output panel.
- Nomeou os quatro painéis: **Viewport** (o mundo 3D no centro), **Explorer** (a árvore de objetos à direita), **Properties** (os campos abaixo do Explorer), **Output** (o console na parte de baixo).

Se o Output panel estava escondido, você o abre em *View → Output*.

</details>

## Palavras para adicionar ao glossário

- **Roblox Studio** — o editor gratuito para criar places no Roblox.
- **Place** — um único mundo de jogo no Roblox.
- **Workspace** — a cena 3D ao vivo que todos os jogadores veem.
- **Explorer** — a árvore de objetos do Studio; mostra cada objeto do place.
- **Output panel** — o console do Studio; `print` e erros aparecem aqui.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 5.2 apresenta o **Luau** — Lua com tipos opcionais. Parece um pouco com JavaScript e um pouco com Python. O seu port do engine começa lá.
