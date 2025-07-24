FROM maven:3.9-sapmachine-17 AS build
COPY src /home/app/src
COPY pom.xml /home/app
RUN mvn -f /home/app/pom.xml clean package

FROM tomcat:11.0.0-jdk17
COPY --from=build /home/app/target/SWORD100CartServiceAPI-0.0.1-SNAPSHOT.war /usr/local/tomcat/webapps/ROOT.war

ENV SERVER_PORT=8080
ENV REDIS_HOST=SWORD100CartServiceDB
ENV REDIS_PORT=6379
ENV EUREKA_HOST=SWORD100EurekaRegistry

#docker build -t sword100cartserviceapi:1 .
#docker run -d -p 8081:8080 --name SWORD100CartServiceAPI --net netSEN300 sword100cartserviceapi:1