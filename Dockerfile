FROM microsoft/dotnet:1.1-sdk AS build

WORKDIR /app
COPY . /app

RUN dotnet restore
RUN dotnet publish src/Facility.GeneratorApi.WebApi -c Release -o /app/out

FROM microsoft/dotnet:1.1-runtime

WORKDIR /app
COPY --from=build /app/out /app/out

EXPOSE 45054
ENTRYPOINT ["dotnet", "out/Facility.GeneratorApi.WebApi.dll"]
