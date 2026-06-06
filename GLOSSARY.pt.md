# GLOSSARY.md

> Tente primeiro o `GLOSSARY.md` em inglês. Use este aqui só quando uma palavra te travar.

> Glossário alfabético vivo de todo termo apresentado em alguma aula. Cada entrada é uma linha: uma definição que você pode ler em português simples, e a aula onde o termo aparece pela primeira vez.

> **Como esta lista cresce:** toda aula com um quadro "Words to watch" acrescenta seus termos aqui no momento de autoria. `/lesson-review` e a auditoria final de consistência verificam entradas faltando.

## A

- **`A.CallTo(...)`** — sintaxe do FakeItEasy para definir o que um fake deve retornar quando um método é chamado. *Módulo 1.8.*
- **`A.Fake<T>()`** — sintaxe do FakeItEasy para criar um fake que implementa a interface `T`. *Módulo 1.8.*
- **AI Unlock** — a transição nomeada do currículo (fim da Fase 3 / M4) em que a IA passa de "só fricção" para "colaborador de verdade". *Módulo 3.9.*
- **aggregate root** — a classe de nível superior que possui e conecta tudo (`Kingdom`). Fica na raiz da árvore de namespace. *Módulo 1.9.*
- **App Service (Azure)** — hospedagem PaaS gerenciada; F1 Free tier $0/mês. *Módulo 3.8.*
- **`AddOpenApi()` / `MapOpenApi()`** — gerador de OpenAPI embutido para ASP.NET Core 9+. *Módulo 3.4.*
- **`AddAuthentication`/`AddCookie`/`AddGoogle`** — trio de fiação de autenticação do ASP.NET Core para o Sign-In With Google. *Módulo 3.5.*
- **authentication** — *quem é você* (login). *Módulo 3.5.*
- **authorisation** — *o que você tem permissão de fazer* (escopo por recurso). *Módulo 3.6.*
- **alias** — um nome curto para uma tabela em uma consulta SQL (`SELECT k.name FROM kingdoms k`). *Módulo 2.5.*
- **`AllowCredentials`** — flag de CORS necessária para autenticação por cookie; incompatível com `AllowAnyOrigin`. *Módulo 4.6.*
- **`async` / `await`** — sintaxe JS para código que pausa no `await` até que uma Promise resolva. *Módulo 4.2.*
- **argument** — o valor real que você passa ao chamar um método (o *parameter* é o nome na definição; o *argument* é o valor). *Módulo 0.6.*
- **arrange / act / assert** — a estrutura convencional de 3 seções de um unit test. *Módulo 1.3.*
- **`AsNoTracking`** — método do EF Core para consultas somente leitura; pula o rastreamento de mudanças, mais rápido e seguro. *Módulo 2.6.*
- **ASCII art** — imagens feitas de caracteres de texto. *Módulo 0.4.*
- **`abstract`** — uma classe ou método sem implementação; as subclasses precisam fornecê-la. *Módulo 1.5 (menção); Módulo 1.10 (tinker).*

## B

- **`base(...)`** — chama o construtor da classe pai a partir do construtor filho. *Módulo 1.5.*
- **base class** (ou *parent class*) — a classe da qual uma classe filha herda. *Módulo 1.5.*
- **`bool`** — `true` ou `false`, nada mais. *Módulo 0.5.*
- **breakpoint** — uma linha marcada onde o depurador pausa a execução. *Módulo 0.8.*

## C

