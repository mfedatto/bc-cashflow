# BC Cash Flow

## Tabela de Conteúdo

- [Descrição](#descrição)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Requisitos para Execução](#requisitos-para-execução)
- [Como Rodar o Projeto Localmente](#como-rodar-o-projeto-localmente)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Diagrama de Implantação](#diagrama-de-implantação)
- [Modelo Entidade-Relacional](#modelo-entidade-relacional)
- [Fluxo de Construção do Ambiente](#fluxo-de-construção-do-ambiente)
- [Registros de Decisão Arquitetural](#registros-de-decisão-arquitetural)
  - [ADR001 - Interface](docs/adr/adr001-interface.md)
  - [ADR002 - Arquitetura](docs/adr/adr002-arquitetura.md)
  - [ADR003 - Banco de dados](docs/adr/adr003-banco-de-dados.md)
  - [ADR004 - Banco de dados](docs/adr/adr004-banco-de-dados.md)
  - [ADR005 - Evolve DB Docker](docs/adr/adr005-evolve-db-docker.md)
  - [ADR006 - Database Setup](docs/adr/adr006-database-setup.md)
  - [ADR007 - Migration tiers](docs/adr/adr007-migration-tiers.md)
  - [ADR008 - Cross Cutting](docs/adr/adr008-cross-cutting.md)
  - [ADR009 - Localization](docs/adr/adr009-localization.md)
  - [ADR010 - Business](docs/adr/adr010-business.md)
  - [ADR011 - Startup context builder](docs/adr/adr011-startup-context-builder.md)
  - [ADR012 - Hangfire antiforgery token](docs/adr/adr012-hangfire-antiforgery-token.md)
  - [ADR013 - Setup](docs/adr/adr013-setup.md)
- [Licença](#licença)

## Descrição

O BC Cash Flow é um sistema desenvolvido em C# para o controle de fluxo de caixa, permitindo a gestão de lançamentos
diários de débitos e créditos, além da consolidação do saldo diário. O sistema é projetado com foco em escalabilidade,
resiliência e segurança, garantindo alta disponibilidade e desempenho.

## Funcionalidades

- Registro de Lançamentos: Adicione lançamentos de débito e crédito com descrição, valor e data.
- Consolidação de Saldo Diário: Gere um relatório consolidado do saldo diário.
- Resiliência: O sistema de lançamentos opera de forma independente, mesmo com falhas no serviço de consolidação.
- Escalabilidade: Suporte para alta demanda de requisições nos momentos de pico.

## Tecnologias Utilizadas

- C#
- .NET Core
- SQL Server (ou outro banco de dados relacional)
- RabbitMQ
- Redis
- Hangfire
- Evolve

## Requisitos para Execução

- Docker Compose

## Como Rodar o Projeto Localmente

1. Clone o repositório:

```bash
git clone https://github.com/mfedatto/bc-cashflow.git
```

2. Navegue até o diretório de infra como código:

```bash
cd iac
```

3. Suba a composição via docker compose:

```bash
docker compose up -d
```

## Estrutura do Projeto

- `/docs/`: Documentação técnica do projeto.
- `/src/`: Código fonte do sistema.
- `/iac/`: Infra como código.
- `/db/`: Databse migration.

## Diagrama de implantação

![Deploymetn Diagram](docs/dd.png)

## Modelo Entidade-Relacional

![Entity-Relationship Model](docs/erm.png)

## Fluxo de construção do ambiente

![Environmetn Build - Workflow Diagram](docs/ebwd.png)

## Registros de decisão arquitetural

- [`ADR001` - Interface](docs/adr/adr001-interface.md)
- [`ADR002` - Arquitetura](docs/adr/adr002-arquitetura.md)
- [`ADR003` - Banco de dados](docs/adr/adr003-banco-de-dados.md)
- [`ADR004` - Banco de dados](docs/adr/adr004-banco-de-dados.md)
- [`ADR005` - Evolve DB Docker](docs/adr/adr005-evolve-db-docker.md)
- [`ADR006` - Database Setup](docs/adr/adr006-database-setup.md)
- [`ADR007` - Migration tiers](docs/adr/adr007-migration-tiers.md)
- [`ADR008` - Cross Cutting](docs/adr/adr008-cross-cutting.md)
- [`ADR009` - Localization](docs/adr/adr009-localization.md)
- [`ADR010` - Business](docs/adr/adr010-business.md)
- [`ADR011` - Startup context builder](docs/adr/adr011-startup-context-builder.md)
- [`ADR012` - Hangfire antiforgery token](docs/adr/adr012-hangfire-antiforgery-token.md)
- [`ADR013` - Setup](docs/adr/adr013-setup.md)

## Licença

Este projeto é licenciado sob a [MIT License](LICENSE).
