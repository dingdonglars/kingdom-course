# Módulo 4.2 — O Browser como Runtime: DOM, DevTools, fetch

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje a página começa a funcionar. O JavaScript lê da API `https://localhost:5xxx/kingdoms` ao vivo e escreve o resultado na página. O mesmo motor que roda desde a Fase 1, o mesmo JSON que o seu `curl` viu na Fase 3 — agora chegando em uma aba do browser e aparecendo como HTML que você pode ver. Você também vai conhecer as três abas de desenvolvedor do browser: Elements (o DOM), Console (rode JavaScript ao vivo) e Network (cada requisição).

"O browser como runtime" é a ideia no título, e ela só quer dizer: o browser é um lugar onde o seu código *roda*, não só um lugar que mostra páginas. O seu JavaScript roda lá, alcança a sua API e edita a página ao vivo. Veja o ciclo completo — e note que ele termina no mesmo motor que você escreveu na Fase 1:

```text
   O BROWSER  (o runtime — seu JavaScript roda aqui)

     index.html + seu JS
          |   fetch("/kingdoms")
          v
     sua API  --usa-->  Kingdom.Engine   (regras da Fase 1, ainda sem mudança)
          |
          |   JSON volta
          v
     seu JS escreve na página (o DOM)  -->  você vê o reino
```

Console (Fase 1), API (Fase 3), agora browser (Fase 4) — três shells, um motor. Hoje você conecta o último salto: JSON em uma página que você pode olhar.

> **Words to watch**
>
> - **DOM** — Document Object Model. A árvore em memória da sua página; o JavaScript a lê e muda.
> - **`document.querySelector`** — encontra um elemento por seletor CSS.
> - **`fetch(url)`** — forma moderna de fazer requisições HTTP a partir do browser.
> - **`async` / `await`** — a forma do JavaScript de escrever código assíncrono que *lê* como síncrono.
> - **CORS** — Cross-Origin Resource Sharing — pronunciado *"cors"*. Segurança do browser em torno de chamadas à API. Primeira vez que nomeia; a explicação de uma vez por termo fica abaixo.

---

## Nova linguagem: JavaScript — e por quê

Você escreveu C# o ano todo. Hoje uma segunda linguagem aparece, e é justo perguntar por que você não pode continuar usando a que você já sabe.

O motivo é simples: **um browser roda apenas uma linguagem de programação — JavaScript.** Não C#, não Python. Se você quiser que uma página *faça* alguma coisa — reagir a um clique, carregar dados, mudar o seu texto — esse código tem que ser JavaScript. É a linguagem para a qual o browser foi construído, e não tem como contornar. (Seu C# ainda está aqui: ele é a API com a qual a página conversa. A metade do browser é que tem que ser JavaScript.)

A boa notícia: você já sabe programar, e o JavaScript é construído das mesmas partes que você usou o ano todo — variáveis, `if`, loops, funções, objetos. Você não está começando de novo. Você está aprendendo palavras novas para ideias que você já tem. Veja a tradução rápida:

| C# | JavaScript | Note |
| --- | --- | --- |
| `string name = "Eldoria";` | `const name = "Eldoria";` | `const` = não pode reatribuir depois; `let` = pode. Nenhum tipo escrito. |
| `int day = 11;` | `let day = 11;` | JavaScript não te obriga a escrever o tipo. |
| `Console.WriteLine(x);` | `console.log(x);` | Imprimir — aqui, no console do DevTools. |
| `void Greet(string n) {}` | `function greet(n) {}` | Sem tipo de retorno, sem tipos de parâmetro. |
| `if (x == 5)` | `if (x === 5)` | **Três** sinais de igual em JavaScript (veja abaixo). |
| `foreach (var b in list)` | `for (const b of list)` | `of`, não `in`. |
| `var k = new Kingdom();` | `const k = { name: "Eldoria", day: 11 };` | Um objeto simples é só `{ chave: valor }` — que é também exatamente o que é JSON. |

Duas diferenças pegam todo programador de C#, então conheça-as agora:

