#FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

#FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "api-public-backoffice.dll"]	
