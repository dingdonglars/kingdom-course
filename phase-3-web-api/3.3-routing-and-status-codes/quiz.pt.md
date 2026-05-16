# Quiz — Módulo 3.3

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Qual é o código de status certo para um POST bem-sucedido que cria um novo recurso?

- **a.** 200 OK com o novo recurso no corpo
- **b.** 201 Created — e inclua um cabeçalho `Location:` apontando para a URL do novo recurso
- **c.** 204 No Content sem corpo
- **d.** 202 Accepted sem nenhuma informação extra

## 2. O que `MapGroup("/kingdoms")` faz?

- **a.** Renomeia o grupo de rotas existente na aplicação
- **b.** Define um prefixo de caminho compartilhado por todos os endpoints registrados nele — então você não repete `/kingdoms` em toda chamada `Map*`
- **c.** Cria um grupo de banco de dados para registros de reino
- **d.** Carrega os reinos existentes do banco de dados

## 3. O que o `:int` em `{id:int}` impõe na hora do roteamento?

- **a.** Nada — é puramente cosmético
- **b.** Uma *restrição de rota* — `/kingdoms/abc` não vai corresponder (não dá para interpretar como int); só rotas onde `{id}` é interpretável como um `int`
- **c.** Que o valor deve ser um inteiro de 32 bits em vez de 64 bits
- **d.** Que o parâmetro `id` é obrigatório em vez de opcional

## 4. Por que usar `try/catch (InvalidOperationException)` para traduzir um registro que falta num 404 é um *cheiro ruim*?

- **a.** Lançar exceções é muito mais lento que retornar false
- **b.** Exceção-como-fluxo-de-controle — exceções deveriam sinalizar situações *excepcionais*, não normais (como "não encontrado"). Um `TryLoad` que retorna `bool` é mais limpo.
- **c.** O C# não permite capturar `InvalidOperationException`
- **d.** Não é um cheiro ruim de jeito nenhum — esse é o padrão recomendado

## 5. Por que usar `Results.NoContent()` (204) para um DELETE bem-sucedido em vez de 200 OK com um corpo?

- **a.** Não há nada de significativo para retornar — o recurso já era. 204 diz "sucesso, sem corpo" — economiza bytes e combina com o que o cliente espera.
- **b.** O framework HTTP exige 204 para qualquer resposta de DELETE
- **c.** 200 OK não é uma resposta válida para o verbo DELETE
- **d.** É uma tradição sem motivo real por trás

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
