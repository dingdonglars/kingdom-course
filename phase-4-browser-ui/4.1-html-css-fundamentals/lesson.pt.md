# Módulo 4.1 — Fundamentos de HTML e CSS

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o reino aparece em uma aba do browser. Não como JSON, não como texto no console — como uma página. A gente escreve o menor HTML e CSS úteis, abre o arquivo no browser e lá está. Sem framework, sem etapa de build, sem `npm install`. Só um arquivo `.html` e um arquivo `.css` que você pode dar dois cliques. O próximo módulo faz a página falar com a sua API ao vivo. Hoje é sobre o layout.

Se você nunca escreveu HTML antes, é mais simples do que parece. **HTML** descreve estrutura: isso é um cabeçalho, isso é uma lista, isso é a seção principal. **CSS** descreve aparência: tanto de espaçamento, aquela fonte, aquelas cores. As duas linguagens ficam separadas de propósito. Se você algum dia se pegar escrevendo cores dentro do HTML, ou estrutura dentro do CSS, algo está no lugar errado.

HTML é na verdade só uma árvore aninhada — caixas dentro de caixas. A página que você constrói hoje tem esta forma:

```text
   html
   |
   +-- body
       |
       +-- header
       |     +-- h1   "Eldoria"
       |     +-- p    "Day -"
       |
       +-- main
             +-- h2  "Resources"
             +-- ul   (the resources list)
             +-- h2  "Buildings"
             +-- ul   (the buildings list)
```

Cada tag que você abre fica dentro de outra tag, e esse aninhamento *é* a estrutura. O CSS então pinta essa árvore, e (no próximo módulo) o JavaScript a edita. Mesma árvore, três trabalhos: HTML a constrói, CSS a estiliza, JS a muda.

> **Words to watch**
>
> - **HTML** — linguagem de marcação. Descreve estrutura: cabeçalhos, listas, seções, links.
> - **CSS** — linguagem de estilo. Descreve aparência: cores, espaçamento, fontes, layout.
> - **DOM** — a árvore em memória que o browser constrói a partir do seu HTML. O JavaScript vai mudá-la no Módulo 4.2.
> - **semantic markup** — usar `<header>`, `<main>`, `<nav>` em vez de `<div>` para tudo.

---

## Passo 1 — o menor HTML útil

Crie `web/index.html` no root do seu repositório:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>My Kingdom</title>
  <link rel="stylesheet" href="styles.css">
</head>
<body>
  <header>
    <h1>Eldoria</h1>
    <p>Day <span id="day">—</span></p>
  </header>
  <main>
    <h2>Resources</h2>
    <ul id="resources"></ul>
    <h2>Buildings</h2>
    <ul id="buildings"></ul>
  </main>
  <script src="kingdom.js"></script>
