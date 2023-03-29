FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Stock/Stock.Application/Stock.Application.csproj", "Stock/Stock.Application/"]
RUN dotnet restore "Stock/Stock.Application/Stock.Application.csproj"
COPY . .
WORKDIR "/src/Stock/Stock.Application"
RUN dotnet build "Stock.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stock.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stock.Application.dll"]
