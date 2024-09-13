import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "connect" privilege accesses service
 */
export default class PrivilegeAccessesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  get(token, privilegeId) {
    const url = this.urlify({
      api: `api/privileges/${privilegeId}/accesses`,
     
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  update(token, privilegeId, input) {
    const url = this.urlify({
      api: `api/privileges/${privilegeId}/accesses`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}