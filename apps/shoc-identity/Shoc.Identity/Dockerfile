FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11106
EXPOSE 11107
EXPOSE 11110
EXPOSE 11111

ENTRYPOINT ["dotnet", "./Shoc.Identity.dll"]
