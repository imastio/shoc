FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /app
COPY ./bin/Release/net5.0/publish/ ./
EXPOSE 11006
ENTRYPOINT ["dotnet", "Shoc.Builder.dll"]