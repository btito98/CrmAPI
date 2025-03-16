# API C# para Sistema de CRM de Marketing

Este repositório contém uma API desenvolvida em C# para gerenciamento de um sistema CRM (Customer Relationship Management) para empresas de marketing.

## Visão Geral

Esta API permite gerenciar:
- Clientes
- Campanhas de marketing
- Leads
- Oportunidades de negócio
- Análise de desempenho

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- PostgreSQL
- Swagger para documentação
- Autenticação JWT
- Keycloak para gerenciamento de autenticação e autorização

## Estrutura do Projeto

- **CRM.API**: Projeto principal da API com controllers e configurações
- **CRM.Domain**: Contém entidades de domínio e interfaces
- **CRM.Infrastructure**: Implementação de persistência e serviços externos
- **CRM.Application**: Camada de serviços de aplicação
- **CRM.Shared**: Camada de recursos compartilhados

## Configuração Inicial

### 1. Clone o Repositório

```bash
git clone https://github.com/btito98/CrmAPI.git
cd CrmAPI
```

### 2. Configure a Connection String

Abra o arquivo `appsettings.json` no projeto CRM.API e altere a connection string conforme suas credenciais:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=SEU_SERVIDOR;Port=5432;Database=CRMdb;Username=SEU_USUARIO;Password=SUA_SENHA;"
  }
}
```

⚠️ **IMPORTANTE**: Substitua SEU_SERVIDOR, SEU_USUARIO e SUA_SENHA pelas credenciais da sua máquina.

### 3. Execute as Migrações

Execute os seguintes comandos para criar o banco de dados:

```bash
dotnet ef migrations add InitialCreate --project CRM.Infrastructure --startup-project CRM.API
dotnet ef database update --project CRM.Infrastructure --startup-project CRM.API
```

### 4. Execute a API

```bash
cd CRM.API
dotnet run
```

A API estará disponível em `https://localhost:7243` e a documentação Swagger em `https://localhost:7243/swagger`.

Autenticação e Autorização
O sistema utiliza Keycloak como provedor de identidade e gerenciador de acesso:

Gerenciamento de Usuários: Criação, atualização e desativação de usuários centralizados no Keycloak
Controle de Acesso: Definição de roles e permissões por recurso
Single Sign-On (SSO): Permite integração com outras aplicações da empresa
Multi-tenancy: Suporte para separação de clientes em diferentes realms
OAuth 2.0/OpenID Connect: Implementação de fluxos de autenticação padrão da indústria

Para acessar endpoints protegidos, é necessário obter um token de acesso do Keycloak e incluí-lo no cabeçalho de autorização das requisições.

