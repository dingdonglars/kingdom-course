# Kingdom — Um Curso de Programação

```
   _  _____ _   _  ____ ____   ___  __  __
  | |/ /_ _| \ | |/ ___|  _ \ / _ \|  \/  |
  | ' / | ||  \| | |  _| | | | | | | |\/| |
  | . \ | || |\  | |_| | |_| | |_| | |  | |
  |_|\_\___|_| \_|\____|____/ \___/|_|  |_|
```

> 🇬🇧 [Read in English](./README.md)

Daqui a um ano, você vai mandar um link de Roblox para os seus amigos e eles vão jogar o seu jogo. Vão fazer login, construir fazendas numa grade, ver os recursos subirem, competir num placar de líderes. O mundo do jogo onde eles estão jogando — as regras de como funciona, o jeito como cresce, a parte que faz ele ser *divertido* — foi escrito por você, em código, do zero.

Antes dessa versão Roblox, você terá construído o mesmo jogo de quatro outras formas: como uma coisa que roda no seu terminal, como uma coisa que se salva num banco de dados para sobreviver entre sessões, como um site que seus amigos podem visitar numa URL pública real com login do Google, e como um jogo jogável em qualquer navegador. **Cinco versões funcionando da mesma ideia.** Cada uma te ensina um pedaço de como software de verdade é feito. Quando você chegar na versão Roblox, a engenharia já não vai ser a parte difícil — vai ser só tradução.

Vale dizer uma coisa. As habilidades não são brinquedo — são o que desenvolvedores de verdade usam para construir o software que você usa todo dia. No fim do ano, o que você entregou pode ser suficiente para conseguir uma vaga de desenvolvedor júnior: empresas pequenas regularmente contratam por portfólio, e o seu vai ser um portfólio real. Ou se você for para a faculdade ou um curso técnico, você chega já sabendo como isso funciona — o que torna a escolha do caminho mais fácil, e o sucesso depois mais provável. De qualquer jeito, você vai estar escolhendo de uma posição de força, não no chute.

É assim que o ano se parece.

---

## Para quem é isso

Alguém com uns quinze anos, sem experiência prévia com código, à vontade no computador, que joga, com quatro a seis horas por semana. Especificamente: você. O curso não pressupõe nenhuma base de programação. Ele *pressupõe* que você vai aparecer, escrever as coisas, e entregar o trabalho mesmo quando ficar desconfortável. Vai ficar desconfortável. É parte do acordo.

---

## O que isso precisa de você

Doze meses é muito tempo. O curso funciona porque os dois lados aparecem.

> **Doze meses é o tempo que você tem, não o tanto de trabalho que tem pela frente.** Foi feito assim de propósito — pra deixar espaço pra escola, família, amigos, o resto da sua vida. Quatro a seis horas por semana é pouco; não é muito trabalho. Se você estiver curtindo e quiser acelerar, dá. Os mesmos módulos com oito ou dez horas por semana fecham em mais ou menos metade do tempo. Pega o ano todo se precisar; termina antes se preferir.

**O que você se compromete a fazer:** aparecer. Investir as horas. Ler o que está na tela antes de me chamar. Anotar as coisas mesmo quando não der vontade. Não desistir na primeira vez que doer. Nada disso precisa ser heroico — quatro a seis horas por semana, toda semana — mas precisa ser *de verdade*. As partes desconfortáveis são as partes onde o aprendizado acontece.

**O que eu me comprometo a fazer:** quando você está nessa, eu também estou. O encontro semanal. Revisões de PR dentro dos prazos. Respostas no `#help` que você pode contar com elas. Mas isso depende de você estar nessa. Se você parar de aparecer, eu vou parar também — quietamente, não como punição, só porque não funciona do outro jeito. Previsibilidade acima de disponibilidade, dos dois lados.

O acordo numa frase: *os dois dentro, ou nenhum.*

### Se você quiser parar

No fim de qualquer fase, você pode decidir que terminou. Sem rancor, sem fracasso — *"fui até a Fase 3 e foi o suficiente"* é uma resposta completa. O curso é construído de modo que cada fase termine com algo real que você guarda, não com algo que só vem depois de mais doze meses. Se você chegar num final de fase e o ano deixar de fazer sentido, fala. A gente fecha direito.

---

## O que você vai realmente conseguir fazer

