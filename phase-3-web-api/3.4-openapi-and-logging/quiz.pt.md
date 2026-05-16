# Quiz — Módulo 3.4

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é uma especificação OpenAPI?

- **a.** Uma biblioteca C# para interpretar JSON
- **b.** Um documento JSON padronizado que descreve cada endpoint de uma API — caminhos, parâmetros, formatos de resposta, códigos de status — legível por máquina
- **c.** Um tipo de autenticação ligado ao OpenID
- **d.** Um formato de log usado pelo ASP.NET

## 2. Por que preferir `log.LogInformation("Created {Id}", id)` em vez de `log.LogInformation($"Created {id}")`?

- **a.** Eles se comportam de forma idêntica em tudo
- **b.** O primeiro usa *log estruturado* — `Id` é capturado como um campo com nome pelos destinos que dão suporte a isso. O segundo perde a estrutura e envia só uma string já montada.
- **c.** O primeiro roda visivelmente mais rápido num caminho crítico
- **d.** É exigido pelo .NET 10 para qualquer chamada de logger

## 3. O que o Scalar (ou o Swagger UI) te dá?

- **a.** Uma biblioteca C# que simula requisições HTTP
- **b.** Uma página HTML interativa gerada a partir da especificação OpenAPI — qualquer um pode navegar e experimentar a API no navegador
- **c.** Um provedor de autenticação para o Login com Google
- **d.** Um executor de testes para endpoints do ASP.NET

## 4. A lição define o nível de log de `Microsoft.AspNetCore` como `Warning`. Qual é o motivo?

- **a.** Para esconder erros de quem opera o sistema
- **b.** O framework emite muita tagarelice de nível `Information` a cada requisição. Defini-lo como `Warning` mantém os seus próprios logs visíveis.
- **c.** É exigido pelo próprio framework
- **d.** Puro desempenho — o log `Information` é lento demais

## 5. Por que o OpenAPI é especialmente útil quando um assistente de IA lê o seu código?

- **a.** Assistentes de IA leem C# bem sem ele
- **b.** A especificação é uma descrição pequena e completa do *que a sua API faz* — uma IA consegue chamar os endpoints certos a partir dela sem ler cada handler. Economiza tokens e evita chutes.
- **c.** Assistentes de IA não leem especificações OpenAPI de jeito nenhum
- **d.** Ela impõe tipagem estrita no JSON que chega

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
