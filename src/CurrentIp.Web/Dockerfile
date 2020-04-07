FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CurrentIp.Web/CurrentIp.Web.csproj", "src/CurrentIp.Web/"]
COPY ["src/CurrentIp.DataModel/CurrentIp.DataModel.csproj", "src/CurrentIp.DataModel/"]
COPY ["src/CurrentIp.Storage/CurrentIp.Storage.csproj", "src/CurrentIp.Storage/"]
RUN dotnet restore "src/CurrentIp.Web/CurrentIp.Web.csproj"
COPY . .
WORKDIR "/src/src/CurrentIp.Web"
RUN dotnet build "CurrentIp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrentIp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrentIp.Web.dll"]