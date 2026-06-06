# Bônus B1.1 — Instalar o LocalDB

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Na Fase 2 você colocou o reino no SQLite. Funciona. Então por que instalar outro banco de dados? Porque a regra engine-vs-shell fez uma promessa. O engine não liga para qual banco de dados ele fala. Trocar o banco de dados deve ser uma mudança de configuração, não uma reescrita. O B1 é o teste que verifica essa promessa. O trabalho de hoje é o primeiro pequeno passo — instalar o **SQL Server LocalDB** na sua máquina. Amanhã a gente aponta o seu código EF Core existente para ele e roda os testes.

LocalDB é a ferramenta certa para isso. É um SQL Server *de verdade* — T-SQL completo, o mesmo engine que grandes empresas rodam em produção — mas a versão feita para desenvolvedores. Instala em cinco minutos. Roda no seu notebook. Não tem nenhum serviço em segundo plano para gerenciar, e não custa nada. Real, mas pequeno.

> **Words to watch**
>
> - **SQL Server** — o banco de dados relacional principal da Microsoft, muito usado na indústria
> - **LocalDB** — a edição para desenvolvedor de um único usuário; uma instância por usuário, sem daemon
> - **connection string** — a linha de texto que o EF usa para encontrar o banco de dados
> - **SSMS** — SQL Server Management Studio; a ferramenta GUI que a gente conhece no B1.3

---

## Passo 1 — instalar o LocalDB

**Windows (o caminho principal):**

1. Acesse [a página do Microsoft LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb).
2. Execute o instalador. Quando ele perguntar o que instalar, escolha **LocalDB only** — pule o SQL Server completo.
3. Depois de instalar, abra o PowerShell:

   ```powershell
   sqllocaldb info
   ```

4. Você deve ver `MSSQLLocalDB` na saída — essa é a instância padrão.
5. Inicie ela:

   ```powershell
   sqllocaldb start MSSQLLocalDB
   ```

**macOS ou Linux:** LocalDB é exclusivo do Windows. Use a imagem do SQL Server no Docker em vez disso:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Strong!Pass1" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

É o mesmo engine SQL Server, só empacotado para Docker. A connection string no B1.2 vai usar `localhost,1433` e a senha SA em vez da forma do LocalDB.

## Passo 2 — confirmar que está lá

Rode uma consulta rápida na sua nova instância:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION"
```

Você deve ver uma longa string de versão. Isso é o SQL Server respondendo você. Se aparecer, a instalação funcionou e você terminou.

## Passo 3 — olhar o formato da connection string

No B1.2 você vai colar uma connection string na sua configuração do EF Core. Existem dois formatos, dependendo do caminho que você tomou:

**LocalDB:**

```
Server=(localdb)\\MSSQLLocalDB;Database=Kingdom;Trusted_Connection=True;
```

**Docker SQL Server:**

```
Server=localhost,1433;Database=Kingdom;User Id=sa;Password=Strong!Pass1;TrustServerCertificate=true;
```

Perceba o que é igual e o que é diferente. As chaves são diferentes — `Server` em vez de `Data Source`, e `Trusted_Connection` é novo — mas o formato é familiar. Você diz ao driver onde o banco está, como chamá-lo e quem você é. A connection string do SQLite diz as mesmas coisas. Só é mais curta, porque SQLite é um arquivo no disco.

## Mexa um pouco

Tente `sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases"` — você verá os bancos de dados do sistema (`master`, `tempdb`, `model`, `msdb`) mas nenhum `Kingdom` ainda. Isso vem amanhã.

Se você já tiver o SSMS (a gente instala de forma adequada no B1.3), conecte em `(localdb)\MSSQLLocalDB` com Autenticação do Windows. A instância está vazia, mas você consegue acessá-la. Ver que ela está lá torna o B1.2 mais real.

## O que você acabou de fazer

Você instalou o SQL Server LocalDB — a versão para desenvolvedor do banco de dados principal da Microsoft. Confirmou que está rodando com `sqllocaldb info`, e rodou sua primeira consulta nele através do `sqlcmd`. A instalação levou uns dez minutos, e o instalador em si foi quase chato. Esse é o ponto. A história toda do B1 é que mudar o banco de dados acontece em pequenos passos, e este é o primeiro. A mudança de três linhas de configuração de amanhã vai colocar o seu código EF Core existente nessa nova instância.

**Conceitos que você já sabe nomear:**

- **LocalDB** — edição para desenvolvedor de um único usuário do SQL Server
- **`(localdb)\MSSQLLocalDB`** — o nome da instância padrão na sua máquina
- **`sqllocaldb info` / `start`** — controle por linha de comando das instâncias do LocalDB
- **connection string** — o texto que o EF usa para encontrar um banco de dados
- **Trusted_Connection / Integrated Security** — autenticação do Windows (sem senha necessária)

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a ideia grande pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem rolar de volta, de memória — abra uma janela nova do PowerShell e prove que a sua instalação do LocalDB está viva:

1. Verifique que a instância padrão existe.
2. Inicie ela.
3. Rode uma consulta que receba uma resposta real do SQL Server.

<details><summary>Travou? Abra aqui para conferir.</summary>

```powershell
sqllocaldb info
sqllocaldb start MSSQLLocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION"
```

- `sqllocaldb info` deve listar `MSSQLLocalDB`.
- `start` deve dizer que a instância foi iniciada (ou que já estava rodando).
- A consulta do `sqlcmd` deve imprimir uma longa string de versão — isso é o SQL Server respondendo você.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module B1.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare (stage) os dois arquivos, mensagem de commit `Module B1.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve para a próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

B1.2 é o teste em si. Três linhas de configuração, e o código do engine EF Core escreve no SQL Server em vez do SQLite. Os seus testes passam sem mudar nada.
