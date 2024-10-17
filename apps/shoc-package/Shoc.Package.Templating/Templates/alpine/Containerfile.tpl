# Use the base image "alpine" with a configurable tag (default to latest)
FROM docker.io/library/alpine:{{ tag ?? "latest" }}

# Define environment variables for user and group IDs with a default value for appUid
ENV SHOC_UID={{ shocUid ?? 1234 }}

# Create a non-root user with a specific UID and GID
RUN addgroup \
        --gid=$SHOC_UID \
        shoc \
    && adduser \
        --uid=$SHOC_UID \
        --ingroup=shoc \
        --disabled-password \
        shoc

# Set the working directory and ensure it has the right permissions
WORKDIR /app
RUN chown -R shoc:shoc /app

# Copy all files into the /app directory
COPY . /app

# Optionally install additional packages if any are provided in the template
{{ if packages && packages.size > 0 }}
RUN apk add --no-cache {{ for package in packages }} {{ package }} {{ end }}
{{ end }}

# Ensure entrypoint is run from the shoc user
USER $SHOC_UID

# Set the entrypoint for the container as an array (required)
ENTRYPOINT [{{ for cmd in entrypoint }} "{{ cmd }}"{{ if for.last == false }}, {{ end }}{{ end }}]
