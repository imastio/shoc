FROM mcr.microsoft.com/dotnet/aspnet:8.0

USER root

# 1. Install necessary tools (Podman, Buildah, uidmap)
RUN apt-get update && apt-get install -y \
    buildah skopeo uidmap slirp4netns \
    && rm -rf /var/lib/apt/lists/*

# 3. Ensure correct permissions for newuidmap and newgidmap
RUN chmod u+s /usr/bin/newuidmap && chmod u+s /usr/bin/newgidmap

USER $APP_UID
WORKDIR /app
COPY ./bin/Release/net8.0/publish/ ./ 

EXPOSE 11150
EXPOSE 11151
EXPOSE 11152
EXPOSE 11153

ENTRYPOINT ["dotnet", "./Shoc.Package.dll"]
