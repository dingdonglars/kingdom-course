# Módulo 0.0.8 — Roast-O-Matic

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Ao final desta sessão, você tem um programa de verdade no seu computador que imprime zoações que você escreveu — e está na internet, no seu próprio repositório do GitHub. Cerca de duas horas. Esta é a sua primeira sessão sozinho. As ferramentas estão instaladas (Módulo 0.0), você leu o primer (Módulo 0.0.5) e agora você escreve o código.

Aqui está o plano de hoje. Você copia o kit do dia 1 para o seu repositório e envia para o GitHub pela primeira vez. Você cria um pequeno projeto em C# chamado Roast-O-Matic. Você o edita para imprimir zoações que você escreveu, e envia também. Depois posta a sua primeira conquista em `journal/wins.md` e no Slack `#wins`.

> **Words to watch**
>
> - **commit** — uma foto rotulada do seu código, com uma mensagem
> - **push** — enviar commits para o GitHub
> - **Source Control panel** — a interface git do VS Code (ícone: galho com um fork)
> - **`dotnet new`** — criar um novo projeto C#
> - **`dotnet run`** — compilar o seu código e rodar o resultado
> - **starter kit** — os arquivos base (convenções, regras de IA, um diário vazio) que todo repositório kingdom tem no primeiro dia

---

## Passo 1 — Coloque o kit do dia 1

O seu repositório `kingdom` está vazio. Hora de copiar os arquivos base: as convenções, as regras de IA (que esperam o Claude chegar mais tarde no ano), o diário vazio e todos os arquivos que as próximas lições vão apontar. A pasta `starter-template/` do repositório do curso os tem prontos.

No Windows Terminal:

```powershell
Copy-Item -Recurse C:\code\kingdom-course\starter-template\* C:\code\kingdom\
```

Isso traz `STANDARDS.md`, `CLAUDE.md` (as regras da IA — não fazem nada até o Claude chegar no Módulo 3.9), `.claude/commands/` (comandos de barra, também esperando o Módulo 3.9), `journal/` (onde ficam suas conquistas e notas), `.github/PULL_REQUEST_TEMPLATE.md`, `.editorconfig`, `.gitignore` e um `README.md` inicial que você vai personalizar mais tarde.

### Faça commit dos arquivos base — a sua primeira vez usando o painel Source Control

Mude para o VS Code (a pasta kingdom está aberta desde o Módulo 0.0 Parte 2). Você faz todos os passos do git pelo painel **Source Control**:

1. Clique no ícone de **Source Control** na barra lateral esquerda — terceiro ícone, parece um galho com um fork. (Atalho: `Ctrl + Shift + G` depois `G`.)
2. Você vai ver uma lista de arquivos novos em *Changes*. Passe o mouse em **Changes** e clique no ícone `+` para preparar todos eles.
3. Na caixa do topo, digite a mensagem do commit: *"day-1 kit"*.
4. Clique no **check azul** para fazer o commit. O commit está no seu computador.
5. Clique em **Sync Changes** na parte inferior do painel (ou no menu `...` → **Push**) para enviar para o GitHub. Na primeira vez, o VS Code abre uma janela do navegador para fazer login no GitHub. Clique em *Authorise Visual Studio Code*, depois volte. O VS Code lembra o login depois disso.

Atualize o seu repositório no github.com. Os arquivos base agora estão no GitHub.

> **Ou no terminal:**
>
> ```powershell
> cd C:\code\kingdom
> git add .
> git commit -m "day-1 kit"
> git push
> ```
>
> Os botões no VS Code fazem exatamente esses quatro passos para você. Durante o ano, você vai usar principalmente os botões. O terminal ainda é útil para as vezes que digitar é mais rápido do que clicar.

---

## Passo 2 — Construa o Roast-O-Matic

Dentro do seu repositório `kingdom`, você vai criar um pequeno projeto de console. Um projeto de console é um programa que roda no terminal e conversa com você por texto. Abra o Windows Terminal:

```powershell
cd C:\code\kingdom
dotnet new console -n RoastOMatic
cd RoastOMatic
dotnet run
```

Você deve ver:

```
Hello, World!
```

**Você acabou de rodar o seu primeiro programa.** Essa saída veio de código de verdade no seu computador.

### Faça ele imprimir zoações

Abra `Program.cs` no VS Code (ele já deve aparecer na lista de arquivos à esquerda). Você vai ver uma linha: `Console.WriteLine("Hello, World!");`. Substitua por isto (você pode copiar de `starter/Program.cs` no repositório do curso):

```csharp
string[] roasts = {
    "Your password is 'password' and we both know it.",
    "Your favorite Roblox game called. It wants its lag back.",
    "I'd insult your code, but you haven't written any yet.",
};

var random = new Random();
var roast = roasts[random.Next(roasts.Length)];
Console.WriteLine(roast);
```

Salve. De volta ao Windows Terminal:

```powershell
dotnet run
```

Uma zoação diferente cada vez. Rode três vezes. **Você está rodando código que escolhe algo ao acaso.**

---

## Passo 3 — Envie o brinquedo para o GitHub

Hora de mandar as zoações que você escreveu para o GitHub. Você já fez isso uma vez (o kit do dia 1 no Passo 1) — mesmo painel, mesmo cinco cliques.

No painel Source Control do VS Code:

