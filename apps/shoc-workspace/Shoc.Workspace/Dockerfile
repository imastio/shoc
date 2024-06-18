FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 
EXPOSE 11020
ENTRYPOINT ["dotnet", "./Shoc.Workspace.dll"]
