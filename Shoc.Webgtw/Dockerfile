FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
COPY ./bin/Release/net6.0/publish/ ./
EXPOSE 11002
ENTRYPOINT ["dotnet", "Shoc.Webgtw.dll"]
