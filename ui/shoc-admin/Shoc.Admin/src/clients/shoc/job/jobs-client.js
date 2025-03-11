import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "job" service
 */
export default class JobsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-job", config);
  }

  getExtendedPage(token, workspaceId, filter, page, size) {

    const url = this.urlify({
      api: `api/management/workspaces/${workspaceId}/jobs/extended`,
      query: { userId: filter?.userId, scope: filter?.scope, status: filter?.status, page, size}
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}