As habilidades não são decorativas. Quando terminar, você vai genuinamente saber:

> **Aviso — as palavras abaixo provavelmente parecem nada com nada agora.** *commit, branch, SQL, Web API, deploy, prompt…* — é normal. Você não precisa saber o que nenhuma delas significa ainda. Cada uma aparece na sua própria aula com a explicação ali do lado. Você aprende elas *usando*, não decorando glossário. Só dá uma olhada na lista — é o destino, não uma prova.

- **Usar git do jeito que desenvolvedores usam** — commit, branch, push, ler um diff, sair de uma bagunça. Principalmente pelo painel Source Control do VS Code (a superfície do dia a dia), com o terminal nas suas mãos para as operações que o painel não tem botão. O mesmo git que sustenta todo projeto de software no planeta.
- **Escrever testes, e confiar neles.** Quando você muda algo e os testes continuam passando, você *sabe* que não quebrou nada. É uma relação diferente com o seu código do que *"acho que tá funcionando"*.
- **Usar um banco de dados de verdade** — escrever SQL na mão, depois usar uma biblioteca que conversa com ele por você. O mesmo padrão que toda aplicação web que você já usou depende.
- **Construir uma Web API** — seu código, acessível por URL, chamado por outros programas pela internet.
- **Fazer deploy na nuvem** — uma URL pública real que qualquer pessoa pode acessar, com auto-deploy a cada push.
- **Trabalhar com um assistente de IA do jeito certo** — fazer engenharia de contexto nos seus prompts, ler a saída com olho crítico, e explicar cada linha que você entrega.
- **Ler o código dos outros.** A habilidade mais subestimada. A maior parte da programação é leitura.

Tudo isso transfere. Você pode usar num emprego, num projeto pessoal, na escola, em qualquer lugar. A maioria é exatamente o que um desenvolvedor júnior faz no primeiro dia.

---

## O que você vai construir, em ordem

O curso é organizado em sete fases — cinco principais, duas opcionais — caminhando até o final em Roblox.

**Fase 0 — Spark Week + Fundamentos.** Seu primeiro mês. Quatro brinquedinhos (um gerador de zoeira, um jogo de adivinhação, uma aventura curtinha, um pequeno utilitário de linha de comando). O ponto é se acostumar com o ritmo: escrever, rodar, comitar, fazer push, comemorar no `#wins`. No fim do mês você está confortável com os movimentos no teclado.

**Fase 1 — Console Kingdom.** A primeira versão real do Kingdom — construções, cidadãos, recursos, dias que avançam. Tudo acontece no seu terminal. Totalmente testado. No fim você tem uma engine de verdade rodando, e aprendeu a regra que sustenta o resto do curso: a *engine* (as regras do reino) é separada do *shell* (a forma como você interage com ela).

**Fase 2 — Persistence.** Seu reino aprende a se lembrar entre execuções. Primeiro como um arquivo de texto, depois JSON, depois um banco SQL real com vários slots de save. No fim dessa fase um jogador pode fechar o programa e voltar amanhã para encontrar o reino exatamente onde deixou.

**Fase 3 — Web API.** O mesmo reino, agora numa URL real da internet. Os amigos fazem login com a conta Google deles. Seu código, hospedado numa plataforma de nuvem real, redeployando a cada push. No meio dessa fase você chega no momento do **AI Unlock** — o ponto onde as regras sobre código gerado por IA expandem. Você ganha esse direito.

**Fase 4 — Browser Kingdom.** Seus amigos abrem uma página, veem o seu reino, clicam, jogam. Você aprende HTML, CSS, JavaScript e TypeScript pelo caminho. A mesma engine que rodou no terminal na Fase 1 agora alimenta uma experiência no navegador.

**Fase 5 — Roblox Kingdom.** *(Final opcional.)* Você porta a engine de novo — agora para Luau, a linguagem do Roblox — e publica um lugar onde os amigos podem jogar *no próprio Roblox*. O motivo de ser opcional e não pulável: quando você termina a Fase 4, já conquistou tudo que o curso se propôs a ensinar. A Fase 5 é o orgulho.

**Fase 6 — Bônus.** Três bônus curtos se você quiser. Um troca o seu banco de dados por outro completamente diferente em três linhas, só pra você sentir como a separação engine/shell é limpa de verdade. Um aprofunda no trabalho com um assistente de IA — a habilidade nomeada de *engenharia de contexto*. Um aprofunda em git de verdade — o modelo por baixo dos comandos que você vem digitando o ano todo.

