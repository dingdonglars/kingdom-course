# Módulo 4.5 — Vitest (Testes de Frontend)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O código do browser recebe testes unitários hoje. **Vitest** é a versão JavaScript do xUnit — rápido, feito para funcionar com Vite e usando o mesmo estilo `expect(x).toBe(y)` que você usou o ano todo. Vamos testar os componentes do Módulo 4.4: dado um `KingdomSlot`, `KingdomCard` deve renderizar o HTML que esperamos; dada uma string com colchetes angulares, `escapeHtml` deve retornar saída com escape.

> **Words to watch**
>
> - **Vitest** — o executor de testes. Feito para funcionar com Vite; sua API é quase igual à do Jest.
> - **`describe` / `it` / `expect`** — o vocabulário padrão de testes.
> - **`toBe` / `toEqual` / `toContain`** — asserções comuns, estilo Jest.
> - **happy-dom** — um ambiente DOM falso para testes que precisam de `document` sem um browser real.

---

## Por que um executor de testes de frontend

Os testes em C# te deram confiança para mudar código. Os testes de frontend te dão a mesma confiança. Sem eles, o que `KingdomCard` renderiza pode parar de corresponder ao que `main.ts` espera, e você não vai notar. Com eles, toda mudança em um componente roda em relação aos seus testes, e um quebrado aparece em segundos.

Os testes de frontend vêm em três grupos aproximados. *Testes unitários* verificam uma função ou componente (Vitest sozinho). *Testes de integração* rodam vários componentes juntos em um DOM falso (Vitest mais `happy-dom`). *Testes end-to-end* rodam um browser real (Playwright — um assunto grande por conta própria, não coberto aqui). Hoje faremos testes unitários e um pouco de testes de integração.

## O que muda neste módulo

- **MODIFICADO:** `web-vite/package.json` — adiciona `vitest` + `happy-dom`
- **MODIFICADO:** `web-vite/vite.config.ts` — conecta a configuração do Vitest
- **NOVO:** `web-vite/src/components/__tests__/escape.test.ts`
- **NOVO:** `web-vite/src/components/__tests__/KingdomCard.test.ts`

## Passo 1 — instale

```powershell
cd web-vite
npm install -D vitest happy-dom
```

## Passo 2 — configuração do Vitest

`web-vite/vite.config.ts` (crie ou modifique):

```ts
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    environment: 'happy-dom',
    globals: true
  }
});
```

`globals: true` deixa você usar `describe`, `it` e `expect` sem importá-los — a mesma sensação do xUnit.

## Passo 3 — primeiros testes

`src/components/__tests__/escape.test.ts`:

```ts
import { describe, it, expect } from 'vitest';
import { escapeHtml } from '../escape';

describe('escapeHtml', () => {
  it('escapes the five characters', () => {
    expect(escapeHtml('<script>')).toBe('&lt;script&gt;');
    expect(escapeHtml('"')).toBe('&quot;');
    expect(escapeHtml("'")).toBe('&#039;');
    expect(escapeHtml('A & B')).toBe('A &amp; B');
  });

  it('leaves safe strings alone', () => {
    expect(escapeHtml('hello world')).toBe('hello world');
  });
});
```

`src/components/__tests__/KingdomCard.test.ts`:

```ts
import { describe, it, expect } from 'vitest';
import { KingdomCard } from '../KingdomCard';

describe('KingdomCard', () => {
  it('renders the kingdom name and day', () => {
    const html = KingdomCard({ id: 1, name: 'Eldoria', day: 11 });
    expect(html).toContain('Eldoria');
    expect(html).toContain('Day 11');
  });

  it('escapes the name', () => {
    const html = KingdomCard({ id: 1, name: '<script>x</script>', day: 1 });
    expect(html).toContain('&lt;script&gt;');
    expect(html).not.toContain('<script>');     // a tag literal NÃO deve aparecer
  });
});
```

O segundo teste é o teste de segurança — ele prova que `escapeHtml` está conectado corretamente.

## Passo 4 — rode

```powershell
npm test
```