- **CDN** — Content Delivery Network; assets estáticos servidos da borda mais próxima de cada usuário. *Módulo 4.6.*
- **CI/CD** — Continuous Integration / Continuous Deployment; build + test + deploy automático a cada push. *Módulo 3.8.*
- **component** — uma função (ou classe) reutilizável que transforma dados em UI. *Módulo 4.4.*
- **context engineering** — escolher o que entra na janela de contexto da IA para que o output se encaixe no projeto. *Módulo 4.0.*
- **CORS** — Cross-Origin Resource Sharing; segurança do navegador em relação a chamadas de API entre origens diferentes. *Módulo 4.2.*
- **claim** — um par chave/valor que um provedor de identidade afirma sobre um usuário (`email`, `name`, `sub`). *Módulo 3.5.*
- **client ID / client secret** — credenciais que o seu app recebe de um provedor OAuth (Google) identificando-o como app registrado. *Módulo 3.5.*
- **cookie auth** — o servidor define um cookie de sessão após o login; o navegador o envia em toda requisição subsequente. *Módulo 3.5.*
- **cascade delete** — quando apagar uma linha pai também apaga suas linhas filhas. Padrão do EF para relacionamentos obrigatórios. *Módulo 2.9.*
- **call stack** — a cadeia de métodos que se chamaram mutuamente até chegar ao ponto atual de execução. *Módulo 0.8.*
- **camelCase** — convenção para variáveis locais e parâmetros (`goldCount`, `firstName`). *Módulo 0.5.*
- **`_camelCase`** — convenção para campos privados dentro de classes (um sublinhado + camelCase). *Módulo 0.5.*
- **cast** — converter um valor de um tipo para outro (`(int)3.99` é `3`). *Módulo 0.5.*
- **class library** — um projeto que compila para uma `.dll`, não um executável; sem `Main`. Usado por outros projetos. *Módulo 1.2.*
- **code-first** — definir o schema em código C#; o ORM gera o SQL. Oposto: database-first. *Módulo 2.6.*
- **collection** — qualquer estrutura de dados que guarda múltiplos valores (lista, dicionário, array). *Módulo 0.7.*
- **composition** — uma classe *contém* outra (em vez de *herdar* dela). Muitas vezes uma alternativa mais flexível à herança. *Módulo 1.5.*
- **`Console.ReadLine`** — lê uma linha de entrada do usuário (string), retorna `null` no EOF. *Módulo 0.1; Módulo 2.10 (com tratamento de EOF).*
- **`Console.SetIn` / `SetOut`** — redireciona os streams do console; permite que testes programem entrada do usuário e capturem a saída. *Módulo 2.10.*
- **CRUD** — Create / Read / Update / Delete; as quatro operações em linhas. *Módulo 2.9.*
- **conditional** — `if` / `else` — uma bifurcação no código. *Módulo 0.2.*
- **`Console.ForegroundColor`** — a propriedade que controla a cor do texto escrito no console a partir daquele ponto. *Módulo 0.4.*
- **`Console.ReadLine()`** — o método que pausa, espera a entrada, retorna a linha como string. *Módulo 0.1.*

## D

- **database** — um armazenamento estruturado de dados, consultável. *Módulo 2.4.*
- **DOM** — Document Object Model; a árvore em memória de uma página HTML; manipulável a partir do JS. *Módulo 4.2.*
- **DevTools** — ferramentas de desenvolvimento do navegador (Elements / Console / Network); F12 na maioria dos navegadores. *Módulo 4.2.*
- **`DateTime`** — uma data *e* uma hora, juntas. *Módulo 0.5.*
- **DB Browser for SQLite** — ferramenta GUI gratuita para abrir e consultar arquivos SQLite. *Módulo 2.8.*
- **`DbContext`** — gateway do EF Core para o banco de dados; um por tempo de vida da conexão. *Módulo 2.6.*
- **`DbSet<T>`** — propriedade do EF Core que representa uma tabela; suporta LINQ. *Módulo 2.6.*
- **debugger** — uma ferramenta que pausa o seu programa em pontos escolhidos para você ver o que está acontecendo. *Módulo 0.8.*
- **deferred execution** — o LINQ não roda quando você chama `.Where(...)`; ele constrói uma receita. O trabalho acontece quando você itera / chama `.ToList()` / `.Count()`. *Módulo 1.6.*
- **dependency injection (DI)** — passar os colaboradores de uma classe via construtor em vez de criá-los dentro dela. *Módulo 1.8.*
- **deterministic** — as mesmas entradas sempre produzem as mesmas saídas. A característica que torna engines testáveis. *Módulo 1.7 (apresentado como a regra que a gente quebrou); Módulo 1.8 (corrigido).*
- **`Dictionary<K, V>`** — uma tabela de lookup de `K` (chave) para `V` (valor); cada chave aparece no máximo uma vez. *Módulo 0.7.*
- **discard (`_`)** — um nome para "não me importo com este valor". Também usado em padrões de switch como o "qualquer outra coisa" do catch-all. *Módulo 1.7.*
- **`dotnet ef`** — ferramenta CLI do EF Core para gerar e aplicar migrations. *Módulo 2.7.*
- **`dotnet ef migrations script`** — gera o SQL que as migrations rodariam (sem executar). *Módulo 2.8.*
- **`double`** — um número que pode ter ponto decimal. *Módulo 0.5.*
- **DTO** — Data Transfer Object — um registro pequeno e só de dados criado especialmente para cruzar uma fronteira (disco, rede, API). *Módulo 2.2.*

## E

