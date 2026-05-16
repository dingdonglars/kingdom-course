# Quiz — Módulo 2.10

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O padrão de *laço de menu* é...

- **a.** `while (true) { imprime o menu; lê a entrada; despacha; }` — o coração de toda interface interativa
- **b.** Uma biblioteca C# chamada `Menu.Loop`
- **c.** Um tipo específico de consulta LINQ
- **d.** Nada padronizado; só uma expressão usada de modo informal

## 2. Por que usar `int.TryParse(raw, out var id)` em vez de `int.Parse(raw)`?

- **a.** São funcionalmente idênticos e qualquer um funciona
- **b.** `TryParse` retorna `bool` indicando sucesso; `Parse` lança `FormatException` em entrada ruim
- **c.** `TryParse` roda mais rápido em toda entrada
- **d.** `int.Parse` foi descontinuado no .NET 8

## 3. O que `Console.ReadLine()` retorna quando o fluxo de entrada se esgota (EOF)?

- **a.** Uma string vazia ("")
- **b.** `null`
- **c.** Lança `EndOfStreamException`
- **d.** A linha lida anteriormente (último valor em cache)

## 4. Por que o teste redireciona `Console.In` e `Console.Out`?

- **a.** Para esconder a saída de teste de quem está rodando a suíte
- **b.** Para roteirizar a entrada do usuário e capturar a saída, para uma interface interativa poder ser testada sem um humano no teclado
- **c.** Para rodar o teste umas três vezes mais rápido
- **d.** Exigido pelo xUnit para qualquer teste que toque o `Console`

## 5. A lição observa que o `Program.cs` agora tem cerca de cinco linhas, enquanto a engine + a store têm por volta de 500. Por que essa proporção é saudável?

- **a.** O runtime faz a interação; a engine faz a lógica; a store faz o salvamento — cada camada é fina e focada
- **b.** Arquivos `Program.cs` menores rodam mais rápido que maiores
- **c.** Tradição que todos os projetos de console .NET seguem
- **d.** O .NET se recusa a compilar um `Program.cs` maior que 500 linhas

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
