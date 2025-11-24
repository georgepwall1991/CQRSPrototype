FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CQRSSolution.OutboxProcessor.AzureFunctions/CQRSSolution.OutboxProcessor.AzureFunctions.csproj", "CQRSSolution.OutboxProcessor.AzureFunctions/"]
COPY ["CQRSSolution.Application/CQRSSolution.Application.csproj", "CQRSSolution.Application/"]
COPY ["CQRSSolution.Domain/CQRSSolution.Domain.csproj", "CQRSSolution.Domain/"]
COPY ["CQRSSolution.Infrastructure/CQRSSolution.Infrastructure.csproj", "CQRSSolution.Infrastructure/"]
RUN dotnet restore "CQRSSolution.OutboxProcessor.AzureFunctions/CQRSSolution.OutboxProcessor.AzureFunctions.csproj"
COPY . .
WORKDIR "/src/CQRSSolution.OutboxProcessor.AzureFunctions"
RUN dotnet build "CQRSSolution.OutboxProcessor.AzureFunctions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CQRSSolution.OutboxProcessor.AzureFunctions.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
