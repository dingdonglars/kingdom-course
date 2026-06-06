# Módulo 3.8 — Deploy para Azure e GitHub Actions CI/CD

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o reino está *na internet*. Uma URL real — `https://kingdom-api-seunome.azurewebsites.net` — que qualquer pessoa no mundo pode visitar. E cada push para `main` redeploya o site automaticamente via GitHub Actions. De *código no meu computador* para *URL ao vivo que se implanta sozinha* em um módulo.

Esse também é o módulo em que vamos desacelerar um momento nos **bons hábitos que mantêm um serviço ao vivo seguro**. Secrets ficam fora do repositório. HTTPS está ligado. Os logs são estruturados. Os hábitos que você define hoje são os que vai manter pelo resto da carreira.

> **Words to watch**
>
> - **App Service (Azure)** — o Azure roda e hospeda seu app web para você; o tier gratuito F1 não custa nada
> - **CI/CD** — Continuous Integration and Continuous Deployment — build e deploy automaticamente a cada push
> - **GitHub Actions** — o jeito embutido do GitHub de rodar jobs; arquivos YAML em `.github/workflows/`
> - **publish profile** — os detalhes de login que o Azure te dá para que os deploys possam fazer login
> - **environment variables** — configurações para produção; nunca coloque secrets no repositório, jamais

---

## Por que Azure App Service Free

Três razões:

1. **Tier gratuito F1** — $0/mês. Os limites: 1 GB de RAM, CPU compartilhada, dorme após 20 minutos sem tráfego, e 60 minutos de tempo de computação por dia. Suficiente para um projeto de aprendizado.
2. **Roda .NET diretamente** — sem container Docker no caminho mais simples; você só envia a saída do build.
3. **Funciona com GitHub** — o Azure tem um wizard de *deploy do GitHub* com um clique que configura a action para você.

Outras opções: Azure Container Apps, Render, Fly.io, AWS App Runner. Mesmas ideias, painéis de controle diferentes.

## O deploy em cinco passos manuais (uma vez só)

> **Cuidado — essa é configuração real na nuvem. Melhor feito com o portal Azure aberto. Anote cada passo em `journal/3.8-deploy-api.md`.**

1. **Crie um App Service**
   - portal.azure.com → Create resource → Web App
   - Nome: `kingdom-api-<seunome>` (precisa ser globalmente único)
   - Runtime stack: .NET 10 (ou último LTS)
   - Pricing: **F1 Free**
   - Região: a mais próxima de você (para responder mais rápido)
2. **Adicione o URI de redirecionamento OAuth ao seu cliente Google** no Google Cloud Console:
   - `https://kingdom-api-<seunome>.azurewebsites.net/signin-google`
3. **Defina as variáveis de ambiente de produção** em App Service → Configuration:
   - `Google__ClientId` = seu client id de prod (ou o mesmo de dev)
   - `Google__ClientSecret` = o secret
   - Note: sublinhado duplo `__` vira `:` para a configuração do ASP.NET — `Google:ClientId`
4. **Obtenha o publish profile** (ou configure Federated identity):
   - App Service → Get publish profile → baixe o arquivo `.PublishSettings`
5. **Adicione o publish profile ao GitHub Secrets**:
   - Repositório GitHub → Settings → Secrets and variables → Actions → New secret
   - Nome: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Valor: cole o conteúdo do arquivo `.PublishSettings`

## O workflow CI/CD

`.github/workflows/deploy.yml`:

```yaml
name: Build & deploy to Azure App Service

on:
  push:
    branches: [main]
  workflow_dispatch:        # também deixa acionar manualmente pela UI

env:
  AZURE_WEBAPP_NAME: kingdom-api-seunome        # MUDE isso
  DOTNET_VERSION: '10.0.x'

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Test
        run: dotnet test --configuration Release --no-build --verbosity minimal

      - name: Publish
        run: dotnet publish Kingdom.Api/Kingdom.Api.csproj -c Release -o publish

      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: app
          path: publish/

  deploy:
    needs: build
    runs-on: ubuntu-latest
    steps:
      - uses: actions/download-artifact@v4
        with:
          name: app
          path: publish/

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v3
        with:
          app-name: ${{ env.AZURE_WEBAPP_NAME }}
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: publish/
```

Leia o que isso faz:

- **`on: push: branches: [main]`** — roda em cada push para `main`
- **Job 1 (`build`)** — restore, build, test, depois publica num diretório `publish/` e envia como saída de build
- **Job 2 (`deploy`)** — baixa a saída de build, depois a envia para o Azure
- **Os testes rodam antes do deploy.** Um teste falhando para o deploy. **CI é o guarda.**

**Faça commit e push do workflow.** No painel Source Control do VS Code (`Ctrl + Shift + G G`):

1. Prepare `.github/workflows/deploy.yml` — clique em `+` ao lado.
2. Mensagem de commit: *"[infra] add deploy-to-Azure workflow"*.
3. Clique no **check** azul para fazer commit.
4. Clique em **Sync Changes** para dar push no GitHub.

> **Ou no terminal:**
>
> ```powershell
> git add .github/workflows/deploy.yml
> git commit -m "[infra] add deploy-to-Azure workflow"
> git push
> ```

GitHub → aba Actions → veja o workflow rodar. Cerca de três minutos depois, seu site está no ar.

## Checklist — hábitos que mantêm um serviço ao vivo seguro

