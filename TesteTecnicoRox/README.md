# Labest Estoque

Sistema de cadastro de produtos e gestão de estoque, desenvolvido como teste técnico para a vaga de Desenvolvedor .NET na **Labest**.

Permite cadastro/autenticação de usuários, cadastro e listagem de produtos, e registro/listagem de movimentações de estoque (entradas e saídas), com o saldo do estoque sendo recalculado automaticamente a cada movimentação.

## Visão geral

| | |
|---|---|
| **Back-end** | C# / .NET 10 (ASP.NET Core Web API) |
| **Front-end** | Vue 3 + Vite |
| **Banco de dados** | SQL Server (via Entity Framework Core) |
| **Autenticação** | ASP.NET Core Identity + JWT |
| **Containers** | Docker / Docker Compose |

## Funcionalidades

- Cadastro e login de usuários (ASP.NET Core Identity + JWT)
- Cadastro, listagem, edição e remoção de produtos
- Registro de movimentações de estoque (entrada/saída), com edição e remoção
- Atualização automática do saldo de estoque a cada movimentação, com validações de domínio (ex: impedir saída maior que o estoque disponível)
- Tela inicial com visão geral do sistema

## Screenshots

### Login
![Tela de login](docs/screenshots/login.png)

### Início
![Tela inicial](docs/screenshots/inicio.png)

### Produtos
![Listagem de produtos](docs/screenshots/produtos.png)

### Cadastro de produto
![Novo produto](docs/screenshots/novo-produto.png)

### Movimentações de estoque
![Listagem de movimentações](docs/screenshots/movimentacoes.png)

### Nova movimentação
![Nova movimentação](docs/screenshots/nova-movimentacao.png)

## Arquitetura

O back-end segue uma arquitetura em camadas:

```
backend/
└── src/
    ├── Rox.Domain          # Entidades e regras de negócio (Produto, MovimentacaoEstoque)
    ├── Rox.Application     # Casos de uso, DTOs, validações (FluentValidation)
    ├── Rox.Infrastructure  # EF Core, Identity, repositórios, JWT
    └── Rox.Api             # Controllers, configuração da API, Swagger
```

O front-end é uma SPA em Vue 3 (Composition API), com Vue Router, Pinia e Axios:

```
frontend/
└── src/
    ├── views/       # Páginas (Login, Cadastro, Produtos, Movimentações, etc.)
    ├── components/  # Componentes reutilizáveis (ex: ConfirmDialog)
    ├── services/     # Camada de chamadas HTTP à API
    ├── stores/       # Estado global (autenticação) com Pinia
    ├── router/       # Rotas e guarda de autenticação
    └── utils/        # Funções utilitárias (formatação de data, etc.)
```

## Como executar

### Opção 1 — Docker (recomendado)

**Pré-requisitos:** [Docker](https://www.docker.com/products/docker-desktop/) e Docker Compose instalados (já incluídos no Docker Desktop).

**1. Clone o repositório:**
```bash
git clone https://github.com/Elwilton/Estoque-Labest.git
cd Estoque-Labest
```

**2. Suba toda a aplicação com um único comando:**
```bash
docker compose up --build
```

Isso cria 3 containers isolados, sem afetar nada já instalado na sua máquina:

| Serviço | Descrição | URL |
|---|---|---|
| `sqlserver` | Banco de dados SQL Server, com volume próprio | porta `1433` |
| `api` | Back-end .NET, aplica as migrations automaticamente no startup | http://localhost:5000 (Swagger em `/swagger`) |
| `frontend` | Front-end Vue buildado e servido via Nginx | http://localhost:8081 |

Aguarde até o terminal mostrar que os 3 containers (`rox-sqlserver`, `rox-api`, `rox-frontend`) estão de pé — o SQL Server precisa ficar "healthy" antes da API iniciar, então a primeira subida pode levar 30-60 segundos.

**3. Acesse a aplicação:**

Abra [http://localhost:8081](http://localhost:8081) no navegador, clique em "Cadastre-se" e crie um usuário para começar a usar o sistema.

**4. Para parar a aplicação:**
```bash
docker compose down
```

**5. Para parar e apagar também os dados do banco** (próxima subida começa do zero):
```bash
docker compose down -v
```

> Se as portas 5000, 8081 ou 1433 já estiverem em uso na sua máquina, edite o arquivo `docker-compose.yml` e ajuste o lado esquerdo do mapeamento de portas (ex: `"5050:8080"`).

### Opção 2 — Localmente

**Back-end:**
```bash
cd backend/src/Rox.Api
dotnet run
```
A API sobe em `http://localhost:5000`.

**Front-end:**
```bash
cd frontend
npm install
npm run dev
```
O front sobe em `http://localhost:5173`.

> A primeira execução da API aplica as migrations do Entity Framework automaticamente, criando o banco de dados.

## Stack utilizada

- **.NET 10** / ASP.NET Core Web API
- **Entity Framework Core** (SQL Server)
- **ASP.NET Core Identity** + **JWT Bearer**
- **FluentValidation**
- **Vue 3** (Composition API) + **Vite**
- **Vue Router** + **Pinia** + **Axios**
- **Docker** / **Docker Compose**
