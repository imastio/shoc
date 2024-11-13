FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11156
EXPOSE 11157
EXPOSE 11158
EXPOSE 11159

ENTRYPOINT ["dotnet", "./Shoc.Job.dll"]
