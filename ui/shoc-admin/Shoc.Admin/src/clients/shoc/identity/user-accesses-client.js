import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" service
 */
export default class UserAccessesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  get(token, userId) {
    const url = this.urlify({
      api: `api/users/${userId}/accesses`,
     
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  update(token, userId, input) {
    const url = this.urlify({
      api: `api/users/${userId}/accesses`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}