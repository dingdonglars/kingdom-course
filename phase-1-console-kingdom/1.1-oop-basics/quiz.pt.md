# Quiz — Módulo 1.1

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual é a diferença entre uma *classe* e um *objeto*?

- **a.** São a mesma coisa, duas palavras para uma ideia só
- **b.** Uma classe é a planta; um objeto é uma coisa específica construída a partir dela com `new`
- **c.** Uma classe é maior que um objeto; objetos são pedaços menores dentro dela
- **d.** Uma classe pode ter métodos; um objeto só pode guardar valores

## 2. O que `public int Level { get; private set; } = 1;` significa?

- **a.** `Level` pode ser lido de fora; só pode ser definido de dentro da classe; vale 1 por padrão
- **b.** `Level` é privado; código de fora não consegue ler ele
- **c.** `Level` é somente leitura e fica sempre em 1
- **d.** Isto é um erro de sintaxe e não vai compilar

## 3. O que o construtor `public Building(string name)` faz?

- **a.** Define um método comum que por acaso tem o mesmo nome da classe
- **b.** Roda quando você escreve `new Building("...")` e prepara o novo objeto
- **c.** Retorna uma string chamada `name` para quem o chamou
- **d.** Declara uma propriedade chamada `name` em todo Building

## 4. Por que `Building.Name` é somente leitura (sem `set`)?

- **a.** Porque o guia de estilo da Microsoft proíbe strings que podem ser definidas
- **b.** Depois que um prédio é construído, o nome dele não deveria mudar — a classe bloqueia mudanças acidentais
- **c.** Porque propriedades de string não suportam setters em C#
- **d.** É uma mania do runtime do .NET; nada importante

## 5. Para que serve um enum?

- **a.** Contar quantos de alguma coisa você tem numa lista
- **b.** Um conjunto fixo de valores com nome, então o compilador recusa qualquer coisa fora do conjunto
- **c.** Guardar uma lista de strings que pode crescer em tempo de execução
- **d.** Substituir classes quando os dados são pequenos demais para uma classe

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
