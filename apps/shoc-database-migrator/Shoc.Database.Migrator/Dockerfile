FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11112
EXPOSE 11113

ENTRYPOINT ["dotnet", "./Shoc.Database.Migrator.dll"]
