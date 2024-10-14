#!/bin/sh

# Save the current PATH variable
SAVED_PATH="$PATH"

# Clear all environment variables
env -i PATH="$SAVED_PATH" buildah "$@"