# Módulo 4.6 — Implante o Frontend (Azure Static Web Apps)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O reino no browser vai ao ar na internet hoje. O **Azure Static Web Apps** hospeda o seu build do Vite de graça, com SSL automático, uma rede de entrega de conteúdo global e o mesmo tipo de deploy automático via GitHub Actions que você configurou para a API na Fase 3. No final do dia você vai ter duas URLs: uma para o backend (`kingdom-api-seunome.azurewebsites.net`) e uma para o frontend (`kingdom-seunome.azurestaticapps.net`).

> **Words to watch**
>
> - **Static Web Apps (Azure)** — hospedagem gerenciada para frontends estáticos; integra com GitHub.
> - **CDN** — Content Delivery Network; serve seus arquivos do servidor de borda mais próximo de cada usuário.
> - **build output** — o que o Vite produz em `dist/`; o deploy envia isso.
> - **same-origin / cross-origin** — seu frontend em um domínio chamando a sua API em outro (de volta ao CORS).

---

## Por que Static Web Apps

Static Web Apps é a opção gratuita e gerenciada do Azure para frontends pequenos. O plano gratuito dá 100 GB de largura de banda por mês, suporte a domínio personalizado e SSL gratuito — suficiente para um projeto como este. Ele se conecta diretamente ao GitHub, então uma vez que você aponta para um repositório, um branch e uma pasta de build, cada push implanta sozinho. Uma CDN global significa que a página carrega rápido não importa onde no mundo o usuário esteja. Auth integrado e proteção por senha estão lá se você quiser.

(Outros serviços fazem o mesmo trabalho: Cloudflare Pages, Netlify, Vercel, GitHub Pages. Mesmos padrões, mesmo resultado.)

## Passo 1 — crie o Static Web App

No Portal do Azure:

- Nome: `kingdom-seunome`
- Plano: **Free**
- Fonte: GitHub → escolha seu repositório e branch
- Predefinições de build: Custom
- Localização do app: `web-vite/`
- Localização da saída: `dist/`

Aguarde cerca de dois minutos. O Azure cria o recurso e faz commit de um arquivo de workflow no seu repositório em `.github/workflows/azure-static-web-apps-*.yml`. Puxe essa mudança para a sua máquina — no painel Source Control do VS Code, clique no menu `...` → **Pull**. (Ou no terminal: `git pull`.)

Isso é o deploy feito. Seu frontend está ao vivo em `kingdom-seunome.azurestaticapps.net`. O workflow que o Azure criou sabe onde encontrar o build do Vite (porque você disse `dist/`), e cada push para `main` constrói e implanta de novo.

## Passo 2 — atualize o CORS na API

Sua API agora precisa permitir a origem do Static Web Apps:

```csharp
app.UseCors(p => p
    .WithOrigins(
        "https://localhost:5173",                       // Vite dev
        "https://kingdom-seunome.azurestaticapps.net"  // prod
    )
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());                                // para o cookie de auth
```

`AllowCredentials()` é necessário porque a auth por cookie precisa disso. Há uma regra de CORS que vale saber: `AllowAnyOrigin()` *não pode ser usado junto com* `AllowCredentials()`. O browser recusa essa combinação, porque ela deixaria qualquer site na internet enviar requisições autenticadas para a sua API enquanto finge ser você. Então você tem que listar as origens específicas em vez disso.

## Passo 3 — o URI de redirecionamento OAuth (sem mudança hoje)

No Google Cloud Console você normalmente adicionaria o callback da nova origem. Mas espere — o fluxo de auth acontece na API, não no frontend. O frontend manda o usuário para `https://kingdom-api-seunome.azurewebsites.net/login`, que então o manda para o Google. Então você não precisa de um novo URI de redirecionamento no console do Google; a nova origem é só uma página diferente que inicia o mesmo fluxo de login.

## Passo 4 — aponte o frontend para a API de produção

Duas opções. Escolha uma.

**Opção A — código fixo por ambiente:**

```ts
const API = import.meta.env.PROD
  ? 'https://kingdom-api-seunome.azurewebsites.net'
  : 'https://localhost:5xxx';
```

`import.meta.env.PROD` é `true` na saída de `npm run build` e `false` em `npm run dev`.

