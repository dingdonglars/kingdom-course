# Módulo 0.9c — The Arena

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Este é o último checkpoint de construção a partir de um arquivo vazio antes do Kingdom — e o mais divertido dos três. Você construiu dois pequenos rastreadores (o Tavern Tab e o Quest Board). Este aqui é um joguinho: uma luta turno a turno numa arena. Monstros entram, você bate neles, eles batem de volta, e a rodada acaba quando ou a arena fica limpa ou você cai.

Ele usa exatamente as mesmas ferramentas da Fase 0 que você já tem — um loop, uma coleção, um method que devolve um número, verificação de número e `try / catch`. Nada novo na linguagem. O que é novo é o *tipo de programa* que você está fazendo. Os dois primeiros só guardavam e mostravam coisas. Este tem um objetivo que você pode ganhar ou perder, e números que mudam a cada turno. É por isso que é o teste de verdade: você não consegue vencer lembrando a ordem dos passos da última vez. Você tem que entender mesmo as peças.

Há também um truque genuinamente novo nele — marcado claramente abaixo: manter dois fatos sobre cada monstro juntos. Todo o resto, você já fez antes.

Mesmo acordo de sempre: construa a partir de um arquivo vazio, sem lições abertas, sem copiar. Depois mostre ao Lars e converse sobre ele. Vá devagar neste — ele recompensa isso.

## As regras

- **Lições fechadas.** Não role de volta pela Fase 0 enquanto constrói. (Travar e ler *uma* lição é permitido — veja abaixo.)
- **Sem copiar.** Digite este do zero — não do seu Tavern Tab, do seu Quest Board nem de nada anterior.
- **Só você.** Cada linha sua.
- **Leve o seu tempo.** Rode com frequência.
- **Travar é informação, não fracasso.** Anote *onde* você travou. Essa nota diz a você e ao Lars exatamente o que praticar.

## Configure o projeto — do jeito padrão

Mesmo hábito dos dois últimos. Um programa, uma janela. (Versão completa: `running-your-project.md`.)

1. Crie o programa dentro da sua pasta `kingdom`:

   ```powershell
   cd C:\code\kingdom
   dotnet new console -o Arena
   ```

2. Abra **aquela** pasta como a janela: **File → Open Folder…** → `C:\code\kingdom\Arena`. A árvore de arquivos à esquerda deve dizer **Arena**, não kingdom.
3. Rode com `dotnet run`. Depure com **F5**.

> **Um programa, uma janela.** Abra a pasta `kingdom` inteira por engano e *Run*/F5 não vai saber qual programa você quer dizer (o erro de *"mais de um projeto"*). O conserto é abrir a pasta do programa único — nunca o seu código. Verifique a barra de título primeiro.

## O que você está construindo — a Arena

Imagine um pequeno poço de luta. Você é o herói. Você começa com **30 HP** (HP quer dizer *health points*, pontos de vida — quanto dano você aguenta antes de cair). Os monstros entram na arena um de cada vez, conforme você os chama. Cada monstro tem dois números próprios:

- o **HP** dele — quanta vida ele tem, e
- o **attack** dele — quão forte ele bate de volta em você.

Um turno funciona assim. Você escolhe um monstro e diz quão forte você bate. A sua espada é forte: ela **dobra** a sua batida. Então uma batida de 4 causa 8 de dano. Esse dano sai do HP do monstro.

Depois, se o monstro ainda estiver vivo, ele bate de volta *em você* pelo número de attack dele, e isso sai dos seus 30 HP. Quando o HP de um monstro chega a zero, ele cai e sai da arena. Quando o *seu* HP chega a zero, você cai e a rodada acabou.

Você vence quando todos os monstros que você chamou caíram. Você perde se o seu HP acabar primeiro. Esse final de ganhar-ou-perder é a grande diferença em relação ao Tavern Tab e ao Quest Board — aqueles só rodavam até você digitar `quit`. Este tem uma linha de chegada.

