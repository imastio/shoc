import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" service
 */
export default class UserGroupsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  getAll(token) {
    const url = this.urlify({
      api: "api/user-groups",
     
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, id) {
    const url = this.urlify({
      api: `api/user-groups/${id}`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getAllReferentialValues(token) {
    const url = this.urlify({
      api: "api/user-groups/referential-values",
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token),
      },
    });
  }

  create(token, input) {
    const url = this.urlify({
      api: "api/user-groups",
     
    });
  
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  
  deleteById(token, id) {
    const url = this.urlify({
      api: `api/user-groups/${id}`,
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}