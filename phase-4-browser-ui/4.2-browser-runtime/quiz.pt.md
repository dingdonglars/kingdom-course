# Quiz — Módulo 4.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é o DOM?

- **a.** Uma biblioteca C# usada pelo ASP.NET para renderizar páginas no lado do servidor
- **b.** O Document Object Model — a árvore viva, em memória, da sua página; o JavaScript lê e altera ela
- **c.** Um tipo de banco de dados que guarda documentos HTML de forma eficiente
- **d.** Um formato de fonte que vem com a maioria dos navegadores modernos hoje

## 2. O que `await fetch(url)` faz?

- **a.** Dispara a requisição e retorna na hora um valor de string de espaço reservado
- **b.** Pausa a execução até a resposta HTTP chegar, e depois retorna um objeto `Response`
- **c.** Lança um erro se a conexão de rede não estiver pronta agora
- **d.** Retorna o corpo da requisição como um objeto JSON já interpretado, diretamente

## 3. Por que você precisa de `async` numa função que usa `await`?

- **a.** Desempenho — funções async rodam mais rápido que as síncronas em engines modernas
- **b.** `await` só pode aparecer dentro de funções `async`; a palavra-chave diz ao compilador para cuidar de suspender e retomar
- **c.** Exigido pelo Chrome, mas outros navegadores rodam sem a marcação
- **d.** Opcional no JavaScript moderno; navegadores antigos precisavam dela por compatibilidade

## 4. O que é CORS?

- **a.** Um padrão de bug comum em código JavaScript assíncrono que iniciantes pegam com frequência
- **b.** Cross-Origin Resource Sharing — segurança do navegador em que o servidor precisa declarar quais origens podem ler as respostas dele
- **c.** Uma biblioteca de log que vem com todo navegador moderno por padrão agora
- **d.** Um esquema de autenticação embutido no navegador, substituindo o OAuth para casos simples

## 5. A lição diz "abra o DevTools toda vez que você trabalhar numa página." Por quê?

- **a.** Tradição na comunidade de frontend desde o começo dos anos 2000
- **b.** Elements, Console e Network juntos te mostram o que de fato está acontecendo; fechar eles é voar às cegas
- **c.** Exigido por alguns navegadores antes de eles executarem JavaScript nos seus arquivos locais
- **d.** Revisores esperam ver o DevTools aberto em qualquer captura de tela que você enviar

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
