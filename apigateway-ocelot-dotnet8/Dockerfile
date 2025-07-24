FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app3

COPY ./ ./
RUN dotnet restore

RUN dotnet publish -c Release -o outBuildFolder

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app4

COPY --from=build-env /app3/outBuildFolder/ .

ENTRYPOINT ["dotnet", "apigateway-ocelot-dotnet8.dll"]
