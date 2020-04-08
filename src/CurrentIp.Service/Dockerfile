FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

ENV CURRENT_IP_SERVICE_TIMEOUT_SECONDS "30"
ENV CURRENT_IP_SERVICE_HOST_URL "app-current-ip-api-prod.azurewebsites.net"
ENV CURRENT_IP_SERVICE_HOST_PORT "80"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CurrentIp.Service/CurrentIp.Service.csproj", "src/CurrentIp.Service/"]
COPY ["src/CurrentIp.ApiClient/CurrentIp.ApiClient.csproj", "src/CurrentIp.ApiClient/"]
COPY ["src/CurrentIp.DataModel/CurrentIp.DataModel.csproj", "src/CurrentIp.DataModel/"]
RUN dotnet restore "src/CurrentIp.Service/CurrentIp.Service.csproj"
COPY . .
WORKDIR "/src/src/CurrentIp.Service"
RUN dotnet build "CurrentIp.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrentIp.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrentIp.Service.dll"]