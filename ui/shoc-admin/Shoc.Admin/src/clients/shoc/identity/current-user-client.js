import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "idetity" service
 */
export default class CurrentUserClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  getEffectiveAccesses(token) {

    const url = this.urlify({
      api: "api/current-user/effective-accesses"
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}