- **`escapeHtml`** — codifica os cinco caracteres especiais de HTML (`<`, `>`, `&`, `"`, `'`); previne XSS ao interpolar entrada do usuário em `innerHTML`. *Módulo 4.4.*
- **ES modules** — `import` / `export` entre arquivos JS; o jeito moderno de compartilhar código. *Módulo 4.3.*
- **event delegation** — ouvir em um pai por eventos que sobem de muitos filhos; escala para muitos itens sem handlers por filho. *Módulo 4.4.*
- **EF Core** — Entity Framework Core; o ORM padrão do .NET. *Módulo 2.6.*
- **`EnsureCreated`** — método do EF que cria o arquivo do banco + tabelas a partir do modelo atual. Não usa migrations; adequado só para bancos novos. *Módulo 2.6.*
- **`__EFMigrationsHistory`** — tabela de controle do EF que rastreia quais migrations foram aplicadas. *Módulo 2.7.*
- **encapsulation** — manter os internos de uma classe privados para que quem chama não possa mexer neles; só os métodos públicos podem mudar o estado. *Módulo 1.2 (prévia), Módulo 1.3 (nomeado).*
- **encoding** — como texto vira bytes. UTF-8 é a resposta para ~tudo escrito nessa década. *Módulo 2.1.*
- **engine** — a parte do código sobre o *domínio* (o kingdom, suas regras). Não sabe de IO, redes, UI. *Módulo 1.2.*
- **engine vs shell** — a disciplina que separa lógica de domínio (engine) de entrada/saída (shell). O fio condutor de todo o curso. *Módulo 1.2.*
- **entity** — uma classe que o EF mapeia para uma tabela. *Módulo 2.6.*
- **event** — algo que aconteceu no kingdom em um dia específico. Guardado como um registro pequeno. *Módulo 1.7.*
- **event log** — a lista em ordem de todos os eventos que o kingdom acumulou. *Módulo 1.7.*
- **EventEngine** — a classe que joga dados a cada tick para decidir se (e qual) evento acontece. *Módulo 1.7; reescrito no Módulo 1.8.*
- **exception** — um erro em tempo de execução que o programa não conseguiu tratar e lançou. *Módulo 0.8.*
- **extension method** — um método estático cujo primeiro parâmetro tem `this`; parece um método de instância onde você o usa. O LINQ é construído sobre esses. *Módulo 1.6.*

## F

- **`[Fact]`** — atributo do xUnit que marca um único método de unit test. *Módulo 1.3.*
- **`fetch(url)`** — jeito moderno do JS de fazer requisições HTTP do navegador; retorna uma Promise. *Módulo 4.2.*
- **factory method** — um método estático que retorna uma instância, usado no lugar de um construtor (`Kingdom.LoadFrom(snap, ...)`). *Módulo 2.3.*
- **fake** (também *mock*, *stub*) — um substituto em tempo de teste para um colaborador real. *Módulo 1.8.*
- **FakeItEasy** — a biblioteca .NET que usamos para fakes de uma linha (`A.Fake<IRandom>()`). *Módulo 1.8.*
- **`File.ReadAllText`** — lê o conteúdo inteiro de um arquivo como string. *Módulo 0.4; Módulo 2.1 (com paths/encoding).*
- **`File.WriteAllText`** — escreve uma string em um arquivo (cria se não existir; sobrescreve se existir). *Módulo 0.4; Módulo 2.1.*
- **`Find` (EF)** — retorna a entidade pela chave primária, `null` se não existir (em vez de `Single` que lança exceção). *Módulo 2.9.*
- **foreign key** — uma coluna cujo valor corresponde a um `id` em outra tabela. *Módulo 2.5.*
- **`for`** — loop baseado em contador (`for (int i = 0; i < n; i++)`). *Módulo 0.7.*
- **`foreach`** — percorre todos os itens de uma coleção, um de cada vez. *Módulo 0.7.*

## G

- **game loop** — o loop que chama `Tick` repetidamente. Coração de todo jogo. *Módulo 1.4.*
- **GitHub Actions** — executor de workflows embutido no GitHub; arquivos YAML em `.github/workflows/`. *Módulo 3.8.*
- **`GetType()`** — um método em todo objeto que retorna seu tipo em tempo de execução; `.Name` dá o nome curto em string. *Módulo 1.5.*
- **`git cherry-pick`** — aplica um commit específico (pelo hash) ao branch atual. *Módulo 1.10.*
- **`git reset --hard`** — destrutivo: descarta mudanças não commitadas e rebobina o ponteiro do branch. *Módulo 1.10.*
- **`git stash`** — guarda de lado mudanças não commitadas (recuperáveis com `git stash pop`). *Módulo 1.10.*
- **global using** — uma diretiva `using` em `GlobalUsings.cs` que se aplica a todos os arquivos do projeto. *Módulo 1.9.*
- **`GROUP BY`** — cláusula SQL que colapsa linhas em uma por valor de agrupamento distinto. Usada com agregados (`COUNT`, `SUM`, `AVG`). *Módulo 2.5.*

