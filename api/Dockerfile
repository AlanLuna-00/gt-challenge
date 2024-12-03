FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["api/api.csproj", "./"]
RUN dotnet restore "api.csproj"

COPY ./api ./
RUN dotnet build "api.csproj" -c Release -o /app/build

RUN dotnet publish "api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Docker

EXPOSE 8080

ENTRYPOINT ["dotnet", "api.dll"]
