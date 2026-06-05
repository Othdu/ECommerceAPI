FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ECommerceAPI/ECommerceAPI.csproj", "ECommerceAPI/"]
RUN dotnet restore "ECommerceAPI/ECommerceAPI.csproj"
COPY . .
WORKDIR "/src/ECommerceAPI"
RUN dotnet build "ECommerceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceAPI.dll"]