1. Abra o painel **Source Control** (`Ctrl + Shift + G G` se não estiver visível).
2. Você vai ver os arquivos mudados em *Changes* — incluindo a nova pasta `RoastOMatic/` e seu conteúdo. Passe o mouse em **Changes** → clique em `+` para preparar tudo.
3. Mensagem do commit: *"first roasts of my own"*.
4. Clique no check azul para fazer o commit.
5. Clique em **Sync Changes** para enviar para o GitHub.

Agora vá ao seu repositório no github.com no navegador. **Atualize.** Seu código está lá. Está na internet.

> **Ou no terminal:**
>
> ```powershell
> cd C:\code\kingdom
> git add .
> git commit -m "first roasts of my own"
> git push
> ```

---

## Passo 4 — Poste a sua primeira conquista

O Slack está pronto desde o Módulo 0.0; hora de usá-lo. Suas conquistas vão em dois lugares, e os dois importam.

**Primeiro, `journal/wins.md` no seu repositório.** Abra o arquivo (ele veio com o kit do dia 1). Adicione uma entrada abaixo da data de hoje:

```markdown
## YYYY-MM-DD — Module 0.0.8 — Roast-O-Matic shipped

First commit, first push, first program of my own. The repo is live.
```

Salve. Faça commit. Faça push. (Você já sabe como — painel Source Control, mensagem *"Module 0.0.8 wins entry"*, Sync.) `wins.md` é o seu registro dentro do repositório. Você vai adicionar uma linha nele a cada marco pelo resto do ano.

**Depois, no Slack `#wins`:**

> 🎉 Roast-O-Matic shipped — *(link para o seu repositório do GitHub)*

É isso. Lars provavelmente não vai responder, e essa é a regra do `#wins` — sem resposta esperada. O post é o registro. Ao final do ano, rolar de volta pelo `#wins` mostra a prova de que você fez o trabalho, ao lado de `journal/wins.md` no seu repositório.

---

## Mexa um pouco

Agora escreva as suas próprias zoações. Sobre os seus amigos. Sobre o YouTuber mais irritante que você conseguir pensar. Sobre o seu gato. Substitua as três zoações padrão por cinco ou seis das suas.

Rode de novo. **Seu código, fazendo o que você mandou.**

Tente imprimir duas zoações ao mesmo tempo — chame `Console.WriteLine(roast)` duas vezes. Ou escolha duas diferentes do array. O que isso precisaria? (Dica: chame `random.Next(...)` de novo.)

---

## O que você acabou de fazer

Você copiou o kit do dia 1, fez o seu primeiro commit e o seu primeiro push usando o painel Source Control do VS Code, criou um projeto C# de verdade com `dotnet new console`, rodou ele, substituiu a única linha `Hello, World!` por código que escolhe uma zoação ao acaso, mudou as zoações para as suas, enviou tudo para o GitHub e postou a sua primeira conquista — tanto em `journal/wins.md` quanto em `#wins`. A maior parte do ano segue esse mesmo ritmo — escrever código, rodá-lo, fazer commit, push, compartilhar a conquista — só com ideias maiores.

**Conceitos que você já sabe nomear:**

- um *repo* — uma pasta de código que o git rastreia, com uma cópia no GitHub
- *commit* — salvar uma foto do seu trabalho, com uma mensagem
- *push* — enviar as suas fotos para o GitHub
- o **painel Source Control** no VS Code — os botões que fazem `add`, `commit`, `push` para você
- *`dotnet run`* — construir o código C# e rodar o resultado
- *`journal/wins.md`* — o seu registro de marcos dentro do repositório, postado em `#wins` ao mesmo tempo

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que os dois grandes movimentos pegaram. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

### 1. Crie e rode um novo projeto, de memória

No Windows Terminal, entre na sua pasta `kingdom`. Sem olhar:

1. Crie um projeto de console novo chamado `Hello`.
2. Entre nele.
3. Rode.

Você deve ver uma linha de texto sendo impressa.

<details><summary>Travou? Abra aqui para conferir.</summary>

```powershell
cd C:\code\kingdom
dotnet new console -n Hello
cd Hello
dotnet run
```

- `dotnet new console -n Hello` cria uma pasta `Hello` com um `Program.cs` e um `.csproj` dentro.
- `cd Hello` entra nela.
- `dotnet run` constrói o código e o roda. Você deve ver `Hello, World!`.

</details>

### 2. Faça commit e push, de memória

Você acabou de criar arquivos novos. De memória, sem rolar de volta:

1. Envie-os para o GitHub usando o painel Source Control — prepare, commit, Sync.
2. Atualize a página do seu repositório no github.com e verifique que a pasta `Hello` está lá.

<details><summary>Travou? Abra aqui para conferir.</summary>

- Abra o painel Source Control (`Ctrl + Shift + G` depois `G`). O cabeçalho deve dizer `kingdom`.
- Passe o mouse em *Changes* e clique em `+` para preparar todos os arquivos novos.
- Digite uma mensagem de commit na caixa, como *"add hello project"*.
- Clique no check azul para fazer o commit.
- Clique em *Sync Changes* para enviar para o GitHub.
- Atualize o seu repositório no github.com — a pasta `Hello` agora está lá.

</details>

## Quiz

Abra o `quiz.md`. Quando terminar, anote as suas respostas e uma frase de raciocínio em `journal/quiz-notes.md` — mesmo formato das entradas anteriores. Traga a que você tiver menos certeza para o próximo sync semanal.

## Próximo

Bem-vindo à programação. Nos vemos no Módulo 0.1 — Tinker, onde o Roast-O-Matic aprende a pedir um nome e zoar os seus amigos pelo nome.
