FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CollectiveAPI/CollectiveAPI.csproj", "CollectiveAPI/"]
COPY ["CollectiveData/CollectiveData.csproj", "CollectiveData/"]
RUN dotnet restore "CollectiveAPI/CollectiveAPI.csproj"
COPY . .
WORKDIR "/src/CollectiveAPI"
RUN dotnet build "CollectiveAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CollectiveAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CollectiveAPI.dll"]
