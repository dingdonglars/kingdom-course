# Módulo 0.0 — Setup

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Ao final de hoje, o seu computador está pronto, a sua pasta de código está criada e você está no Slack. Você vai instalar um editor, um compilador, um terminal e o git. Vai criar uma conta no GitHub. Vai criar as duas pastas onde cada linha de código do ano vai viver. E vai entrar no espaço de chat onde você e Lars se falam durante a semana. Tudo isso com Lars do seu lado. A partir do próximo módulo, você trabalha sozinho. Isso leva cerca de duas horas, geralmente menos.

## O que você precisa primeiro

- Um computador Windows (você tem esse).
- **O seu celular**, carregado. Você vai instalar o Slack nele durante a Parte 3.
- Lars sentado do seu lado. Algumas telas de instalação são mais fáceis com alguém que já fez isso antes. A conta do GitHub, o convite do Slack e a instalação no celular ficam mais tranquilos com Lars na sala.
- Umas duas horas sem interrupções.

> **Depois de hoje — leia o protocolo do mentor.** `MENTOR-PROTOCOL.md` é o acordo de como Lars e você vão trabalhar durante o ano: a regra dos 20 minutos, os quatro canais, o sync semanal e como você pode parar se quiser. Você encontra em dois lugares. Está no seu computador em `C:\code\kingdom-course\MENTOR-PROTOCOL.md` (você o copia na Parte 2). Também está fixado em `#all-kingdom-hq` depois que você entrar no Slack na Parte 3. Leia na primeira semana e traga para o primeiro sync semanal.

---

## Parte 1 — Ferramentas (Lars te guia; cerca de 50 minutos)

Você vai instalar seis ferramentas, adicionar uma extensão no VS Code e dizer ao git quem você é. Lars fica com você. Cada instalação é basicamente clicar em *próximo, próximo, próximo*. Nenhuma delas é difícil.

Instale o **VS Code primeiro**. Assim, o instalador do Git pode colocar o VS Code como o seu editor padrão, em vez do Vim. Vim é um editor antigo e complicado, e você não quer conhecê-lo hoje.

1. **VS Code** — o editor onde você vai escrever código durante o próximo ano. Baixe em <https://code.visualstudio.com/>. Opções padrão. Certifique-se de que *"Add to PATH"* está marcado (já vem marcado por padrão).
2. **Git for Windows** — a ferramenta que todo desenvolvedor no mundo usa para salvar seu trabalho. Baixe em <https://git-scm.com/download/win>. Use as opções padrão **exceto em uma tela**: quando o instalador perguntar *"Choosing the default editor used by Git"*, escolha **Use Visual Studio Code as Git's default editor**. Está na lista suspensa. O VS Code já está instalado desde o passo 1, então ele aparece lá. Vim é o que o instalador escolheria sozinho. Você não precisa dele hoje.
3. **.NET SDK 10** — o kit para criar programas em C#. Baixe em <https://dotnet.microsoft.com/download/dotnet/10.0>. Opções padrão.
4. **Node.js LTS** — você vai precisar disso mais tarde, na Fase 4, para a versão do navegador. Baixe em <https://nodejs.org/>. Escolha a versão LTS e use as opções padrão.
5. **Windows Terminal** — um aplicativo de terminal moderno. Tem fontes melhores, várias abas ao mesmo tempo, e mais. No menu Iniciar, abra a **Microsoft Store**, pesquise **Windows Terminal** e clique em **Get**. No Windows 11 ele costuma já estar instalado. Pesquise primeiro; se já estiver lá, pule a instalação.
6. **Uma conta no GitHub** — o seu lar na internet para código. Cadastre-se em <https://github.com>. Você escolhe o nome de usuário. Ele vai fazer parte de quem você é como desenvolvedor, então escolha um que você goste.

### Verifique as instalações

Abra o **Windows Terminal** (pesquise no menu Iniciar — fixe-o na barra de tarefas enquanto estiver lá; você vai abrí-lo cinco vezes por dia). Ele inicia no PowerShell. Digite:

```powershell
git --version
dotnet --version
node --version
```

Você deve ver três números de versão. Se algum deles mostrar "not found", fale com Lars.

### Adicione a extensão C# Dev Kit no VS Code

