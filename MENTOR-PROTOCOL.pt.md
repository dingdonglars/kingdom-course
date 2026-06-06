# Como a gente trabalha — você e Lars

> Tente primeiro o `MENTOR-PROTOCOL.md` em inglês. Use este aqui só quando uma palavra te travar.

> Um acordo de duas vias sobre como a gente conduz o ano juntos. Este arquivo fica no repositório do curso porque os dois precisamos lê-lo, não só o Lars. Leia uma vez agora. Volte sempre que algo parecer errado.

## Por que isso existe

Doze meses é muito tempo. Se a gente não tiver um ritmo, os dois vão derivar — você vai se sentir travado e não vai saber quem chamar; o Lars vai se sentir por fora e não vai saber o que perguntar. Então a gente estabelece o ritmo desde o início. A maior parte é assíncrona (você escreve, Lars lê depois) com um encontro semanal. Esse também é o jeito como equipes de desenvolvimento adultas lidam com exatamente o mesmo problema, então o padrão em si é uma das coisas que você está aprendendo.

O resto deste arquivo é só os detalhes.

## Como pedir ajuda — a regra dos 20 minutos

Quando algo quebra ou não faz sentido, a regra é:

1. **Tente por conta própria por 20 minutos.** Leia o erro. Pesquise na aula de novo. Releia o trecho de código que está se comportando mal. Tente uma coisa óbvia.
2. **Se você ainda estiver travado, pergunte ao Claude.** Cole o erro, diga o que você tentou, faça a pergunta.
3. **Se o Claude também não conseguir ajudar, mande mensagem para o Lars no Slack `#help`.** Mostre o que você tentou — o erro, a sua tentativa, a coisa específica para a qual você quer ajuda.

Isso não é barreira de acesso. É a habilidade mais útil de todo esse curso. A versão de você que faz uma *boa* pergunta — erro colado, tentativa nomeada, pergunta específica no final — é a versão que vai ser contratável em um ano. A versão que digita "socorro meu código está quebrado" é a versão que ainda vai estar no Google às 23h de uma sexta.

Um exemplo trabalhado. Pergunta ruim:

> *"meu arquivo de save não está funcionando"*

Pergunta boa:

> *"Estou tentando carregar o arquivo de save do kingdom na inicialização. Recebi este erro:*
>
> ```
> JsonReaderException: Unexpected character encountered while parsing value
> ```
>
> *Eu verifiquei — o arquivo está vazio. Eu o apaguei e rodei de novo, mas agora recebo FileNotFoundException. Tentei envolver o carregamento em try/catch e retornar um `Kingdom()` novo, mas parece errado. Qual é o padrão padrão para 'carregar se existir, senão começar do zero'?"*

Mesmo problema. Facilidade de ajudar completamente diferente. A versão boa nomeia o erro, nomeia a tentativa, faz uma pergunta específica. Desenvolva esse músculo agora.

**Língua é grátis.** Se você não souber uma palavra em inglês, pergunte. Em qualquer lugar, a qualquer hora, sem precisar de introdução. Perguntar *"o que significa 'idiomatic'?"* não é estar travado — é normal. O curso está em inglês de propósito; buscar uma definição faz parte do combinado, não é uma falha.

## As seis formas de conversar

Existem seis tipos diferentes de conversa que a gente tem durante o ano. Saber em qual você está mantém os dois sãos.

**1. Wins.** Quando algo funciona — o primeiro kingdom que rodou por trinta dias sem travar, o primeiro endpoint que retornou JSON de verdade, a primeira vez que você consertou um bug sem ajuda — deixa uma linha no Slack `#wins`. Screenshot ou trecho de output, uma frase de contexto. O Lars não precisa responder. O ponto é o rastro. Em doze meses, o `#wins` é o próprio histórico — leia de volta e é a prova de que você cresceu.

**2. Perguntas assíncronas.** Quando você estiver travado e a regra dos 20 minutos disser para chamar, poste no Slack `#help`. O Lars responde em 24–48 horas, geralmente mais rápido. Não interrompa o trabalho dele mandando mensagem — `#help` é o canal. Ele vai ver. Se algo estiver pegando fogo e 48 horas for lento demais, veja o Nível 5 abaixo.

**3. Revisão de PR de milestone.** Todo milestone (M0 ao M6) termina com um Pull Request de verdade — o mesmo fluxo que equipes profissionais usam. Você abre o PR, linka em `#milestones`, Lars revisa em 48–72 horas. A maior parte da revisão acontece como comentários no PR. Uma parte acontece cara a cara (presencialmente ou por vídeo) — especialmente nos milestones maiores onde há mais para percorrer.

**4. O sync semanal.** Um encontro agendado por semana, presencialmente se estivermos juntos, por vídeo caso contrário. É onde as perguntas maiores caem — respostas do quiz das quais você não tinha certeza, escolhas de design que você quer discutir, coisas que precisam de um quadro branco. Você traz o que estiver na sua cabeça; Lars traz o que ele notou. O horário do calendário é fixo; se Lars precisar mudar, ele avisa antes, não depois.

