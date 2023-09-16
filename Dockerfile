FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV NODE_MAJOR=18

# Install Node as the "dotnet build" step needs to install third-party dependencies using npm
RUN apt-get update
RUN apt-get install -y ca-certificates curl gnupg
RUN mkdir -p /etc/apt/keyrings
RUN curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg
RUN echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list
RUN apt-get update
RUN apt-get install nodejs -y

WORKDIR /src

COPY DomainStatusReport.csproj .

RUN dotnet restore "DomainStatusReport.csproj"

COPY . .

RUN dotnet build "DomainStatusReport.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "DomainStatusReport.csproj" -c Release -o /app/publish

FROM base AS production

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "DomainStatusReport.dll" ]
