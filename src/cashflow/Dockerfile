FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet build --no-restore

RUN dotnet test --no-restore --verbosity normal

FROM build AS publish
RUN dotnet publish Bc.CashFlow.Web -c Release -o /app/web --no-restore
RUN dotnet publish Bc.CashFlow.Scheduler -c Release -o /app/scheduler --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/web ./web
COPY --from=publish /app/scheduler ./scheduler

ARG WEB_PORT=80
ARG SCHEDULER_PORT=5000

ENV WEB_PORT=${WEB_PORT}
ENV SCHEDULER_PORT=${SCHEDULER_PORT}

EXPOSE ${WEB_PORT}
EXPOSE ${SCHEDULER_PORT}

COPY entrypoint.sh ./entrypoint.sh
RUN sed -i 's/\r$//' ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
