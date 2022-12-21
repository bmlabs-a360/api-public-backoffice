#docker build -t api-public-backoffice .
#docker run --rm -d  -p 3002:80 api-public-backoffice

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /app
ARG PROJECT=./api-public-backoffice.csproj
COPY $PROJECT ./
RUN dotnet restore $PROJECT
COPY . ./
RUN dotnet publish $PROJECT -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim 
WORKDIR /app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "api-public-backoffice.dll"]	