**5. Resgate.** Para quando você está *realmente* travado — não "este teste não está passando", mas "acho que quebrei meu repositório e estou com medo de fazer qualquer git". Bata na porta do Lars (ou ligue para ele). No mesmo dia, melhor esforço. Isso deve ser raro por design — é um canal de emergência, não um padrão.

**6. Reagrupamento.** Algumas semanas o código em si não é o problema; o problema é o *momentum*. Você está cansado, a aula parece chata, o kingdom parece pesado. Café, caminhada, "vamos só conversar" — sem pauta. Mais fácil de acionar do que o resgate. Não precisa ser sobre o curso. Melhor esforço dentro de alguns dias.

## Os dois lados do combinado

Este protocolo funciona porque os dois sustentamos a nossa parte.

**Você se compromete a:**

- Usar a regra dos 20 minutos antes de chamar.
- Mostrar o que você tentou quando perguntar. Erro colado, tentativa nomeada, pergunta específica.
- Abrir um PR de verdade para revisões de milestone — não um casual *"terminei, pode checar?"*
- Aparecer nos syncs semanais preparado. Ter algo para mostrar, uma pergunta a levantar ou um assunto que você quer discutir.

**Lars se compromete a:**

- Manter o horário do sync semanal. Se precisar mudar, ele avisa com antecedência.
- Responder ao `#help` e às revisões de PR dentro das janelas acima. Mesmo só *"vi, vou responder no sábado"* conta — silêncio não conta.
- Não sumir. Se algo der errado do lado do Lars, ele diz: *"semana pesada, sync move para terça."* Previsibilidade vale mais que disponibilidade.

## Slack — `kingdom-hq`

A gente usa Slack, não WhatsApp, para o curso. O Slack nos deixa ter canais separados para tipos separados de conversa, então `#wins` não afoga o `#help`. O plano gratuito está ótimo — 90 dias de histórico, mais do que suficiente para duas pessoas.

Os quatro canais:

- **`#all-kingdom-hq`** — meta. Agendamento, planos, "vou ficar fora na próxima terça."
- **`#wins`** — Nível 1. Celebrar coisas. Sem necessidade de resposta.
- **`#help`** — Nível 2. Perguntas travado-e-destravado. Tópico do canal: *Mostre o que você tentou.*
- **`#milestones`** — Nível 3. Coloque o link do seu PR aqui, mais o app Slack do GitHub posta atualizações.

Se você não souber qual canal usar, use `#all-kingdom-hq`. O Lars move se precisar.

## Com o que todo milestone termina

Todo milestone (M0 ao M6) termina com o mesmo ritual de três etapas. Não pule — é pequeno, leva dez minutos e o *rastro* que constrói vale mais do que qualquer milestone individual.

**1. Uma entrada em `wins.md`.** No seu repositório, abra `journal/wins.md` e escreva um parágrafo com as suas próprias palavras. O que você construiu? O que te surpreendeu? O que você faria diferente? Livre, com data, curto. Em M6, o seu `wins.md` são doze meses de você-como-desenvolvedor na sua própria voz. O você do futuro vai reler.

**2. Um post no Slack `#wins`.** Coloque o link para o PR mergeado. Um screenshot ou trecho de output. Legenda de uma linha.

**3. Uma linha de antes/depois.** Escolha a coisa que você não conseguia fazer antes deste milestone e a coisa que consegue fazer agora, e coloque-as em uma frase. Guarde em `wins.md`. Exemplo: *"Seis semanas atrás eu nunca tinha aberto um terminal. Hoje eu publiquei um engine de kingdom determinístico com trinta e cinco testes."* Essa é a linha que mais importa. Escreva.

Os troféus ASCII nas aulas são divertidos. O rastro em `wins.md` é a camada *durável* por baixo.

## Com o que todo quiz termina

Toda aula termina com um quiz pequeno — cinco perguntas de múltipla escolha. São para *você*, não para o Lars. O trabalho deles é mostrar o que você meio que entendeu para você poder cutucar antes de virar uma ideia errada endurecida.

Depois de fazer o quiz, escreva suas respostas em `journal/quiz-notes.md`. Uma letra por pergunta, uma frase dizendo *por que* você escolheu aquela letra. Mesmo hábito do `wins.md`, só cadência menor.

Você não recebe um gabarito no repositório do curso (isso é deliberado — os gabaritos ficam separados para que o quiz seja real). No próximo sync semanal, o Lars tem o gabarito na frente dele e vocês percorrem as que você marcou. Se uma pergunta estiver te bloqueando genuinamente no meio da semana, mande para o `#help` como qualquer outra coisa — mas a maioria das perguntas do quiz pode esperar pelo sync.

## Nota final

Todo este documento é um plano, não um contrato. Se algo não estiver funcionando — a janela de 24 horas é longa demais, o `#help` está silencioso, o horário do sync está errado, os rituais parecem burocracia — diga. A gente muda. O ritmo importa; as regras específicas não. O ponto é que os dois estamos no mesmo ano, com as mesmas expectativas, e nenhum de nós está chutando o que o outro está fazendo.
