# Use the base image "alpine" with a configurable tag (default to latest)
FROM alpine:{{ tag ?? "latest" }}

# Define environment variables for user and group IDs with a default value for appUid
ENV APP_UID={{ appUid ?? 1234 }}

# Create a non-root user with a specific UID and GID
RUN addgroup \
        --gid=$APP_UID \
        app \
    && adduser \
        --uid=$APP_UID \
        --ingroup=app \
        --disabled-password \
        app

# Set the working directory and ensure it has the right permissions
WORKDIR /app
RUN chown -R app:app /app

# Copy all files into the /app directory
COPY . /app

# Optionally install additional packages if any are provided in the template
{{ if packages && packages.size > 0 }}
RUN apk add --no-cache {{ for package in packages }} {{ package }} {{ end }}
{{ end }}

# Set the entrypoint for the container as an array (required)
ENTRYPOINT [{{ for cmd in entrypoint }} "{{ cmd }}"{{ if for.last == false }}, {{ end }}{{ end }}]