Estes são os comandos que ele precisa entender:

| Comando | O que faz |
|---|---|
| `spawn <name> <hp> <attack>` | Traz um monstro para a arena: o nome, a vida e quão forte ele bate de volta. |
| `hit <name> <swing>` | Bate naquele monstro. A sua espada dobra a sua batida. Se o monstro viver, ele bate de volta. |
| `field` | Mostra todo monstro ainda de pé — o HP e o attack dele. |
| `threat` | Mostra o total de HP que resta entre *todos* os monstros — quanto de luta ainda falta. |
| `hero` | Mostra o seu próprio HP. |
| `help` | Lista os comandos. |
| `quit` | Sai da arena mais cedo. |

Uma execução de exemplo:

```text
The Arena. Type 'help' for commands.
> spawn goblin 10 3
A goblin enters the arena. 10 HP, hits for 3.
> spawn troll 18 6
A troll enters the arena. 18 HP, hits for 6.
> hero
You have 30 HP.
> hit goblin 4
You swing for 4. Your sword doubles it to 8. The goblin has 2 HP left.
The goblin hits back for 3. You have 27 HP.
> hit goblin 4
You swing for 4. Your sword doubles it to 8. The goblin falls!
> threat
18 HP of monsters still standing.
> hit troll 10
You swing for 10. Your sword doubles it to 20. The troll falls!
The arena is clear. You win!
```

Não precisa copiar esse texto. Faça do seu jeito. A lista de obrigatórios abaixo é o que importa.

## A ideia nova — dois dicionários que compartilham uma chave

Aqui está a parte nova, e é o motivo deste checkpoint ser um degrau a mais do que o último.

Cada monstro tem **dois** números para lembrar: o HP dele *e* o attack dele. Mas um único `Dictionary<string, int>` só guarda **um** número por nome. Então como você armazena dois números para o mesmo monstro?

O truque: manter **dois dicionários que compartilham a mesma chave** — o nome do monstro.

```csharp
var hp     = new Dictionary<string, int>();   // hp["goblin"]     = 10
var attack = new Dictionary<string, int>();   // attack["goblin"] = 3
```

Os dois usam o nome do monstro como chave. `hp["goblin"]` te diz a vida do goblin; `attack["goblin"]` te diz quão forte o goblin bate. Lidos juntos, eles descrevem um monstro.

A única regra que você deve seguir: **mantê-los em sincronia.** Sempre que um monstro entra, escreva nos *dois*. Sempre que um monstro cai, remova dos *dois*. Se eles saírem de sincronia — um nome em um mas não no outro — você vai ter um erro no momento em que buscá-lo. (É exatamente o tipo de coisa que o seu `try / catch` vai capturar calmamente em vez de travar.)

Só isso. Nenhuma palavra-chave nova do C# — só um jeito organizado de usar dois de uma ferramenta que você já conhece.

## Uma palavrinha sobre ponto e vírgula e chaves

Antes de construir, um lembrete curto sobre algo que confunde todo mundo no começo: *onde vai o `;` e onde vão as `{ }`.* Acertar isso sozinho é metade de escrever código que roda. Leia devagar — vale a pena.

Um **statement** (instrução) é uma única ordem. Um statement termina com ponto e vírgula. Três ordens significam três pontos e vírgula:

```csharp
int score = 0;
score = score + 5;
Console.WriteLine(score);
```

Mas uma linha que **abre um bloco** não é uma ordem terminada — então ela *não* termina com ponto e vírgula. Ela termina com uma chave de abertura `{`. As linhas que abrem um bloco são coisas como `while (...)`, `if (...)`, `foreach (...)` e a primeira linha de um method:

```csharp
while (score < 10)
{
    score = score + 1;
}
```

Olhe com atenção para as três linhas:

