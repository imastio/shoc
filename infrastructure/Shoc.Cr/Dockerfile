FROM distribution/distribution:edge

# Install curl using apk
RUN apk add --no-cache curl

# Create directory for JWKs and certificates
RUN mkdir -p /etc/certs/jwks

# Copy custom config file
COPY config.yml /etc/docker/registry/config.yml

# Copy the entrypoint script
COPY entrypoint.sh /usr/local/bin/entrypoint.sh

# Make sure the script is executable
RUN chmod +x /usr/local/bin/entrypoint.sh

EXPOSE 11004

# Set the custom entrypoint
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]