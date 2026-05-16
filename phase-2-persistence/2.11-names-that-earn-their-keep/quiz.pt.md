# Quiz — Módulo 2.11 (com tema de nomes)

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que `KingdomManager` é um nome fraco?

- **a.** Usa letras demais e deixa os leitores mais lentos
- **b.** *Manager* é uma palavra-ruído — ela te diz que a coisa existe, não o que ela faz
- **c.** Conflita com uma palavra-chave reservada do C#
- **d.** As convenções de nomes proíbem explicitamente sufixos com mais de cinco letras

## 2. O nome local de uma letra só, `b`, para um prédio dentro de um método de 5 linhas é...

- **a.** Sempre errado; nomes deveriam ter sempre pelo menos quatro letras
- **b.** Tudo bem — o escopo é minúsculo, o tipo é óbvio pelo código ao redor
- **c.** Exigido por alguns guias de estilo importantes do C#
- **d.** Má prática, porque nomes de uma letra só são pouco profissionais

## 3. Por que fazer a festa de renomeação em *uma sessão focada* em vez de "conforme eu for"?

- **a.** Digita-se mais rápido quando você agrupa as teclas
- **b.** PRs de pura renomeação são fáceis de revisar, e uma sentada só pega nomes relacionados que deveriam mudar juntos
- **c.** Para impressionar um colega que está revisando
- **d.** Nenhum motivo real; qualquer abordagem funciona igualmente bem

## 4. Qual é a ferramenta certa para renomear ao longo de uma base de código?

- **a.** Buscar-e-substituir na mão em cada arquivo
- **b.** A refatoração de Renomear do IDE (F2) — atualiza toda referência, comentário e teste de uma vez, com segurança
- **c.** Uma regex aplicada com `sed` ou PowerShell
- **d.** `git ls-files | xargs sed -i 's/old/new/g'`

## 5. A lição diz *"todo nome paga um custo e gera um benefício."* Qual é o custo?

- **a.** Ciclos de CPU gastos procurando o símbolo em tempo de execução
- **b.** O leitor tem que aprender ele; nomes fracos geram menos do que custam
- **c.** Tempo de compilação gasto resolvendo o símbolo
- **d.** Espaço em disco ocupado pela string mais longa

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