**Opção B — variáveis de ambiente do Vite (mais limpa):**

`.env.production`:

```
VITE_API_URL=https://kingdom-api-seunome.azurewebsites.net
```

`.env.development`:

```
VITE_API_URL=https://localhost:5xxx
```

No código: `const API = import.meta.env.VITE_API_URL;`. Mais limpo, e mais difícil de configurar errado.

## O que muda neste módulo

- **NOVO:** `web-vite/.env.development` e `web-vite/.env.production`
- **MODIFICADO:** `web-vite/src/main.ts` — usa `import.meta.env.VITE_API_URL`
- **MODIFICADO:** `Kingdom.Api/Program.cs` — lista de permissões CORS com credenciais
- **NOVO:** `journal/4.6-deploy-frontend.md` — suas notas de configuração do Azure

## Mexa um pouco

Abra `https://kingdom-seunome.azurestaticapps.net` em uma janela anônima. Você pode jogar. Mostre para um amigo; ele não precisa instalar nada.

Rode `npm run build` na sua máquina, depois `npm run preview`. Isso testa o build de produção antes de você implantar e pega problemas de "funciona no dev, quebra em prod" cedo.

Adicione um `staticwebapp.config.json` para roteamento de SPA — quando alguém abre uma URL profunda, o arquivo diz ao Static Web Apps para servir `index.html` em vez disso.

Verifique a aba GitHub Actions. Cada push inicia um build e um deploy. Cerca de três minutos do push ao ar.

## O que você acabou de fazer

O frontend está na internet. Você criou um Static Web App no Azure (plano gratuito), deixou-o gerar o workflow do GitHub Actions, puxou o arquivo de workflow para a sua máquina e agora o seu projeto Vite constrói e implanta a cada push para `main`. Você adicionou a origem de produção à lista de permissões CORS da API e aprendeu por que `AllowAnyOrigin()` e `AllowCredentials()` não podem ser usados juntos. O frontend lê `import.meta.env.VITE_API_URL`, então dev e prod apontam para APIs diferentes sem mudança de código. Dois serviços agora rodam lado a lado: a API em uma URL, o frontend em outra. É assim que aplicativos de produção normalmente são configurados.

**Conceitos que você já sabe nomear:**

- **Static Web Apps** — hospedagem estática gratuita do Azure mais integração com GitHub
- **build output (`dist/`)** — o que o Vite produz; o que o Azure implanta
- **`import.meta.env.VITE_*`** — variáveis de ambiente em tempo de compilação do Vite
- **`AllowCredentials()`** — flag CORS necessária para auth por cookie; incompatível com `AllowAnyOrigin()`
- **dois serviços, uma arquitetura** — frontend e backend implantam independentemente

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Da sua própria cabeça, liste os passos que levaram o seu frontend da sua máquina para uma URL ao vivo. Responda em ordem:

1. O que você disse ao Azure?
2. O que o Azure criou para você?
3. O que faz ele implantar de novo depois da primeira vez?

<details><summary>Travou? Abra aqui para conferir.</summary>

Você deve ser capaz de nomear esses:

- No Portal do Azure você criou um **Static Web App**, escolheu o plano **Free** e apontou para o seu repositório GitHub e branch.
- Você definiu a **localização do app** como `web-vite/` e a **localização da saída** como `dist/` (onde o Vite coloca o build).
- O Azure criou um **arquivo de workflow do GitHub Actions** em `.github/workflows/` e fez commit dele no seu repositório. Você **puxou** esse arquivo para a sua máquina.
- A partir daí, **cada push para `main`** constrói e implanta sozinho — sem passos extras.
- Você adicionou a URL do frontend ao vivo à **lista de permissões CORS** da API, com `AllowCredentials()` para o cookie de auth (e você não pode combinar isso com `AllowAnyOrigin()`).

Se você consegue dizer mais ou menos isso, o procedimento ficou. Os nomes exatos dos menus importam menos do que o fluxo: diga ao Azure onde está o build, ele conecta o auto-deploy, cada push vai ao ar.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.6 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.6 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.7 fecha a Fase 4: o ritual do marco M5 mais a reflexão da Fase 0 — releia o seu código da Spark Week de novo, agora que você sabe mais, e note o quanto você avançou.
