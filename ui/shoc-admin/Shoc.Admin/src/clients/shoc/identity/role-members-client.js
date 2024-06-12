import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "connect" service
 */
export default class RoleMembersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  getAll(token, roleId) {
    const url = this.urlify({
      api: `api/roles/${roleId}/members`,
     
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, roleId, id) {
    const url = this.urlify({
      api: `api/roles/${roleId}/members/${id}`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, input) {
    const url = this.urlify({
      api: `api/roles/${input.roleId}/members`,
     
    });
  
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  
  deleteById(token, roleId, id) {
    const url = this.urlify({
      api: `api/roles/${roleId}/members/${id}`,
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}