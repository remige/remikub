FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.11 AS build-back
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./remikub ./
RUN dotnet restore && dotnet publish -c Release -o out

# Build Front image
FROM node:12.16.2-alpine3.9 AS build-front 
WORKDIR /app.front
COPY ./remikub.front/ .
RUN yarn install && yarn build

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.11
WORKDIR /app
COPY --from=build-back /app/out .
COPY --from=build-front /app.front/dist ./wwwroot
ENTRYPOINT ["dotnet", "remikub.dll"]
