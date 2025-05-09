# LoginManager

Este projeto é uma API Web Minimal chamada **LoginManager**, desenvolvida com ASP.NET Core 9. Ele implementa autenticação utilizando JWT (JSON Web Tokens) e gerenciamento de usuários com o ASP.NET Core Identity. A estrutura do projeto é organizada em uma API mínima, com separação dos endpoints em pastas, garantindo uma arquitetura limpa e fácil de manter.

## Funcionalidades

- **Autenticação**: Implementação de autenticação via JWT para garantir a segurança das requisições.
- **Autorização**: Controle de acesso baseado em roles, permitindo diferentes níveis de permissão para usuários.
- **Customização do Identity**: A classe `IdentityUser` foi personalizada para incluir campos adicionais que fazem sentido para o negócio.

## Estrutura do Projeto

- **Endpoints**: Organizados em pastas para melhor separação e organização.
- **Models**: Definições dos modelos de dados, incluindo a classe `CustomUser` que herda de `IdentityUser`.
- **Services**: Implementação de serviços que encapsulam a lógica de negócios, incluindo a geração de tokens JWT.
- **Configuration**: Configurações necessárias para a aplicação, incluindo as definições de autenticação.

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server com LocalDB para persistência de dados.

## Como Executar o Projeto

1. Clone o repositório:
   ```bash
   git clone <URL-do-repositorio>
   cd LoginManager
