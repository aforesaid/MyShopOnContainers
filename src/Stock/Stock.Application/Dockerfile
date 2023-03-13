FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Stock/MyShopOnContainers.Stock/MyShopOnContainers.Stock.csproj", "Stock/MyShopOnContainers.Stock/"]
RUN dotnet restore "Stock/MyShopOnContainers.Stock/MyShopOnContainers.Stock.csproj"
COPY . .
WORKDIR "/src/Stock/MyShopOnContainers.Stock"
RUN dotnet build "MyShopOnContainers.Stock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyShopOnContainers.Stock.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyShopOnContainers.Stock.dll"]
