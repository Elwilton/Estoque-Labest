FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Rox.Domain/Rox.Domain.csproj src/Rox.Domain/
COPY src/Rox.Application/Rox.Application.csproj src/Rox.Application/
COPY src/Rox.Infrastructure/Rox.Infrastructure.csproj src/Rox.Infrastructure/
COPY src/Rox.Api/Rox.Api.csproj src/Rox.Api/
RUN dotnet restore src/Rox.Api/Rox.Api.csproj

COPY . .
RUN dotnet publish src/Rox.Api/Rox.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Rox.Api.dll"]