## H

- **`happy-dom`** — ambiente DOM falso e rápido para testes que precisam de `document` sem um navegador real. *Módulo 4.5.*
- **HMR** — Hot Module Replacement; edite o código, salve, o navegador atualiza sem perder o estado. *Módulo 4.3.*
- **HTML** — linguagem de marcação que descreve a estrutura da página. *Módulo 4.1.*
- **HTTP** — Hypertext Transfer Protocol; requisição e resposta cliente→servidor. *Módulo 3.1.*
- **HTTPS-only** — configuração do App Service que exige TLS para todas as requisições; necessário para autenticação por cookie. *Módulo 3.8.*

## I

- **`IClassFixture<T>`** — atributo do xUnit que marca um fixture de teste compartilhado (uma instância para todos os testes da classe). *Módulo 3.7.*
- **`IClock`** — interface para "que horas são". Produção: `SystemClock`. Testes: um fake. *Módulo 1.8.*
- **`ILogger<T>`** — interface de log do ASP.NET Core; injetada por DI por consumidor; carrega o tipo do consumidor como categoria do log. *Módulo 3.4.*
- **integration test** — um teste que exercita múltiplos componentes juntos (em vez de unit = um). *Módulo 3.7.*
- **idempotent** — fazer duas vezes tem o mesmo efeito que fazer uma vez (`GET`, `PUT`, `DELETE`). *Módulo 3.1.*
- **`IInterface`** — convenção para nomes de interface (`IBuilding`, `IRandom`). *Módulo 0.5.*
- **`Include`** — método do EF Core para carregar ansiosamente uma propriedade de navegação relacionada (`db.Kingdoms.Include(k => k.Buildings)`). *Módulo 2.6.*
- **inheritance** — uma classe declara que herda de outra com `:`; recebe todos os campos/métodos do pai, pode adicionar mais, pode sobrescrever os `virtual`. *Módulo 1.5.*
- **`InlineData`** — atributo do xUnit que fornece um conjunto de entradas a um `[Theory]`. *Módulo 1.3.*
- **`INNER JOIN`** — apenas as linhas que combinam nos dois lados do JOIN. *Módulo 2.5.*
- **`int`** — um número inteiro (sem ponto decimal); intervalo de aproximadamente ±2 bilhões. *Módulo 0.5.*
- **`int.Parse(...)`** — transforma uma string de dígitos em `int`; lança exceção se não for um inteiro válido. *Módulo 0.2.*
- **`int.TryParse(...)`** — *tenta* fazer o parse, retorna `bool` para sucesso, escreve o valor via `out`. O padrão padrão para entrada de usuário não validada. *Módulo 2.10.*
- **interface** — um contrato — *formas* de método/propriedade, sem corpos. Muitas classes podem implementar a mesma interface. *Módulo 1.8.*
- **`internal`** — modificador de visibilidade; tipo/membro visível apenas dentro do mesmo assembly. *Módulo 1.9.*
- **`IRandom`** — a interface do engine para números aleatórios. Produção: `SystemRandom`. Testes: um fake. *Módulo 1.8.*

## J

- **JavaScript** — a linguagem de runtime do navegador; para o que o TypeScript compila. *Módulo 4.2.*
- **JOIN** — combina linhas de duas tabelas em uma condição de correspondência. *Módulo 2.5.*
- **JSON** — JavaScript Object Notation; o formato universal de dados em texto. *Módulo 2.2.*
- **`JsonSerializer`** — API do `System.Text.Json`. `Serialize` (objeto → string), `Deserialize<T>` (string → T). *Módulo 2.2.*

## K

## L

