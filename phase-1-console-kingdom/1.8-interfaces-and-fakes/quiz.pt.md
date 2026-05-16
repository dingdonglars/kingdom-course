# Quiz — Módulo 1.8

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é uma interface?

- **a.** Uma camada gráfica usada para desenhar a interface de usuário do jogo
- **b.** Um contrato — formatos de método e propriedade, sem corpo. Muitas classes podem implementá-lo.
- **c.** O oposto de uma classe — interfaces guardam dados, classes guardam comportamento
- **d.** Um recurso só do Java que não existe no C# moderno

## 2. O que *injeção de dependência* significa nesta lição?

- **a.** Inserir código num programa em execução depois do tempo de compilação
- **b.** Passar os colaboradores (`IRandom`, `IClock`) pelo construtor em vez de criá-los com `new` lá dentro
- **c.** Usar um framework de injeção de dependência de terceiros, como o Microsoft.Extensions.DependencyInjection
- **d.** Carregar a quente um assembly de classe enquanto o programa roda

## 3. O que `A.CallTo(() => rng.NextDouble()).Returns(0.1)` faz?

- **a.** Chama `NextDouble()` uma vez numa instância real de `Random`
- **b.** Diz ao `rng` falso que, sempre que qualquer código chamar `rng.NextDouble()`, retorne `0.1`
- **c.** Lança uma exceção se `NextDouble()` for chamado mais de uma vez
- **d.** Registra a chamada sem mudar o que `rng` retorna

## 4. Por que `Kingdom` mantém um construtor `Kingdom(string name)` de só um argumento que encadeia para o novo?

- **a.** Compatibilidade com versões antigas — testes mais velhos usam `new Kingdom("Test")` e quebrariam sem ele
- **b.** Para economizar digitação no `Program.cs`
- **c.** Porque o C# exige que toda classe tenha um construtor sem argumentos
- **d.** Foi adicionado por acidente e poderia ser removido sem consequência

## 5. A lição diz *"toda dependência externa entra por uma interface."* O que isso te dá?

- **a.** Código mais rápido em tempo de execução, porque o despacho por interface é mais barato que chamadas diretas
- **b.** A mesma engine roda no console, na web, no navegador, no Roblox — cada shell liga as próprias implementações. Os testes trocam as implementações reais por falsas.
- **c.** Binários compilados menores, porque interfaces evitam duplicação
- **d.** Nada concreto — é uma preferência de estilo sem benefício real

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
