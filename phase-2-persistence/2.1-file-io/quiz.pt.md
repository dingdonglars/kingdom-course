# Quiz — Módulo 2.1

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que usar `Path.Combine("a", "b")` em vez de `"a\\b"` ou `"a/b"`?

- **a.** Mais rápido que concatenação de string em tempo de execução
- **b.** Escolhe o separador certo para o sistema operacional — seu código funciona no Windows, no Mac e no Linux
- **c.** O compilador C# recusa caminhos em string
- **d.** Ele criptografa o caminho antes de gravar

## 2. O que `Directory.CreateDirectory(folder)` faz se a pasta já existir?

- **a.** Lança `IOException` porque a pasta está em uso
- **b.** Nada — é idempotente (não faz nada quando a pasta já está lá)
- **c.** Apaga a pasta e recria ela do zero
- **d.** Retorna `false` e deixa a pasta em paz

## 3. Qual codificação `File.WriteAllText(path, text)` usa por padrão no .NET moderno?

- **a.** ASCII (um byte por caractere)
- **b.** UTF-8 sem BOM (o padrão moderno)
- **c.** UTF-16 (o antigo padrão do Windows)
- **d.** O que o sistema operacional preferir

## 4. Por que usar `Path.GetTempPath()` nos testes em vez de gravar na pasta do projeto?

- **a.** A pasta temporária tem acesso a disco mais rápido
- **b.** Arquivos temporários não poluem a árvore de código-fonte, o sistema operacional limpa eles, e execuções de teste em paralelo não colidem
- **c.** É uma exigência da linguagem C# para testes
- **d.** Para esconder os arquivos de teste do histórico do git

## 5. A lição diz *"a engine tem zero mudanças neste módulo."* Por que isso importa?

- **a.** Prova que a separação engine-versus-shell funciona — o disco é uma preocupação do shell
- **b.** Economiza digitação durante a lição
- **c.** Coincidência; nada a se ler nisso
- **d.** Os testes quebrariam de formas inesperadas, se não fosse assim

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
