# Quiz — Módulo 5.6

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é uma Part no Roblox?

- **a.** Um arquivo de código que contém scripts e recursos
- **b.** O bloco de construção 3D básico — tem forma, tamanho, posição, cor, material; fica no `Workspace`
- **c.** Uma função Lua que constrói algo visível
- **d.** Um tipo de script usado só para lógica visual

## 2. O que `Instance.new("Part", workspace)` faz?

- **a.** Cria uma nova Part e a coloca como filha de `workspace` — a Part aparece na cena 3D ao vivo na hora
- **b.** Carrega uma Part que tinha sido salva no disco antes
- **c.** Importa uma Part da Toolbox do Roblox
- **d.** Compila uma nova classe Part para uso posterior

## 3. Por que a lição mantém um mapa `tileToBuilding`?

- **a.** A engine e a representação visual precisam ficar em sincronia. Sem o mapa, você não consegue saber qual tile já tem um prédio quando ele é clicado de novo.
- **b.** Melhora o desempenho em tempo de execução do tratamento de cliques
- **c.** O Roblox exige o mapa para o `ClickDetector` funcionar
- **d.** Preferência de estilo — qualquer estrutura de dados funcionaria

## 4. Para que serve um `ClickDetector`?

- **a.** Detectar cliques do mouse numa Part; dispara um evento do lado do servidor com o jogador que clicou como argumento
- **b.** Detectar bugs e erros de tempo de execução
- **c.** Um filho obrigatório para qualquer Part renderizar no `Workspace`
- **d.** Detecção de áudio na superfície de uma Part

## 5. A regra da lição: "a engine é a fonte da verdade, o visual é a projeção dela." Aplique ela.

- **a.** A engine decide o que é verdade (a lista de prédios do reino); o visual segue a partir disso. Não atualize o visual sem passar pela engine.
- **b.** Engine e visual são independentes e podem discordar
- **c.** O visual é o autoritativo; a engine reflete o que o jogador vê
- **d.** Os dois são sempre iguais porque compartilham os mesmos dados

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
