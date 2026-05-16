# Quiz — Módulo 3.5

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. A primeira regra da autenticação é...

- **a.** Sempre fazer o hash das senhas com bcrypt antes de guardar elas
- **b.** Não invente a sua própria — use um provedor bem testado (Google, Microsoft, GitHub, Auth0)
- **c.** Sempre exigir autenticação de dois fatores em toda conta
- **d.** Sempre fazer as senhas terem pelo menos dezesseis caracteres

## 2. O que é um *claim* neste contexto?

- **a.** Um atributo C# usado numa classe
- **b.** Um pedaço de informação de identidade que o provedor (Google) afirma sobre o usuário — `email`, `name`, `sub` (o id estável do usuário)
- **c.** Um relatório de bug aberto contra uma biblioteca de autenticação
- **d.** Uma anotação OpenAPI marcando um campo como obrigatório

## 3. Por que o *Client Secret* nunca é guardado no repositório?

- **a.** Qualquer um lendo o repositório (ou qualquer um dos bots que varrem repositórios públicos atrás de segredos vazados) ganha a capacidade de se passar pelo seu app para o Google. Use user-secrets ou variáveis de ambiente no lugar.
- **b.** Ele é longo demais para caber num arquivo de config típico
- **c.** O Google exige que ele fique em algum lugar com uma extensão `.secret`
- **d.** Puro desempenho — segredos no repositório são mais lentos de carregar

## 4. O que `.RequireAuthorization()` num endpoint faz?

- **a.** Registra a requisição no log e continua normalmente
- **b.** Retorna `401 Unauthorized` para qualquer requisição sem um cookie de autenticação válido — antes de o handler rodar
- **c.** Loga o usuário automaticamente usando uma conta padrão
- **d.** Adiciona uma anotação OpenAPI de que o endpoint exige autenticação, mas não bloqueia requisições

## 5. Por que preferir `sub` em vez de `email` como o identificador estável do usuário?

- **a.** `email` pode mudar (o usuário atualiza o Gmail dele); `sub` é permanente e único para aquela conta Google
- **b.** `sub` é mais curto e economiza espaço de banco de dados
- **c.** `email` é proibido por regulamentos de privacidade
- **d.** Eles são exatamente equivalentes para fins de identificação

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
