FROM registry:2 

RUN apk --update --no-cache --virtual build-dependencies add apache2-utils

RUN mkdir /auth

ADD config.yml /etc/docker/registry/config.yml 

ADD docker-entrypoint.sh /entrypoint.sh



