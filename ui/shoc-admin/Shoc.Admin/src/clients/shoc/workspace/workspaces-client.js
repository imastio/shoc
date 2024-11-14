import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "workspace" service
 */
export default class WorkspacesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-workspace", config);
  }

 
  getAll(token){

    const url = this.urlify({
      api: `api/management/workspaces`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   getById(token, id){

    const url = this.urlify({
      api: `api/management/workspaces/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Create a mailing profile
   * 
   */
   create(token, input){

    const url = this.urlify({
      api: `api/management/workspaces`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


   updateById(token, id, input) {

    const url = this.urlify({
      api: `api/management/workspaces/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   deleteById(token, id){

    const url = this.urlify({
      api: `api/management/workspaces/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
}