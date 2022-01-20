set -e

mysql --protocol=socket -uroot -p$MYSQL_ROOT_PASSWORD <<EOSQL
CREATE USER '$SHOC_USERNAME'@'%' IDENTIFIED WITH caching_sha2_password BY '$SHOC_PASSWORD';
GRANT ALL PRIVILEGES ON shoc.* TO '$SHOC_USERNAME'@'%';
EOSQL
