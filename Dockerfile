FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/LuzVital.Pontos.Api -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
# Dentro do container, "localhost" é o próprio container; a API principal roda na máquina.
ENV ASPNETCORE_HTTP_PORTS=5132 \
    ApiAlertas__Url=http://host.docker.internal:5131
EXPOSE 5132
ENTRYPOINT ["dotnet", "LuzVital.Pontos.Api.dll"]