- `while (score < 10)` — **sem ponto e vírgula.** Ela não terminou. As `{ }` embaixo dela são o corpo dela. Um ponto e vírgula aqui é um dos erros mais comuns que existem.
- `score = score + 1;` — uma ordem de verdade, terminada, então ela **termina com `;`**.
- A `{` na própria linha tem uma `}` correspondente para fechar o bloco.

Dois hábitos que pegam quase todo problema de chave e ponto e vírgula, e você consegue fazer os dois sozinho:

1. **Leia cada linha e pergunte: "Isto é uma ordem terminada, ou está abrindo um bloco?"** Ordem terminada → termine com `;`. Abrindo um bloco → termine com `{`, sem `;`.
2. **Todo abridor tem um fechador.** Todo `(` precisa do seu `)`. Toda `{` precisa da sua `}`. Quando você digitar uma `{`, digite a `}` logo em seguida e depois preencha o meio. Assim você nunca perde a conta.

Se o programa não rodar e o erro mencionar `; expected` ou `} expected`, volte direto para esta lista. É quase sempre uma destas duas coisas — não a parte difícil do seu código.

## Os obrigatórios

O seu programa precisa mostrar todos os seis abaixo, mais a ideia dos dois dicionários acima. É isto que o Lars vai procurar — as mesmas seis habilidades do Tavern Tab e do Quest Board, num cenário novo.

1. **Um loop** que roda turno após turno. Diferente dos dois últimos, este loop pode terminar de três jeitos: você digita `quit`, todo monstro cai (você vence), ou o seu HP chega a zero (você perde). Um `break` ou um `return` é como você sai dele.

2. **Duas coleções que compartilham uma chave** — os dois `Dictionary<string, int>` acima. O nome do monstro é a chave nos dois. `hp[name]` guarda a vida dele; `attack[name]` guarda a batida dele. Adicione nos dois juntos, remova dos dois juntos.

3. **Um method que você escreveu e que retorna um valor** — não um `void`. Você tem dois lugares naturais para isso, e os dois fazem um pouco de matemática:
   - Um method `Damage` que recebe uma batida e devolve o número dobrado (`swing * 2`). Você o chama toda vez que bate.
   - O total `threat`: um method que percorre cada monstro e soma o HP deles, e então *devolve* a soma.

   Escolha pelo menos um destes para ser um method de verdade que recebe algo e devolve um número. (Fazer os dois é ainda melhor.)

4. **Verificação de número com `int.TryParse`.** Agora você verifica números em dois lugares. Quando um monstro entra você verifica **dois** números — o HP *e* o attack dele. Quando você bate, você verifica a batida. Se algum deles não for número — digamos que alguém digite `spawn goblin ten 3` — o programa não pode travar. Ele deve dizer algo educado e seguir em frente.

5. **Um `try / catch`** em volta do tratamento de comandos, para que um deslize inesperado (como buscar um monstro que não está lá) não mate o programa todo.

6. **Mensagens claras** feitas com string interpolation — por exemplo `$"The {name} hits back for {power}. You have {heroHp} HP."`.

## Construa

Construa uma peça de cada vez, e rode após *cada* peça. Esse hábito é o que transforma isto de assustador em fácil. Uma boa ordem:

1. **O loop e o `quit` primeiro.** Leia uma linha, e se for `quit`, pare. Imprima uma linha de boas-vindas acima do loop. Rode. Garanta que digitar `quit` realmente sai.
2. **Adicione `help`.** Só imprima a lista de comandos. Rode.
3. **Adicione `spawn`.** Divida a linha em partes. Verifique **os dois** números com `TryParse`. Escreva o HP num dicionário e o attack no outro, usando o nome como chave nos dois. Imprima uma linha dizendo que o monstro chegou. Rode — e de propósito, tente um número ruim em cada lugar para provar que não trava.
4. **Adicione `hero` e `field`.** `hero` só imprime o seu HP. `field` percorre os monstros e imprime o HP e o attack de cada um. Estes são rápidos e deixam você *ver* os seus dados. Rode após cada um.
5. **Adicione `hit`.** Este é o coração do jogo, então vá devagar:
   - Verifique que o nome do monstro está mesmo lá.
   - Verifique que a batida é um número.
   - Calcule o dano (o seu method `Damage` — a batida dobrada) e tire do HP daquele monstro.
   - Se o HP do monstro agora for zero ou menos, ele cai: remova dos **dois** dicionários e diga isso. Depois verifique — esse era o último monstro? Se for, você vence.
   - Senão, o monstro bate de volta: tire o attack dele do seu HP, e diga isso. Depois verifique — o *seu* HP chegou a zero? Se chegou, você perde.

   Rode depois que cada parte funcionar. Teste uma batida que mata, uma que não mata, e um nome que não está lá.