- **lambda** — a sintaxe `x => expr`. Uma função descartável escrita inline. *Módulo 1.6.*
- **`LEFT JOIN`** — toda linha da tabela da esquerda; correspondências da direita ou `NULL`. *Módulo 2.5.*
- **LINQ** — Language-Integrated Query: métodos como `.Where`, `.Select`, `.OrderBy`, `.Sum` que funcionam em qualquer coleção. *Módulo 0.7 (menção); Módulo 1.6 (em profundidade).*
- **list** — uma coleção ordenada de coisas que você pode adicionar, remover e percorrer. *Módulo 0.3.*
- **`List<string>`** — uma lista especificamente de strings. O `<string>` diz "esta é uma lista de strings"; você vai ver `<int>`, `<Building>` depois. *Módulo 0.3.*
- **`List<T>`** — lista ordenada genérica de `T`s. *Módulo 0.7.*
- **loop** — um trecho de código que roda repetidamente até ser mandado parar. *Módulo 0.2.*

## M

- **`MapGet` / `MapPost` / `MapPut` / `MapDelete`** — métodos de minimal-API para registrar rotas. *Módulo 3.1.*
- **`MapGroup`** — agrupa rotas que compartilham um prefixo de path. *Módulo 3.3.*
- **menu loop** — `while (true) { imprimir menu; ler entrada; despachar; }` — o coração de todo shell interativo. *Módulo 2.10.*
- **minimal API** — sintaxe leve do ASP.NET Core (`app.MapGet(...)`); sem controllers, só lambdas. *Módulo 3.1.*
- **mode flag** — linha única em `CLAUDE.md` que controla o comportamento de assistência da IA (`pre-unlock` / `post-unlock`). *Módulo 3.9.*
- **method** — um pedaço de código com nome que faz uma coisa. *Módulo 0.1 (chamado); Módulo 0.3 (definido); Módulo 0.6 (em profundidade).*
- **migration** — uma mudança de schema versionada e reversível. Gerada por `dotnet ef migrations add`. *Módulo 2.7.*
- **`Migrate()`** — aplica todas as migrations pendentes ao banco (em código, equivalente a `dotnet ef database update`). *Módulo 2.7.*

## N

- **navigation property** — uma propriedade de entidade que aponta para uma entidade relacionada ou uma lista delas; o EF carrega via `Include`. *Módulo 2.6.*
- **noise word** — palavras genéricas como `Manager`, `Helper`, `Util`, `Data`, `Info` que não dizem o que a coisa realmente faz. *Módulo 2.11.*
- **nullable** — um tipo que também pode conter `null` (nenhum valor); escrito com `?` no final. *Módulo 0.5.*

## O

- **OAuth 2.0** — protocolo de autorização delegada (deixa o Google verificar o usuário; diz ao meu app quem ele é). *Módulo 3.5.*
- **`OfType<T>()`** — método LINQ: filtra uma coleção para itens cujo tipo em tempo de execução é `T`. *Módulo 1.6.*
- **OIDC (OpenID Connect)** — camada de identidade sobre o OAuth 2.0. *Módulo 3.5.*
- **OpenAPI** — documento JSON padronizado que descreve cada endpoint de uma API; contrato legível por máquina. *Módulo 3.4.*
- **organisation** — dividir o código em múltiplos métodos para que cada um faça uma coisa. *Módulo 0.3.*
- **`OrderBy` / `OrderByDescending`** — métodos de ordenação do LINQ. *Módulo 1.6.*
- **ORM** — Object-Relational Mapper; traduz entre objetos em memória e linhas em um banco relacional. EF Core, Dapper, etc. *Módulo 2.6.*
- **overload** — múltiplos métodos com o mesmo nome mas tipos de parâmetro diferentes. *Módulo 0.6.*
- **`override`** — palavra-chave para substituir um método `virtual` em uma subclasse. O compilador insiste nisso (para que você não sobrescreva por acidente). *Módulo 1.5.*

## P

- **parameter** — o nome de uma entrada declarada na definição de um método (`string kingdom` em `Greet(string kingdom)`). *Módulo 0.6.*
- **publish profile** — credenciais que o Azure fornece para habilitar deploys. *Módulo 3.8.*
- **parameters (SQL)** — espaços reservados `$name` / `@name` que enviam valores separadamente da consulta — derrotam SQL injection. *Módulo 2.4.*
- **PascalCase** — convenção para nomes de tipo, nomes de método e propriedades (`Building`, `Console.WriteLine`). *Módulo 0.5.*
- **path** — o endereço de um arquivo no disco. Construa com `Path.Combine(...)` para portabilidade. *Módulo 2.1.*
- **`Path.Combine`** — une segmentos de path usando o separador do SO. Sempre use isso, não `+ "/"`. *Módulo 2.1.*
- **`PRIMARY KEY AUTOINCREMENT`** — idioma SQL para "dê a cada linha um id único automaticamente". *Módulo 2.4.*
- **predicate** — uma função que retorna `bool` (ex.: `b => b.Level > 1`). O LINQ usa esses em todo lugar. *Módulo 1.6.*
- **projection** — selecionar apenas as colunas que você precisa (`.Select(k => new KingdomSlotInfo(...))`); o EF gera SQL com apenas essas colunas. *Módulo 2.9.*
- **project reference** — uma linha em `.csproj` dizendo "eu dependo deste outro projeto". *Módulo 1.2.*
- **property-based testing** — afirmar que uma propriedade vale para muitas entradas, não só um caso específico. `[Theory]+[InlineData]` é uma semente manual. *Módulo 2.3.*

