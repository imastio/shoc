import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "package" service
 */
export default class WorkspaceBuildTasksClient extends BaseAxiosClient {

  constructor(config) {
    super("shoc-package", config);
  }

  getAll(token, workspaceId) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/build-tasks`
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/build-tasks`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  uploadBundleById(token, workspaceId, id, formData) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/build-tasks/${id}/bundle`
    });
    
    return this.webClient.put(url, formData, {
      headers: {
        ...this.authBearer(token),
        'Content-Type': 'multipart/form-data'
      },
      maxBodyLength: Infinity
    });
  }
}