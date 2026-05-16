# Quiz — Módulo 4.6

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que o Azure Static Web Apps é um bom host de frontend para este projeto?

- **a.** Plano gratuito, integração com o GitHub, CDN e SSL nativos, zero infraestrutura para gerenciar
- **b.** É o único host que suporta builds de TypeScript em ambientes de produção
- **c.** Exigido pelo Azure quando a API também está hospedada no App Service na mesma região
- **d.** Ele junta um banco de dados, autenticação e serviço de e-mail no plano gratuito por padrão

## 2. Por que você não pode usar `AllowAnyOrigin()` junto com `AllowCredentials()`?

- **a.** Isso deixaria qualquer site enviar requisições autenticadas para a sua API em nome do seu usuário; o navegador recusa a combinação
- **b.** Desempenho — a combinação causa lentidão mensurável em toda requisição de origem cruzada
- **c.** Exigido pelo Azure como uma restrição de configuração em implantações de Static Web Apps
- **d.** Eles funcionam bem juntos; a lição está enganada nesse ponto

## 3. Para que `import.meta.env.PROD` é avaliado em `npm run dev`?

- **a.** `true` — o dev é tratado como produção para fins de variáveis de ambiente
- **b.** `false` — o modo dev e o modo de produção são distintos
- **c.** `undefined` — a propriedade só é definida durante os builds, não durante o dev
- **d.** Lança um erro porque `import.meta` não é permitido no modo dev

## 4. O que é uma CDN?

- **a.** Um tipo de banco de dados que guarda em cache respostas de API em vários servidores
- **b.** Content Delivery Network — seus arquivos em cache em servidores de borda pelo mundo todo; os usuários pegam eles do mais próximo
- **c.** Uma biblioteca de log usada pelo Azure Static Web Apps para rastrear erros de frontend
- **d.** Exigida pelo HTTPS como parte da cadeia de validação de certificado em sites modernos

## 5. Por que dois serviços (frontend e backend) em vez de um?

- **a.** Cada um escala de forma independente, implanta de forma independente, pode ser substituído de forma independente — o layout padrão de produção
- **b.** Exigido pelo Azure para qualquer plano de hospedagem gratuito desde a atualização de 2024
- **c.** Execução mais rápida em tempo de execução quando os dois serviços rodam em máquinas separadas
- **d.** Tradição da era inicial de hospedagem dos anos 2000; apps modernos geralmente combinam os dois

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
