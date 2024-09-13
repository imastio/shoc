FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11130
EXPOSE 11131
EXPOSE 11132
EXPOSE 11133

ENTRYPOINT ["dotnet", "./Shoc.Registry.dll"]