Uma extensão é um pequeno complemento que dá poderes extras ao VS Code. Sem essa extensão, os seus arquivos C# (arquivos `.cs`) não têm sugestões de código, nem depurador, nem cores.

1. Abra o VS Code.
2. Clique no ícone de **Extensions** na barra lateral esquerda (quatro quadrados, quarto ícone). Atalho: `Ctrl + Shift + X`.
3. Pesquise **C# Dev Kit** (publicador: Microsoft).
4. Clique em **Install**. Ele traz junto o servidor de linguagem C#. Isso leva cerca de um minuto.

Quando você abrir um arquivo `.cs` mais tarde, verá cores, sugestões de código e um pequeno link *Run | Debug* acima do `Main`. Tudo isso vem dessa extensão.

> **Um hábito que vale aprender agora.** Existe uma regra que torna rodar e depurar sempre tranquilo: abra cada programa na *sua própria janela* — um programa, uma janela. É uma página curta. Leia `running-your-project.md` uma vez agora e volte nele na primeira vez que o *Run* ou o depurador der problema.

### Diga ao git quem você é

Todo commit recebe um nome e um e-mail. Defina uma vez só, para todo o computador, iguais à sua conta do GitHub:

```powershell
git config --global user.name "seu-usuario-no-github"
git config --global user.email "o-email-que-voce-usou-no-github@example.com"
```

Use o mesmo e-mail com que você se cadastrou no GitHub — é assim que o GitHub liga os commits à sua foto de perfil e ao gráfico de contribuições. Confirme com:

```powershell
git config --global user.name
git config --global user.email
```

Você deve ver o que acabou de digitar sendo repetido de volta.

---

## Parte 2 — Configure a sua pasta de trabalho (cerca de 15 minutos; você dirige)

Antes de qualquer código, configure a pasta onde tudo do curso vai viver.

> **O que o git realmente é, em poucas frases.** Git é uma ferramenta que tira *fotos* do seu código. Toda vez que você faz um commit, ele salva o estado inteiro da pasta com um rótulo, para que você possa voltar a ele mais tarde. As fotos ficam em dois lugares: no seu computador (a cópia *local*) e no GitHub (a cópia *remota*). Fazer push copia as fotos novas para o GitHub. Você pode puxá-las de volta para qualquer outro computador. Um *commit* é uma foto rotulada com uma mensagem curta dizendo o que mudou. Um *push* envia esses commits para a internet. Você vai fazer dezenas deles na primeira semana. Parece estranho antes de parecer normal, e tudo bem. A grande ideia é esta: o seu código nunca é só *o arquivo como está agora*. É toda a história de como ele chegou até aqui, e o git se lembra de tudo.

### Um lar para o seu código

Escolha uma pasta no seu computador onde vai ficar todo o seu trabalho do curso. Não coloque dentro de `Documents`. O Windows muitas vezes copia essa pasta para o OneDrive, e o OneDrive atrapalha o git. A escolha mais simples e limpa é uma pasta no topo do seu drive `C:`. Abra o Windows Terminal e digite:

```powershell
mkdir C:\code
```

