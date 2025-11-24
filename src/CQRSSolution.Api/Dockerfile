FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CQRSSolution.Api/CQRSSolution.Api.csproj", "CQRSSolution.Api/"]
COPY ["CQRSSolution.Application/CQRSSolution.Application.csproj", "CQRSSolution.Application/"]
COPY ["CQRSSolution.Domain/CQRSSolution.Domain.csproj", "CQRSSolution.Domain/"]
COPY ["CQRSSolution.Infrastructure/CQRSSolution.Infrastructure.csproj", "CQRSSolution.Infrastructure/"]
RUN dotnet restore "CQRSSolution.Api/CQRSSolution.Api.csproj"
COPY . .
WORKDIR "/src/CQRSSolution.Api"
RUN dotnet build "CQRSSolution.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CQRSSolution.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CQRSSolution.Api.dll"]
