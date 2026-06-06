# Bônus B1.3 — Conhecendo o SSMS

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Ontem você moveu o seu reino para o SQL Server com três linhas de configuração. Agora os seus dados vivem em um banco de dados Microsoft de verdade, e existe uma ferramenta profissional para trabalhar com ele: **SQL Server Management Studio**, ou SSMS para abreviar. É a GUI que a maioria dos administradores de banco de dados que trabalham na área mantém aberta o dia todo. É gratuita, tem todas as funções, e dez minutos clicando por aí é suficiente para se sentir confortável. Depois disso você consegue navegar em qualquer SQL Server que encontrar, pelo resto da sua carreira.

O ponto de hoje não é aprender todas as partes do SSMS — ele tem partes que ninguém usa. O ponto é aprender os cinco movimentos básicos para a ferramenta parar de parecer estranha.

> **Words to watch**
>
> - **SSMS** — SQL Server Management Studio; a GUI padrão para SQL Server
> - **Object Explorer** — árvore do lado esquerdo mostrando cada servidor, banco de dados, tabela, view
> - **query window** — o editor onde você digita SQL e aperta F5 para rodar
> - **execution plan** — um diagrama mostrando como o banco de dados vai rodar uma consulta
> - **Activity Monitor** — visão ao vivo do que as consultas estão fazendo agora

---

## Passo 1 — instalar o SSMS

Baixe de [a página do Microsoft SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms). É gratuito e tem cerca de 700 MB.

SSMS é exclusivo do Windows. No macOS ou Linux, instale o **Azure Data Studio** em vez disso — a versão da Microsoft que roda em qualquer sistema. A UI é um pouco diferente, mas o trabalho é o mesmo: conectar em um SQL Server, navegar nele, rodar consultas, e olhar os planos de execução.

Depois de instalar, abra o SSMS. O diálogo Connect aparece.

## Passo 2 — conectar ao LocalDB

Preencha o diálogo:

- **Server type:** Database Engine
- **Server name:** `(localdb)\MSSQLLocalDB`
- **Authentication:** Windows Authentication

Clique em **Connect**. O painel Object Explorer à esquerda mostra a sua instância do LocalDB. Expanda:

- **Databases**
- → o seu banco `Kingdom_*` (um por slot de save dos testes de ontem)
- → **Tables**
- → `Kingdoms`

Clique com o botão direito na tabela `Kingdoms` e escolha **Select Top 1000 Rows**. Uma query window abre com SQL gerado automaticamente no topo e as linhas mostradas abaixo. Essas são linhas reais que o seu engine escreveu ontem, mostradas pela própria ferramenta de banco de dados da Microsoft. Mesmos dados, vistos por uma ferramenta diferente.

## Passo 3 — cinco movimentos para saber

Uma vez conectado, essas são as coisas que você usa todo dia:

1. **`Ctrl+N`** — abre uma nova query window. Cole qualquer SQL. Aperte `F5` para rodar.
2. **Clique direito na tabela → Design** — vê o schema (colunas, tipos, restrições) em uma visão de formulário.
3. **Clique direito no banco → Properties → Files** — vê onde o arquivo de dados `.mdf` fica no disco.
4. **Activity Monitor** (barra de ferramentas, parece um ícone de gráfico) — visão ao vivo das consultas rodando, bloqueios e esperas. Leia isso quando algo parecer lento.
5. **Query → Display Estimated Execution Plan** (`Ctrl+L`) — mostra como o banco de dados planeja rodar sua consulta. Buscas de índice, scans, joins, tudo isso. Leia isso quando uma consulta *está* lenta.

O plano de execução é o melhor recurso. Quando uma consulta demora mais do que você espera, o plano te diz por quê — geralmente porque um índice está faltando ou um join está combinando muito mais linhas do que você pensou. Mesmo no nível em que você está agora, olhar um plano de execução te diz mais sobre como um banco de dados pensa do que qualquer post de blog vai te dizer.

## Passo 4 — tente algumas coisas

Em uma nova query window, rode:

```sql
SELECT * FROM Kingdoms;
```

Depois:

```sql
SELECT name FROM sys.databases;
```

Essa segunda consulta o catálogo do sistema — a própria lista de bancos de dados do SQL Server. Útil para responder "o que está aqui de verdade?"

Para exportar um banco de dados: clique com o botão direito no seu banco `Kingdom_*` → **Tasks** → **Generate Scripts**. Escolha *"Schema and data"* no assistente. O resultado é um grande arquivo `.sql` com as definições das tabelas mais os `INSERT` statements para recriar cada linha. Útil para backups, para compartilhar uma cópia com alguém, ou para verificar o que mudou em uma migration.

## Mexa um pouco

Abra um dos arquivos de migration gerados pelo EF. Ou rode `dotnet ef migrations script -o init.sql` para escrever todo o histórico de migration como um arquivo SQL. Abra no SSMS. Leia. A saída do EF é só SQL — uma vez que você consegue ler o que ele produz, o framework para de parecer mágica.

Clique com o botão direito em um índice → **Properties** → veja as estatísticas de uso. O SSMS mantém o controle de quantas vezes cada índice é usado. Índices que ninguém lê são espaço desperdiçado; índices que todos leem estão fazendo o seu trabalho.

Abra o Activity Monitor sem nada rodando. Depois rode uma consulta lenta de propósito — `SELECT COUNT(*) FROM sys.objects, sys.objects, sys.objects` — e veja aparecer. Cancele antes do seu notebook superaquecer.

## O que você acabou de fazer

Você instalou o SSMS, conectou na sua instância do LocalDB, e usou os cinco movimentos básicos: query window, visão Design, propriedades de arquivo, Activity Monitor, e plano de execução. A mesma ferramenta funciona contra qualquer SQL Server em qualquer lugar — seu notebook, a máquina de um colega, um Azure SQL Database na nuvem — e os movimentos não mudam. Cinco minutos para instalar, dez minutos clicando, e agora você consegue inspecionar qualquer SQL Server que encontrar por anos.

**Conceitos que você já sabe nomear:**

- **SSMS** — a GUI padrão do SQL Server
- **Object Explorer** — árvore de servidores, bancos e tabelas
- **query window (`Ctrl+N`)** — escreva e rode SQL
- **execution plan** — mapa visual de como uma consulta vai rodar
- **Activity Monitor** — visão ao vivo das consultas rodando

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória — abra o SSMS do zero e chegue em uma das suas próprias linhas:

1. Conecte na sua instância do LocalDB.
2. Encontre um dos seus bancos `Kingdom_*` na árvore.
3. Abra uma nova query window.
4. Rode uma consulta que mostre as linhas que o seu engine escreveu.

<details><summary>Travou? Abra aqui para conferir.</summary>

- Diálogo Connect: Server type **Database Engine**, Server name `(localdb)\MSSQLLocalDB`, Authentication **Windows Authentication**, depois **Connect**.
- No Object Explorer à esquerda, expanda **Databases** → um dos seus bancos `Kingdom_*` → **Tables** → `Kingdoms`.
- Aperte `Ctrl+N` para uma nova query window e rode:

```sql
SELECT * FROM Kingdoms;
```

Você deve ver as linhas que o seu engine escreveu, mostradas pela própria ferramenta da Microsoft.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B1.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B1.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B1.4 fecha o bônus com uma reflexão curta: um parágrafo no `journal/B1-what-i-learned.md` dizendo o que a mudança provou.
