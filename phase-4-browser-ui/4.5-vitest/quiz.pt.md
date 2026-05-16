# Quiz — Módulo 4.5

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Vitest está para o JavaScript assim como ___ está para o C#.

- **a.** EF Core — a camada de acesso a dados usada no seu projeto de persistência
- **b.** xUnit — o executor de testes que você usa desde o Módulo 1.3
- **c.** ASP.NET Core — o framework web que você usa na API
- **d.** NuGet — o gerenciador de pacotes que traz bibliotecas de terceiros

## 2. O que o `happy-dom` te dá?

- **a.** Um navegador real rodando sem interface na sua máquina de desenvolvimento para os testes
- **b.** Um DOM falso e rápido (`document`, `window`) para testes que precisam manipular elementos
- **c.** Uma biblioteca de log usada internamente pelo Vitest para acompanhar verificações que falharam
- **d.** Um verificador de tipos que roda junto com o Vitest durante a suíte de testes

## 3. O teste de XSS verifica tanto `toContain('&lt;script&gt;')` QUANTO `not.toContain('<script>')`. Por que os dois?

- **a.** Garantia dupla — o primeiro prova que o escape aconteceu; o segundo prova que a tag literal não está em lugar nenhum da saída
- **b.** Exigido pela API de verificação do Vitest para qualquer string que contém marcação HTML
- **c.** Motivos de desempenho — verificações isoladas rodam mais devagar em suítes de teste modernas
- **d.** Tradição da era inicial do Jest nos padrões de teste de frontend

## 4. O que é o modo de observação (watch mode)?

- **a.** `npm test -- --watch` — roda de novo só os testes afetados quando um arquivo muda; ciclo de feedback curto enquanto você edita
- **b.** Uma execução manual da suíte de testes inteira depois de cada vez que você salva
- **c.** Um modo de integração contínua que roda os testes a cada push para o GitHub
- **d.** Uma configuração obrigatória no `package.json` para os testes acharem os arquivos deles

## 5. A lição diz para mirar "as partes que quebrariam silenciosamente se mudadas", e não 100% de cobertura. Por quê?

- **a.** Correr atrás de cobertura produz testes ruins — testar getters triviais, simular tudo; cobrir as partes que importam é o que dá retorno
- **b.** 100% de cobertura é tecnicamente impossível de atingir em qualquer base de código de frontend moderna
- **c.** Cobertura mais alta roda mensuravelmente mais devagar durante o desenvolvimento
- **d.** A ferramenta de cobertura do Vitest se recusa a contar os casos triviais de qualquer forma

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
