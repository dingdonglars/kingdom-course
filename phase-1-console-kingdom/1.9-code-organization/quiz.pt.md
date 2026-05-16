# Quiz — Módulo 1.9

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O compilador não liga para pastas. Então por que usá-las?

- **a.** O compilador liga, sim; pastas afetam a ordem de compilação
- **b.** Pastas são uma convenção que ajuda humanos a percorrer o código. Combinar pastas com namespaces mantém o layout consistente em dois lugares.
- **c.** Pastas aceleram o `dotnet build` ao paralelizar a compilação
- **d.** Principalmente para parecer organizado em capturas de tela e revisões

## 2. O que é um *global using*?

- **a.** Uma diretiva `using` num arquivo como `GlobalUsings.cs` que se aplica a todos os arquivos do projeto
- **b.** Um `using` só para `System`, aplicado como padrão pelo SDK
- **c.** Uma palavra-chave que não existe — é uma proposta da comunidade ainda não incluída no C#
- **d.** Uma diretiva `using` comum; "global" é só um rótulo no estilo de comentário

## 3. Por que a *raiz de agregação* (`Kingdom.cs`) fica no nível de cima em vez de numa subpasta?

- **a.** Convenção — a classe raiz mora na raiz, não enterrada numa subpasta com o nome dela mesma
- **b.** O C# exige que a raiz de agregação fique na raiz do projeto
- **c.** Compila mais rápido quando não está numa subpasta
- **d.** Foi colocada lá por acidente e ficou

## 4. Qual é o limite aproximado para dividir uma pasta plana em subpastas?

- **a.** Exatamente cinco arquivos; o guia de estilo do .NET insiste nisso
- **b.** Por volta de sete arquivos — uma regra prática flexível. Mais do que você consegue percorrer num relance.
- **c.** Cem arquivos; abaixo disso, plana está bom
- **d.** Nunca — plana é melhor, dividir sempre adiciona atrito

## 5. A lição alerta contra criar sub-namespaces antes da hora. Por quê?

- **a.** Sub-namespaces adicionam um custo em tempo de execução a cada chamada de método
- **b.** Eles adicionam linhas de `using` e o atrito de "de onde veio esse tipo". Valem a pena em projetos médios/grandes, mas são exagero em projetos minúsculos.
- **c.** O compilador emite avisos para projetos com sub-namespaces demais
- **d.** Criar sub-namespaces antes da hora é proibido pelo guia de estilo do .NET

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
