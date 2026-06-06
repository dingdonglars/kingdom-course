# Módulo 4.4 — UI Componentizada

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje a página é dividida em partes reutilizáveis. Um `KingdomCard` renderiza um slot. Um `ResourceList` renderiza os recursos. O arquivo principal vira um pequeno *orquestrador* — o arquivo que carrega os dados e diz a cada componente o que fazer, como um regente na frente de uma banda. É a mesma ideia de dividir um script em classes lá no Módulo 1.1: tire as peças reutilizáveis e o resto do código fica mais fácil de ler.

Você ainda não está usando um framework. Esses componentes são funções TypeScript simples: os dados entram, HTML ou DOM saem. A mesma ideia funciona em React (`function Component({ slot })`), Vue, Svelte e qualquer outro framework. Frameworks adicionam re-renderização automática em cima dessa ideia, mas a ideia em si continua a mesma.

> **Words to watch**
>
> - **component** — uma função reutilizável que transforma dados em UI.
> - **template literal** — uma string de backtick que deixa você interpolar valores: `` `Day ${slot.day}` ``.
> - **render function** — uma função que recebe dados e retorna DOM (ou uma string HTML).
> - **event listener** — `el.addEventListener('click', fn)` — reaja à interação do usuário.
> - **delegation** — escute em um elemento pai para eventos subindo de muitos filhos.
> - **XSS** — Cross-Site Scripting. A classe de bug onde entrada de usuário sem escape roda como script.

---

## Por que componentizar

Quando `main.ts` passa de cem linhas, fica difícil acompanhar o que se conecta ao quê. Componentes são uma forma de manter código em caixas separadas — cada uma faz um trabalho de renderização. `KingdomCard.ts` sabe como desenhar um slot. `main.ts` vira *carregue os dados, passe para os componentes, pronto*.

## Dois estilos de componente

Há duas formas comuns de escrever uma render function. Escolha uma para um projeto e use em todo ele.

A primeira é **renderizar como string** — retornar uma string HTML e deixar o pai defini-la como `innerHTML`:

```ts
export function KingdomCard(slot: KingdomSlot): string {
  return `
    <article class="card">
      <h2>${escapeHtml(slot.name)}</h2>
      <p>Day ${slot.day}</p>
    </article>
  `;
}
```

Fácil de ler e escrever. Bom para páginas pequenas, mas mais lento para árvores grandes, porque cada renderização constrói a string inteira de novo e o browser tem que lê-la de novo.

A segunda é **renderizar como DOM** — construir elementos diretamente e retorná-los:

```ts
export function KingdomCard(slot: KingdomSlot): HTMLElement {
  const card = document.createElement('article');
  card.className = 'card';

  const h2 = document.createElement('h2');
  h2.textContent = slot.name;
  card.appendChild(h2);

  const p = document.createElement('p');
  p.textContent = `Day ${slot.day}`;
  card.appendChild(p);

  return card;
}
```

Mais para escrever. Mais rápido para árvores grandes, e `textContent` escapa a entrada do usuário sozinho — então você não pode causar um bug de XSS por engano.

Para a UI do reino, renderizar como string com um helper `escapeHtml` é suficiente. É isso que vamos usar.

## A armadilha do XSS

> ⚠ **`innerHTML` mais entrada de usuário é um bug de segurança.** Esse tipo de bug se chama **XSS** — Cross-Site Scripting. Se o nome de um reino é `<script>alert(1)</script>` e você o coloca direto no HTML, esse script roda no browser de todos que visualizarem a página. Use `textContent` e `appendChild` (a opção modo DOM), ou escape toda string que você joga no HTML. Nunca coloque dados brutos do usuário em `innerHTML`.

## O que muda neste módulo

- **NOVO:** `web-vite/src/components/KingdomCard.ts`
- **NOVO:** `web-vite/src/components/escape.ts` — o helper de escape de HTML
- **MODIFICADO:** `web-vite/src/main.ts` — usa os componentes

`web-vite/src/components/escape.ts`:

```ts
export function escapeHtml(s: string): string {
  return s
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;');
}
```

`web-vite/src/components/KingdomCard.ts`:

```ts
import type { KingdomSlot } from '../types';
import { escapeHtml } from './escape';

export function KingdomCard(slot: KingdomSlot): string {
  return `
    <article class="card">
      <h2>${escapeHtml(slot.name)}</h2>
      <p>Day ${slot.day}</p>
    </article>
  `;
}
```

