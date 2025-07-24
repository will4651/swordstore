FROM maven:3.9-sapmachine-17 AS build
COPY src /home/app/src
COPY pom.xml /home/app
RUN mvn -f /home/app/pom.xml clean package

#FROM tomcat:8-jdk8-openjdk
FROM tomcat:11.0.0-jdk17
COPY --from=build /home/app/target/SWORD100ServiceAPI.war /usr/local/tomcat/webapps/ROOT.war

ENV SERVER_PORT=8080
ENV MONGO_HOST=SWORD100ServiceDB
ENV MONGO_PORT=27017
ENV MONGO_DB=SwordService
ENV EUREKA_HOST=SWORD100EurekaRegistry

# docker build -t sword100serviceapi:1 .
# docker run -d -p 8081:8080 --name SWORD100ServiceAPI --net netSEN300 sword100serviceapi:1