# HelpDesk WPF — EF Core + MySQL

**Autor:** Guilherme Santiago
**RM:** 552321

---

## ⚙️ Como rodar do zero

### 1. Pré-requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- MySQL Server rodando localmente
- `dotnet-ef` instalado globalmente:
```bash
dotnet tool install --global dotnet-ef
```

### 1.1 Dependências (Pacotes NuGet)
Para rodar o EF Core com MySQL neste projeto, os seguintes pacotes são indispensáveis e já devem estar listados no projeto (basta rodar `dotnet restore` para baixar as dependências, ou instalá-los manualmente caso esteja criando do zero):
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Pomelo.EntityFrameworkCore.MySql
```
*(Também utilizamos os pacotes `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Configuration.Json` e `Microsoft.Extensions.Configuration.UserSecrets` para proteger a string de conexão).*

Caso ocorra algum erro informando dependências ou "Arquivo de ativos não encontrado" na hora de executar os próximos passos, lembre-se de rodar a restauração do projeto:
```bash
dotnet restore
```

---

### 2. Configurar a conexão com o banco

Na pasta **raiz do projeto** (`/`), configure a string de conexão com seus dados do MySQL:

```bash

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=HelpDesk;Uid=root;Pwd=SUA_SENHA;"
```

> Substitua `SUA_SENHA` pela senha do seu MySQL local.

---

### 3. Criar as Tabelas no Banco de Dados

Como o projeto já possui a pasta `Migrations` configurada no código, você **não precisa gerar novas migrações**. Basta aplicar as existentes no seu banco de dados.
Se o seu banco de dados estiver completamente vazio (ambiente zerado), o comando abaixo vai criar o banco `HelpDesk` automaticamente do zero já com todas as estruturas necessárias.

Execute:
```bash
dotnet ef database update
```

Isso criará automaticamente o banco `HelpDesk` com as tabelas:
- `Clientes`
- `Tecnicos`
- `Chamados`
- `Departamentos`
- `Equipamentos`

---

### 4. Rodar a aplicação WPF

```bash

dotnet run
```

Ou abra `WpfApp1.sln` no **Visual Studio 2022** e pressione **F5**.

---

## 📌 Resumo dos comandos (ordem correta)

```bash
# 1. Restaurar as dependências e baixar os pacotes NuGet
dotnet restore

# 2. Configurar senha do MySQL
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=HelpDesk;Uid=root;Pwd=SENHA;"

# 3. Aplicar as migrações no banco (já cria todas as tabelas em bancos zerados baseando-se nas migrations do projeto)
dotnet ef database update

# 4. Rodar o WPF

dotnet run
```

---

## 📝 Notas de Arquitetura e Decisões de Projeto

**1. Busca por ID**
A busca por ID (`BuscarPorIdAsync`) **foi implementada no backend** cumprindo o requisito. Porém, não é utilizada pelo usuário no frontend: em aplicações gráficas modernas, não há necessidade de decorar ou digitar IDs, pois a seleção para edição/exclusão é feita com cliques diretos na lista.

**2. Ausência de Paginação e Sobrecarga**
Carregar todos os registros de uma vez sem filtro geraria sobrecarga de memória, o que normalmente exigiria criar paginação. Isso foi resolvido utilizando o **`.AsNoTracking()`** nas consultas do Entity Framework. Essa função remove o monitoramento de cache das entidades, deixando as requisições extremamente leves e rápidas, mitigando a sobrecarga e dispensando a necessidade de paginação para este escopo.
