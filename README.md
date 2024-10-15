# BC Cash Flow

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

## Requisitos para Execução

- .NET SDK 8.0 ou superior
- Banco de dados SQL Server

## Como Rodar o Projeto Localmente

1. Clone o repositório:

```bash
git clone https://github.com/mfedatto/bc-cashflow.git
```

2. Navegue até o diretório do projeto:

```bash
cd bc-cashflow
```

3. Acesse o diretório da aplicação:

```bash
cd src/cashflow/CashFlow.WebUI
```

4. Restaure as dependências:

```bash
dotnet restore
```

5. Configure o banco de dados no arquivo de configuração `appsettings.json`.
6. Execute as migrações para configurar o banco de dados:

```bash
dotnet ef database update
```

7. Rode a aplicação (no diretório do .csproj):

```bash
dotnet run
```

## Estrutura do Projeto

`/src/`: Código fonte do sistema.
`/tests/`: Testes automatizados.
`/docs/`: Documentação técnica do projeto.

## Licença

Este projeto é licenciado sob a [MIT License](LICENSE).