</body>
</html>
```

Oito elementos, e essa é a estrutura inteira. Dê dois cliques no arquivo no explorador de arquivos — o browser abre. Você vai ver a página simples: um cabeçalho "Eldoria", um Day vazio, duas listas vazias. A página pede um arquivo `kingdom.js` que ainda não existe. O browser só pula e continua. A gente vai escrever esse arquivo no próximo módulo.

Se HTML é novidade, veja como ler uma linha. Uma parte como `<h1>Eldoria</h1>` tem três partes: uma **tag de abertura** `<h1>`, o **conteúdo** `Eldoria` e uma **tag de fechamento** `</h1>` — a `/` quer dizer "fechar este". O nome da tag (`h1`, `p`, `ul`, `li`) diz que *tipo* de coisa é: um cabeçalho, um parágrafo, uma lista, um item de lista. Algumas tags também carregam **atributos** — rótulos extras dentro da tag de abertura, como `id="day"` em `<span id="day">`. Um `id` é só um nome que você cola em um elemento para que seu CSS e JavaScript possam encontrá-lo depois. Essa é a linguagem toda: caixas com nome, algumas com rótulos, aninhadas umas dentro das outras.

Uma nota sobre os elementos que você vai ver mais: `<header>` é o topo da página, `<main>` é o corpo, `<nav>` é para menus, `<article>` e `<section>` agrupam conteúdo relacionado, e `<h1>` a `<h6>` são cabeçalhos em ordem de importância. Usar esses em vez de `<div>` em tudo é o que **semantic markup** significa. Leitores de tela, mecanismos de busca e você mesmo depois leem significado a partir do nome da tag.

## Passo 2 — o menor CSS útil

Crie `web/styles.css`:

```css
* { box-sizing: border-box; }
body {
  font-family: system-ui, sans-serif;
  max-width: 720px;
  margin: 2rem auto;
  padding: 0 1rem;
  color: #222;
  background: #fafaf7;
}
header {
  border-bottom: 1px solid #ddd;
  padding-bottom: 1rem;
  margin-bottom: 1rem;
}
h1 { margin: 0 0 0.25rem 0; font-size: 2rem; }
ul { padding-left: 1.5rem; }
li { padding: 0.25rem 0; }
```

Antes dos hábitos, o panorama geral de *como o CSS funciona* — porque esta ideia é a linguagem inteira.

Um arquivo CSS é uma lista de **regras**. Cada regra tem duas partes: um **seletor** que escolhe a quais elementos ela se aplica, e um conjunto de **declarações** dentro de `{ }` que dizem como esses elementos devem parecer.

```text
   h1 { color: teal; }
   ^^   ^^^^^^^^^^^^
   |    |
   |    as declarações -- o que mudar (propriedade: valor)
   o seletor -- quais elementos esta regra pinta
```

Então `h1 { color: teal; }` se lê como *"encontre todo `<h1>` na página, deixe o texto verde-azulado."* `body { ... }` estiliza o `<body>`. Você também pode apontar para um elemento específico pelo seu `id` usando `#` — `#day { ... }` estiliza `<span id="day">` — ou um grupo inteiro usando uma classe com `.`. **Essa é a ligação inteira entre seus dois arquivos:** o HTML nomeia as caixas, e o CSS aponta para elas com seletores e as pinta. Mude a estrutura do HTML com um arquivo; mude como ela *parece* com o outro; nunca misture os dois. Uma vez que os seletores fazem sentido, toda regra CSS que você ler é só *quais elementos* mais *o que mudar*.

Seu CSS pode ficar em um de dois lugares. Estamos usando um arquivo `styles.css` separado, linkado do HTML com `<link rel="stylesheet" href="styles.css">` — isso mantém estrutura e aparência em seus próprios arquivos, o que é o que você quer para qualquer coisa real. A outra forma é escrever as regras direto dentro de uma tag `<style>` no `<head>` da página, como `<style> h1 { color: teal; } </style>`. Mesmas regras, mesmos seletores — só o local difere. O `<style>` inline é prático para um experimento de uma página pequena (você vai usá-lo em *Por sua conta* abaixo); o arquivo linkado é o hábito para tudo maior.

Com isso em mente, as poucas linhas acima também têm três hábitos que vale a pena manter.

O primeiro é **`box-sizing: border-box`** em todos os elementos. Sem isso, a largura que você define em uma caixa não inclui o padding ou a borda. Então um elemento com `width: 200px` e `padding: 10px` na verdade ocupa 220 pixels. Com `border-box`, a largura é exatamente o que você definiu. Adicione uma vez, no `*`, no topo do arquivo.

O segundo é **`max-width` mais `margin: auto`** no body. Isso centraliza o conteúdo e impede que as linhas de texto fiquem longas demais para ler. Linhas longas em uma tela grande são cansativas de ler; esta é a forma mais barata de consertar isso.

O terceiro é **fontes do sistema** — `system-ui` usa a fonte que o sistema operacional já usa para seus próprios menus (Segoe UI no Windows, San Francisco no macOS, Roboto no Android). Nada para baixar, rápido, e parece certo por padrão. Fontes personalizadas são boas quando você precisa delas, mas para a UI do reino, a fonte do sistema é suficiente.

Atualize a página. Já parece uma página de verdade.

## Passo 3 — o que não está neste módulo

Uma lista curta, para que você saiba onde estão os limites por hoje.

