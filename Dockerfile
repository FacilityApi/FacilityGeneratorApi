FROM microsoft/dotnet:1.1-sdk

WORKDIR /app
COPY . /app

RUN dotnet restore
RUN dotnet publish src/Facility.GeneratorApi.WebApi -c Release -o /app/out

EXPOSE 45054
ENTRYPOINT ["dotnet", "out/Facility.GeneratorApi.WebApi.dll"]
