
export enum WorkspacePermissions {
    WORKSPACE_VIEW = "workspace_view",
    WORKSPACE_UPDATE = "workspace_update",
    WORKSPACE_DELETE = "workspace_delete",
    WORKSPACE_LIST_MEMBERS = "workspace_list_members",
    WORKSPACE_UPDATE_MEMBER = "workspace_update_member",
    WORKSPACE_DELETE_MEMBER = "workspace_delete_member",
    WORKSPACE_LIST_INVITATIONS = "workspace_list_invitations",
    WORKSPACE_CREATE_INVITATION = "workspace_create_invitation",
    WORKSPACE_UPDATE_INVITATION = "workspace_update_invitation",
    WORKSPACE_DELETE_INVITATION = "workspace_delete_invitation",
    WORKSPACE_LIST_SECRETS = "workspace_list_secrets",
    WORKSPACE_LIST_CLUSTERS = "workspace_list_clusters",
    WORKSPACE_CREATE_CLUSTER = "workspace_create_cluster",
    WORKSPACE_UPDATE_CLUSTER = "workspace_update_cluster",
    WORKSPACE_DELETE_CLUSTER = "workspace_delete_cluster",
    WORKSPACE_CREATE_SECRET = "workspace_create_secret",
    WORKSPACE_UPDATE_SECRET = "workspace_update_secret",
    WORKSPACE_DELETE_SECRET = "workspace_delete_secret",
    WORKSPACE_LIST_USER_SECRETS = "workspace_list_user_clusters",
    WORKSPACE_CREATE_USER_SECRET = "workspace_create_user_cluster",
    WORKSPACE_UPDATE_USER_SECRET = "workspace_update_user_cluster",
    WORKSPACE_DELETE_USER_SECRET = "workspace_delete_user_cluster",
}