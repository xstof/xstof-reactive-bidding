FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

# install node.js on docker image:
# see: https://github.com/nodesource/distributions/blob/master/README.md#debinstall
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get update & apt-get install -y nodejs

WORKDIR /src
COPY ["BiddingWeb.csproj", "."]
RUN dotnet restore "BiddingWeb.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "BiddingWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BiddingWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BiddingWeb.dll"]
