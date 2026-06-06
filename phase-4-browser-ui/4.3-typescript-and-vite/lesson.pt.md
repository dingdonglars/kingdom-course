# Módulo 4.3 — TypeScript + Vite + Módulos JS

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O código do browser fica mais sério hoje. Duas novas ferramentas entram. **TypeScript** traz a verificação de tipos que você teve em C# o ano todo — o mesmo *`slot.day` é um número* verificando, escrito em um arquivo JavaScript. **Vite** traz as ferramentas modernas — um servidor de desenvolvimento com hot-reload, suporte real a módulos e builds rápidas. A terceira mudança é mais silenciosa mas importa: `import` e `export` entre arquivos substituem o estilo antigo de `<script src=...>` onde tudo compartilhava um espaço global. O projeto web começa a parecer uma base de código de verdade.

Esta é a primeira vez que você vai ver TypeScript e Vite pelo nome. **TypeScript** é JavaScript com um sistema de tipos adicionado em cima — o mesmo tipo de verificação de tipos que C# te deu o ano todo. **Vite** (você pronuncia *"vit"*) é um conjunto de ferramentas para projetos web. Duas dessas ferramentas importam hoje, e ambas têm trabalhos simples:

- Um **servidor de desenvolvimento** — um programa pequeno que roda enquanto você trabalha. Ele serve sua página em `http://localhost:5173`, e toda vez que você salva um arquivo ele atualiza o browser instantaneamente. Pense nele como a versão web de `dotnet run`, exceto que ele se atualiza enquanto você digita. (Esse truque de atualização instantânea tem um nome — *hot-reload*, ou HMR.)
- Um **bundler** — quando o projeto está pronto para ir ao ar, ele junta todos os seus arquivos separados e as bibliotecas que eles usam e os empacota em poucos arquivos pequenos e rápidos para o browser baixar. Você não roda isso à mão com frequência; é o passo `npm run build` no final.

Antes as pessoas usavam uma ferramenta mais antiga e mais lenta chamada *webpack* para esses trabalhos. Vite é a mais nova que a maioria dos projetos usa agora. Você não precisa aprender webpack — só saiba que o Vite está silenciosamente fazendo essas duas coisas por você.

Uma coisa que pega todo mundo no início: o browser nunca roda TypeScript de verdade. Você escreve a versão tipada; uma etapa de build remove os tipos e entrega ao browser JavaScript simples.

```text
   você escreve          etapa de build                  browser roda
   main.ts      --Vite / tsc compila-->   main.js          (JavaScript simples;
   (com tipos)                            (tipos sumidos)   ele nunca vê os tipos)
```

Os tipos estão lá para pegar *seus* erros enquanto você escreve — uma vez que o código vai, eles fizeram o trabalho e desapareceram. É por isso que TypeScript pode adicionar tanta segurança sem custar nada ao browser.

> **Words to watch**
>
> - **TypeScript (TS)** — JavaScript com tipos. Compila para JS simples que o browser roda de verdade.
> - **Vite** — servidor de desenvolvimento e bundler moderno para frontend. Pronunciado *"vit"*.
> - **ES modules** — `import` / `export` entre arquivos JavaScript; a forma moderna.
> - **`tsconfig.json`** — configuração do compilador TypeScript.
> - **HMR** — Hot Module Replacement. Edite um arquivo, salve, o browser atualiza sem perder o estado.

---

## Por que TypeScript

O ano todo, C# esteve silenciosamente te protegendo — e vale notar isso agora, porque hoje você sentiu como é *sem* essa proteção.

Toda vez que você escreveu C#, o compilador verificou seu trabalho *antes do programa rodar*. Digite `kingdom.Buildigs` em vez de `Buildings` e você recebia um sublinhado vermelho e a build se recusava a rodar. Passar uma `string` onde um método queria um `int` — pego. Esquecer um argumento, renomear uma propriedade e perder um lugar, ler um campo que não existe — pego, pego, pego, cada vez com a linha exata apontada. Toda essa rede de segurança é o **sistema de tipos**: porque cada valor tinha um tipo conhecido (`int`, `string`, `Building`), o compilador conseguia dizer quando algo não encaixava. Você dependia disso tanto que ficou invisível.

JavaScript simples não tem nada disso. Ele roda o que você digitou. O erro de digitação `slot.naem` não dá erro — ele te dá `undefined`, e você só descobre mais tarde quando a página mostra um espaço em branco onde o nome deveria estar. Três meses depois, uma mudança renomeia `name` para `kingdomName` e nada te avisa até a UI silenciosamente quebrar. Todo erro que o compilador C# costumava pegar na build, o JavaScript simples envia feliz.

