FROM mysql:8.0.42-debian

COPY cnf/all.cnf /etc/mysql/conf.d/ 

COPY init/000_shoc-database/* /docker-entrypoint-initdb.d/

EXPOSE 11001

ENTRYPOINT ["docker-entrypoint.sh"]
CMD ["mysqld"]