1. **Nenhum tipo escrito, e nada verifica o seu código antes de rodar.** Você não declara `int` ou `string`, e o browser roda o que você escreveu. Um erro de digitação como `slot.naem` não dá erro — ele silenciosamente devolve `undefined`, e você descobre mais tarde quando algo parece errado. (O Módulo 4.3 adiciona TypeScript para colocar essa rede de segurança de volta.)
2. **Use `===`, não `==`.** Sempre três sinais de igual para comparar. A versão de dois sinais do JavaScript tem regras surpreendentes que levam a bugs confusos; simplesmente evite.

Isso é suficiente para ler o código de hoje. Você vai ficar confortável mais rápido do que espera — as formas já são as que você pensa.

## Passo 1 — abra o DevTools

Pressione F12 na maioria dos browsers (Cmd+Option+I no Mac). Três abas que você vai usar o tempo todo:

- **Elements** — o DOM ao vivo. Clique em qualquer nó, expanda, edite seus atributos ali mesmo. A página atualiza enquanto você edita.
- **Console** — rode JavaScript ao vivo. Digite `2 + 2` e pressione Enter. Digite `document.querySelector('h1')` e veja o elemento H1 impresso de volta.
- **Network** — toda requisição HTTP que a página faz. Clique em qualquer uma para ver seus cabeçalhos, corpo da resposta e tempo.

Abra o DevTools toda vez que você trabalhar em uma página. Ele não esconde nada de você; trabalhar com ele fechado significa que você não pode ver o que está acontecendo.

## Passo 2 — básico do DOM

O DOM tem um pequeno conjunto de comandos que fazem a maior parte do trabalho. Aqui estão eles, em cinco linhas:

```js
// Encontrar um elemento
const h1 = document.querySelector('h1');
h1.textContent = "Eldoria, the Brave";

// Encontrar um elemento pelo id
const dayEl = document.getElementById('day');
dayEl.textContent = "12";

// Construir novos elementos
const ul = document.querySelector('#resources');
const li = document.createElement('li');
li.textContent = "Gold: 100";
ul.appendChild(li);
```

`querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`. Todo aplicativo de browser faz a maior parte do seu trabalho com esses cinco. Quanto maior o seu arquivo ficar, com mais frequência esses cinco aparecem.

## Passo 3 — `fetch` e `await`

`fetch(url)` é como o JavaScript faz requisições HTTP em um browser moderno. Ele retorna uma *promise* — um valor que chega depois. A palavra-chave `await` espera pela promise e te dá o valor quando ele chega. A função precisa ser marcada como `async` antes de você poder usar `await` dentro dela.

```js
async function loadKingdom() {
  const response = await fetch('https://localhost:5xxx/kingdom');
  if (!response.ok) {
    console.error("Request failed:", response.status);
    return;
  }
  const data = await response.json();
  return data;
}
```

Dois `await`s é o padrão normal. O primeiro espera os cabeçalhos da resposta chegarem. O segundo espera o corpo ser lido como JSON. Ambos são viagens pela rede, e ambos valem a espera.

Erros async são seu próprio tipo de bug. Esqueça o `await` e você vai estar chamando `.json()` em um objeto Promise, depois `.name` em `undefined`, e o erro que você vê aponta para o lugar errado. Leia a saída do console do DevTools com cuidado quando você trabalhar com `fetch`.

## Passo 4 — o aviso sobre CORS

Quando a sua página está em `http://localhost:5500` e a sua API está em `https://localhost:5xxx`, elas contam como duas origens diferentes. O browser verifica o cabeçalho `Access-Control-Allow-Origin` da API. Se a sua API não diz *"esta origem é permitida,"* o browser não vai nem deixar o seu JavaScript ver a resposta. Esse recurso de segurança se chama **CORS** (pronunciado *"cors"*) — Cross-Origin Resource Sharing. Ele existe para que uma página maliciosa não possa silenciosamente chamar a API de outro site enquanto finge ser você.

Uma correção de duas linhas em `Kingdom.Api/Program.cs`:

```csharp
builder.Services.AddCors();
// ...
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
```

Para produção, substitua `AllowAnyOrigin` pela origem específica do frontend. Faremos isso no Módulo 4.6 quando o frontend for implantado.

## Passo 5 — o starter do delta

