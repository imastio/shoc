FROM adoptopenjdk/openjdk11:alpine-jre
RUN mkdir -p /opt/imast/webgtw

EXPOSE 11000

COPY target/webgtw.jar /opt/imast/webgtw

ENTRYPOINT exec java $JAVA_OPTS -jar /opt/imast/webgtw/webgtw.jar

