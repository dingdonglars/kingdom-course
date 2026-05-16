# Quiz — Módulo 1.5

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `: Building` em `class Farm : Building` significa?

- **a.** `Farm` *contém* um `Building` como um campo nela mesma
- **b.** `Farm` *herda de* `Building` — ganha os campos e métodos dele, pode sobrescrever os `virtual`, pode adicionar mais
- **c.** `Farm` é exatamente a mesma classe que `Building`, com um nome diferente
- **d.** `Farm` substitui `Building` em todo lugar; `Building` não existe mais

## 2. O que `: base(name)` faz em `public Farm(string name) : base(name) { }`?

- **a.** Chama o construtor de `Building` com o argumento `name` para a preparação da classe-pai rodar
- **b.** Renomeia a fazenda para o que `name` disser, toda vez
- **c.** Marca o construtor como parte de uma classe-base
- **d.** Nada que tenha sentido — é uma decoração opcional

## 3. Por que `override` é obrigatório (e não opcional) ao substituir um método `virtual`?

- **a.** Não é obrigatório; o compilador fica satisfeito sem ele
- **b.** Para você não substituir um método sem querer — digitar `override` é um sinal deliberado
- **c.** Para deixar o código mais longo e a digitação mais lenta
- **d.** É uma adição recente à linguagem e substitui o `new`

## 4. O que `b.GetType().Name` retorna para `b = new Farm("Main Farm")`?

- **a.** `"Main Farm"`
- **b.** `"Building"`
- **c.** `"Farm"`
- **d.** `"object"`

## 5. A lição diz *"prefira composição em vez de herança."* Por quê?

- **a.** Composição roda mais rápido em tempo de execução do que herança
- **b.** Herança é proibida no C# moderno e o compilador avisa quando ela é usada
- **c.** Cadeias longas de herança ficam rígidas; composição (uma classe *contém* outra) é mais flexível. Um nível de profundidade está ok.
- **d.** Composição usa menos memória do que herança

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
