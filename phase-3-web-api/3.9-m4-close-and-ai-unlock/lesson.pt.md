# Módulo 3.9 — Fechamento do M4 + o AI Unlock

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O reino está na internet. Amigos fazem login com Google, salvam o progresso, e jogam. A Fase 3 está pronta. Este módulo é três coisas ao mesmo tempo: o **fechamento do marco M4** (log de wins, antes/depois, o ritual por marco), **a chegada do Claude Code** (conta + instalação), e o **AI Unlock** (o momento em que as regras em torno de código assistido por AI mudam). Até hoje você trabalhou sem um assistente de AI — Lars era quem ajudava quando você travava, pelo Slack `#help`, sob a regra dos 20 minutos. Hoje o Claude chega, e no mesmo dia as regras se abrem: você pode pedir ao AI para *escrever código para você* — desde que possa explicar cada linha que vai mesclar.

Essa é a mudança mais importante em todo o ano. A disciplina que você construiu pelas Fases 0 a 3 — a regra motor-vs-shell, testes que dão o mesmo resultado toda vez, ler antes de escrever, nomear as coisas de propósito — é o que torna hoje seguro. Um AI que pode escrever seu código é uma ferramenta poderosa, e ela só funciona bem se essa disciplina já estiver lá. Você ganhou isso.

> **Words to watch**
>
> - **AI Unlock** — a mudança nomeada do curso: AI vai de "só quando você está travado" para "colaborador real"
> - **mode flag** — uma única linha no `CLAUDE.md` que o AI lê para decidir como se comportar
> - **viva** (vee-vah) — uma verificação oral individual do seu código; o mentor (Lars) escolhe uma linha e pede que você a explique
> - **AI-assistance section** — um bloco obrigatório em cada descrição de PR pós-unlock: quais linhas o AI escreveu, e quais você escreveu?

---

## O que está pronto

Você entregou:

- Um reino console (Fase 1, M2)
- Persistência com EF Core, save slots, uma UI interativa (Fase 2, M3)
- Uma API HTTP ao vivo na internet, com Google sign-in, multi-usuário, testes de integração, e auto-deploy (Fase 3, M4)

Contando: quatro marcos alcançados, cerca de doze semanas do curso para trás, oitenta e tantos testes passando entre o motor, a persistência, e a API. **Você não é mais um iniciante.**

## O ritual do marco M4

Mesmo padrão que M2 e M3 — mas este importa mais, porque M4 é o marco em que o AI Unlock entra em vigor:

1. **Atualize o README** na raiz do repositório — volte pelas quatro seções do Módulo 0.4. *Como rodar* agora precisa da URL ao vivo mais o passo `dotnet user-secrets`; *O que aprendi* ganha um parágrafo da Fase 3 (HTTP, OpenAPI, OAuth, persistência multi-usuário, deploy no App Service). Todo fechamento de marco volta ao README — o mesmo hábito que você começou no fechamento do M2.
2. Abra `journal/wins.md`.
3. Escreva a entrada M4:

   ```markdown
   ## M4 — Phase 3 — Live API

   - Live URL: `https://kingdom-api-seunome.azurewebsites.net`
   - Friends can sign in via Google and play
   - 80+ tests pass; CI/CD redeploys on every push to `main`
   - Real production hygiene: secrets out of repo, HTTPS-only, structured logs

   **Before:** the kingdom died when the program closed.
   **After:**  the kingdom lives on the internet at a URL you can text.

   Posted to `#wins` on YYYY-MM-DD.
   ```

4. Tire um screenshot da sua URL ao vivo mais a Scalar UI. Poste no `#wins`.
5. **Marque o marco.** Esse é só via CLI — o painel não tem um botão para tags: `git tag m4-phase-3-complete && git push origin m4-phase-3-complete`
6. **Abra o PR M4.** No github.com → seu repositório `kingdom` → o banner *"phase-3 had recent pushes — Compare & pull request"* (ou *Pull requests → New pull request*, base `main`, compare `phase-3`). Título: `M4 — Phase 3 — Live API`. Body: os bullets de `wins.md` deste marco + `**Reviewer:** @dingdonglars` + a seção de AI-assistance do template pós-unlock. (Deixe a seção de AI-assistance vazia para este PR — a Fase 3 foi pré-unlock — mas inclua de qualquer forma, para marcar que as novas regras começam agora.) Lars revisa → Aprova → você faz Merge → deleta a branch `phase-3`. Na sua máquina: `git switch main && git pull`. (Passo a passo completo: Módulo 1.10.)

## O AI Unlock

O Claude chegou no modo `pre-unlock` (seu padrão — o que ele lê no `CLAUDE.md` quando instalado pela primeira vez). As regras pré-unlock:

- **Não:** escrever código de exercício do curso, responder quizzes, ou refatorar por você
- **OK quando pedido (limitado):** ajudar quando você está travado (problemas de git, configuração de ambiente, mensagens de erro que você já tentou), explicar um conceito *depois* de você ter tentado, escrever boilerplate de rotina
- **Sempre ok:** consultar sintaxe, sugerir nomes, responder *"isso é boa prática?"*

O Claude lê `Current mode: pre-unlock` e diz não quando você pede para ele escrever seu código. **Hoje, você muda a flag para `post-unlock`.** O novo quadro:

- *Não* ainda se aplica — mas agora a linha é *"não pule o aprendizado,"* não *"não escreva código"*
- *OK quando pedido* fica maior — o AI pode escrever código para você agora, com uma regra estrita anexada
- *Sempre ok* fica igual

### A regra rígida pós-unlock

> **Você precisa ser capaz de explicar cada linha de código gerado por AI antes de fazer o merge.**

Na prática, quando você pede código ao AI, o trabalho do AI também muda. As respostas dele terminam com:

> *"Antes de fazer merge disso, me explique o que cada linha faz. Se você não puder explicar uma linha, me pergunte sobre ela em vez de fazer o merge."*

Esse é o hábito que impede você de fazer ship de código que funciona mas que você não entende. Código que você não entende não pode ser depurado, refatorado, ou construído em cima depois. **Ser capaz de explicar é a regra para fazer o merge.**

### O template de PR ganha uma seção

Toda descrição de PR pós-unlock inclui uma seção de AI-assistance:

```markdown
## AI assistance

- Bot: Claude / Copilot / Cursor / etc.
- Lines I wrote myself: [files / line ranges]
- Lines AI wrote (and I understand): [files / line ranges]
- Anything I'm unsure about: [be honest — flag for the mentor]
```

`/milestone-review` lê esta seção e a usa para configurar a viva. **Você vai ser pedido para explicar uma linha que o AI escreveu, escolhida aleatoriamente.** Se você não puder, o merge espera — não como punição, mas como um *"há uma lacuna aqui; vamos fechar antes que acumule ao longo do tempo."*

## Configurar o Claude Code — instalar + conta

Antes de mudar a flag de modo, o Claude Code precisa estar na sua máquina e conectado a uma conta. Lars fica com você para esta parte — ele coloca o cartão na assinatura, mas a conta está no **seu nome e seu email**, com uma senha que só você sabe.

### 1. Crie sua conta Anthropic, junto com Lars

No navegador, vá para <https://console.anthropic.com>. Cadastre-se com seu email, escolha uma senha (só você sabe), e termine a confirmação de email. Lars insere os detalhes do cartão para o nível de assinatura que ele escolheu. Você agora tem uma conta Anthropic que Lars paga.

### 2. Instale o Claude Code

Claude Code é a versão de linha de comando — um chat que roda no seu terminal, na mesma pasta que o seu repositório, com o seu código bem ali para ele ler. Instale:

```powershell
npm install -g @anthropic-ai/claude-code
```

Verifique:

```powershell
claude --version
```

Você deve ver um número de versão.

### 3. Faça login

No Windows Terminal:

```powershell
cd C:\code\kingdom
claude
```

O primeiro lançamento abre um navegador para fazer login na Anthropic. Faça login com a conta que você acabou de criar. Após o login, o navegador fecha, o terminal mostra um prompt, e o Claude está rodando na sua pasta kingdom.

### 4. Tente um slash command

Digite `/` no prompt do Claude. Você vai ver uma lista que inclui `/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`. Esses vieram com o kit do dia 1 (em `.claude/commands/`) e estavam esperando o Claude chegar.

Escolha um. *"explique o que `git push` realmente faz, brevemente"* — `/explain-this-concept` é o certo para isso. Leia o que volta. Digite `/exit` (ou `Ctrl + C`) para sair.

As regras completas — o que perguntar, o que não perguntar, os três buckets — estão em `ai-tools.md`. Leia esta noite; você vai voltar a ele durante o resto do ano.

> **Por que Claude e não Copilot ou ChatGPT?** Claude é o que Lars usa, então é o que Lars pode te ajudar com. Copilot, Cursor, e ChatGPT também são boas ferramentas — depois que você sabe os padrões que o curso ensina com Claude, esses padrões se transferem para os outros. O *porquê* completo está em `ai-tools.md`.

---

## Mudando a flag de modo — passo manual

Abra `CLAUDE.md` na raiz do seu repositório kingdom (`C:\code\kingdom\CLAUDE.md`). Encontre a linha:

```diff
-**Current mode: `pre-unlock`.**
+**Current mode: `post-unlock`.**
```

Mude. Salve.

Na próxima vez que você (ou qualquer agente AI) abrir este projeto, o AI lê o novo modo e se comporta do novo jeito. **Você não precisa mudar nada no prompt do AI — esta linha no arquivo é o que o controla.**