Daqui para frente, tudo relacionado a este curso vai dentro de `C:\code\`.

### Coloque o curso no seu computador

Você está lendo isso no GitHub agora. De agora em diante, vai ler as lições do seu próprio computador. Isso é mais rápido, funciona sem internet, e o mesmo download inclui o kit inicial que você vai precisar no próximo módulo. No Windows Terminal:

```powershell
cd C:\code
git clone https://github.com/dingdonglars/kingdom-course
```

Agora você tem `C:\code\kingdom-course\` no seu computador: todas as lições, o glossário e o kit inicial.

### Crie o SEU repositório no GitHub

Este é o repositório para *o seu* ano de trabalho. É separado do repositório do curso. Vá para <https://github.com/new>.

1. **Dê o nome `kingdom` ao repositório.** *(As lições todas dizem `kingdom`. Se você escolher um nome diferente, use o seu nome onde a lição disser `kingdom`.)*
2. **Deixe como Public.**
3. **Não marque "Add a README file".** O kit do dia 1, no próximo módulo, já traz o dele.
4. Clique em **Create repository**.

Copie a URL do repositório no botão verde **Code**.

### Adicione Lars como colaborador

Mais para frente no ano, Lars vai revisar os seus pull requests. Para permitir isso, dê acesso a ele agora enquanto você ainda está no github.com. Na página do seu repositório, clique em **Settings** (canto superior direito, na fila de abas), depois em **Collaborators** (barra lateral esquerda), depois em **Add people**. Digite `dingdonglars`, escolha-o na lista, escolha acesso **Read** e clique em **Add to repository**.

Lars recebe um e-mail de convite e aceita do lado dele. Depois disso, ele pode revisar os seus pull requests e dar o carimbo oficial de *Approve*. Acesso de leitura significa que ele só pode olhar e revisar. Ele não pode enviar nem mesclar nada no seu repositório.

### Clone o SEU repositório em `C:\code\`

No VS Code:

1. Abra o VS Code.
2. `Ctrl + Shift + P` para abrir o Command Palette → digite *"Git: Clone"* → Enter.
3. Cole a URL do repositório.
4. **Quando o seletor de arquivos abrir, vá para `C:\code\` e clique em _Select Repository Location_.** O seletor muitas vezes começa na sua pasta pessoal ou em `Documents`. Certifique-se de que a barra de caminho mostra `C:\code\` antes de clicar. Se não, o repositório vai parar no lugar errado.
5. Quando o VS Code perguntar *"Would you like to open the cloned repository?"*, clique em **Open**.

Agora você tem `C:\code\kingdom\` no seu PC, conectado ao GitHub, aberto no VS Code.

> **A partir de amanhã**, existem duas formas de voltar aqui. No VS Code: *File → Open Recent → kingdom*. Ou no Windows Terminal: digite `cd C:\code\kingdom`, depois `code .` — o ponto significa *"esta pasta."* De qualquer forma, o VS Code abre na pasta kingdom.

> **Ou no terminal.** Você também pode fazer o clone pelo PowerShell. Isso é útil quando você está longe do VS Code, ou seguindo um tutorial que usa o terminal:
>
> ```powershell
> cd C:\code
> git clone <a-url-que-voce-acabou-de-copiar>
> cd kingdom
> ```
>
> Os dois jeitos fazem a mesma coisa. Durante o ano, você vai usar principalmente os botões do git do VS Code. O terminal continua lá para as vezes que for mais rápido.

---

## Parte 3 — Entre no Slack (Lars fica com você; cerca de 25 minutos)

O Slack é onde você e Lars se falam durante o ano quando não estão na mesma sala: as suas conquistas, os pedidos de ajuda e os links dos marcos. Fazer essa parte com Lars bem aqui significa que o link de convite está pronto para colar, a instalação no celular é verificada e vocês configuram as notificações juntos. A partir do momento em que essa parte terminar, `#help` é onde você vai quando estiver travado.

### Aceite o convite

Lars te manda um link para um espaço chamado **`kingdom-hq`**. Abra-o.

- Cadastre-se com o seu **e-mail pessoal**, não o escolar. Escolas às vezes bloqueiam logins externos.
- **Nome de exibição:** algo simples e em letras minúsculas, como o seu primeiro nome. Lars vê isso. É um espaço privado.
- Adicione uma foto de perfil se quiser. Uma foto ou um avatar são ótimos — qualquer coisa que mostre que é você.

### Instale o Slack no seu laptop

A versão no navegador funciona, mas o aplicativo de desktop é mais rápido e trata as notificações direito. Baixe em <https://slack.com/downloads>. Faça login com o mesmo e-mail. Escolha o espaço `kingdom-hq`.

### Instale o Slack no seu celular

Isso importa mais do que você pensa. Lars não responde na hora. Ele responde em `#help` quando pode, não no segundo em que você pergunta. Se o Slack só estiver no seu laptop, você vai perder respostas que chegam enquanto você está longe dele. App Store ou Play Store, pesquise Slack, instale, faça login.

### Configure as notificações

Do jeito padrão, o Slack te avisa em toda mensagem. Para um Slack de duas pessoas, isso até que está certo, mas alguns ajustes por canal ajudam:

- `#wins` → notificações **ligadas**. Você vai querer saber quando Lars posta uma conquista *sua* (acontece).
- `#help` → notificações **ligadas**. Quando Lars responder, você quer ver rápido.
- `#milestones` → notificações **ligadas**. Mesmo motivo.
- `#all-kingdom-hq` → notificações **só em menções**. Mensagens de agendamento não precisam de aviso sonoro.

