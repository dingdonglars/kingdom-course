# Quiz — Módulo 3.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que usar um DTO de requisição/resposta em vez de retornar a `Kingdom` da engine diretamente?

- **a.** A serialização JSON nem sempre consegue lidar com classes com construtores que precisam de um `IRandom`; e o layout que vai pela rede deve ficar estável, pequeno e explícito
- **b.** É puramente uma otimização de desempenho para objetos grandes
- **c.** É um hábito antigo mantido por tradição
- **d.** O ASP.NET se recusa a serializar tipos que não são record

## 2. O que `Math.Clamp(days ?? 1, 1, 100)` realmente faz?

- **a.** Lança uma exceção se `days` for null
- **b.** Deixa `days` como 1 por padrão se for null, e depois força o valor para dentro da faixa de 1 a 100 — validação básica de entrada numa linha só
- **c.** Multiplica `days` por 1 e retorna o resultado
- **d.** Ordena o valor de entrada numa lista

## 3. Por que usar `Results.Ok(value)` em vez de só retornar `value`?

- **a.** São exatamente idênticos em tudo
- **b.** `Results.Ok(value)` deixa você controlar o código de status — útil quando um handler retorna códigos diferentes em condições diferentes (200 aqui, 404 em outro lugar)
- **c.** É exigido pelo .NET 10 para qualquer handler que retorna um record
- **d.** Ele comprime o JSON antes de mandar de volta para o cliente

## 4. O parâmetro de minimal API `(int? days)` lê de onde na requisição?

- **a.** Do corpo da requisição, como JSON
- **b.** Da query string — `?days=5` — porque um parâmetro primitivo e opcional se liga à URL
- **c.** De um cabeçalho HTTP personalizado chamado `X-Days`
- **d.** De um cookie definido por uma requisição anterior

## 5. A lição diz *validar na fronteira*. Qual é o raciocínio?

- **a.** A camada externa é onde a entrada não confiável entra. Rejeite a entrada ruim ali, para a engine nunca ter que se defender.
- **b.** A validação na fronteira roda mais rápido que a validação na engine
- **c.** A especificação HTTP obriga a validação na fronteira
- **d.** É uma preferência de estilo sem impacto real

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
