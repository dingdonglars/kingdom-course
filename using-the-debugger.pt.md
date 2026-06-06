# Usando o depurador — um mini guia

> Tente primeiro o `using-the-debugger.md` em inglês. Use este aqui só quando uma palavra te travar.

> Leia isto uma vez. Volte sempre que quiser ver seu código rodar, ou caçar um bug.
>
> **Setup assumido:** Windows + VS Code (com C# Dev Kit), seu projeto aberto.

Um depurador deixa você **pausar seu programa numa linha e olhar dentro dele** — ver cada variável, rodar uma linha de cada vez, e acompanhar os valores mudarem. Ele transforma *"acho que é isso que acontece"* em *"consigo ver exatamente o que acontece."* É o jeito mais rápido de entender código, e o jeito mais rápido de achar um bug.

(Você conheceu o depurador no Módulo 0.8. Esta página é a referência curta para voltar a consultar.)

## Inicie

- Abra o arquivo que você quer observar (por exemplo `Program.cs`).
- Aperte **F5**. O programa começa *sob o depurador*.
- Para rodar *sem* pausar nos breakpoints, aperte **Ctrl+F5**.

Se o F5 não fizer nada, ou perguntar "qual projeto?", isso é uma questão de *qual pasta / qual projeto* — não um problema do depurador. Veja o `running-your-project.md`.

## Breakpoints — onde ele pausa

Um **breakpoint** é uma linha que você marca para o programa parar ali.

- **Coloque um:** clique na faixa estreita logo à esquerda do número de uma linha. Um ponto vermelho aparece. (Ou ponha o cursor na linha e aperte **F9**.)
- **Remova:** clique no ponto vermelho de novo (ou aperte F9 de novo).
- O programa pausa **logo antes daquela linha rodar** — a linha fica amarela, e ela *ainda não aconteceu*.
- Coloque quantos quiser.

## As teclas que você mais vai usar

| Tecla | O que faz |
|---|---|
| **F5** | Inicia a depuração / **Continua** (roda até o próximo breakpoint) |
| **Ctrl+F5** | Roda **sem** o depurador |
| **F9** | Adiciona / remove um breakpoint na linha atual |
| **F10** | **Step over** — roda esta linha, vai para a próxima |
| **F11** | **Step into** — entra *dentro* do método que esta linha chama |
| **Shift+F11** | **Step out** — termina este método e volta para quem o chamou |
| **Shift+F5** | **Para** a depuração |
| **Ctrl+Shift+F5** | **Reinicia** do começo |

**Step over vs step into:** o **F10** roda uma chamada de método de uma vez só; o **F11** a segue *por dentro*, linha por linha. Use o F11 quando quiser ver o que um método de fato *faz*; o F10 quando você confia nele e só quer seguir adiante.

## Enquanto está pausado — os painéis

Aperte **Ctrl+Shift+D** para abrir a view **Run and Debug** à esquerda. Ela tem quatro painéis:

- **Variables** — toda variável à vista agora, com seu valor. Clique na setinha ao lado de um objeto (como `kingdom`) para abri-lo e ver seus campos, listas e dicionários por dentro.
- **Watch** — expressões que *você* escolhe acompanhar. Clique em **+** e digite algo como `kingdom.Day` ou `kingdom.Resources.Get(Resource.Food)`. Ela se atualiza toda vez que o programa pausa. (Ou clique com o botão direito numa variável → **Add to Watch**.)
- **Call Stack** — a cadeia de métodos que levou até aqui: quem chamou quem. A linha de cima é onde você está agora. Clique em qualquer linha abaixo para pular para aquele método e ver *as variáveis dele*.
- **Breakpoints** — uma lista de todos os seus breakpoints. Marque / desmarque para ligá-los e desligá-los sem apagar.

**Dica:** enquanto pausado, só **passe o mouse por cima de qualquer variável no código** para ver o valor dela num pequeno pop-up.

## Mude um valor enquanto está pausado

Você pode editar o programa *enquanto ele está parado*:

- No painel **Variables**, dê dois cliques num valor, digite um novo, e aperte **Enter**. O programa continua com o seu novo valor.
- Exemplo: pause dentro do game loop, mude `Day` para `49`, depois continue — agora você está testando o que acontece no dia 50 sem esperar por ele.

Você também pode digitar expressões no **Debug Console** (o painel embaixo enquanto pausado): digite `kingdom.Day` e aperte Enter para vê-lo, ou `kingdom.Resources.Get(Resource.Gold)` para checar um número. Você pode até chamar métodos dali.

## Pause só quando importa — breakpoints condicionais

Um breakpoint normal pausa *toda* vez. Às vezes você só se importa com um caso.

- Clique com o botão direito num ponto vermelho → **Edit Breakpoint…** → digite uma condição como `Day == 50`. Agora ele só pausa quando isso for verdade.
- Perfeito quando um bug aparece na 50ª volta de um loop, não na 1ª.

(Há também um **Logpoint** — botão direito na faixa → *Add Logpoint…* — que **imprime uma mensagem em vez de pausar**. Útil quando você quer observar sem parar o programa.)

## Um tour de 60 segundos no seu reino

Quando você tiver um game loop (Módulo 1.4):

1. Coloque um breakpoint na primeira linha dentro de `AdvanceDay`.
2. **F5** — ele pausa no começo do primeiro dia.
3. Abra **Variables**; ache `Resources` e `Day`.
4. **F10** pelas linhas e veja `Food` cair e `Day` subir.
5. **F11** em `b.Tick(...)` para entrar *dentro* da produção de um prédio.
6. **F5** para terminar, ou **Shift+F5** para parar.

## Se o F5 se comportar mal

Nove em cada dez vezes é a pasta ou o projeto errado aberto, não o seu código. Veja o `running-your-project.md`: na Fase 0 você abre a pasta de um programa; a partir da Fase 1 você abre a solution `kingdom-game`, e a partir da Fase 3 você escolhe qual projeto o F5 inicia.

## Cola rápida

- **Pausar aqui:** clique na faixa à esquerda da linha / **F9**
- **Ir:** **F5**  ·  **Rodar, sem depurar:** **Ctrl+F5**  ·  **Parar:** **Shift+F5**
- **Uma linha:** **F10**  ·  **Entrar num método:** **F11**  ·  **Sair:** **Shift+F11**
- **Ver valores:** painel Variables · hover · Debug Console
- **Acompanhar uma expressão:** Watch (**+**)
- **Quem chamou isto:** Call Stack
- **Mudar um valor:** dois cliques nele no Variables
