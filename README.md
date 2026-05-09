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

---

### 2. Configurar a conexão com o banco

Na pasta **raiz do projeto** (`/`), configure a string de conexão com seus dados do MySQL:

```bash

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=HelpDesk;Uid=root;Pwd=SUA_SENHA;"
```

> Substitua `SUA_SENHA` pela senha do seu MySQL local.

---

### 3. Criar e Atualizar o banco de dados

Para gerar uma nova migração antes de atualizar (esse comando irá gerar a pasta `Migrations` no seu projeto):
```bash

dotnet ef migrations add UpdateModel
```

Para aplicar as migrações e criar o banco efetivamente (mesmo que o banco de dados esteja vazio ou ainda não exista, este comando é necessário):
```bash
dotnet ef database update
```

Isso cria automaticamente o banco `HelpDesk` com as tabelas:
- `Clientes`
- `Tecnicos`
- `Chamados`
- `Departamentos`

---

### 4. Rodar a aplicação WPF

```bash

dotnet run
```

Ou abra `WpfApp1.sln` no **Visual Studio 2022** e pressione **F5**.

---

## 📌 Resumo dos comandos (ordem correta)

```bash



# 2. Configurar senha do MySQL
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=HelpDesk;Uid=root;Pwd=SENHA;"

# 3. Criar migração (gera a pasta Migrations no projeto) e atualizar banco
dotnet ef migrations add UpdateModel
dotnet ef database update

# 4. Rodar o WPF

dotnet run
```
