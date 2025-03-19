import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "job" service
 */
export default class WorkspaceJobTasksClient extends BaseAxiosClient {

  constructor(config) {
    super("shoc-job", config);
  }

  getLogsBySequenceUrl(token, workspaceId, jobId, sequence){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/jobs/${jobId}/tasks/by-sequence/${sequence}/logs`
    });

    return Promise.resolve({ data: { token, url }})
  }
}