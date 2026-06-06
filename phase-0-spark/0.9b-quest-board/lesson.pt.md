# Módulo 0.9b — Quest Board

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você construiu o Tavern Tab a partir de um arquivo vazio. Aqui está mais um, do mesmo tipo de checkpoint: um pequeno programa novo, feito do zero, usando só o que a Fase 0 te ensinou. Depois você mostra para Lars e conversa sobre ele.

Por que um segundo? Porque as habilidades que te levam pelo Kingdom inteiro — um loop, uma coleção, um method que devolve algo, verificar números, capturar erros — ficam mais fortes cada vez que você as busca da sua própria cabeça em vez de copiar. O Tavern Tab provou que você consegue fazer. Este prova que não foi por acaso. Leve o seu tempo; não há pressa.

## As regras

As mesmas de antes. Elas são o que fazem a verificação ter significado.

- **Lições fechadas.** Não role de volta pela Fase 0 enquanto constrói. (Se travar, veja "Se você travar" abaixo — isso é permitido, e é o ponto.)
- **Sem copiar.** Não cole do seu Tavern Tab, Roast-O-Matic ou Inventory Tool. Digite este do zero.
- **Só você.** Sem pedir a resposta para um amigo — cada linha deve ser sua.
- **Leve o seu tempo.** Vá devagar. Rode com frequência.
- **Travar é informação, não fracasso.** Quando você travar, anote *onde*. Essa anotação diz a você e ao Lars exatamente o que praticar.

## Configure o projeto — do jeito padrão

Antes de escrever uma linha, abra o projeto do jeito certo. Este é o hábito que mantém o *Run* e o depurador tranquilos. A versão completa está em `running-your-project.md`; aqui está a forma curta.

1. Abra um terminal e crie o novo programa dentro da sua pasta `kingdom`:

   ```powershell
   cd C:\code\kingdom
   dotnet new console -o QuestBoard
   ```

2. Abra **aquela** pasta como a janela: **File → Open Folder…** → `C:\code\kingdom\QuestBoard`. A árvore de arquivos à esquerda deve dizer **QuestBoard**, não kingdom.
3. Rode com `dotnet run`. Depure com **F5**. Os dois funcionam sem configuração extra, porque há exatamente um programa na janela.

> **Um programa, uma janela.** Se você abrir a pasta `kingdom` inteira em vez disso, `dotnet run` e **F5** não vão saber qual programa você quer dizer — esse é o erro de *"mais de um projeto"*. O conserto nunca é o seu código; é abrir a pasta do programa único. Verifique a barra de título primeiro.

## O que você está construindo — o Quest Board

Uma pequena ferramenta de linha de comando que rastreia as missões em um quadro de avisos e as moedas que cada uma paga.

Roda em um loop, lendo um comando de cada vez, até o usuário digitar `quit`. Os comandos que ela deve entender:

| Comando | O que faz |
|---|---|
| `post <name> <reward>` | Coloca uma missão no quadro por aquelas moedas. (Se o nome da missão já estiver lá, a nova recompensa substitui a antiga.) |
| `done <name>` | A missão foi concluída — tire do quadro. |
| `reward <name>` | Mostra quanto uma missão paga. |
| `board` | Mostra todas as missões abertas e suas recompensas. |
| `bounty` | Mostra o total de moedas prometidas em *todas* as missões abertas. |
| `help` | Lista os comandos. |
| `quit` | Sai do programa. |

Uma execução de exemplo pode parecer assim:

```text
Quest Board. Type 'help' for commands.
> post slay-dragon 100
Quest 'slay-dragon' posted for 100 coins.
> post fetch-herbs 20
Quest 'fetch-herbs' posted for 20 coins.
> bounty
Total bounty on the board: 120 coins.
> done fetch-herbs
Quest 'fetch-herbs' is done.
> board
slay-dragon — 100 coins.
> quit
Bye.
```

Você não precisa usar esse texto. Faça o seu. O que importa é a lista abaixo.

## Os obrigatórios

Seu programa tem que mostrar todos os seis. É isso que Lars vai procurar — os mesmos seis que o Tavern Tab precisava, em um formato novo.

