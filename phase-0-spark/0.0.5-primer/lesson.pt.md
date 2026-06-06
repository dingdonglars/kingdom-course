# Módulo 0.0.5 — Primer: o que realmente está no seu computador

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

As ferramentas estão instaladas e as duas pastas estão no seu computador. Antes de você escrever qualquer código, aqui vai uma caminhada de meia hora pelas partes do computador que o resto do curso espera que você entenda. Pastas e arquivos. O terminal. O que "rodar um comando" realmente significa. Como ler um caminho. Nada disso é programação ainda. É o que a programação fica em cima. Entenda isso direito e o próximo módulo — e todos os seguintes — fica muito mais fácil de acompanhar.

Para a maior parte disso, você vai ter o Explorador de Arquivos e o Windows Terminal abertos em duas janelas. Você faz tudo na mão; não tem nada para instalar.

> **Words to watch**
>
> - **filesystem** — a árvore de pastas e arquivos no seu computador
> - **drive** — um contêiner no topo do filesystem; `C:\` é o drive onde o Windows vive
> - **path** — o endereço completo de um arquivo ou pasta, como `C:\code\kingdom\Program.cs`
> - **extension** — a parte depois do ponto em um nome de arquivo (`.md`, `.cs`, `.json`); diz aos programas que tipo de conteúdo ele tem
> - **terminal** — uma janela onde você digita comandos e o computador responde digitando de volta
> - **shell** — o programa *dentro* do terminal que interpreta os seus comandos; PowerShell é o shell padrão do Windows
> - **process** — um programa que está *vivo* (rodando) no seu computador neste momento
> - **working directory** — a pasta em que o terminal está "sentado" atualmente; os comandos que você digita normalmente agem nessa pasta
> - **.NET SDK** — o kit que você instalou no Dia 1 para compilar e rodar programas em C#; `dotnet` é o comando principal
> - **compiler** — um programa que transforma o código-fonte (os seus arquivos `.cs`) em algo que o computador realmente consegue executar

---

## Passo 1 — Pastas e arquivos

Abra o **Explorador de Arquivos** (tecla Windows + E). Clique em *Este computador* na barra lateral esquerda. Você vê os drives — geralmente pelo menos `C:` (onde o Windows vive), e talvez `D:`, `E:`, e assim por diante. Dentro de cada drive há uma árvore de *pastas*. Dentro das pastas há mais pastas, ou *arquivos*. Isso é o filesystem inteiro.

Clique em `C:\` (está chamado de `Disco Local (C:)` na barra lateral). Olhe ao redor. Você vai ver pastas como `Windows`, `Program Files` e `Users`. Não mude essas pastas. O Windows vive em algumas delas e quebra se você deletar algo. Elas estão aqui só para que você saiba que existem.

Agora clique em `C:\code\` (a pasta que você criou no Dia 1). Dentro, você deve ver exatamente duas pastas: `kingdom-course/` (o curso que você clonou) e `kingdom/` (o seu próprio repositório, vazio por enquanto).

Abra `kingdom-course/`. Olhe ao redor. *Pastas dentro de pastas dentro de pastas.* É assim que está organizado. Todo arquivo vive no fundo de alguma árvore de pastas. Toda pasta tem um pai, exceto o próprio drive. Navegar significa ir *para cima* (de volta ao drive) ou *para baixo* (mais fundo em uma pasta).

Pense no filesystem como uma árvore de cabeça para baixo: o drive é a raiz no topo, os galhos são as pastas e as folhas são os arquivos. Ou, se preferir, pense nas pastas como lugares no Roblox. Um lugar pode ter outros lugares e itens dentro, e você entra em um para ver o que está lá dentro.

> **A pasta em que você está importa.** A maioria das coisas neste curso age na *pasta em que você está atualmente*. Rode um comando na pasta errada e você terá um erro confuso. Quando algo não funcionar, a primeira coisa a verificar é *"estou na pasta certa?"*

### Duas pastas, dois papéis

Seu `C:\code\` tem exatamente duas pastas agora, e elas fazem trabalhos diferentes.

- `kingdom-course/` — **o livro do curso**. Lições, kit inicial, glossário. Você lê; nunca edita. Quando o curso for atualizado, você puxa as mudanças com `git pull` dentro da pasta. Pense nela como um livro da biblioteca.
- `kingdom/` — **o seu repositório**. Tudo que você escreve vai aqui. Você faz commit, push e um dia uma revisão real de pull request acontece aqui. Essa é a pasta com o seu nome.

**A regra:** nunca edite nada dentro de `kingdom-course/`. Não as lições, não os arquivos iniciais, nem mesmo um erro de digitação que você encontrar. Se algo estiver errado, poste em `#help`. Lars corrige no original e você puxa o conserto com `git pull`. Se você editar a sua própria cópia, o próximo `git pull` falha com um conflito de merge. Um problema pequeno vira um grande muito rápido.