- [x] **Secrets**: em variáveis de ambiente / Key Vault, nunca no repositório (Módulo 3.5 + este módulo)
- [x] **Só HTTPS**: App Service → TLS/SSL settings → "HTTPS Only = On"
- [x] **Redirecionamento OAuth**: URL de produção adicionada ao cliente Google
- [x] **Logging**: logs estruturados visíveis em App Service → Log stream
- [ ] **DB**: SQLite no App Service está bem para um projeto hobby, mas no tier Free o arquivo é temporário — é perdido quando o app reinicia. Para o Módulo 3.9 em diante, mude para Azure SQL Database (ainda elegível ao tier gratuito) ou PostgreSQL.
- [ ] **Domínio personalizado mais certificado**: opcional; cuidado para você se sair do tier Free.
- [ ] **Monitoramento**: Application Insights (tier gratuito) para números reais e traces.

## Mexa um pouco

Depois do deploy, visite `https://kingdom-api-<seunome>.azurewebsites.net`. O reino está no ar. Mande a URL para um amigo. Ele clica em Sign In with Google, faz login, e começa a jogar.

Observe `App Service → Log stream` enquanto um amigo usa o app. Suas chamadas `LogInformation` estruturadas aparecem em tempo real.

Faça push de uma pequena mudança. Veja a action rodar. Veja a URL atualizar. Esse é o loop de deploy que você vai usar por anos.

Tente `https://kingdom-api-<seunome>.azurewebsites.net/openapi/v1.json` — a descrição OpenAPI também está no ar. Qualquer pessoa pode ler como sua API funciona.

## O ponto principal

Nunca faça deploy à mão duas vezes. O primeiro deploy à mão está bem. O segundo é um sinal de que você deve automatizar. Automação torna deploys chatos. Deploys chatos são deploys que você faz com frequência. Quando você faz deploy com frequência, cada mudança é pequena. Mudanças pequenas significam que você pode mover rápido e consertar coisas rapidamente. O workflow que você escreveu hoje é o que torna tudo isso tranquilo daqui para frente.

## O que você acabou de fazer

Você pegou sua API local e colocou na internet pública numa URL que qualquer pessoa pode visitar. Você configurou um Azure App Service no tier Free, definiu as variáveis de ambiente de produção, baixou um publish profile, e o guardou como um secret do GitHub. Depois escreveu um workflow do GitHub Actions que faz build, testa, e faz deploy a cada push para `main` — três minutos do `git push` para uma URL no ar. Os testes guardam o deploy, então um teste falhando para a release. Você também passou pelo checklist de hábitos que mantêm um serviço ao vivo seguro: só HTTPS, secrets fora do repositório, e logs estruturados que você pode observar no log stream do App Service. O loop de deploy que você configurou hoje é o que você vai usar por anos.

**Conceitos que você já sabe nomear:**

- **App Service** — o Azure roda e hospeda seu app web; o tier F1 Free é para uso hobby
- **CI/CD** — build e deploy automaticamente a cada push
- **GitHub Actions** — roda jobs a partir de um arquivo YAML em `.github/workflows/`
- **publish profile** — os detalhes de login para o passo de deploy, guardados como secret do GitHub
- **environment variables (prod)** — configurações e secrets, definidos na plataforma, nunca no repositório
- **HTTPS-only** — necessário para cookie auth; ative em App Service settings

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — descreva o loop de deploy da sua própria cabeça. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

No papel:

1. Escreva os passos que acontecem automaticamente entre você rodar `git push` para `main` e seu site estar no ar — em ordem.
2. Depois responda: onde nessa ordem os testes rodam, e o que acontece se um teste falhar?

<details><summary>Travou? Abra aqui para conferir.</summary>

O loop, em ordem:

1. Você faz `git push` para `main`.
2. GitHub Actions inicia o workflow (porque ele observa `push` para `main`).
3. **Job de build:** restore, build, **testa**, depois publica a saída.
4. **Job de deploy:** baixa essa saída e envia para o Azure App Service.
5. Cerca de três minutos depois, a URL no ar está atualizada.

Os testes rodam no job de build, **antes** do deploy. Se um teste falha, o job de build para, então o job de deploy nunca roda e o código quebrado nunca chega ao site ao vivo. É por isso que a regra é *"nunca faça deploy à mão duas vezes"* — automatize uma vez, os testes guardam cada release, e os deploys ficam chatos e seguros.

</details>

## Movimento git da semana — `gh pr` pela CLI

Você tem aberto pull requests clicando no github.com. A CLI `gh` é um jeito mais rápido depois que você usou algumas vezes. Instale de [cli.github.com](https://cli.github.com/) — é um instalador de arquivo único.

Depois de instalar, rode `gh auth login` uma vez para conectar com sua conta GitHub.

> **Esse é só via CLI — o painel não tem um botão para isso.** `gh` é uma ferramenta de linha de comando por design:
>
> ```powershell
> gh pr create --title "M4 — Web API" --body "..."
> gh pr list                          # PRs no repositório atual
> gh pr view 12                       # veja detalhes do PR #12
> gh pr checks                        # status CI do PR atual
> ```

A página web no github.com funciona bem. `gh` é só mais rápido depois que você sabe os comandos. Tente `gh pr create` no próximo PR e veja se você gosta.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 3.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 3.8 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos pelo painel/CLI se precisar de um guia. Traga as respostas do quiz de que você estiver menos certo para a próxima conversa semanal.

## Próximo

O Módulo 3.9 fecha a Fase 3: **o fechamento do marco M4 mais o AI Unlock**. Esse é um grande — você configura o Claude Code pela primeira vez, e a flag de modo de AI muda de `pre-unlock` (o padrão inicial do Claude) para `post-unlock` (ajuda real). Tudo que você constrói da Fase 4 em diante tem o Claude como um colaborador real.
