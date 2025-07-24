FROM mcr.microsoft.com/mssql/server:2019-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=abc123!!@

COPY ./sqlserverentrypoint.sh /.
COPY ./sqlserversetup.sql /.

#ADD sqlserversetup.sql /docker-entrypoint-initdb.d/
ENTRYPOINT [ "/bin/bash", "sqlserverentrypoint.sh" ]

CMD [ "/opt/mssql/bin/sqlservr" ]