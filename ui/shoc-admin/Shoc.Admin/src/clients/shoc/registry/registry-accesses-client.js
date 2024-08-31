import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "registry" service
 */
export default class RegistryAccessesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-registry", config);
  }

  /**
   * Get all access definitions
   * 
   * @returns {object} The access definitions
   */
  getAll(token) {

    const url = this.urlify({
      api: "api/access-definitions"
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}