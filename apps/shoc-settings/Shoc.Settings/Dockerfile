FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11116
EXPOSE 11117
EXPOSE 11118
EXPOSE 11119

ENTRYPOINT ["dotnet", "./Shoc.Settings.dll"]
