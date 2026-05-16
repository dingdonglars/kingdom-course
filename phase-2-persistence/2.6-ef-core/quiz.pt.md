# Quiz — Módulo 2.6

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que ORM significa, e o que ele faz?

- **a.** Object-Relational Mapper — traduz entre objetos na memória e linhas num banco de dados relacional
- **b.** Online Resource Manager — busca recursos de um servidor remoto
- **c.** Object Reference Model — uma forma de compartilhar identidade de objeto entre processos
- **d.** Order-Routing Module — um recurso de sistemas de pagamento

## 2. Por que não mapeamos `Kingdom.Engine.Kingdom` direto para uma tabela de banco de dados?

- **a.** Funcionaria bem; só escolhemos não fazer
- **b.** A classe da engine tem interfaces, campos privados e um construtor `IRandom`/`IClock`; o EF precisa de classes simples, só de propriedades
- **c.** O EF Core não suporta classes de outros projetos da solução
- **d.** Desempenho — o EF roda mais rápido em classes de entidade do que em classes de engine

## 3. O que `ctx.Kingdoms.Add(entity); ctx.SaveChanges();` faz?

- **a.** Adiciona a entidade só ao estado em memória; nenhuma gravação no banco
- **b.** Prepara a entidade para INSERT, e depois descarrega todas as mudanças preparadas no banco de dados numa única transação
- **c.** Envia uma requisição de rede para um processo de servidor de banco separado
- **d.** Lê a entidade de volta do banco de dados e retorna ela

## 4. O que `Include(k => k.Buildings)` faz numa consulta?

- **a.** Filtra a consulta para só reinos com pelo menos um prédio
- **b.** Diz ao EF para também carregar a lista de Buildings relacionada; sem isso, a propriedade de navegação fica vazia
- **c.** Ordena os resultados por contagem de prédios, do menor para o maior
- **d.** Adiciona um novo prédio a todo reino retornado

## 5. A lição mapeia `modelo da engine ↔ entidade ↔ banco de dados`. Por que três camadas?

- **a.** Cada camada tem um trabalho só, e qualquer uma delas pode ser trocada sem reescrever as outras
- **b.** Tradição sem justificativa moderna clara
- **c.** O .NET exige três camadas em qualquer projeto de persistência
- **d.** Desempenho — a camada extra guarda em cache buscas caras

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
