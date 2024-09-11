import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "cluster" service
 */
export default class ClustersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-cluster", config);
  }
 
  getAllExtended(token){

    const url = this.urlify({
      api: `api/clusters/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}