#!/bin/sh

# Config file path
CONFIG_FILE="/etc/docker/registry/config.yml"

# Paths
JWK_FILE="/etc/certs/jwks/jwks.json"

# Try to get realm from environment variable first, fall back to config file
if [ -n "$AUTH_TOKEN_REALM" ]; then
    REALM_URL="$AUTH_TOKEN_REALM"
else
    # Extract the realm URL from the config file
    REALM_URL=$(grep 'realm:' "$CONFIG_FILE" | awk '{print $2}' | tr -d '"')
fi

# Deduce the base URL from the realm
BASE_URL=$(echo "$REALM_URL" | sed 's|/token||')

# Define the JWKs URL based on the deduced base URL
JWK_URL="$BASE_URL/jwks"

# Download the JWKs and store it in the well-known location, allow untrusted HTTPS
echo "Downloading JWKs from $JWK_URL ..."
curl -k -f -o "$JWK_FILE" "$JWK_URL"

cat $JWK_FILE

# Check if the download was successful
if [ $? -ne 0 ]; then
  echo "Failed to download JWKs. Exiting."
  exit 1
fi

# Start the Docker registry with the specified configuration
registry serve /etc/docker/registry/config.yml
