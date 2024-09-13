FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11138
EXPOSE 11139
EXPOSE 11140
EXPOSE 11141

ENTRYPOINT ["dotnet", "./Shoc.Cluster.dll"]
