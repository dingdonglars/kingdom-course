# Módulo 0.9 — Foundations Check

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

A Fase 0 acabou. Você tem dois programas finalizados no GitHub, e as partes básicas do C# agora têm nomes. Antes do Kingdom começar, há um checkpoint: construa um pequeno programa novo a partir de um arquivo vazio, usando o que você aprendeu na Fase 0 — sem lições abertas e sem copiar código antigo. Depois mostre para Lars e converse sobre ele.

Este é um checkpoint. A Fase 1 — o Kingdom — começa quando você e Lars estiverem contentes de que você realmente sabe o básico.

Não é uma armadilha, e não é uma corrida. Leve o seu tempo com este pequeno programa — não há pressa. Se houver algo de que você não tem certeza, este é o lugar mais fácil e mais gentil para descobrir — agora, antes do Kingdom precisar disso.

## Por que paramos aqui

A Fase 1 é diferente da Fase 0. Os brinquedos que você fez até agora foram concluídos em um dia cada. O Kingdom é um projeto só que cresce por seis semanas. Cada nova lição usa o que a última ensinou. Classes usam methods. Testes usam classes. O game loop usa tudo isso.

Isso é ótimo quando você realmente sabe as partes anteriores. É difícil quando você as conhece só pela metade, porque uma coisa pequena que você não entendeu bem agora vira uma grande confusão algumas lições depois — e nessa altura é difícil até ver onde a confusão começou.

Então verificamos o básico primeiro. Um pequeno programa, uma conversa honesta. Esse é o checkpoint inteiro.

## As regras

Leia antes de começar. Elas são o que fazem a verificação ter significado.

- **Lições fechadas.** Não role de volta pela Fase 0 enquanto constrói. (Se travar, veja "Se você travar" abaixo — isso é permitido, e é o ponto.)
- **Sem copiar.** Não cole do Roast-O-Matic nem do Inventory Tool. Digite este do zero.
- **Só você.** Sem pedir a resposta para um amigo — cada linha deve ser sua. (Você ainda não está usando IA no curso; isso vem muito mais tarde, então por enquanto esta é fácil.)
- **Leve o seu tempo.** Não há pressa neste aqui. Vá devagar. Rode com frequência.
- **Travar é informação, não fracasso.** Quando você travar, anote *onde*. Essa anotação é útil — ela diz a você e ao Lars exatamente o que praticar.

## O que você está construindo — o Tavern Tab

Uma pequena ferramenta de linha de comando que rastreia o que cada aldeão deve na taverna.

Roda em um loop, lendo um comando de cada vez, até o usuário digitar `quit`. Estes são os comandos que ela deve entender:

| Comando | O que faz |
|---|---|
| `order <name> <coins>` | Adiciona aquelas moedas à conta do aldeão. Se o aldeão for novo, inicia a conta. |
| `paid <name>` | O aldeão pagou — zera a conta dele. |
| `tab <name>` | Mostra a conta de um aldeão. |
| `all` | Mostra todos os aldeões e o que devem. |
| `total` | Mostra o total devido por *todos*. |
| `help` | Lista os comandos. |
| `quit` | Sai do programa. |

Uma execução de exemplo pode parecer assim:

```text
Tavern Tab. Type 'help' for commands.
> order bob 3
Bob now owes 3 coins.
> order bob 2
Bob now owes 5 coins.
> order alice 4
Alice now owes 4 coins.
> total
The whole village owes 9 coins.
> paid bob
Bob has paid up.
> all
Alice owes 4 coins.
> quit
Bye.
```

Você não precisa usar exatamente esse texto. Faça o seu. O que importa é a lista abaixo.

## Os obrigatórios

Seu programa tem que mostrar todos os seis. É isso que Lars vai procurar.

1. **Um loop** que fica pedindo comandos até o usuário digitar `quit`.
2. **Uma coleção** que guarda o nome de cada aldeão e o que ele deve. (Um `Dictionary<string, int>` é perfeito para isso.)
3. **Um method que você escreveu e que retorna um valor** — não um `void`. O comando `total` é o lugar natural: um method que soma todas as contas e *devolve* o número.
4. **Verificação de número com `int.TryParse`.** Se alguém digitar `order bob abc`, o programa não deve travar. Deve dizer algo educado e continuar.
5. **Um `try / catch`** para que um erro inesperado não mate o programa inteiro.
6. **Mensagens claras** feitas com string interpolation (`$"Bob now owes {amount} coins."`).

## Construa

Comece pequeno e adicione uma peça de cada vez. Uma boa ordem:

1. Escreva o loop de comandos primeiro — leia uma linha e, se for `quit`, pare. Rode.
2. Adicione `help`. Rode.
3. Adicione `order` — divida a linha em partes, verifique o número com `TryParse`, armazene no dicionário. Rode. Teste um número ruim de propósito.
4. Adicione `tab`, `all` e `paid`. Rode após cada um.
5. Escreva o method `total` por último. Rode.
6. Envolva o tratamento de comandos em um `try / catch`. Rode mais uma vez.

Rode após *cada* pequeno passo. Esse é o hábito que faz isso parecer fácil em vez de assustador.

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

Quando o seu Tavern Tab rodar e fizer os seis obrigatórios, marque um horário com Lars. Este é o checkpoint de verdade, e é uma parte normal e amigável do trabalho — não um exame assustador.

Você vai:

1. **Rodá-lo para ele.** Ele vai tentar algumas coisas, incluindo um número ruim, para verificar que não trava.
2. **Explicar o seu código a ele**, com as suas palavras. Ele vai perguntar "por que esta linha está aqui?" sobre alguns pontos. Não há perguntas armadilha; ele só quer ouvir *você* explicar *o seu* código.
3. **Talvez fazer uma pequena mudança na hora** — ele pode pedir para você adicionar uma coisinha enquanto ele assiste. Essa é a melhor forma de mostrar que as ideias são realmente suas.

Quando você e Lars estiverem contentes, você passou o checkpoint e o Kingdom começa.

Se houver algo de que você não tem certeza, isso não é reprovação. Significa que você encontrou exatamente o que praticar antes que pudesse causar problemas mais tarde. Você vai passar um pouco de tempo naquela peça, depois mostrar de novo. Isso é uma vitória, não um revés.

## Fechamento

Não tem quiz desta vez — construir o Tavern Tab *é* a verificação.

1. **Progresso** — uma linha em `journal/progress.md`: `Module 0.9 — Foundations Check — DATE — built the Tavern Tab from scratch.`
2. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare o seu trabalho, mensagem do commit `Module 0.9 done`, Sync.
3. **Poste em `#wins`** — uma linha sobre o seu Tavern Tab, mais a URL do commit.

O "pronto" de verdade para este módulo é passar o checkpoint com Lars — não o push. O push só salva o seu trabalho.

## Próximo

Mais dois checkpoints curtos como este vêm primeiro — Módulo 0.9b (o **Quest Board**) e Módulo 0.9c (o **Caravan Ledger**). Cada um é mais um programa pequeno construído a partir de um arquivo vazio, para que as habilidades da Fase 0 sejam sólidas como rocha antes do Kingdom começar a depender delas.

Depois, a Fase 1 — **o Kingdom começa.** No Módulo 1.1 você conhece as suas primeiras **classes** e constrói a primeira versão do Kingdom: prédios, cidadãos, recursos. Você vai usar tudo que acabou de mostrar que consegue fazer.
