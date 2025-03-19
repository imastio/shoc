import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "job" service
 */
export default class WorkspaceJobsClient extends BaseAxiosClient {

  constructor(config) {
    super("shoc-job", config);
  }

  getBy(token, workspaceId, filter) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/jobs`,
      query: {...filter}
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getByLocalId(token, workspaceId, localId) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/jobs/by-local-id/${localId}`
    });
    
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, workspaceId, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/jobs`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  submit(token, workspaceId, id, input) {
    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/jobs/${id}/submit`
    });
    
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}