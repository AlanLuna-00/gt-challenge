FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["client/client.csproj", "./"]
RUN dotnet restore "client.csproj"

COPY ./client ./
RUN dotnet publish "client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .

ENTRYPOINT ["nginx", "-g", "daemon off;"]