No celular: toque no nome do espaço, depois em *Notifications*, depois em cada canal.

### Os quatro canais

| Canal | Para que serve |
|---|---|
| `#all-kingdom-hq` | Agendamentos, planos e tudo que não cabe em outro lugar. O **MENTOR-PROTOCOL** está fixado aqui — um clique sempre que você quiser reler as regras. |
| `#wins` | Coisas que você terminou e enviou. Um post por conquista real. Sem resposta esperada. |
| `#help` | Travou? Pergunte aqui. O tópico do canal é *"Mostre o que você tentou."* A regra dos 20 minutos vale. |
| `#milestones` | Os sete grandes momentos — M0 a M6 — e os seus links de marco. |

Você não vai postar nada em `#wins` hoje. Isso vem no final do próximo módulo de código, quando o Roast-O-Matic existir e houver algo real para comemorar. Por enquanto, Lars te leva até a mensagem fixada do **MENTOR-PROTOCOL** em `#all-kingdom-hq` para que você saiba onde ela está.

---

## O que você acabou de fazer

Você instalou seis ferramentas (VS Code, Git, .NET 10, Node.js, Windows Terminal e uma conta no GitHub). Adicionou a extensão C# Dev Kit. Disse ao git quem você é. Escolheu uma pasta limpa para o trabalho do ano. Copiou o curso para o seu computador. Criou o seu próprio repositório `kingdom` no GitHub e o clonou no VS Code. E entrou no espaço Slack `kingdom-hq` no laptop e no celular. Nenhum desses passos foi *programar*. Eles montaram a sua oficina. Todo desenvolvedor que trabalha hoje começou fazendo o mesmo tipo de coisa, e você passou por tudo em uma tarde com Lars do seu lado.

**Conceitos que você já sabe nomear:**

- *repo* — uma pasta de código que o git rastreia, com uma cópia no GitHub
- *clone* — copiar um repositório remoto para o seu computador
- *PATH* — a lista de pastas que o Windows procura quando você digita um comando no terminal
- *painel Source Control* — os botões no VS Code que executam o git para você (você vai usá-los pela primeira vez em 0.0.8)
- *conta no GitHub* — o seu lar na internet para código
- os quatro canais do Slack — `#all-kingdom-hq`, `#wins`, `#help`, `#milestones`

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que você consegue encontrar o caminho de volta ao seu trabalho amanhã. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

De memória, volte ao seu trabalho de duas maneiras:

1. Feche o VS Code completamente, abra de novo e volte para a sua pasta `kingdom`.
2. Agora faça o mesmo pelo Windows Terminal: entre na pasta e depois abra-a no VS Code a partir daí.

Os dois jeitos devem te levar na mesma pasta `C:\code\kingdom`.

<details><summary>Travou? Abra aqui para conferir.</summary>

- No VS Code: *File → Open Recent → kingdom*. O VS Code abre na pasta `kingdom`.
- No Windows Terminal:
  ```powershell
  cd C:\code\kingdom
  code .
  ```
  O `.` significa "esta pasta", então o VS Code abre no `kingdom`.
- De qualquer forma, a barra de título do VS Code deve mostrar `kingdom`, e você deve ver seus arquivos na barra lateral esquerda.

</details>

## Fechamento

Sem quiz hoje — a prova do trabalho está na tela. O terminal mostra três números de versão; `C:\code\` tem duas pastas dentro (`kingdom-course` e `kingdom`); o VS Code está aberto na pasta kingdom vazia; `kingdom-hq` está aberto no app Slack do desktop e no celular.

## Próximo

Duas lições curtas antes de você escrever qualquer código. **Módulo 0.0.5 — Primer** é um capítulo para ler sozinho sobre o que realmente está no seu computador: pastas, arquivos, o que um terminal realmente faz, o que significa *rodar* um programa e como ler um caminho. Depois, **Módulo 0.0.8 — Roast-O-Matic** é a sua primeira sessão sozinho: primeiro programa, primeiro commit, primeiro push, primeiro post em `#wins`.

Nos vemos em 0.0.5.
