CLS

REM use this .bat file to test your auto-creation of the sql server db and its tables

docker stop SWORD100OrderServiceDB
docker rm SWORD100OrderServiceDB
docker rmi sword100orderservicedb:1

docker build -f sqlserver.dockerfile -t sword100orderservicedb:1 .
docker run --name SWORD100OrderServiceDB -p 1433:1433 --net netSEN300 -d sword100orderservicedb:1
