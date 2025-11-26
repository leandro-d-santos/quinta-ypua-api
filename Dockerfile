# ===========================
# 1. Build stage
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solution e todos os projetos
COPY QuintaYpua.sln .
COPY API/Api.csproj API/
COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY Data/Data.csproj Data/
COPY IoC/IoC.csproj IoC/
COPY Configuration/Configuration.csproj Configuration/

# Restaura dependências
RUN dotnet restore API/Api.csproj

# Copia todo o restante
COPY . .

# Publica somente a API
RUN dotnet publish API/Api.csproj -c Release -o /app/publish

# ===========================
# 2. Runtime stage
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar arquivos publicados
COPY --from=build /app/publish .

# Render define a variável PORT automaticamente
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8066}

EXPOSE 8066

ENTRYPOINT ["dotnet", "Api.dll"]
