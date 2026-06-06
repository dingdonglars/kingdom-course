# Rodando o seu projeto — a única regra que te salva

> Tente primeiro o `running-your-project.md` em inglês. Use este aqui só quando uma palavra te travar.

> Leia uma vez. Volte aqui sempre que o *Run* ou o depurador não se comportar.
>
> **Configuração assumida:** Windows + VS Code, o seu repositório `kingdom` em `C:\code\kingdom`.

Existe um hábito que faz rodar e depurar "simplesmente funcionar", e pular esse hábito é a forma mais comum de um iniciante ficar travado meia hora em algo que nem é um bug. Aqui está:

## A regra: um programa, uma janela

**Quando você trabalhar em um programa, abra *a própria pasta daquele programa* como janela do VS Code — não a pasta `kingdom` inteira.**

Cada coisa que você constrói na Fase 0 vive na própria pasta dentro de `kingdom`:

```
C:\code\kingdom\
├─ RoastOMatic\        <- um programa (tem o próprio .csproj)
├─ InventoryTool\      <- um programa (tem o próprio .csproj)
├─ QuestBoard\         <- um programa (tem o próprio .csproj)
└─ journal\            <- não é um programa, só as suas anotações
```

Quando você quiser rodar o `QuestBoard`, a janela do VS Code precisa ser **`C:\code\kingdom\QuestBoard`** — só aquela pasta. Não `C:\code\kingdom`.

## Por que isso importa (o que pegou as pessoas)

O VS Code e a ferramenta `dotnet` descobrem *o que rodar* a partir da pasta que você tem aberta.

- Abra **a pasta de um programa**, e existe exatamente uma coisa para rodar. O *Run* e o depurador escolhem sem fazer perguntas.
- Abra **a pasta `kingdom` inteira**, e eles veem *vários* programas lado a lado e não sabem qual você quer dizer. Então `dotnet run` para com um erro como *"Specify which project to use because this folder contains more than one project,"* e apertar **F5** não faz nada útil.

Esse erro não é o seu código quebrado. É a ferramenta dizendo *"qual deles?"* O jeito de resolver nunca é mudar o código — é abrir a pasta certa.

## Crie um novo programa (você vai fazer isso no início de cada checkpoint)

No terminal, de dentro da sua pasta `kingdom`:

```powershell
cd C:\code\kingdom
dotnet new console -o QuestBoard
```

Isso cria uma nova pasta `QuestBoard` com um programa vazio dentro (troque `QuestBoard` pelo nome do que você está construindo). Depois abra ela como a própria janela — próximo passo.

## Abra como a janela

**File → Open Folder…** → escolha `C:\code\kingdom\QuestBoard` → Open.

A barra de título e a árvore de arquivos à esquerda devem mostrar **QuestBoard**, não kingdom. Esse é o sinal de que você fez certo.

> **O seu git ainda funciona.** Mesmo a janela mostrando só um programa, o VS Code olha *para cima* e encontra o repositório `kingdom`, então o painel Source Control ainda diz **`kingdom`** e o Sync ainda envia tudo. (Mais sobre o passo do journal lá embaixo.)

## Rode

No terminal (abra um com o atalho do terminal, ou *View → Terminal*):

```powershell
dotnet run
```

Sem `--project`, sem precisar do nome da pasta — há só um programa em vista, então o `dotnet run` simples já basta.

## Depure

1. Abra o `Program.cs`.
2. Clique na faixa estreita logo à esquerda de um número de linha. Um **ponto vermelho** aparece — esse é um breakpoint.
3. Aperte **F5**. O programa começa e pausa quando chega naquela linha.
4. Use o painel **Variables** à esquerda para ver o que está em cada variável, **F10** para rodar uma linha de cada vez e **F5** de novo para continuar.

Como a janela tem exatamente um programa, o F5 sabe o que iniciar. Sem configuração extra, sem menus.

## Quando o *Run* ou o F5 não se comportar — confira isso primeiro

| O que você vê | O que significa | O jeito de resolver |
|---|---|---|
| `dotnet run` diz *"more than one project"* | Você está na pasta `kingdom`, não na pasta de um programa | Abra a pasta do programa como a janela |
| **F5** não faz nada, ou pede para "select a project" | Mesma coisa — muitos programas em vista | Abra a pasta do programa como a janela |
| A árvore de arquivos à esquerda diz **kingdom**, não o seu programa | O repositório inteiro está aberto | **File → Open Folder…** → a pasta do programa |

Nove de dez vezes, "o depurador está quebrado" é na verdade "a pasta errada está aberta." Confira a barra de título antes de qualquer outra coisa.

## A única exceção — e como lidar

As anotações em `journal\` (`progress.md`, `quiz-notes.md`) ficam no **topo** de `kingdom`, não dentro da pasta do programa. Então quando o *Wrap up* de um checkpoint pedir para você editar `journal/progress.md` e fazer commit, você tem duas escolhas fáceis:

- **Faça commit pelo terminal.** O terminal dentro da janela do seu programa ainda vê o repositório inteiro, então isso funciona direto:
  ```powershell
  git add .
  git commit -m "Module 0.9b done"
  git push
  ```
- **Ou reabra a janela kingdom** para o wrap-up: **File → Open Folder…** → `C:\code\kingdom`, edite os arquivos do seu journal e faça commit e Sync pelo painel Source Control como de costume.

Qualquer um dos dois está certo. Escolha o que parecer menos complicado. Todo o resto na aula — construir, rodar, depurar — fica na janela de um programa só.
