# Quiz — Módulo 2.3

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é um *teste de ida e volta*?

- **a.** Um teste que percorre o mesmo caminho de código duas vezes
- **b.** Um teste que salva o modelo, carrega ele de volta, e verifica que o modelo carregado é igual ao original
- **c.** Um teste para aplicativos de reserva de viagens
- **d.** Um teste que chama um servidor remoto e verifica a resposta

## 2. Por que a lição diz que *"persistência é uma das pressões mais honestas sobre um modelo"*?

- **a.** Salvar no disco é lento o suficiente para expor bugs de tempo
- **b.** Para salvar e carregar exatamente, cada pedaço de estado que importa precisa ser alcançável; estado escondido aparece como um campo que falta
- **c.** JSON é rígido quanto a tipos de campo e rejeita modelos frouxos
- **d.** Persistência simplesmente exige mais testes do que outras funcionalidades

## 3. O que é um *método de fábrica*?

- **a.** Um conceito de injeção de dependência do ASP.NET para construir serviços
- **b.** Um método estático que retorna uma instância — usado no lugar de (ou junto com) um construtor
- **c.** Um construtor com um nome diferente, idêntico de resto
- **d.** Um método necessário para a herança funcionar em C#

## 4. Por que o segundo construtor de `Building` é `protected` em vez de `public`?

- **a.** Ele só faz sentido para subclasses (Farm/Lumberyard/Mine), não para quem chama de fora da engine
- **b.** Sem `protected`, o arquivo não compilaria
- **c.** O .NET exige, por convenção, que o segundo construtor seja protected
- **d.** Construtores protected rodam mais rápido que os public

## 5. A lição usa `[Theory] + [InlineData]` para rodar o mesmo teste com quatro contagens de dia diferentes. Como esse padrão se chama na forma mais geral dele?

- **a.** Herança múltipla, aplicada a casos de teste
- **b.** Teste baseado em propriedades — verificar que alguma *propriedade* vale para muitas entradas, não só uma
- **c.** Mocking, em que as entradas substituem dependências reais
- **d.** Teste de integração, em que vários componentes são exercitados juntos

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