1. **Um loop** que fica pedindo comandos até o usuário digitar `quit`.
2. **Uma coleção** que guarda o nome de cada missão e sua recompensa. (Um `Dictionary<string, int>` é perfeito — o nome é a chave, a recompensa é o valor.)
3. **Um method que você escreveu e que retorna um valor** — não um `void`. O comando `bounty` é o lugar natural: um method que soma todas as recompensas e *devolve* o número.
4. **Verificação de número com `int.TryParse`.** Se alguém digitar `post slay-dragon abc`, o programa não deve travar. Deve dizer algo educado e continuar.
5. **Um `try / catch`** para que um erro inesperado não mate o programa inteiro.
6. **Mensagens claras** feitas com string interpolation (`$"Quest '{name}' posted for {reward} coins."`).

## Construa

Comece pequeno e adicione uma peça de cada vez. Uma boa ordem:

1. Escreva o loop de comandos primeiro — leia uma linha e, se for `quit`, pare. Rode.
2. Adicione `help`. Rode.
3. Adicione `post` — divida a linha em partes, verifique a recompensa com `TryParse`, armazene no dicionário. Rode. Teste um número ruim de propósito.
4. Adicione `reward`, `board` e `done`. Rode após cada um.
5. Escreva o method `bounty` por último. Rode.
6. Envolva o tratamento de comandos em um `try / catch`. Rode mais uma vez.

Rode após *cada* pequeno passo. Esse hábito é o que faz isso parecer fácil em vez de assustador.

## Se você travar

Isso é permitido — é a parte útil. Descubra em qual peça você travou, volte e leia *só aquela lição*, depois volte e continue. Anote que precisou dela.

| Travou em… | Volte para |
|---|---|
| Lendo entrada, imprimindo, `$"..."` | Módulo 0.1 — Tinker |
| O loop `while`, `if`, `break` | Módulo 0.2 — Number Guess |
| Escrever um method que retorna um valor | Módulo 0.6 — Methods |
| `Dictionary` — armazenar e buscar por nome | Módulo 0.7 — Collections |
| `int.TryParse`, `try / catch` | Módulo 0.8 — Errors and Debugging |

## Mostre ao Lars — o checkpoint

Quando o seu Quest Board rodar e fizer os seis obrigatórios, marque um horário com Lars. Esta é uma parte normal e amigável do trabalho — não um exame assustador.

Você vai:

1. **Rodá-lo para ele.** Ele vai tentar algumas coisas, incluindo um número ruim, para verificar que não trava.
2. **Explicar o seu código a ele**, com as suas palavras. Ele vai perguntar "por que esta linha está aqui?" sobre alguns pontos. Não há perguntas armadilha; ele só quer ouvir *você* explicar *o seu* código.
3. **Talvez fazer uma pequena mudança na hora** — ele pode pedir para você adicionar uma coisinha enquanto ele assiste. Essa é a melhor forma de mostrar que as ideias são realmente suas.

Se houver algo de que você não tem certeza, isso não é reprovação. Significa que você encontrou exatamente o que praticar antes que pudesse causar problemas mais tarde. Você vai passar um pouco de tempo naquela peça, depois mostrar de novo. Isso é uma vitória.

## Fechamento

Não tem quiz desta vez — construir o Quest Board *é* a verificação.

1. **Progresso** — uma linha em `journal/progress.md`: `Module 0.9b — Quest Board — DATE — built the Quest Board from scratch.`
2. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare o seu trabalho, mensagem do commit `Module 0.9b done`, Sync. (Se a sua janela for a pasta QuestBoard, o commit pelo terminal mostrado em `running-your-project.md` é o jeito mais fácil.)
3. **Poste em `#wins`** — uma linha sobre o seu Quest Board, mais a URL do commit.

O "pronto" de verdade é passar o checkpoint com Lars — o push só salva o seu trabalho.

## Próximo

Mais um checkpoint depois deste — Módulo 0.9c, o **Caravan Ledger** — e depois a Fase 1, o Kingdom, começa.