6. **Adicione `threat` por último.** Escreva o method que percorre cada monstro, soma o HP deles e retorna o total. Faça o comando imprimir. Rode.
7. **Envolva o tratamento de comandos em `try / catch`.** Rode mais uma vez, e tente quebrar de propósito — um número faltando, um monstro que não está lá, um comando sem sentido. Nada disso deve travar o programa.

## Se você travar

Isto é permitido — é a parte útil. Descubra a peça em que travou, volte e leia *só aquela lição*, depois volte e siga. Anote que precisou dela.

| Travou em… | Volte para |
|---|---|
| Lendo entrada, imprimindo, `$"..."` | Módulo 0.1 — Tinker |
| O loop `while`, `if`, `break` | Módulo 0.2 — Number Guess |
| Um method que recebe entrada e retorna um valor | Módulo 0.6 — Methods |
| `Dictionary` — armazenar e buscar por nome | Módulo 0.7 — Collections |
| `int.TryParse`, `try / catch` | Módulo 0.8 — Errors and Debugging |

## Mostre ao Lars — o checkpoint

Quando a sua Arena rodar e fizer os seis obrigatórios, marque um horário com Lars. Isto é uma parte normal e amigável do trabalho — não uma prova assustadora.

Você vai:

1. **Rodá-la para ele.** Ele vai tentar algumas coisas, incluindo um número ruim em cada slot e um monstro que não está lá, para verificar que não trava.
2. **Explicar o seu código a ele**, com as suas palavras — especialmente como os dois dicionários ficam em sincronia e como o seu method calcula o dano ou o total de ameaça.
3. **Talvez fazer uma pequena mudança na hora** — ele pode pedir para você adicionar uma coisinha enquanto assiste. É o melhor jeito de mostrar que as ideias são mesmo suas.

Se algo estiver instável, não é reprovação — é exatamente o que firmar antes do Kingdom precisar. Um pouco de prática naquela peça, depois mostrar de novo. Isso é uma vitória.

## Fechamento

Sem quiz — construir a Arena *é* a verificação.

1. **Progresso** — uma linha em `journal/progress.md`: `Module 0.9c — The Arena — DATE — built the Arena from scratch.`
2. **Commit e push** — primeiro verifique que o Source Control diz `kingdom` (não `kingdom-course`!), depois prepare o seu trabalho, mensagem do commit `Module 0.9c done`, Sync. (O commit pelo terminal em `running-your-project.md` é o jeito mais fácil de uma janela de programa único.)
3. **Poste em `#wins`** — uma linha sobre a sua Arena, mais a URL do commit.

O "pronto" de verdade é passar o checkpoint com Lars — o push só salva o seu trabalho.

## Próximo

Esse é o último checkpoint. O próximo é **Fase 1 — o Kingdom começa.** No Módulo 1.1 você conhece as suas primeiras **classes** e constrói a primeira versão do Kingdom: prédios, cidadãos, recursos. E lembra como você manteve dois dicionários em sincronia para cada monstro? Uma classe é a resposta organizada para isso — uma coisa só que guarda todos os números de um monstro juntos. Tudo que você acabou de mostrar que consegue fazer, você vai usar desde a linha um.
