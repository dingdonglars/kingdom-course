# Quiz — Módulo 1.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual é a diferença entre uma biblioteca de classes e um projeto de aplicativo de console?

- **a.** Bibliotecas de classes são menores e servem só para utilitários
- **b.** Aplicativos de console têm um `Main` e produzem um `.exe`; bibliotecas de classes não têm `Main` e produzem uma `.dll` que outros projetos usam
- **c.** Bibliotecas de classes podem conter métodos mas não classes
- **d.** São a mesma coisa; só a extensão do arquivo muda

## 2. Qual depende de qual?

- **a.** A engine depende do console
- **b.** O console depende da engine
- **c.** Os dois dependem um do outro igualmente
- **d.** Nenhum — eles são construídos de forma independente e ligados em tempo de execução

## 3. Por que `Console.WriteLine` é permitido em `Kingdom.Console.Program.cs` mas não em `Kingdom.Engine.Kingdom.cs`?

- **a.** Não é permitido em nenhum dos dois; ambos deveriam usar um logger
- **b.** A engine deve funcionar a partir de qualquer runtime — `Console.WriteLine` amarraria ela ao console
- **c.** Não há um motivo real; é só preferência de estilo
- **d.** Desempenho — `Console.WriteLine` é lento demais para código de engine

## 4. O que `<ProjectReference Include="..\Kingdom.Engine\Kingdom.Engine.csproj" />` faz?

- **a.** Copia os arquivos-fonte da engine para o projeto de console na hora do build
- **b.** Diz ao projeto de console que ele depende do projeto da engine
- **c.** Adiciona a engine ao controle de versão junto com o console
- **d.** Renomeia o projeto da engine para combinar com o console

## 5. O projeto da engine não tem `OutputType`. Qual é o valor padrão?

- **a.** `Exe` — todo projeto produz um executável por padrão
- **b.** `Library` — ele produz uma `.dll` que outros projetos usam
- **c.** `WinExe` — um executável específico do Windows
- **d.** Nada — o build falha até `OutputType` ser definido

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
