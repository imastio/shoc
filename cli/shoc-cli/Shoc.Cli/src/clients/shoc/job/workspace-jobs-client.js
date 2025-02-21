import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "job" service
 */
export default class WorkspaceJobsClient extends BaseAxiosClient {

  constructor(config) {
    super("shoc-job", config);
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