JavaScript vem no Módulo 4.2 e no Módulo 4.3 — hoje a página só fica parada. Frameworks como React ou Svelte vêm depois, e são opcionais. Ferramentas de build (Vite) vêm no Módulo 4.3. Layout CSS real — Grid, Flexbox — é um assunto grande por si só. O básico aqui é suficiente para a página do reino, e você pode ler mais sobre layout por conta própria quando precisar.

Este módulo é o ponto de partida. Os próximos cinco constroem em cima dele.

## Passo 4 — o starter do delta

- **NOVO:** `web/index.html`
- **NOVO:** `web/styles.css`
- **NOVO:** `web/kingdom.js` — placeholder vazio; preenchemos no Módulo 4.2.

Crie o `kingdom.js` vazio para que a tag `<script>` tenha algo para carregar. Uma linha é suficiente: `// kingdom.js — populated in Module 4.2`.

## Mexa um pouco

Mude o background do body para a sua cor favorita. Atualize. Você vê o resultado de imediato — essa é uma das coisas boas do trabalho de frontend. Não tem etapa de compilação, sem rebuild, nada entre a sua edição e ver na tela.

Adicione um `<nav>` com três links (só `href="#"` por enquanto) dentro do `<header>`. Veja que o browser os coloca em uma linha por padrão. CSS é o que faz eles parecerem um menu; o HTML só diz "esses são links de navegação."

Abra o DevTools do browser (F12 na maioria dos browsers) e clique no painel Elements. Olhe o `<header>`. Você está vendo a árvore ao vivo — o DOM. A gente vai falar com ela a partir do JavaScript no próximo módulo.

Adicione `<meta name="viewport" content="width=device-width, initial-scale=1">` no `<head>`. A página agora aparece corretamente em um celular. Uma linha, grande efeito.

## O que você acabou de fazer

Você escreveu uma página da web à mão. O HTML definiu a estrutura — um cabeçalho com um nome e um contador de dias, uma seção principal com duas listas. O CSS deu espaçamento, uma largura sensata e uma fonte do sistema, mais a linha `box-sizing` que corrige a surpresa de largura mais comum. Você abriu o arquivo no browser e ele renderizou. Sem framework, sem etapa de build, sem instalação — só dois arquivos e uma aba. Cerca de quarenta linhas de código no total. Você já conheceu os três hábitos úteis (`box-sizing: border-box`, `max-width` para comprimento de linha legível, fontes do sistema) que resolvem a maioria das páginas simples. HTML primeiro, CSS depois.

**Conceitos que você já sabe nomear:**

- **HTML** — marcação; a estrutura da página
- **CSS** — estilos; a aparência da página
- **elementos semânticos** — `<header>`, `<main>`, `<nav>`, `<article>`, `<section>`
- **`box-sizing: border-box`** — a largura inclui padding e borda
- **fontes do sistema** — `system-ui` resolve para a fonte nativa de UI do sistema operacional

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Crie um arquivo `.html` novo e vazio e, da sua própria cabeça, construa isso:

1. Um esqueleto HTML completo — `<!DOCTYPE html>`, depois `<html>`, `<head>` e `<body>`.
2. Um `<h1>` dentro do body, com o nome do seu reino nele.
3. Uma regra CSS (em uma tag `<style>` no `<head>`, ou um arquivo `.css` linkado) que dá uma cor a esse cabeçalho.
4. Abra o arquivo no browser.

Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve. O browser é o seu avaliador: se o cabeçalho aparecer colorido, você acertou.

<details><summary>Travou? Abra aqui para conferir.</summary>

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>My Page</title>
  <style>
    h1 { color: teal; }
  </style>
</head>
<body>
  <h1>Eldoria</h1>
</body>
</html>
```

As tags exatas e a cor não importam. O que importa: uma linha `<!DOCTYPE html>`, a divisão `<head>`/`<body>`, estrutura no HTML e aparência no CSS — nunca o contrário.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.2 traz o browser para a vida. Você vai conhecer o DOM de verdade, abrir o DevTools e escrever JavaScript que chama a sua API ao vivo — a página aprende a buscar JSON e renderizá-lo.
