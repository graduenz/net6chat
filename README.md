Net6Chat
---

This is a simple multi-room chat application built with .NET 6 with a web app and two consumers.

Tools used:

- .NET 6 and ASP.NET
- ASP.NET Identity
- SQL Server
- SignalR
- CAP (with RabbitMQ)
- Docker

## Running the project

Run the following commands in this repository's root:

1. Build the Docker images
    - `docker-compose build`
2. Run only SQL Server before other applications
    - `docker-compose up -d sqlserver`
3. Run the project's EF migrations
    - `dotnet ef database update --project .\Net6Chat.Migrations\ --startup-project .\Net6Chat.WebApp\`
4. Run all remaining applications
    - `docker-compose up -d`

## Entity Framework

If you do not have the global EF tools installed, please run the following command:

`dotnet tool install --global dotnet-ef`

## Stock Bot

Once chat applications are running and you are in a chat room, you can send the following command as a message to check for a stock's price:

`/stock=AAPL.US`

Change `AAPL.US` to the stock you want.

## More information

RabbitMQ takes a few seconds to start, so the stock bot and persistence consumers might not work in the first two minutes after the containers are running.