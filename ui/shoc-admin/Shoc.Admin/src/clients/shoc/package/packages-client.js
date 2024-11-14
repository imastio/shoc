import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "package" service
 */
export default class PackagesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-package", config);
  }

  getExtendedPage(token, workspaceId, filter, page, size) {

    const url = this.urlify({
      api: `api/packages/extended`,
      query: { workspaceId, userId: filter?.userId, scope: filter?.scope, page, size}
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}