> **E quando o ano fechar** — depois que M6 entregar e seu jogo no Roblox estiver no ar — tem um **diploma** impresso com seu nome. O Lars assina; você emoldura se quiser. Não é uma medalhinha digital, não é um sticker. É uma página de verdade que nomeia a engenharia que você realmente fez: commits, milestone pull requests, code reviews, testes, deploy contínuo, a disciplina de explicar cada linha de código que você entrega. O diploma é o que você leva no fim.

---

## Como uma semana típica funciona

Na maioria das semanas você vai fazer um ou dois **módulos**. Um módulo é uma aula independente — ler, fazer, quiz, comitar. Tem uma pasta para cada um. Lá dentro você encontra um `lesson.md` (a aula), uma pasta `starter/` (o esqueleto de código de onde você parte), e um `quiz.md` (umas perguntas para fixar o que acabou de chegar).

A cada poucas semanas você atravessa um **marco** (M0 até M6). Um marco é o ponto *digno de orgulho* — o lugar onde você tem algo real para mostrar. Cada marco termina com uma entrada no diário de vitórias, um post no Slack, e uma sessão presencial de revisão com Lars.

O ritmo é o ritmo de desenvolvedores trabalhando. Pegar isso agora significa não ter que pegar depois sob pressão.

---

## Por onde começar

Abra `phase-0-spark/0.0-setup/lesson.md`. É o dia um — instalações e o seu primeiro `git clone`, presencial com o Lars. A partir daí, sozinho: um primer curto (`0.0.5`) sobre o que está de fato no seu computador, depois o seu primeiro programa em `0.0.8` (Roast-O-Matic). Cerca de uma semana depois, você vai ter mandado esse primeiro programa para o GitHub e postado seu primeiro `#wins`.

Outros documentos que vale saber que existem, mas você não precisa ler em ordem:

- **`MENTOR-PROTOCOL.md`** — como Lars e você trabalham juntos: quando me chamar, como pedir ajuda, o que esperar de volta. Leia antes do módulo 0.0.
- **`ai-tools.md`** — como usar o Claude (seu assistente de IA) durante o curso.
- **`vscode-tips.md`** — os dez hábitos que transformam o VS Code de "parede de menus" numa ferramenta que você pilota. Dá uma olhada depois do Módulo 0.0.
- **`STYLE.md`** — como cada aula é estruturada, pra você saber o que esperar.
- **`STANDARDS.md`** — as convenções de código, nomeação e PR usadas em todo o curso.
- **`ENGLISH-NOTES.md`** — por que o curso é em inglês, e o pequeno apoio quando uma palavra te trava.
- **`GLOSSARY.md`** — todo termo que o curso ensina, em ordem alfabética.

---

## Onde pedir ajuda

**Lars** é seu mentor. O protocolo completo *como trabalhamos juntos* mora no `MENTOR-PROTOCOL.md` na raiz desse repo — descreve quando me chamar, como pedir ajuda, e o que esperar de volta. Leia antes do módulo 0.0.

**Claude** é seu assistente de IA — mas ele chega no AI Unlock (M3.9, mais ou menos na metade do ano), não no Dia 1. Até lá, quando você empaca (uma mensagem de erro confusa, uma bagunça no git) você me chama no `#help` sob a regra de 20 minutos do `MENTOR-PROTOCOL.md`. Depois do AI Unlock, o Claude entra como colaborador de verdade — pode te ajudar a *escrever* código também, sob a regra de que *você tem que conseguir explicar cada linha que mantiver*. A história completa está no `ai-tools.md`; você volta nele depois que o Claude estiver instalado.

---

## Uma nota sobre o que NÃO está nesse repo

O **Kingdom de referência** — a versão funcional do projeto que Lars escreveu como exemplo finalizado — vive num repo separado. **Não abra até a sua fase estar pronta.** O ponto não é copiar a versão dele; é escrever a sua, depois comparar. Sua fase só conta como terminada quando você construiu sozinho e *aí* deu uma espiada.

Seu **próprio trabalho** vive no seu próprio repo, que você cria no primeiro dia a partir do `starter-template/`.

---

Bem-vindo ao Kingdom. Vamos construir alguma coisa.
