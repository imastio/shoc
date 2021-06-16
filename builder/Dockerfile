FROM adoptopenjdk/openjdk11:alpine-jre
RUN mkdir -p /opt/imast/scheduler

EXPOSE 8801

COPY target/scheduler.jar /opt/imast/scheduler

ENTRYPOINT exec java $JAVA_OPTS -jar /opt/imast/scheduler/scheduler.jar

