FROM adoptopenjdk/openjdk11:alpine-jre
RUN mkdir -p /opt/imast/builder

EXPOSE 11002

COPY target/builder.jar /opt/imast/builder

ENTRYPOINT exec java $JAVA_OPTS -jar /opt/imast/builder/builder.jar