## Q

## R

- **`Random`** — a classe da biblioteca padrão para números aleatórios. Poderosa e perigosa — nunca use diretamente dentro de um engine. *Módulo 1.7.*
- **REST conventions** — regras informais para combinações de verbo + path + status (GET lista, POST cria + 201, DELETE + 204). *Módulo 3.3.*
- **`.RequireAuthorization()`** — guarda de endpoint; 401 se não houver cookie de autenticação válido. *Módulo 3.5.*
- **`Results.Ok` / `NotFound` / `BadRequest` / `Created` / `NoContent`** — helpers de minimal-API que controlam o código de status. *Módulo 3.2 / 3.3.*
- **route parameter** — `{id}` no path; vinculado a um argumento de método; pode ter restrição de tipo (`{id:int}`). *Módulo 3.3.*
- **`record`** — palavra-chave C# para uma pequena classe de dados imutável onde dois records com os mesmos campos são iguais automaticamente, com ToString e desconstrução. Perfeito para eventos. *Módulo 1.7.*
- **README** — o documento no topo de todo repositório. Quatro seções que importam: o que / como rodar / o que você aprendeu / o que vem depois. *Módulo 0.4 (intro); Módulo 1.10 (em profundidade).*
- **rename party** — uma sessão focada que faz só renomeações, nada mais. Atômica, revisável, segura via IDE. *Módulo 2.11.*
- **rescue rule (git)** — leia o estado (`git status`, `git log --oneline -10`) antes de agir sobre ele. *Módulo 1.10.*
- **`ResourceLedger`** — a classe do engine do Kingdom que possui o `Dictionary<Resource, int>` e expõe `Get`/`Add`/`Spend`/`Snapshot`/`SetTo`. *Módulo 1.2; SetTo adicionado no Módulo 2.3.*
- **return value** — o valor que um método devolve; declarado pelo tipo antes do nome do método. *Módulo 0.6.*
- **round-trip test** — salva e depois carrega; verifica que o modelo carregado é igual ao original. *Módulo 2.3.*

## S

