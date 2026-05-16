# Quiz — Módulo 3.8

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que o Azure App Service Free F1 te dá?

- **a.** Uma máquina virtual dedicada que você mesmo administra
- **b.** Hospedagem gratuita (1 GB de RAM, CPU compartilhada, dorme depois de 20 minutos sem uso, 60 minutos de computação por dia) — boa para a URL de um projeto de hobby
- **c.** Um banco de dados gerenciado com backups diários
- **d.** Um nome de domínio personalizado de graça, mais o certificado

## 2. O que CI/CD significa?

- **a.** C++ Improvement / C# Development
- **b.** Integração Contínua (build e teste automáticos a cada push) e Implantação Contínua (deploy automático a cada push para um branch)
- **c.** Compiler ID / Compiled
- **d.** Dois termos sem relação que só por acaso compartilham uma abreviação

## 3. Por que o workflow roda os testes *antes* de implantar?

- **a.** O Azure recusa implantações sem um relatório de testes anexado
- **b.** A CI é a guarda. Um teste que falha faz a implantação falhar. Você nunca empurra código quebrado para produção por acidente.
- **c.** Puro desempenho — rodar testes aquece o cache da CPU para o deploy
- **d.** O GitHub Actions exige um passo de teste em todo arquivo de workflow

## 4. Por que `Google__ClientSecret` (sublinhado duplo) é o nome da variável de ambiente, e não `Google:ClientSecret`?

- **a.** Eles são intercambiáveis em todo sistema operacional
- **b.** Variáveis de ambiente do Linux não permitem `:`. O leitor de configuração do ASP.NET Core interpreta `__` (sublinhado duplo) como `:` ao carregar de variáveis de ambiente.
- **c.** É exigido pelo .NET 10 especificamente
- **d.** Preferência da Microsoft sem motivo real

## 5. A lição diz *nunca implante na mão duas vezes*. O que isso quer dizer aqui?

- **a.** Implantações manuais são proibidas pelo Azure
- **b.** A primeira implantação manual está ok; a segunda significa que você deveria automatizar. A automação deixa as implantações chatas → frequentes → mudanças pequenas → iteração rápida.
- **c.** Implantações manuais são lentas mas, fora isso, ok
- **d.** O Azure restringe quantas implantações manuais você pode fazer por dia

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
