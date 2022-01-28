FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app
COPY . /app

RUN dotnet restore
RUN dotnet publish src/Facility.GeneratorApi.WebApi -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/out /app/out

EXPOSE 45054
ENTRYPOINT ["dotnet", "out/Facility.GeneratorApi.WebApi.dll"]
