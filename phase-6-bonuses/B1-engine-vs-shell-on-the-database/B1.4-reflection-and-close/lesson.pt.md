# Bônus B1.4 — Reflexão e Fechamento

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O B1 termina com um parágrafo. Você mudou um banco de dados em três linhas, conheceu o SSMS, e seus testes ficaram verdes o tempo todo. O trabalho de hoje é escrever o que isso prova — com suas próprias palavras, no seu próprio diário — e fechar o bônus.

O ponto de escrever não é ter uma escrita bonita. É que colocar o que aconteceu com suas próprias palavras transforma isso em algo que você se lembra. Uma mudança que você fez e esqueceu desaparece. Uma mudança que você descreveu com suas próprias palavras fica com você.

---

## Passo 1 — escrever o parágrafo

Abra `journal/B1-what-i-learned.md`. Tem um template esperando por você com três perguntas:

1. **O que a troca exigiu?**
2. **O que isso prova?**
3. **O que você faria diferente se começasse de novo hoje?**

Mire em cerca de 150 palavras no total. Não enrole. O bônus inteiro é uma grande resposta para a pergunta *"a regra engine-vs-shell valeu a pena?"* e seu parágrafo é a sua resposta.

Uma resposta modelo (a sua vai dizer algo diferente, na sua própria voz):

> Changing SQLite for SQL Server took three lines: a NuGet package change, the `UseSqlServer(...)` provider call, and the new connection string format. Then I regenerated migrations and all 71 tests passed unchanged. That proves the engine-vs-shell rule isn't just a nice phrase — it's a real pattern that survives a database swap. If I started over, I'd add a SQL Server provider check earlier, maybe somewhere in Phase 3, so the rule gets a real test sooner. But it would have held either way.

A resposta modelo tem cerca de 110 palavras. Esse é mais ou menos o tamanho que você está mirando — claro, sem enrolação.

## Passo 2 — marcar o bônus

Quando seu parágrafo estiver escrito e commitado no seu diário, marque o bônus como completo. Este aqui é só CLI — o painel não tem botão para tags:

```powershell
git tag b1-engine-vs-shell-on-the-database-complete
git push origin b1-engine-vs-shell-on-the-database-complete
```

Não tem PR de milestone para o B1 — é um bônus que fica por conta própria. O parágrafo de reflexão é o que você fica, e a tag é o marcador.

## O que é opcional a seguir

Se o B1 pareceu bom, aqui estão alguns próximos passos naturais (nenhum obrigatório):

- Tente o **Azure SQL Database** — a versão na nuvem do SQL Server. Mesma chamada de provider, connection string diferente. Mesma troca, agora pela rede em vez de na sua própria máquina.
- Tente o **PostgreSQL** para tornar isso três bancos de dados. Uma empresa diferente, o mesmo padrão de três linhas.
- Leia um capítulo de *Designing Data-Intensive Applications* de Martin Kleppmann. O melhor livro sobre toda essa camada.

## O que você acabou de fazer

Você terminou um bônus pequeno e focado. O arco todo foi: instalar o LocalDB, mudar três linhas de configuração, assistir cada teste passar, instalar o SSMS para olhar o resultado, e escrever o que isso significou. Cada passo foi pequeno de propósito, porque o ponto do bônus era transformar a regra engine-vs-shell em algo que você pudesse ver e tocar — não apenas uma coisa dita na Fase 1. O parágrafo de reflexão no seu diário é o que você fica.

**Conceitos que você já sabe nomear:**

- **engine-vs-shell, provada** — a disciplina sobrevive a uma troca real de banco de dados
- **troca de provider** — três linhas de configuração mais regeneração de migration
- **migration regen** — mudança de provider significa regenerar o SQL de migration
- **SSMS como ferramenta vitalícia** — a GUI que viaja com você

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória:

1. Nomeie os quatro passos que o arco levou, em ordem.
2. Depois diga, em uma frase, o que a coisa toda provou.

<details><summary>Travou? Abra aqui para conferir.</summary>

Os quatro passos, em ordem:

1. Instalar o LocalDB (B1.1).
2. Mudar três linhas de configuração e regenerar as migrations (B1.2).
3. Ver cada teste passar sem mudança (B1.2, Passo 4).
4. Abrir o SSMS para ver as linhas que o seu engine escreveu (B1.3).

O que provou: a regra engine-vs-shell é real — o banco de dados é uma peça que você pode trocar em poucas linhas, e o engine e seus testes nunca percebem.

</details>

## Fechamento

Sem quiz nesta lição — o parágrafo de reflexão é o artefato. Então:

1. **Progresso** — uma linha no `journal/progress.md`: `Module B1.4 — Reflection and Close — DATE — closed B1: 3-line DB swap, all tests green. Learnt: one sentence.`
2. **Commit e push** — seu parágrafo de reflexão mais a linha de progresso, mensagem de commit `Module B1.4 done`, Sync.
3. **Poste no `#wins`** — *"B1 closed."* com a URL do commit (a tag do Passo 2 é separada; não precisa de um post próprio no #wins).

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar.

## Próximo

Você terminou o B1. **B2 (Context Engineering)** é o bônus mais profundo — ele nomeia e melhora o que você tem feito com ferramentas de IA desde o AI Unlock no Módulo 4.0.