**Trabalhando com os dois no VS Code.** Abra cada um em sua própria janela. Use *File → Open Folder* → `C:\code\kingdom`. Então, em uma nova janela do VS Code, use *File → Open Folder* → `C:\code\kingdom-course`. Agora você tem dois ícones do VS Code na barra de tarefas, um por repositório, e a barra de título diz qual é qual. Não junte os dois em um só workspace. Mantê-los como duas janelas torna muito mais difícil digitar na errada por acidente.

## Passo 2 — Extensões de arquivo: como os programas sabem o que você tem

Todo arquivo tem um nome e, geralmente, uma *extensão* — a parte depois do ponto. A extensão diz aos programas que tipo de conteúdo o arquivo guarda. Não é mágica. É só um hábito que todo mundo concorda em seguir.

Em `C:\code\kingdom-course\`, você vai ver arquivos com extensões diferentes:

| Extensão | O que significa | Exemplo |
|---|---|---|
| `.md` | Markdown — texto simples formatado. As lições são escritas assim. | `README.md` |
| `.cs` | Código-fonte C#. Os seus programas vão viver nesses arquivos. | `Program.cs` |
| `.csproj` | Arquivo de projeto C# — diz ao compilador como montar o código. | `RoastOMatic.csproj` |
| `.json` | Dados estruturados, usados para configuração e jogos salvos mais tarde. | `appsettings.json` |
| `.gitignore` | Um arquivo que o git lê para saber quais arquivos ignorar. | `.gitignore` |

Duas coisas que vale saber:

1. **A extensão é parte do nome do arquivo**, mesmo que o Windows muitas vezes a esconda. Se você vê `Program` no Explorador de Arquivos, o nome real no disco é provavelmente `Program.cs`. Para fazer o Windows sempre mostrar as extensões: no Explorador de Arquivos, clique em *Exibir* → *Mostrar* → *Extensões de nome de arquivo* e marque. **Faça isso agora.** Extensões escondidas causam bugs confusos. Por exemplo, você salva `Program.txt` achando que salvou `Program.cs`.
2. **Renomear a extensão não muda o que está dentro.** Se você renomear `Program.cs` para `Program.txt`, ele não vira texto. Ainda é código C#; o nome agora diz algo errado sobre ele. Qualquer ferramenta que decide o que fazer baseada na extensão vai tratá-lo do jeito errado.

## Passo 3 — O terminal, e o que o PowerShell realmente é

Abra o **Windows Terminal**. Você vê um prompt — `PS C:\Users\SeuNome>` ou algo parecido — e depois um cursor piscando, esperando você digitar. O que você está olhando é *o terminal*: uma janela onde texto entra e texto sai.

Mas o que realmente *lê e roda* o que você digita? Isso é o **shell**. O shell é um programa que roda *dentro* do terminal. Ele lê o que você digita e transforma em ações. No Windows, o shell padrão é o **PowerShell**. No Mac e Linux costuma ser **bash** ou **zsh**. Shells diferentes, mesma ideia: você digita um comando, ele roda, você vê a saída.

Pense nisso como uma janela de chat com o seu computador. Você digita uma pergunta ou instrução, e o computador digita de volta. A diferença de um chat normal: o computador não tem personalidade, e ele precisa que você seja exato. Digite o comando certo e você recebe a resposta. Digite um errado e você recebe uma mensagem de erro — geralmente uma que te diz exatamente o que está faltando.

Tente isso. No Windows Terminal, digite:

```powershell
cd C:\code
```

Aperte Enter. O prompt muda — agora mostra que você está em `C:\code`. O comando `cd` significa *"change directory"*, e ele moveu o seu *working directory* para `C:\code`. *Working directory* significa simplesmente *"a pasta em que este terminal está atualmente."*

Agora digite:

```powershell
ls
```

Aperte Enter. Você vê uma lista do que está em `C:\code\` — as mesmas duas pastas que o Explorador de Arquivos te mostrou no Passo 1: `kingdom-course/` e `kingdom/`. `ls` é abreviação de *"list"*. O terminal pode ver exatamente o que o Explorador de Arquivos vê. Só mostra como texto em vez de ícones.

Digite `cd kingdom-course`, depois `ls` de novo. Você vê o que está dentro do repositório do curso — os mesmos arquivos que você viu no Explorador de Arquivos. Duas janelas olhando para a mesma coisa.

> **Por que dois jeitos de olhar para a mesma coisa?** O Explorador de Arquivos é ótimo para navegar e arrastar coisas. O terminal é ótimo para fazer o computador trabalhar para você: rodar ferramentas, construir código, fazer a mesma coisa para centenas de arquivos ao mesmo tempo. A maior parte da programação é feita no terminal, porque a maioria das ferramentas de programação é rodada digitando comandos.

## Passo 4 — O que "rodar um comando" realmente significa

Esta é a seção mais importante do primer.

Quando você digita `cd C:\code` e aperta Enter, o PowerShell roda esse comando e termina quase instantaneamente. O prompt volta, pronto para o próximo comando. *Aquele comando acabou.*

Mas alguns comandos demoram. Alguns nunca terminam sozinhos. Quando você digitar `dotnet run` mais tarde, esse comando vai:

1. Transformar o seu código C# em um programa. (Transformar código em programa se chama *compilar*.)
2. Iniciar esse programa.
3. Esperar o programa terminar. Isso pode levar alguns milissegundos, ou pode nunca terminar se o programa fizer um loop para sempre.

Enquanto `dotnet run` está *rodando*, o terminal parece ocupado. Está mostrando a saída do programa, mas não vai aceitar novos comandos. O prompt sumiu ou está congelado. **O programa está rodando, dentro do PowerShell, dentro da janela do terminal.**

```
Windows Terminal  (a janela que você pode ver)
  └── PowerShell  (o shell dentro da janela)
       └── o seu programa  (o processo rodando dentro do PowerShell)
