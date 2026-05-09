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

---

### 2. Configurar a conexão com o banco

Na pasta **raiz do projeto** (`/`), configure a string de conexão com seus dados do MySQL:

```bash

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=HelpDesk;Uid=root;Pwd=SUA_SENHA;"
```

> Substitua `SUA_SENHA` pela senha do seu MySQL local.

---

### 3. Criar o banco de dados

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


# 1. Configurar senha do MySQL
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=HelpDesk;Uid=root;Pwd=SENHA;"

# 2. Criar banco e tabelas
dotnet ef database update

# 3. Rodar o WPF

dotnet run
```
