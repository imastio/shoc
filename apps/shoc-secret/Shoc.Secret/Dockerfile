FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11144
EXPOSE 11145
EXPOSE 11146
EXPOSE 11147

ENTRYPOINT ["dotnet", "./Shoc.Secret.dll"]
