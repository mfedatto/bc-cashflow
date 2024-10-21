ARG DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION}

WORKDIR /app

RUN apt update && \
    apt install -y wget tar

ENV EVOLVE_VERSION="3.2.0"

RUN wget https://github.com/lecaillon/Evolve/releases/download/${EVOLVE_VERSION}/evolve_${EVOLVE_VERSION}_Linux-64bit.tar.gz -O evolve.tar.gz && \
    tar -xzf evolve.tar.gz -C /app && \
    rm evolve.tar.gz

COPY . /app/
RUN find /app -name "*.sh" -exec sed -i 's/\r$//' {} \;
RUN chmod +x /app/*

RUN rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["/app/entrypoint.sh"]