- **`save slot`** — uma linha na tabela kingdoms; um save que o jogador pode escolher de uma lista. *Módulo 2.9.*
- **Static Web Apps (Azure)** — hospedagem gerenciada para frontends estáticos; tier gratuito com CDN + SSL + auto-deploy do GitHub. *Módulo 4.6.*
- **semantic markup** — `<header>`/`<main>`/`<nav>` em vez de `<div>` em todo lugar; carrega significado para leitores de tela e SEO. *Módulo 4.1.*
- **Scalar** — alternativa moderna e leve ao Swagger UI; renderiza specs do OpenAPI como HTML interativo. *Módulo 3.4.*
- **scoped query** — toda leitura/escrita filtra por dono (`WHERE OwnerSub = ?`). *Módulo 3.6.*
- **status code** — número HTTP de 3 dígitos na resposta (200, 404, 500, ...). *Módulo 3.1.*
- **structured logging** — entradas de log com campos nomeados, consultáveis diretamente. *Módulo 3.4.*
- **`sub` (claim)** — o id de usuário estável e globalmente único do Google; o identificador canônico no armazenamento. *Módulo 3.5 / 3.6.*
- **Swagger UI** — página HTML interativa gerada a partir de uma spec do OpenAPI. *Módulo 3.4.*
- **schema drift** — quando o schema do banco diverge do modelo do código. As migrations existem para prevenir isso. *Módulo 2.7.*
- **seed** — um número passado para `Random` para que duas instâncias `Random(seed)` produzam a mesma sequência. Usado para execuções reproduzíveis. *Módulo 1.8.*
- **`SELECT` / `INSERT` / `UPDATE` / `DELETE`** — as quatro instruções SQL de "fazer". *Módulo 2.4.*
- **serialise / deserialise** — transformar um objeto em string (ou bytes) e de volta. *Módulo 2.2.*
- **shell** — a parte do código que fala com o mundo externo (console, arquivos, rede, UI). Muitos shells, um engine. *Módulo 1.2.*
- **`Shouldly`** — uma biblioteca de assertions fluente. `value.ShouldBe(expected)` em vez de `Assert.Equal(expected, value)`. *Módulo 1.3.*
- **`ShouldBe(...)`** — a assertion principal do Shouldly: lança com uma mensagem clara se o valor não combinar. *Módulo 1.3.*
- **side effect** — quando um método muda o estado em algum lugar em vez de (ou além de) retornar um valor. `AdvanceDay()` é um grande side effect. *Módulo 1.4.*
- **`Single` (LINQ)** — retorna o único elemento que combina com um predicado; lança se zero ou muitos. *Módulo 2.6.*
- **snapshot** — uma imagem completa dos dados do kingdom em um momento no tempo (um record DTO). *Módulo 2.3.*
- **solution (`.slnx`)** — um arquivo que agrupa projetos relacionados para que possam ser construídos juntos. *Módulo 1.2.*
- **SQL** — Structured Query Language; a linguagem dos bancos de dados relacionais. *Módulo 2.4.*
- **SQL injection** — o que acontece quando você concatena entrada do usuário em SQL. Derrotado por parâmetros. *Módulo 2.4.*
- **SQLite** — um engine de banco SQL autocontido; uma biblioteca, sem servidor, o banco é um arquivo. *Módulo 2.4.*
- **`SqliteConnection`** — handle para um arquivo de banco SQLite. Sempre envolva em `using`. *Módulo 2.4.*
- **`static`** — um método ou campo pertencente ao tipo em si, não a uma instância específica. *Módulo 0.6 (prévia); Módulo 1.1 (em profundidade).*
- **`string`** — um pedaço de texto no código, entre `"aspas duplas"`. *Módulo 0.1; Módulo 0.5 (formal).*
- **string interpolation** — a sintaxe `$"..."` que deixa você enfiar variáveis dentro de uma string com `{chaves}`. *Módulo 0.1.*
- **subclass** (ou *child class*) — uma classe que herda de uma classe base/pai. *Módulo 1.5.*
- **sub-namespace** — `Kingdom.Engine.Buildings` é um sub-namespace de `Kingdom.Engine`. Mesmo projeto, balde diferente. *Módulo 1.9.*
- **`switch` expression** — padrão moderno: `value switch { pattern => result, ... }`. Mais limpo do que escadas de if/else if. *Módulo 1.7.*
- **`System.Text.Json`** — a biblioteca JSON moderna do .NET. *Módulo 2.2.*
- **`SystemClock`** — implementação de produção de `IClock`; retorna `DateTime.UtcNow`. *Módulo 1.8.*
- **`SystemRandom`** — implementação de produção de `IRandom`; envolve `System.Random`. *Módulo 1.8.*

## T

- **table** — uma grade de linhas e colunas; a unidade de armazenamento em um banco relacional. *Módulo 2.4.*
- **template literal** — backticks JS `` `Day ${day}` `` para string interpolation. *Módulo 4.4.*
- **TypeScript** — JavaScript + tipos; compila para JS. *Módulo 4.3.*
- **`[Theory]` + `[InlineData]`** — atributos do xUnit para testes parametrizados: mesma lógica, entradas diferentes. *Módulo 1.3.*
- **`this` parameter** — a palavra-chave no primeiro parâmetro de um método estático que o transforma em extension method. *Módulo 1.6.*
- **`throw`** — a palavra-chave para lançar uma exception. *Módulo 0.8.*
- **tick** — um passo do tempo de jogo. No nosso kingdom, um tick = um dia. *Módulo 1.4.*
- **transaction** — um grupo de operações no banco que todas têm sucesso ou todas falham juntas. *Módulo 2.9.*
- **`try / catch`** — envolve código arriscado em `try`; se lançar, o bloco `catch` correspondente roda. *Módulo 0.8.*
- **`try / finally`** — garante que o bloco `finally` rode (ex.: limpeza de teste) mesmo que o `try` lance. *Módulo 2.1.*
- **type** — que tipo de valor algo é (número, texto, verdadeiro/falso, data). *Módulo 0.5.*

## U

