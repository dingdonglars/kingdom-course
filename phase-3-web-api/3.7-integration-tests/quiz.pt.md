# Quiz — Módulo 3.7

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual é a diferença entre um teste de unidade e um teste de integração?

- **a.** São nomes diferentes para o mesmo tipo de teste
- **b.** Testes de unidade exercitam um componente isolado; testes de integração exercitam vários juntos — pegando bugs de costura (roteamento, serialização, ligação da autenticação)
- **c.** Testes de integração foram descontinuados desde o .NET 9
- **d.** Testes de unidade são mais lentos e testes de integração são mais rápidos

## 2. O que `WebApplicationFactory<Program>` faz?

- **a.** Compila o projeto numa DLL pronta para implantação
- **b.** Inicializa a sua API inteira no mesmo processo (sem porta real) e te dá um `HttpClient` para fazer requisições reais contra ela
- **c.** Simula toda dependência da aplicação
- **d.** Desliga a autenticação em todo endpoint

## 3. Por que o `Program.cs` termina com `public partial class Program { }`?

- **a.** É exigido por todo projeto ASP.NET Core
- **b.** Para que `WebApplicationFactory<Program>` possa referenciar o tipo `Program` — o Program de nível superior precisa estar visível para o projeto de testes
- **c.** É uma preferência de estilo sem efeito real
- **d.** Ele habilita a geração da documentação OpenAPI

## 4. A lição recomenda mais ou menos cinco a dez testes de integração. Por que tão poucos?

- **a.** Eles são mais lentos e mais frágeis que testes de unidade. Cubra as costuras críticas — autenticação-exigida, especificação OpenAPI, endpoints-chave — e conte com testes de unidade para a amplitude.
- **b.** Eles são difíceis de escrever e melhor evitar
- **c.** O framework limita quantos podem rodar num projeto
- **d.** A Microsoft exige uma contagem máxima por motivos de desempenho

## 5. Por que usar um banco de dados temporário por fixture?

- **a.** Para os testes não interferirem uns com os outros nem com o seu banco de desenvolvimento. Cada fixture ganha um banco novinho; ele some depois da execução.
- **b.** Puro desempenho — bancos temporários são mais rápidos que os baseados em disco
- **c.** O EF Core exige um caminho de banco único em toda execução de teste
- **d.** Para economizar banda em execuções de integração contínua

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
