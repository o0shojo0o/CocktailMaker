FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CocktailMaker/CocktailMaker.csproj", "CocktailMaker/"]
RUN dotnet restore "CocktailMaker/CocktailMaker.csproj"
COPY . .
WORKDIR "/src/CocktailMaker"
RUN dotnet build "CocktailMaker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CocktailMaker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/logs
RUN mkdir /app/data
ENTRYPOINT ["dotnet", "CocktailMaker.dll"]