**TypeScript coloca essa rede de segurança de volta.** Você escreve os tipos — `interface KingdomSlot { id: number; name: string; day: number; }` — e agora o editor e o compilador rejeitam `slot.naem` e os erros de tipo errado, exatamente como C# fazia o ano todo. O mesmo JavaScript ainda roda por baixo; TypeScript é só a camada de verificação em que você escreve, e ela é removida quando o código é construído. Não é uma ideia nova para aprender — é o hábito de C# que você já tem, levado para o browser.

Você vai continuar usando JavaScript simples para scripts pontuais pequenos. Alcance TypeScript no momento em que um projeto tem três ou mais arquivos, ou tipos que outras pessoas — ou você do mês que vem — precisam confiar.

## Por que Vite

`<script src="kingdom.js">` só te leva até certo ponto. Frontends de verdade querem módulos (`import { x } from './y.ts'`), precisam de uma etapa de build para compilar TypeScript, querem hot-reload enquanto editam e querem bundling para produção (muitos arquivos pequenos comprimidos em alguns minificados). Vite faz tudo isso com uma configuração de uma linha e quase nenhuma config para escrever. É a escolha padrão moderna.

## O que muda neste módulo

Você está saindo da pasta `web/` simples (HTML e JavaScript) para um projeto Vite.

- **NOVO:** `web-vite/` — criado via `npm create vite@latest`
- **PORTADO:** a lógica de `kingdom.js` para `web-vite/src/main.ts` com TypeScript
- **MODIFICADO:** o projeto da API com CORS para permitir o servidor de desenvolvimento do Vite (padrão `http://localhost:5173`)

## Passo 1 — crie o projeto Vite

```powershell
npm create vite@latest web-vite -- --template vanilla-ts
cd web-vite
npm install
npm run dev
```

Primeiro, uma palavra sobre `npm`, já que esta é a sua primeira vez usando. **npm** é o gerenciador de pacotes do JavaScript — o mesmo trabalho que o NuGet faz em C#. Ele baixa as bibliotecas externas que um projeto precisa (para uma pasta chamada `node_modules`) e roda os comandos de um projeto. As três linhas acima são exatamente isso: `npm create vite@latest` cria o projeto, `npm install` baixa as bibliotecas e `npm run dev` inicia o servidor de desenvolvimento que você acabou de ler. Você vai usar esses mesmos poucos comandos em todo projeto web daqui para frente.

O Vite configura o projeto, instala cerca de vinte pacotes pequenos e inicia o servidor de desenvolvimento. Abra `http://localhost:5173`. Você vai ver a página de boas-vindas padrão do Vite.

`vanilla-ts` é o nome do template para "TypeScript, sem framework." Estamos ficando com TypeScript simples para esta fase; você pode adicionar um framework em cima depois se quiser.

## Passo 2 — tipos para a API

Crie `web-vite/src/types.ts`:

```ts
export interface KingdomSlot {
  id: number;
  name: string;
  day: number;
}
```

Um lugar que diz como um slot parece. Em todo lugar do projeto que lida com um slot, importa esta interface; se você mudar o layout, todo lugar que o usa é verificado.

## Passo 3 — porte `kingdom.js` para `main.ts`

Substitua `web-vite/src/main.ts`:

```ts
import './style.css';
import type { KingdomSlot } from './types';

const API = 'https://localhost:5xxx';   // MUDE

async function loadKingdom(): Promise<KingdomSlot | null> {
  const resp = await fetch(`${API}/kingdoms`);
  if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
  const slots = (await resp.json()) as KingdomSlot[];
  return slots[0] ?? null;
}

function render(slot: KingdomSlot | null) {
  const root = document.querySelector<HTMLDivElement>('#app')!;
  if (!slot) {
    root.innerHTML = '<p>No kingdoms yet.</p>';
    return;
  }
  root.innerHTML = `
    <header>
      <h1>${slot.name}</h1>
      <p>Day ${slot.day}</p>
    </header>
  `;
}

loadKingdom()
    .then(render)
    .catch(err => {
        document.querySelector<HTMLDivElement>('#app')!.textContent = `Error: ${err.message}`;
    });
```

