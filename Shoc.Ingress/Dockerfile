FROM nginx:1.21.1-alpine

RUN mkdir /certs

COPY config/nginx.conf /etc/nginx/conf.d/default.conf
COPY config/ssl.conf /etc/nginx/ssl.conf
COPY config/common.conf /etc/nginx/common.conf
COPY config/common-location.conf /etc/nginx/common-location.conf


ADD certs/localhost.key /certs/domain.key
ADD certs/localhost.crt /certs/domain.crt

EXPOSE 11000
CMD ["nginx", "-g", "daemon off;"]