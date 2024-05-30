# use this link for details
https://blog.tonysneed.com/2019/10/13/enable-ssl-with-asp-net-core-using-nginx-and-docker/

# generate cert keys
openssl req -x509 -nodes -days 4096 -newkey rsa:2048 -keyout localhost.key -out localhost.crt -config localhost.conf -passin pass:password

# generate pfx
openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt