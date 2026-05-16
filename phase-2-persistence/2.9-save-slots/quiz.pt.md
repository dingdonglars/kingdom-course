# Quiz — Módulo 2.9

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. CRUD significa...

- **a.** Code, Refactor, Update, Deploy — o ciclo de desenvolvimento
- **b.** Create / Read / Update / Delete — as quatro operações em linhas
- **c.** Class, Record, Update, Database — um padrão de dados do C#
- **d.** Uma convenção de nomes específica do LINQ para consultas

## 2. Por que usar `Find(id)` em vez de `Single(k => k.Id == id)` para apagar-se-existir?

- **a.** São funcionalmente idênticos e qualquer um funciona
- **b.** `Find` retorna `null` se a linha não existir; `Single` lança erro — `null` é a resposta certa para "apague se existir"
- **c.** `Find` roda mais rápido em todo banco de dados que o EF suporta
- **d.** Pura preferência de estilo, sem diferença de comportamento

## 3. O que `.Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))` faz no EF?

- **a.** Filtra as linhas onde `KingdomSlotInfo` corresponde
- **b.** Projeta cada linha num `KingdomSlotInfo`; o EF gera SQL que puxa só essas três colunas
- **c.** Renomeia a tabela para `KingdomSlotInfo`
- **d.** Ordena as linhas por `Id`, `Name`, `Day` nessa ordem

## 4. Por que `Update` substitui a lista `Buildings` inteira (limpar + adicionar) em vez de comparar as diferenças?

- **a.** Simplicidade — para uma lista pequena isso é correto e barato; listas grandes comparariam as diferenças para minimizar atualizações
- **b.** O EF Core não suporta adicionar a uma coleção existente
- **c.** `AddRange` é o único método do EF que funciona com propriedades de navegação
- **d.** Desempenho — limpar-e-readicionar é mais rápido que comparar diferenças em todo caso

## 5. Por que *"listar, depois carregar"* é a regra?

- **a.** Desempenho — listar primeiro aquece o cache para o carregamento
- **b.** Experiência de uso — mostre ao jogador todos os slots antes de perguntar qual carregar; qualquer outra coisa pede que ele decore IDs
- **c.** É exigido pelo framework EF Core
- **d.** Tradição sem motivo claro por trás

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
