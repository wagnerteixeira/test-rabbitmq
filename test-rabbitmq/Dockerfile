FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["test-rabbitmq/test-rabbitmq.csproj", "test-rabbitmq/"]
RUN dotnet restore "test-rabbitmq/test-rabbitmq.csproj"
COPY . .
WORKDIR "/src/test-rabbitmq"
RUN dotnet build "test-rabbitmq.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test-rabbitmq.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test-rabbitmq.dll"]