**Faça commit.** *"[M4] AI Unlock — flip mode flag pre-unlock → post-unlock"*. (Painel Source Control → prepare → commit → Sync. Ou no terminal: `git add . && git commit -m "[M4] AI Unlock — flip mode flag pre-unlock → post-unlock" && git push`.)

## O que fica igual

- **Regras rígidas não mudam:** sem nomes no seu trabalho visível, só inglês, sem inferência de escola/família.
- **Três buckets não mudam:** a fronteira entre amarelo e vermelho se moveu; os buckets em si não.
- **Protocolo do mentor não muda:** regra dos 20 minutos, sync semanal, revisão de PR de marco.
- **Ritual por marco não muda:** log de wins, post no Slack, antes/depois.

## Mexa um pouco

Compare o mesmo prompt antes e depois da mudança. Peça ao Claude *"escreva uma função que converte um Kingdom em uma string JSON."* Antes do unlock: ele diz não. Depois do unlock: ele escreve a função e pede que você explique cada linha.

Leia `CLAUDE.md` do começo ao fim mais uma vez. Você vai voltar a ele muitas vezes este ano.

Olhe alguns PRs de "AI me ajudou" em projetos de código aberto bem gerenciados. Note o padrão: commits pequenos, revisão cuidadosa arquivo por arquivo, e o autor humano ainda entendendo a coisa toda. O AI ajuda; o autor ainda entende o código.

## O ponto principal

O AI é uma ferramenta poderosa, não um piloto automático. Ferramentas poderosas precisam de respeito. A disciplina que você construiu pelas Fases 0 a 3 — motor vs shell, testes que dão o mesmo resultado toda vez, ler antes de escrever, nomes claros que se justificam — é o que te deixa usar o AI bem e manter o controle. Sem essa disciplina, código escrito por AI vira um problema. Com ela, o AI te faz fazer mais sem te dar mais bugs.

## O que você acabou de fazer

Você fechou o M4 — o marco mais importante do ano. O seu reino está na internet, amigos podem fazer login e jogar, e CI/CD redeploya a cada push. Oitenta e tantos testes passam em três projetos. Você também fez o AI Unlock: a flag de modo no `CLAUDE.md` foi de `pre-unlock` para `post-unlock` em três repositórios, o que significa que todo agente AI que abrir esses projetos daqui para frente segue as novas regras. A regra rígida fica: você precisa ser capaz de explicar cada linha de código escrito por AI antes de fazer o merge. A Fase 4 começa com o AI como um colaborador real — e com a disciplina que você passou seis meses construindo, pronta para tornar essa mudança segura.

**Conceitos que você já sabe nomear:**

- **AI Unlock** — a mudança nomeada; M4; hoje
- **mode flag** — uma única linha no `CLAUDE.md` que controla como o AI se comporta
- **viva** — verificação oral individual em marcos; você explica uma linha escolhida aleatoriamente
- **AI-assistance PR section** — obrigatória após o unlock; o que você escreveu vs o que o AI escreveu
- **explicação como regra de merge** — você não pode fazer ship de código que não pode explicar

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — explique o AI Unlock da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Com suas próprias palavras, responda três coisas no papel:

1. Que linha única no `CLAUDE.md` você mudou hoje, e de que para que?
2. Qual é a única regra rígida para código escrito por AI agora?
3. O que fica exatamente igual depois do unlock?

<details><summary>Travou? Abra aqui para conferir.</summary>

1. Você mudou **`Current mode: pre-unlock`** para **`Current mode: post-unlock`**. Essa linha é o que todo agente AI lê para decidir como se comportar — você não toca no prompt do AI, só a flag no arquivo.
2. A regra rígida: **você precisa ser capaz de explicar cada linha de código escrito por AI antes de fazer o merge.** Se você não puder explicar uma linha, você pergunta sobre ela em vez de fazer o merge. Código que você não pode explicar não pode ser depurado ou construído em cima depois.
3. Não muda: as regras rígidas (sem nomes no seu trabalho visível, só inglês), os três buckets (só a linha entre amarelo e vermelho se moveu), o protocolo do mentor (regra dos 20 minutos, sync semanal, revisão de PR de marco), e o ritual por marco.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.9 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.9 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

> *Você acabou de entregar o M4. Hora do ritual: atualizar o README, entrada no `wins.md`, post no `#wins`, frase de antes/depois. Depois tire o resto do dia.*

## Próximo

**A Fase 4 começa.** A Fase 4 é o **reino no navegador** — seu motor movido para uma camada externa JavaScript/TypeScript, servida junto com sua API. Com o AI Unlock agora em vigor, você vai poder trabalhar por mudanças muito mais rápido.
