FROM maven:3.8.3-jdk-11-slim AS builder

COPY pom.xml /tmp/
RUN mvn -B dependency:go-offline -f /tmp/pom.xml -s /usr/share/maven/ref/settings-docker.xml
COPY src /tmp/src/
WORKDIR /tmp/
RUN mvn -B -s /usr/share/maven/ref/settings-docker.xml package

FROM adoptopenjdk/openjdk11:alpine-jre
RUN mkdir -p /app

EXPOSE 11000

COPY --from=builder /tmp/target/*.jar /app/app.jar

ENTRYPOINT exec java $JAVA_OPTS -jar /app/app.jar
