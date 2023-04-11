FROM nginx:stable-alpine

COPY ./build /usr/share/nginx/html
COPY cnf/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 11014
CMD ["nginx", "-g", "daemon off;"]