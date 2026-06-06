# Bônus B2.4 — Lendo a Saída da IA com Cuidado

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

A saída da IA parece confiante. Muitas vezes não deveria. A lição de hoje é sobre o passo eval — a parte onde você lê o que voltou e decide se fica com ele. Existe uma leitura de cinco pontos que você consegue fazer em cada resposta de código em uns dois minutos. Faça vinte vezes e vira automático.

O ponto não é que a saída da IA é ruim. Na maioria das vezes é boa. O ponto é que a IA estar confiante não é o mesmo que você ter verificado. As vezes em que ela produz uma aparência de algo confiante que não funciona são exatamente as vezes em que uma leitura cuidadosa te salva de aceitar algo quebrado.

> **Words to watch**
>
> - **invented API** — a IA chama um método que não existe
> - **silent edge case** — o caminho feliz funciona; o caso vazio/null/zero quebra
> - **style drift** — a saída usa padrões diferentes do seu codebase
> - **over-helpful** — a saída adiciona recursos que você não pediu
> - **swallowed error** — a saída captura uma exceção e a ignora

---

## A leitura de cinco pontos

Para cada resposta de código, em ordem:

1. **Procure APIs inventadas.** Qualquer chamada de método que pareça desconhecida — *você escreveu isso? Existe?* Faça grep das que você não tem certeza.
2. **Verifique o caminho infeliz.** O que acontece quando a entrada é null, vazia, zero, ou enorme? O código lidou com isso? O teste da IA lidou com isso?
3. **Compare o estilo com um exemplo recente.** Abra `KingdomEfStore.cs` (ou qualquer arquivo que seja a correspondência mais próxima). Mesmos padrões? Mesmos nomes? Mesmos tipos de retorno?
4. **Procure over-help.** Ele mudou código que você não pediu? *"I also refactored X"* é um sinal de alerta.
5. **Procure erros engolidos.** Blocos `catch (Exception)` que não relançam e não logam viram bugs silenciosos em produção.

Dois minutos por resposta. Depois de vinte respostas isso é automático — seus olhos vão para os pontos fracos primeiro, como um chef provando a própria sopa antes de servir.

## Exemplo trabalhado

**Você pede:** *"Em `KingdomEfStore.cs`, adicione `LoadRichest(string ownerSub)`."*

**A IA retorna:**

```csharp
public async Task<KingdomSlotInfo?> LoadRichestAsync(string ownerSub)
{
    using var ctx = new KingdomDbContext(_dbPath);
    try
    {
        return await ctx.Kingdoms
            .Where(k => k.OwnerSub == ownerSub)
            .OrderByDescending(k => k.Gold)
            .Select(k => new KingdomSlotInfo(k.Id, k.Name, k.Day))
            .FirstOrDefaultAsync();
    }
    catch
    {
        return null;
    }
}
```

A leitura de cinco pontos sinaliza:

1. **API inventada?** `FirstOrDefaultAsync` é EF Core de verdade. Passou.
2. **Caminho infeliz?** Sem correspondência retorna `null` (correto, via `FirstOrDefaultAsync`). Passou.
3. **Desvio de estilo?** Os seus métodos existentes não são async. A IA tornou este async sem você pedir. *Desvio.*
4. **Over-help?** Você não pediu async. *Sim — over-help.*
5. **Erro engolido?** `catch { return null; }` esconde tudo — incluindo erros de rede, falta de memória, e bugs de programação. Uma falha real silenciosamente vira "sem reinos." *Ruim.*

Sua resposta de volta: *"Drop the async — existing methods aren't async. Drop the catch — let exceptions propagate."* Mais duas linhas de feedback, e código limpo na segunda passagem.

## Quando empurrar de volta, quando aceitar

| Situação | O que fazer |
| --- | --- |
| API inventada | Sempre rejeite; peça a API real |
| Desvio de estilo em um padrão de todo o projeto | Empurre de volta |
| Desvio de estilo em uma pequena variação | Aceite se ler claramente |
| Over-help | Empurre de volta; peça só o que você especificou |
| Erros engolidos | Sempre rejeite; tipos de exceção com nome e `throw` são obrigatórios |
| Caminho infeliz perdido | Empurre de volta; peça o caso de teste |

O padrão não é "saída perfeita" — é *saída que você consegue explicar na viva.* Se você consegue explicar cada linha, fique com ela. Se não consegue, faça mais perguntas para a IA até conseguir.

## Mexa um pouco

Escolha uma saída recente da IA que você aceitou. Rode a leitura de cinco pontos nela agora, olhando para trás. Você deixou algo passar? Acontece — a leitura é um hábito, e hábitos levam repetição para construir.

Tente um prompt vago de propósito e veja a IA preencher as lacunas com adivinhações. Agora rode a mesma tarefa com um scope apertado (B2.3) e um ponteiro para um arquivo de exemplo (B2.2). O over-help quase desaparece.

Tente um prompt que diga claramente *"no async"* e *"no try/catch unless asked."* Compare a saída. A IA segue regras estritas quando você as escreve.

## O que você acabou de fazer

Você conheceu a leitura de cinco pontos — APIs inventadas, caminhos infelizes, desvio de estilo, over-help, erros engolidos — e passou por um exemplo trabalhado onde quatro dos cinco apareceram em uma resposta. Dois minutos por resposta da IA é suficiente para pegar a maior parte do que escorrega por entre os dedos. O padrão é *saída que você consegue explicar*, não *saída que parece certa* — essas são coisas muito diferentes, e a leitura de cinco pontos é o que mostra a diferença.

**Conceitos que você já sabe nomear:**

- **invented API** — chamada de método que não existe
- **silent edge case** — quebrado em null, vazio, ou zero
- **style drift** — padrões diferentes do seu código
- **over-help** — faz mais do que você pediu
- **swallowed error** — `catch` sem relançar ou logar

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória:

1. Liste todos os cinco pontos que você verifica em cada resposta de código.
2. Depois diga o único padrão que decide se você fica com a saída ou empurra de volta.

<details><summary>Travou? Abra aqui para conferir.</summary>

A leitura de cinco pontos:

1. **APIs inventadas** — cada chamada de método realmente existe? Faça grep das que você não tem certeza.
2. **O caminho infeliz** — o que acontece em entrada null, vazia, zero, ou enorme?
3. **Desvio de estilo** — mesmos padrões, nomes e tipos de retorno que um arquivo recente como `KingdomEfStore.cs`?
4. **Over-help** — mudou código que você não pediu?
5. **Erros engolidos** — um `catch` que não relança ou loga.

O padrão: fique com a saída só se você conseguir explicar cada linha na viva. Se não conseguir, faça mais perguntas até conseguir — *saída que você consegue explicar*, não *saída que parece certa*.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B2.4 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B2.4 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B2.5 fecha o bônus: um tour pelas ferramentas de IA por aí (Claude vs Copilot vs Cursor), depois a sua reflexão escrita sobre o ano.
