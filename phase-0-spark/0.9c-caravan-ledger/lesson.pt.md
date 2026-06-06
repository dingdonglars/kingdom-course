# Módulo 0.9c — Caravan Ledger

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Este é o último checkpoint de construção do zero antes do Kingdom. É um pequeno degrau a mais do que o Quest Board, e há uma ideia genuinamente nova nele — marcada claramente abaixo, para que você saiba exatamente o que é novo e por quê. Todo o resto é o kit da Fase 0 que você já tem.

Mesmo acordo de antes: construa a partir de um arquivo vazio, sem lições abertas, sem copiar. Depois mostre ao Lars e converse sobre ele. Leve o seu tempo — este recompensa ir devagar.

## As regras

- **Lições fechadas.** Não role de volta pela Fase 0 enquanto constrói. (Travar e ler *uma* lição é permitido — veja abaixo.)
- **Sem copiar.** Digite este do zero — não do seu Quest Board nem de nada anterior.
- **Só você.** Cada linha sua.
- **Leve o seu tempo.** Rode com frequência.
- **Travar é informação, não fracasso.** Anote *onde* você travou.

## Configure o projeto — do jeito padrão

Mesmo hábito do Quest Board. Um programa, uma janela. (Versão completa: `running-your-project.md`.)

1. Crie o programa dentro da sua pasta `kingdom`:

   ```powershell
   cd C:\code\kingdom
   dotnet new console -o CaravanLedger
   ```

2. Abra **aquela** pasta como a janela: **File → Open Folder…** → `C:\code\kingdom\CaravanLedger`. A árvore de arquivos deve dizer **CaravanLedger**, não kingdom.
3. Rode com `dotnet run`. Depure com **F5**.

> **Um programa, uma janela.** Abra a pasta `kingdom` inteira por engano e *Run*/F5 não vai saber qual programa você quer dizer (o erro de *"mais de um projeto"*). O conserto é abrir a pasta do programa único — nunca o seu código. Verifique a barra de título primeiro.

## O que você está construindo — o Caravan Ledger

Uma ferramenta de linha de comando que rastreia caravanas de comércio na estrada. Cada caravana carrega um número de **caixas**, e cada caixa tem um **preço**. O valor de uma caravana é simplesmente *caixas × preço*.

Roda em um loop até o usuário digitar `quit`. Os comandos:

| Comando | O que faz |
|---|---|
| `add <name> <crates> <price>` | Registra uma caravana: quantas caixas ela carrega e o preço de uma caixa. |
| `sell <name>` | A caravana chegou ao mercado e vendeu — tire do registro. |
| `value <name>` | Mostra quanto uma caravana vale (caixas × preço). |
| `ledger` | Lista todas as caravanas: suas caixas, preço e valor. |
| `wealth` | Mostra o valor total de *todas* as caravanas na estrada. |
| `help` | Lista os comandos. |
| `quit` | Sai do programa. |

Uma execução de exemplo:

```text
Caravan Ledger. Type 'help' for commands.
> add silk 10 5
Caravan 'silk': 10 crates at 5 coins each.
> add spice 4 25
Caravan 'spice': 4 crates at 25 coins each.
> value silk
'silk' is worth 50 coins.
> wealth
Total wealth on the road: 150 coins.
> sell silk
Caravan 'silk' sold.
> wealth
Total wealth on the road: 100 coins.
> quit
Bye.
```

Faça o texto do jeito que quiser. A lista abaixo é o que importa.

## A ideia nova — dois dicionários que compartilham uma chave

Aqui está a parte nova, e é o motivo todo deste checkpoint ser um degrau a mais.

Cada caravana tem **dois** números para lembrar: quantas caixas, *e* o preço por caixa. Um único `Dictionary<string, int>` só guarda **um** número por nome. Então como você armazena dois?

O truque: manter **dois dicionários que compartilham a mesma chave** — o nome da caravana.

```csharp
var crates = new Dictionary<string, int>();   // crates["silk"] = 10
var price  = new Dictionary<string, int>();   // price["silk"]  = 5
```

Os dois usam o nome da caravana como chave. `crates["silk"]` te diz quantas caixas a caravana de seda tem; `price["silk"]` te diz o preço de uma delas. Juntos, eles descrevem uma caravana.