Algumas coisas nesse arquivo são novas e valem nomear. `import './style.css'` funciona porque o Vite sabe como fazer bundle de imports de CSS. `import type { KingdomSlot }` é um import somente de tipo — ele é removido quando o código é construído. `Promise<KingdomSlot | null>` é o tipo de retorno da função; o compilador verifica todo lugar que a chama. `document.querySelector<HTMLDivElement>(...)` é a versão tipada de `querySelector` — você passa o tipo do elemento, então não precisa converter o resultado você mesmo. O `!` depois da chamada é a afirmação não-nula: ela diz ao TypeScript *"confie em mim, isso não é nulo."* Use só quando você tiver certeza. O `as KingdomSlot[]` no JSON parseado é você dizendo ao TypeScript que a API retorna o layout que você declarou. TypeScript não consegue verificar o layout real enquanto o programa roda, então você diz o que esperar.

## Passo 4 — experimente

```powershell
npm run dev
```

Visite `http://localhost:5173`. A página renderiza. Agora edite `main.ts` — mude um cabeçalho. Salve. O browser atualiza sem um reload completo, e qualquer coisa que você já tinha configurado na página fica como estava. Isso é HMR. Ao longo de milhares de edições, isso te economiza horas.

O terminal mostra erros de TypeScript enquanto você salva (`error TS2322: Type 'string' is not assignable to type 'number'`). Pego quando o código compila, não enquanto ele roda — a mesma ajuda que você teve de C# o ano todo.

## Passo 5 — o build de produção

```powershell
npm run build
```

Isso cria uma pasta `dist/` — JavaScript minificado, nomes de arquivo com hash, pronto para implantar. O próximo passo seria `npm run preview` para testar o build de produção na sua própria máquina, depois implantar no Static Web Apps no Módulo 4.6.

## Mexa um pouco

Adicione um erro de digitação: `slot.naem`. O editor sublinha imediatamente. TypeScript pega o que o JavaScript deixaria passar.

Adicione uma incompatibilidade de número e string: `const day: number = "twelve";`. A mesma coisa — o editor sinaliza antes mesmo de você salvar.

Tente importar um módulo CSS: `import styles from './stuff.module.css'`. O Vite lida com isso sozinho, e os nomes de classe ficam restritos a esse arquivo automaticamente.

Defina `"strict": true` em `tsconfig.json` e construa de novo. Você vai ver bastante avisos — é a sua chance de limpar o código enquanto ainda é pequeno.

## O que você acabou de fazer

O projeto subiu um nível. Você configurou um projeto Vite (`web-vite/`), definiu uma interface `KingdomSlot` em `types.ts` e moveu o código de carregamento do reino para `main.ts` como TypeScript. O compilador agora rejeita erros de digitação e incompatibilidades de tipo; o HMR significa que edições aparecem no browser sem perder o que está na página. Um build de produção é um comando (`npm run build`) e cria uma pasta `dist/` que você pode implantar. O arquivo TypeScript é cerca de vinte linhas mais longo do que a versão JavaScript que substituiu — e essas linhas extras se pagam na primeira vez que uma mudança pega um erro em tempo de compilação.

**Conceitos que você já sabe nomear:**

- **TypeScript** — JavaScript com tipos; compila para JS simples
- **Vite** — servidor de desenvolvimento e bundler moderno para frontend
- **ES modules** — `import` e `export` entre arquivos
- **HMR** — edite, salve, o browser atualiza sem perder o estado
- **tipos em cada fronteira** — a mesma disciplina de DTO, no browser

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Em um arquivo `.ts`, da sua própria cabeça:

1. Escreva e faça `export` de uma `interface` que descreve um slot de reino — um `id` que é um número, um `name` que é uma string e um `day` que é um número.
2. Crie uma variável desse tipo, e coloque uma string onde o número `day` deveria ir.
3. Salve, e veja o que acontece.

Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve. O compilador é o seu avaliador: ele deve sublinhar o tipo errado imediatamente.

<details><summary>Travou? Abra aqui para conferir.</summary>

```ts
export interface KingdomSlot {
  id: number;
  name: string;
  day: number;
}

const slot: KingdomSlot = { id: 1, name: "Eldoria", day: "twelve" };
// TypeScript sinaliza isso: "twelve" é uma string, mas day deve ser um número.
```

O ponto: cada campo tem um tipo após os dois pontos, e TypeScript verifica cada valor em relação a ele. A última linha foi feita para falhar — esse sublinhado vermelho é a razão toda de adicionar tipos.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.4 constrói a **UI componentizada** — extraindo peças reutilizáveis (um `KingdomCard`, um `ResourceList`) para que adicionar uma nova tela fique barato.
