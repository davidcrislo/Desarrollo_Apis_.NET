FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/ApiCuentas.Domain/*.csproj src/ApiCuentas.Domain/
COPY src/ApiCuentas.Application/*.csproj src/ApiCuentas.Application/
COPY src/ApiCuentas.Infrastructure/*.csproj src/ApiCuentas.Infrastructure/
COPY src/ApiCuentas.Api/*.csproj src/ApiCuentas.Api/
RUN dotnet restore src/ApiCuentas.Api/ApiCuentas.Api.csproj

COPY src/ src/
RUN dotnet publish src/ApiCuentas.Api/ApiCuentas.Api.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApiCuentas.Api.dll"]