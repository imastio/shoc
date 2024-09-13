import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "cluster" service
 */
export default class ClustersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-cluster", config);
  }
 
  getAll(token, workspaceId){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getAllExtended(token, workspaceId){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   getById(token, workspaceId, id){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getExtendedById(token, workspaceId, id){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters/${id}/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   create(token, workspaceId, input){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   updateById(token, workspaceId, id, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateConfigurationById(token, workspaceId, id, input) {

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters/${id}/configuration`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   deleteById(token, workspaceId, id){

    const url = this.urlify({
      api: `api/workspaces/${workspaceId}/clusters/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
}