- **unit test** — um pequeno pedaço de código que verifica um comportamento específico de um método específico. *Módulo 1.3.*
- **user-secrets (`dotnet user-secrets`)** — armazenamento de segredos só local (fora do repositório); o ASP.NET Core carrega automaticamente em desenvolvimento. *Módulo 3.5.*
- **`using` (resource)** — `using var x = new SqliteConnection(...)` — garante que `Dispose` rode ao fim do escopo. *Módulo 2.4.*
- **UTF-8** — codificação de texto; o padrão para ~tudo escrito nessa década. *Módulo 2.1.*

## V

- **variable** — um lugar com nome para guardar um dado e usá-lo depois. *Módulo 0.1.*
- **Vite** — servidor de desenvolvimento frontend moderno + bundler; HMR rápido, suporte a TS, quase zero config. *Módulo 4.3.*
- **Vitest** — executor de testes nativo do Vite; equivalente ao xUnit para JS/TS. *Módulo 4.5.*
- **`VITE_*` env vars** — variáveis de ambiente em tempo de compilação do Vite (`import.meta.env.VITE_API_URL`). *Módulo 4.6.*
- **verb (HTTP)** — `GET`, `POST`, `PUT`, `PATCH`, `DELETE` — o que o cliente quer fazer. *Módulo 3.1.*
- **viva** — defesa oral 1:1 nos milestones; o mentor pergunta "explique esta linha escrita pela IA" de forma aleatória. *Módulo 3.9.*
- **`virtual`** — um método que as subclasses têm *permissão* de sobrescrever. A classe base fornece um padrão. *Módulo 1.4.*
- **`void`** — o tipo de retorno especial que significa "este método não devolve nada". *Módulo 0.6.*

## W

- **`WebApplication.CreateBuilder(args)`** — ponto de entrada de minimal-API. Retorna um builder. *Módulo 3.1.*
- **`WebApplicationFactory<TEntryPoint>`** — helper de teste do ASP.NET Core que inicializa o app em processo. *Módulo 3.7.*
- **`Where`** — filtro LINQ: mantém os itens que o predicado aprova. *Módulo 1.6.*
- **`when` (pattern)** — adiciona um predicado a um braço de switch: `1 when k.Citizens.Count > 0 => ...`. *Módulo 1.7.*
- **`while`** — palavra-chave que inicia um loop "continue fazendo isso enquanto a condição for verdadeira". *Módulo 0.2.*
- **`WriteIndented`** — flag de `JsonSerializerOptions` para JSON legível por humanos (multi-linha + indentado). *Módulo 2.2.*

## X

- **XML doc comment** — comentário `///` acima de um tipo ou método público; o IDE o mostra como tooltip / entrada do IntelliSense. *Módulo 1.10.*
- **XSS** — Cross-Site Scripting; injetar JS via strings sem escape em `innerHTML`. *Módulo 4.4.*

## Y

## Z

---

## Fase 5 (Roblox / Luau) — apêndice

- **`BindToClose`** — hook de encerramento do servidor Roblox; ~30s para descarregar o estado. *Módulo 5.7.*
- **`ClickDetector`** — helper Roblox; filho de uma Part, dispara eventos de clique no servidor. *Módulo 5.6.*
- **DataStore** — persistência k/v do Roblox; só no servidor. *Módulo 5.7.*
- **Instance.new(class, parent)** — cria um objeto Roblox em tempo de execução. *Módulo 5.6.*
- **Lua / Luau** — linguagem de script do Roblox; Lua + tipos. *Módulo 5.2.*
- **metatable** — tabela de comportamento; mecanismo de OOP do Lua via `__index`. *Módulo 5.3.*
- **ModuleScript** — unidade de código importável do Roblox; carregada com `require`. *Módulo 5.3.*
- **Part** — bloco de construção 3D básico do Roblox; visível no Workspace. *Módulo 5.6.*
- **`pcall(fn)`** — chamada protegida do Lua (try/catch). *Módulo 5.7.*
- **RemoteEvent** — mensagem assíncrona de um sentido cliente↔servidor no Roblox. *Módulo 5.4.*
- **`require(...)`** — carrega um ModuleScript; armazenado em cache. *Módulo 5.3.*
- **Roblox Studio** — editor do Roblox; gratuito para Windows/Mac. *Módulo 5.1.*
- **`ServerScriptService`** — lar dos scripts só de servidor no Explorer do Studio. *Módulo 5.1 / 5.4.*
- **`setmetatable(t, mt)`** — anexa uma metatable a uma tabela. *Módulo 5.3.*
- **`task.wait(seconds)`** — pausa leve do Roblox; por corrotina. *Módulo 5.5.*
- **Workspace** — cena 3D ao vivo do Roblox; replicada para os clientes. *Módulo 5.1.*