## `main.ts` orquestra

```ts
import './style.css';
import type { KingdomSlot } from './types';
import { KingdomCard } from './components/KingdomCard';

const API = 'https://localhost:5xxx';

async function main() {
  const root = document.querySelector<HTMLDivElement>('#app')!;
  try {
    const resp = await fetch(`${API}/kingdoms`);
    if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
    const slots = (await resp.json()) as KingdomSlot[];

    if (slots.length === 0) {
      root.innerHTML = '<p>No kingdoms yet.</p>';
      return;
    }
    root.innerHTML = slots.map(KingdomCard).join('');
  } catch (err) {
    root.textContent = `Error: ${(err as Error).message}`;
  }
}

main();
```

`slots.map(KingdomCard)` — o componente é só uma função, então faça `map` sobre os slots e você obtém um array de strings HTML. `.join('')` junta tudo em uma string. Uma linha de orquestração por componente.

## Delegação de eventos

Quando você tem muitos cards, adicionar um handler de clique em cada um desperdiça esforço. Em vez disso, escute no pai e verifique qual filho foi clicado:

```ts
root.addEventListener('click', (e) => {
  const card = (e.target as HTMLElement).closest('article.card');
  if (!card) return;
  console.log('Clicked card:', card);
});
```

`closest` sobe a partir do elemento clicado até encontrar um pai que corresponda. Esse padrão funciona para qualquer UI com "muitos itens similares" — um handler pode cobrir milhares de cards.

## Mexa um pouco

Adicione um componente `ResourceList` que recebe um `Map<string, number>` e renderiza um `<ul>`. Use-o em `main`.

Adicione um botão Tick em cada card. No clique, POST para `/kingdoms/{id}/tick` e renderize de novo.

Adicione CSS a `.card` — uma borda, padding, uma sombra pequena. A mesma render function agora parece melhor de imediato; esse é o ponto de ter uma render function.

Tente deixar de fora o `escapeHtml` uma vez. Adicione um reino com o nome `<script>alert(1)</script>` pela API. O script roda. Isso é um bug XSS real. Coloque o escape de volta; o alerta desaparece.

## O que você acabou de fazer

Você dividiu a página em componentes. `KingdomCard` é uma função que transforma um `KingdomSlot` em HTML; `escapeHtml` mantém a entrada do usuário segura dentro de `innerHTML`; `main.ts` só carrega dados e chama `slots.map(KingdomCard).join('')`. Você também conheceu **XSS** — o tipo de bug onde strings sem escape rodam como script — e o helper `escapeHtml` que o previne. Cerca de quarenta linhas de TypeScript em três arquivos. O mesmo padrão funciona em React, Vue ou qualquer framework que você encontrar depois.

**Conceitos que você já sabe nomear:**

- **component** — uma função reutilizável de dados para UI
- **`escapeHtml`** — torna a entrada do usuário segura para interpolação em `innerHTML`
- **XSS** — Cross-Site Scripting; um ataque através de strings sem escape
- **delegação de eventos** — escute em um pai; despache por `e.target`
- **render function** — `(dados) => HTML`; a ideia que todo framework copia

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Da sua própria cabeça, escreva uma função de componente pequena:

1. Ela recebe um `KingdomSlot` e retorna uma string HTML.
2. Coloque o nome em um `<h2>` e o dia em um `<p>`.
3. Passe o nome por `escapeHtml` para que a entrada do usuário fique segura.

<details><summary>Travou? Abra aqui para conferir.</summary>

```ts
import type { KingdomSlot } from '../types';
import { escapeHtml } from './escape';

export function KingdomCard(slot: KingdomSlot): string {
  return `
    <article class="card">
      <h2>${escapeHtml(slot.name)}</h2>
      <p>Day ${slot.day}</p>
    </article>
  `;
}
```

A forma a lembrar: uma função simples, dados entram, uma string HTML sai. A única regra de segurança é `escapeHtml` em volta de qualquer texto que o usuário digitou — sem isso, um nome como `<script>` rodaria como código.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.5 apresenta o **Vitest** — o executor de testes para código de browser. Mesmo instinto do xUnit, com sabor JavaScript. Ele pega os bugs que os seus tipos TypeScript não conseguem.
