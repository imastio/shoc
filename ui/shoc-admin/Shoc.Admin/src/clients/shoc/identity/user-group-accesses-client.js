import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" service
 */
export default class UserGroupAccessesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  get(token, groupId) {
    const url = this.urlify({
      api: `api/user-groups/${groupId}/accesses`,
     
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  update(token, groupId, input) {
    const url = this.urlify({
      api: `api/user-groups/${groupId}/accesses`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}