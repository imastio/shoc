import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" service
 */
export default class UserGroupMembersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  getAll(token, groupId) {
    const url = this.urlify({
      api: `api/user-groups/${groupId}/members`,
     
    });
 
    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, groupId, id) {
    const url = this.urlify({
      api: `api/user-groups/${groupId}/members/${id}`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, input) {
    const url = this.urlify({
      api: `api/user-groups/${input.groupId}/members`,
     
    });
  
    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  
  deleteById(token, groupId, id) {
    const url = this.urlify({
      api: `api/user-groups/${groupId}/members/${id}`,
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}