O Vitest encontra arquivos `*.test.ts` e roda cada `describe`/`it`. Você vai ver ticks verdes (ou cruzes vermelhas) imediatamente. Para o modo watch, `npm test -- --watch` roda novamente só os testes afetados por uma mudança de arquivo. Esse feedback rápido é uma grande parte de como o trabalho moderno de frontend se sente.

## Passo 5 — integração DOM leve

Para testar componentes que usam o DOM, o ambiente `happy-dom` te dá um `document` falso:

```ts
import { describe, it, expect, beforeEach } from 'vitest';

describe('main rendering', () => {
  beforeEach(() => {
    document.body.innerHTML = '<div id="app"></div>';
  });

  it('renders the empty state', () => {
    const root = document.querySelector('#app')!;
    root.innerHTML = '<p>No kingdoms yet.</p>';
    expect(root.textContent).toContain('No kingdoms yet');
  });
});
```

`document` funciona nos testes porque `happy-dom` te dá um — cerca de dez vezes mais rápido do que iniciar o Chrome de verdade. Para testes de browser completo, Playwright é a ferramenta; veja por conta própria se quiser.

## Mexa um pouco

Adicione relatório de cobertura: `npm test -- --coverage`. Veja quais linhas não são cobertas por testes. Não mire em 100% — mire em cobrir *as partes que quebraria sem aviso se você as mudasse*. Perseguir um número de cobertura alto leva a testes ruins.

Simule `fetch` com `vi.fn()` — o helper de spy e mock do Vitest. Teste o tratamento de erros do main sem fazer uma chamada de rede real.

Adicione um snapshot test: `expect(KingdomCard(slot)).toMatchSnapshot()`. O Vitest escreve a saída esperada em um arquivo `__snapshots__/` na primeira vez, depois sinaliza qualquer mudança depois disso.

## O que você acabou de fazer

O código do browser agora tem testes. Você conectou o Vitest ao projeto Vite, definiu o ambiente de testes como `happy-dom` e escreveu quatro testes em dois arquivos: `escapeHtml` cobre os cinco caracteres e o caso de não fazer nada, e `KingdomCard` cobre tanto o caso normal quanto o caso de segurança (provando que o escape está conectado). Cerca de sessenta linhas de código de teste; um comando (`npm test`) os roda todos em menos de um segundo. O mesmo hábito de testar que você tem construído desde o Módulo 1.3 — *se pode quebrar, escreva um teste* — agora funciona no browser também.

**Conceitos que você já sabe nomear:**

- **Vitest** — executor de testes construído para Vite; a versão JavaScript do xUnit
- **`describe` / `it` / `expect`** — o vocabulário padrão de testes
- **happy-dom** — DOM falso rápido para testes
- **modo watch** — roda novamente os testes afetados ao salvar
- **snapshot test** — capture a saída uma vez; falhe em qualquer mudança futura

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve. O executor de testes é o seu avaliador: um tick verde significa que você acertou a forma.

Em um arquivo `.test.ts`, da sua própria cabeça, escreva um teste Vitest para `escapeHtml`:

1. Use um `describe`, um `it` e um `expect(...).toBe(...)`.
2. Verifique que `escapeHtml('<script>')` transforma os colchetes angulares em `&lt;script&gt;`.
3. Rode `npm test`.

<details><summary>Travou? Abra aqui para conferir.</summary>

```ts
import { describe, it, expect } from 'vitest';
import { escapeHtml } from '../escape';

describe('escapeHtml', () => {
  it('escapes angle brackets', () => {
    expect(escapeHtml('<script>')).toBe('&lt;script&gt;');
  });
});
```

A forma a lembrar: `describe` nomeia a coisa sob teste, `it` nomeia um comportamento, e `expect(actual).toBe(expected)` faz a verificação. É o mesmo instinto `expect(x).toBe(y)` que você tem desde os seus testes em C#.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.5 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.5 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.6 é o último módulo técnico da Fase 4: implante o frontend no Azure Static Web Apps com GitHub Actions. Depois o Módulo 4.7 fecha a Fase 4 com M5 e um exercício tranquilo onde você relê o seu código da Fase 0.