```

Três camadas. A janela segura o shell; o shell segura o programa que está rodando. **Se você fechar a janela, você para o PowerShell, e isso para o programa com ele.** É assim que "rodar" funciona em um computador: as coisas que estão rodando vivem dentro de outras coisas que estão rodando, e a cadeia inteira tem que ficar aberta.

É por isso que, no Dia 1, a regra era *não feche o PowerShell enquanto um comando está rodando*. Fechar a janela no meio de um comando é como puxar o cabo de energia de um micro-ondas na metade: o que estava acontecendo simplesmente para, pela metade. Às vezes tudo bem. Às vezes deixa uma bagunça.

**O jeito certo de parar um programa que está rodando:**

- Se o programa termina sozinho, ótimo. Ele simplesmente acaba, e você recupera o prompt.
- Se você quiser pará-lo antes, aperte `Ctrl + C`. Isso envia um sinal de *"por favor, pare"*. O programa geralmente para de forma limpa e devolve o prompt.
- *Aí* você pode fechar a janela se quiser. A janela é só o contêiner. Fechá-la depois que o programa parou é tranquilo.

Uma palavra que vale saber: um *programa que está rodando* se chama um **process**. Seu computador tem centenas de processes rodando agora — o próprio Windows, o Chrome, o terminal, todo jogo e aplicativo que você tem aberto. Cada um é algo que o computador está mantendo vivo. Quando um process termina, ele desaparece.

> **Salvar e rodar não são a mesma coisa.** Salvar um arquivo `.cs` escreve o código no disco, e ele fica lá até você deletar. Rodar com `dotnet run` inicia um process que existe só enquanto o comando está rodando. *Salvar o código não o roda. Rodá-lo não salva nada novo no disco.* São duas ações completamente separadas. As pessoas as confundem no começo; você não vai confundir agora.

## Passo 5 — O .NET SDK: o seu kit C#

Vamos olhar para aquele comando `dotnet run` do último passo. O que é `dotnet`? De onde ele vem?

Quando você instalou o **.NET SDK** no Dia 1, o que você realmente adicionou ao seu computador foi um *kit*: um conjunto de programas, bibliotecas e regras que juntos transformam código C# em um programa que funciona. *SDK* significa *Software Development Kit*. Muitas plataformas têm um — o iOS SDK, o Android SDK, o Roblox Studio como um tipo de SDK. Todos significam mais ou menos a mesma coisa: *"aqui está tudo que você precisa para criar software para esta plataforma, em uma instalação."*

`dotnet` é o comando principal do SDK. Três formas que você vai usar muito:

- `dotnet --version` — mostra a versão do SDK instalado (você usou isso no Dia 1 para verificar a instalação).
- `dotnet new console -n MyApp` — cria um novo projeto de aplicativo de console C# chamado `MyApp`. Isso faz uma pasta, coloca um `Program.cs` e um `MyApp.csproj` dentro dela, e agora você tem algo que pode construir.
- `dotnet run` — *compila* o código C# no projeto atual e depois *roda* o resultado. Dois passos em um comando.

O que tem *dentro* do SDK que faz o `dotnet run` funcionar?

- O **compilador** — um programa que transforma os seus arquivos `.cs` em um `.dll` ou `.exe` que o seu computador pode rodar. C# não é lido linha por linha enquanto roda; ele é compilado para uma forma que o computador entende primeiro.
- O **runtime** — o motor que carrega e roda o resultado compilado. Mesmo depois de compilar, os programas C# precisam desse motor para funcionar. O SDK inclui um.
- **Bibliotecas embutidas** — código que vem pronto, para que seus programas possam usá-lo sem instalar nada extra. Quando você chamar `Console.WriteLine` no próximo módulo, esse method vive em uma biblioteca embutida que veio com o SDK.
- A ferramenta **NuGet** — quando você precisar de uma biblioteca que outra pessoa escreveu e que não é embutida, o `dotnet` a baixa de uma loja pública chamada NuGet e a adiciona ao seu projeto. Você não vai usar isso por um tempo; só conheça a palavra.

Então quando você roda `dotnet --version` e vê `10.0.x`, você está confirmando que **.NET 10's SDK está instalado e pronto para compilar e rodar código C#**. É isso que está dentro do kit.

## Passo 6 — Caminhos: endereços para arquivos

Um **path** é o endereço completo de um arquivo ou pasta. `C:\code\kingdom\Program.cs` é um path. Lido da esquerda para a direita, é um passeio do drive até o arquivo:

- `C:` — o drive (o drive principal do Windows)
- `\code\` — uma pasta sob a raiz do drive
- `\kingdom\` — uma pasta dentro de `code`
- `\Program.cs` — o arquivo dentro de `kingdom`

O `\` (barra invertida) é o que o Windows usa para separar pastas. No Mac e Linux é `/` (barra normal). Algumas ferramentas do Windows aceitam as duas. Use `\` e você estará sempre certo no Windows.

Dois tipos de path que você vai ver o ano todo:

**Paths absolutos** começam na raiz de um drive, como `C:\code\kingdom\Program.cs`. Funcionam em qualquer pasta em que o terminal esteja, porque cada parte do endereço está escrita.

**Paths relativos** começam no working directory. Se o seu terminal está em `C:\code\kingdom\`, então `Program.cs` é um path relativo que significa "o arquivo `Program.cs` nesta pasta." Da mesma forma, `RoastOMatic\Program.cs` significa "o `Program.cs` dentro da pasta `RoastOMatic` dentro desta pasta."

Dois atalhos que aparecem em todo lugar:

- `.` (ponto simples) significa *"esta pasta"* — o working directory atual. `code .` abre o VS Code nesta pasta. `git add .` prepara toda alteração nesta pasta.
- `..` (dois pontos) significa *"a pasta pai"* — um nível acima. `cd ..` move o terminal um nível acima. De `C:\code\kingdom\`, `cd ..` leva você para `C:\code\`.

Juntando tudo: de `C:\code\kingdom\`, o path `..\kingdom-course\STANDARDS.md` significa *"suba para `code`, então entre em `kingdom-course`, então vá para `STANDARDS.md`."* Ler paths fica rápido depois de ler alguns.

Tente isso no Windows Terminal:

```powershell
cd C:\code\kingdom-course
ls
cd ..
ls
cd kingdom
ls
```

Três saídas do `ls`, três pastas diferentes, todas alcançadas caminhando pela árvore de paths. Você já está navegando pelas pastas do jeito que um desenvolvedor faz.

## Passo 7 — Mantendo a ordem: por que `C:\code\`, e não `Documents`

Você colocou o seu trabalho do ano em `C:\code\`. Por que não em `Documents`?

Porque `Documents` é uma pasta especial no Windows. Dois motivos pelos quais ela causa problema:

1. **O OneDrive muitas vezes copia `Documents` para a nuvem sozinho.** O OneDrive é ótimo para arquivos do Word, mas é *terrível* para código. Ele bloqueia arquivos enquanto você está salvando, copia commits pela metade e às vezes renomeia coisas sem avisar. O OneDrive e o git atrapalham um ao outro, e o git é quem perde. Você não quer isso.
2. **Mistura tudo junto.** `Documents` acaba guardando PDFs da escola, capturas de tela, formulários baixados e um relatório que seu primo escreveu em 2019. Código se perde lá dentro. Código em `C:\code\` é *só* código, então você sempre sabe onde procurar.

Manter as coisas organizadas é um pequeno hábito, mas ajuda o ano todo:

- **Uma pasta principal para código.** Essa é `C:\code\`. Tudo relacionado a código vai dentro dela.
- **Uma pasta por projeto, diretamente dentro dessa pasta.** `C:\code\kingdom\`, `C:\code\kingdom-course\`, e outros mais tarde. Não coloque uma pasta de projeto dentro de outra.
- **Os nomes das pastas combinam com os nomes dos repositórios.** O seu repositório no GitHub é `kingdom`, e a pasta no disco é `kingdom`. Mesmo nome, sem surpresas.
- **Não mova pastas na mão depois que o git estiver envolvido.** Uma vez que uma pasta é um repositório git, trate o caminho dela como parte do que ela é. Se você realmente precisar mover, primeiro certifique-se de que `git status` está limpo, depois mova.

Essas não são regras que você vai ser testado. São hábitos que evitam certos tipos de confusão. Comece-os agora e você vai evitar toda uma série de momentos *"por que isso não está funcionando"*.

---

## Mexa um pouco

Abra o **Explorador de Arquivos** e o **Windows Terminal** lado a lado na tela.

No Explorador de Arquivos, vá para `C:\code\kingdom-course\`. No Windows Terminal, digite `cd C:\code\kingdom-course` e `ls`. Note que os mesmos arquivos aparecem nos dois. Uma pasta, duas visões.

No Explorador de Arquivos, dê um duplo clique em `phase-0-spark/`. No Windows Terminal, digite `cd phase-0-spark` e `ls`. Mesmo lugar de novo, mesmos arquivos.

Agora tente subir. No Explorador de Arquivos, clique na seta *voltar*, ou clique em `phase-0-spark` na barra de caminho no topo. No Windows Terminal, digite `cd ..`. Verifique que os dois voltaram para `kingdom-course`.

Agora isto: no Windows Terminal, digite `code .` (com o ponto, que significa *esta pasta*). O VS Code abre no `kingdom-course`. **É o mesmo `kingdom-course` que você estava olhando no Explorador de Arquivos e listando no PowerShell.** Três janelas na mesma pasta, três jeitos diferentes de olhar para ela.

Esse é o quadro todo. Pastas e arquivos ficam no disco. Um ou mais programas (Explorador de Arquivos, PowerShell, VS Code) os mostram de jeitos diferentes. Os comandos agem em qualquer pasta em que o programa estiver.

---

## O que você acabou de fazer

Você caminhou pelo filesystem. Viu `C:\` e a árvore de pastas dentro dele, e aprendeu que o Explorador de Arquivos e o PowerShell são duas visões do mesmo disco. Conheceu as extensões de arquivo e ativou o *"mostrar extensões"* no Explorador de Arquivos para que parem de ficar escondidas. Aprendeu que o terminal é uma janela, o shell (PowerShell no Windows) roda dentro dela, e os programas que você inicia rodam dentro do shell — três camadas, onde parar uma para tudo dentro dela. Leu paths absolutos e relativos, usou `cd` e `..` para se mover e adotou os hábitos que mantêm `C:\code\` uma pasta de trabalho organizada para o ano. Nada disso é "código" ainda, mas toda linha de código que você escrever vai viver neste filesystem, rodar neste shell e seguir essas regras de path.

**Conceitos que você já sabe nomear:**

- *filesystem* — a árvore de pastas e arquivos; drives no topo, arquivos nas folhas
- *path* — o endereço completo de um arquivo; absoluto começa no drive, relativo começa no working directory
- *terminal vs shell* — a janela vs o programa dentro dela que roda comandos; PowerShell é o shell padrão do Windows
- *process* — um programa que está rodando agora; fechar o terminal para os processes dentro dele
- *.NET SDK* — o kit que você roda com `dotnet`; compilador mais runtime mais bibliotecas mais NuGet, tudo instalado no Dia 1

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que você consegue se mover pelas pastas. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra o Windows Terminal. Sem olhar:

1. Entre em `C:\code` e liste o que tem lá.
2. Entre em `kingdom-course` e liste de novo.
3. Volte um nível para `C:\code`.

Observe como o prompt muda cada vez — ele sempre mostra a pasta em que você está atualmente.

<details><summary>Travou? Abra aqui para conferir.</summary>

```powershell
cd C:\code
ls
cd kingdom-course
ls
cd ..
```

- Depois de `cd C:\code`, o prompt mostra `C:\code`.
- O primeiro `ls` mostra duas pastas: `kingdom-course` e `kingdom`.
- O segundo `ls` mostra os arquivos dentro de `kingdom-course`.
- `cd ..` significa "suba um nível", então o prompt volta para `C:\code`. (`..` é a pasta pai; `.` é esta pasta.)

</details>

## Quiz

Este é o seu primeiro quiz, então aqui vai uma explicação rápida de como eles funcionam — só uma vez, aqui. As próximas lições terminam com o ponteiro padrão de uma linha.

**Quizzes são autoavaliações, não provas.** Cinco ou seis questões de múltipla escolha, sem limite de tempo, sem notas. O objetivo é você se verificar: a lição fez sentido ou não? Um quiz que vai mal não é um problema. Só mostra quais partes vale trazer para o próximo sync semanal.

**Onde você escreve as respostas.** Normalmente, as respostas vão em `journal/quiz-notes.md` dentro do seu repositório kingdom. Uma pequena coisa nesta primeira vez: esse arquivo ainda não está no seu repositório. Você vai copiá-lo no próximo módulo (`0.0.8`), como parte do kit do dia 1. Então para este primeiro quiz, escreva as respostas em qualquer lugar rápido — um aplicativo de notas, papel, o celular, não importa. Depois que você copiar o kit em `0.0.8`, mova-as para `journal/quiz-notes.md` de forma adequada. A partir do próximo quiz, você escreve direto nesse arquivo.

**O formato é curto.** Uma letra para cada questão, mais uma frase rápida sobre *por que* você a escolheu. Você pode ver como fica no topo do arquivo modelo — abra `C:\code\kingdom-course\starter-template\journal\quiz-notes.md` se quiser uma olhada. O *por que* importa mais do que a letra. *"Escolhi b porque..."* é a parte que você vai reler meses depois e realmente aprender algo. A letra sozinha não ajuda depois de uma semana.

**Não escreva as respostas no próprio `quiz.md`.** Mantenha esse arquivo limpo. Você pode querer voltar daqui a um mês ou dois e tentar o quiz de novo, e um quiz com as respostas já nele não é realmente um quiz.

**Se você travar em uma questão.** Tente por vinte minutos. Releia a parte da lição sobre a qual a questão trata. Se ainda não fechar, poste no Slack em `#help` (está aberto desde o Módulo 0.0). A regra dos 20 minutos no `MENTOR-PROTOCOL.md` é exatamente para isso. Há uma diferença entre tentar e ir devagar chegando lá, e só ficar olhando sem progredir.

**Depois, traga a questão da qual você estava menos certo para o próximo sync semanal.** Esse é o ritmo: ler a lição, fazer o quiz, escrever as respostas, conversar sobre as partes que você não tem certeza no sync. Todo módulo a partir daqui funciona assim.

**Tem uma cópia em português se o inglês te bloquear.** Todo quiz tem um `quiz.pt.md` ao lado do `quiz.md` — o mesmo quiz, em português. Veja como usar: sempre tente o `quiz.md` em inglês primeiro. Só abra o `quiz.pt.md` quando uma *palavra* em inglês te parar. Ele está lá para ajudar com a língua, não para pular o raciocínio.

Abra o `quiz.md` agora.

## Próximo

Você agora sabe o que está por baixo das lições. **Módulo 0.0.8 — Roast-O-Matic** é a sua primeira sessão sozinho. Você vai escrever um programa de verdade, fazer commit, push e postar a sua primeira conquista. Nos vemos lá.
