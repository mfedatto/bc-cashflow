FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build-env

WORKDIR /app

COPY . ./

RUN dotnet restore
RUN dotnet build --no-restore
RUN dotnet test --no-restore --verbosity normal
RUN dotnet publish Bc.CashFlow.Scheduler -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff

WORKDIR /app

COPY --from=build-env /app/out .

ARG HTTP_PORT=80

ENV HTTP_PORT=${HTTP_PORT}

EXPOSE ${HTTP_PORT}

ENTRYPOINT ["dotnet", "Bc.CashFlow.Scheduler.dll", "--urls", "http://*:${WEB_PORT}"]
