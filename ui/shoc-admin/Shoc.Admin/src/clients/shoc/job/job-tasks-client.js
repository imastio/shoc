import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "job" service
 */
export default class JobTasksClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-job", config);
  }

  getAllExtended(token, workspaceId, jobId) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/jobs/${jobId}/tasks/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getExtendedById(token, workspaceId, jobId, id) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/jobs/${jobId}/tasks/${id}/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getLogsById(token, workspaceId, jobId, id, onProgress) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/jobs/${jobId}/tasks/${id}/logs`
    });

    return this.webClient.get(url, {
      onDownloadProgress: onProgress,
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}