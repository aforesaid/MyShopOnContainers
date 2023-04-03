FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Shop/Shop.Application/Shop.Application.csproj", "Shop/Shop.Application/"]
RUN dotnet restore "Shop/Shop.Application/Shop.Application.csproj"
COPY . .
WORKDIR "/src/Shop/Shop.Application"
RUN dotnet build "Shop.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shop.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop.Application.dll"]
