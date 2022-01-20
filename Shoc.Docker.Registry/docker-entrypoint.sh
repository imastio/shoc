#!/bin/sh

set -e

htpasswd -nb -B $IMAST_DOCKER_REGISTRY_USER $IMAST_DOCKER_REGISTRY_PASSWORD > /auth/htpasswd

case "$1" in
    *.yaml|*.yml) set -- registry serve "$@" ;;
    serve|garbage-collect|help|-*) set -- registry "$@" ;;
esac

exec "$@"
