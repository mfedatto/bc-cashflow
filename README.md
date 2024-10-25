# BC Cash Flow

## Tabela de Conteúdo

- [Descrição](#descrição)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Requisitos para Execução](#requisitos-para-execução)
- [Como Rodar o Projeto Localmente](#como-rodar-o-projeto-localmente)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Teste de carga](#teste-de-carga)
  - [Relatórios de execução de teste de carga](#relatórios-de-execução-de-teste-de-carga)
    - [Run 001](#run-001)
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

O BC Cash Flow é um sistema desenvolvido em C# para o controle de fluxo de caixa |  permitindo a gestão de lançamentos
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
- JMeter

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
- `/db/`: Database migration.
- `/tests/`: Planos de teste.

## Teste de carga

O teste de carga foi criado usando JMeter e as definições estão no arquivo `/tests/load-test.jmx`.

### Relatórios de execução de teste de carga

#### Run 001

Ambiente novo, construído totalmente pelo `docker compose up -d`.

##### Host

- Intel Core i7-9700KF CPU @ 3.60/4.90GHz (stock) 8 cores
- 64,0 GB RAM
- Windows 11 amd64

##### Containers

| Component | Tecnology            | Hostname             | CPUs  | RAM (MB) |
| :-------- | :------------------- | :------------------- | ----: | -------: |
| Web UI    | ASP.Net 8            | `cashflow-web`       | `1.0` |    `256` |
| Scheduler | ASP.Net 8 + Hangfire | `cashflow-scheduler` | `0.2` |    `128` |
| Database  | SQL Server           | `cashflow-db`        | `1.0` |   `1536` |
| Cache     | KeyDB (Redis)        | `cashflow-cache`     | `0.6` |     `64` |
| Queue     | RabbitMQ             | `cashflow-queue`     | `0.2` |    `128` |

##### Results

| Label                       | # Samples | Average | Min  | Max    | Std. Dev. | Error %  | Throughput | Received KB/sec | Sent KB/sec | Avg. Bytes |
| :-------------------------- | --------: | ------: | ---: | -----: | --------: | -------: | ---------: | --------------: | ----------: | ---------: |
| `GET /`                     |   `11108` |     `2` |  `0` |  `818` |   `10.93` | `0.000%` |  `5.88675` |         `17.95` |      `0.68` |   `3123.0` |
| `GET /Users`                |   `11108` |    `10` |  `0` | `1209` |   `21.05` | `0.000%` |  `5.88675` |         `20.33` |      `0.71` |   `3536.0` |
| `GET /Users/Details/1`      |   `11106` |     `6` |  `0` |  `919` |   `14.34` | `0.000%` |  `5.88598` |         `20.01` |      `0.77` |   `3481.0` |
| `GET /Accounts`             |   `11106` |     `9` |  `1` |  `919` |   `20.24` | `0.000%` |  `5.88596` |         `22.87` |      `0.73` |   `3978.6` |
| `GET /Transactions`         |   `11106` |    `67` |  `3` | `1260` |   `50.08` | `0.000%` |  `5.88564` |         `96.79` |      `0.75` |  `16840.2` |
| `POST /Transactions/Create` |   `11100` |   `103` | `20` | `1261` |   `83.42` | `0.000%` |  `5.88267` |         `97.50` |      `2.37` |  `16971.9` |
| TOTAL                       |   `66634` |    `33` |  `0` | `1261` |   `57.06` | `0.000%` | `35.31212` |        `275.44` |      `6.02` |   `7987.4` |

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
- [`ADR014` - Dockerfile](docs/adr/adr014-dockerfile.md)

## Licença

Este projeto é licenciado sob a [MIT License](LICENSE).