A única regra que você deve seguir: **mantê-los em sincronia.** Sempre que adicionar uma caravana, escreva nos *dois*. Sempre que remover uma, remova dos *dois*. Se eles saírem de sincronia — um nome em um mas não no outro — você vai ter um erro ao buscá-lo. (É exatamente o tipo de coisa que o seu `try / catch` vai capturar calmamente em vez de travar.)

Só isso. Nenhuma palavra-chave nova do C# — só um jeito organizado de usar dois de uma ferramenta que você já conhece.

## Os obrigatórios

Todos os seis, como antes, mais a ideia dos dois dicionários acima:

1. **Um loop** até o usuário digitar `quit`.
2. **Coleções** — os dois `Dictionary<string, int>` acima, compartilhando o nome da caravana como chave.
3. **Um method que você escreveu e que retorna um valor.** Aqui ele faz um pouco de *matemática*: um method `Value` que recebe o nome de uma caravana e retorna `caixas × preço`. O seu total `wealth` é um segundo method que retorna valor e soma o valor de cada caravana.
4. **Verificação de número com `int.TryParse`** — e agora você verifica **dois** números ao adicionar uma caravana (as caixas *e* o preço). Se algum não for número, diga e continue; não trave.
5. **Um `try / catch`** em volta do tratamento de comandos.
6. **Mensagens claras** com string interpolation (`$"'{name}' is worth {worth} coins."`).

## Construa

Uma peça de cada vez, rodando após cada:

1. O loop de comandos — leia uma linha, pare em `quit`. Rode.
2. Adicione `help`. Rode.
3. Adicione `add` — divida a linha, verifique **os dois** números com `TryParse`, escreva nos **dois** dicionários. Rode. Tente um número ruim em cada lugar de propósito.
4. Adicione `value` — escreva o method `Value` que retorna caixas × preço, e faça o comando imprimir. Rode.
5. Adicione `ledger` e `sell` (lembre: `sell` remove dos *dois* dicionários). Rode após cada um.
6. Escreva `wealth` por último — um method que percorre cada caravana e soma o valor de cada uma. Rode.
7. Envolva o tratamento de comandos em `try / catch`. Rode mais uma vez.

## Se você travar

Descubra a peça, leia *só aquela lição*, volte e anote que precisou dela.

| Travou em… | Volte para |
|---|---|
| Lendo entrada, imprimindo, `$"..."` | Módulo 0.1 — Tinker |
| O loop `while`, `if`, `break` | Módulo 0.2 — Number Guess |
| Um method que recebe entrada e retorna um valor | Módulo 0.6 — Methods |
| `Dictionary` — armazenar e buscar por nome | Módulo 0.7 — Collections |
| `int.TryParse`, `try / catch` | Módulo 0.8 — Errors and Debugging |

## Mostre ao Lars — o checkpoint

Quando o seu Caravan Ledger rodar e fizer os seis obrigatórios, marque um horário com Lars.

Você vai:

1. **Rodá-lo para ele.** Ele vai tentar algumas coisas, incluindo um número ruim em cada slot, para verificar que não trava.
2. **Explicar o seu código a ele**, com as suas palavras — especialmente como os dois dicionários ficam em sincronia e como o seu method `Value` calcula o valor de uma caravana.
3. **Talvez fazer uma pequena mudança na hora** enquanto ele assiste.

Se algo estiver instável, não é reprovação — é exatamente o que firmar antes do Kingdom precisar. Um pouco de prática naquela peça, depois mostrar de novo. Isso é uma vitória.

## Fechamento

Sem quiz — construir o Caravan Ledger *é* a verificação.

1. **Progresso** — uma linha em `journal/progress.md`: `Module 0.9c — Caravan Ledger — DATE — built the Caravan Ledger from scratch.`
2. **Commit e push** — verifique que o Source Control diz `kingdom` (não `kingdom-course`!), prepare o seu trabalho, mensagem do commit `Module 0.9c done`, Sync. (O commit pelo terminal em `running-your-project.md` é o jeito mais fácil de uma janela de programa único.)
3. **Poste em `#wins`** — uma linha sobre o seu Caravan Ledger, mais a URL do commit.

O "pronto" de verdade é passar o checkpoint com Lars.

## Próximo

Esse é o último checkpoint. O próximo é **Fase 1 — o Kingdom começa.** No Módulo 1.1 você conhece as suas primeiras **classes** e constrói a primeira versão do Kingdom: prédios, cidadãos, recursos. Tudo que você acabou de mostrar que consegue fazer, você vai usar desde a linha um.
