import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" roles service
 */
export default class RolePrivilegesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  getAll(token, id) {
    const url = this.urlify({
      api: `api/roles/${id}/privileges`,
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, input) {
    const url = this.urlify({
      api: `api/roles/${input.roleId}/privileges`,
    });
  
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, privilegeId, roleId) {
    const url = this.urlify({
        api: `api/roles/${roleId}/privileges/${privilegeId}`,
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}