- **MODIFICADO:** `web/kingdom.js` — fetch + atualização do DOM
- **MODIFICADO:** `Kingdom.Api/Program.cs` — adiciona CORS

`web/kingdom.js`:

```js
const API = 'https://localhost:5xxx';   // MUDE para a porta da sua API

async function loadKingdom() {
  try {
    const resp = await fetch(`${API}/kingdoms`);   // requer auth — ajuste conforme necessário
    if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
    const slots = await resp.json();
    if (slots.length === 0) {
      document.querySelector('main').textContent = "No kingdoms yet — create one via the API.";
      return;
    }
    renderSummary(slots[0]);
  } catch (err) {
    console.error('Failed to load kingdom:', err);
    document.querySelector('main').textContent = `Error: ${err.message}`;
  }
}

function renderSummary(slot) {
  document.getElementById('day').textContent = slot.day;
  document.querySelector('h1').textContent = slot.name;
}

loadKingdom();
```

Abra `web/index.html` no browser. A página carrega, chama sua API e mostra os dados do reino ao vivo. Esta é a sua primeira chamada completa a partir do browser até a API e de volta.

## Mexa um pouco

Abra o DevTools, mude para a aba Network e recarregue a página. Encontre a requisição `/kingdoms`. Clique nela. Leia o painel de resposta. É o mesmo JSON que o seu `curl` viu na Fase 3 — mesmo motor, mostrado em um lugar novo.

Esqueça o `await` de propósito uma vez. Olhe o erro no console. Esse é o bug async mais comum que você vai ver este ano, então aprenda a reconhecê-lo agora.

Adicione uma linha `console.log(slots)` no seu handler. Fazer log no console do DevTools é uma forma fácil de ver o que está realmente passando pelo seu código.

Tente um `fetch` em uma porta errada. Note que uma mensagem de erro de CORS parece diferente de uma mensagem de erro de rede — elas estão te dizendo coisas diferentes. Essa diferença importa quando você está tentando achar um bug.

## O que você acabou de fazer

O reino agora carrega em uma aba do browser. O JavaScript chamou a sua API ao vivo, recebeu JSON de volta e escreveu os valores no DOM — `slot.name` virou o título da página, `slot.day` preencheu o contador de dias. Você conheceu as cinco funções do DOM que vai mais usar (`querySelector`, `getElementById`, `textContent`, `createElement`, `appendChild`), o padrão de `fetch` mais `async` / `await`, e **CORS** — a regra de segurança do browser que precisa que o servidor diga "esta origem é permitida" antes que o seu JavaScript possa ler a resposta. Cerca de trinta linhas de JavaScript, e um loop completo do browser até a API e de volta.

**Conceitos que você já sabe nomear:**

- **DOM** — a árvore ao vivo da sua página; o JavaScript a lê e muda
- **`querySelector` / `textContent` / `createElement` / `appendChild`** — o pequeno conjunto de comandos que faz a maior parte do trabalho
- **`fetch(url)`** — requisição HTTP moderna a partir do JavaScript
- **`async` / `await`** — escreva código assíncrono que lê como síncrono
- **CORS** — o servidor precisa permitir a sua origem antes que o browser te mostre a resposta

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Da sua própria cabeça, escreva e rode isso (o Console do DevTools é um lugar rápido para tentar):

1. Uma função `async` que recebe uma `url`.
2. Dentro dela, `await fetch(url)` para obter a resposta.
3. Um segundo `await` em `response.json()` para ler o corpo como JSON.
4. Retorne os dados — depois chame a função para ela realmente rodar.

Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

<details><summary>Travou? Abra aqui para conferir.</summary>

```js
async function load(url) {
  const response = await fetch(url);
  if (!response.ok) {
    console.error("Request failed:", response.status);
    return;
  }
  const data = await response.json();
  return data;
}
```

O que tem que estar certo: `async` na função, `await` antes de `fetch`, e um segundo `await` antes de `.json()`. Esqueça qualquer um dos `await`s e você acaba trabalhando com uma Promise em vez do valor — esse é o bug mais comum aqui.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.3 apresenta TypeScript e Vite. Os tipos voltam; as ferramentas de build trazem hot-reload, módulos e o resto da experiência moderna de desenvolvimento frontend.
