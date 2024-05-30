FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 
EXPOSE 11012
ENTRYPOINT ["dotnet", "./Shoc.Database.Migrator.dll"]
