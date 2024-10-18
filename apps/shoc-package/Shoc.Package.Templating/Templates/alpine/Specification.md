This section outlines the expected properties and configuration options for the Alpine base image template. These properties can be defined in your `build.shoc.yaml` file to customize the generated container image.

#### Properties

- **tag** (`string`, optional):
    - The tag of the Alpine base image to use. Defaults to `latest`.
    - Example: `"3.14"`

- **shocUid** (`int`, optional):
    - The user ID (UID) to use when creating the non-root user `shoc` inside the container. Defaults to `1234`.
    - Example: `1000`

- **packages** (`array`, optional):
    - A list of additional packages to install using Alpine's `apk` package manager. Leave empty or omit to install no additional packages.
    - Example: `["curl", "git", "vim"]`

- **entrypoint** (`array`, required):
    - Defines the entrypoint for the container in array format. The entrypoint determines the initial command or script that runs when the container starts.
    - Example: `["/app/start.sh"]`

---

#### Notes

1. The **tag** property allows you to specify a specific version of Alpine (e.g., `3.14`) or use the default `latest`.
2. The **shocUid** ensures that files and processes inside the container are handled by a non-root user, providing better security.
3. The **packages** property leverages Alpine's lightweight package manager, making it easy to extend the base image with only the tools you need.
4. The **entrypoint** must be specified as an array, and it is required. This ensures the container behaves as expected upon startup.