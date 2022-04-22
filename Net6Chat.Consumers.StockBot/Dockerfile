FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Net6Chat.Consumers.StockBot/Net6Chat.Consumers.StockBot.csproj", "Net6Chat.Consumers.StockBot/"]
RUN dotnet restore "Net6Chat.Consumers.StockBot/Net6Chat.Consumers.StockBot.csproj"
COPY . .
WORKDIR "/src/Net6Chat.Consumers.StockBot"
RUN dotnet build "Net6Chat.Consumers.StockBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Net6Chat.Consumers.StockBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Net6Chat.Consumers.StockBot.dll"]