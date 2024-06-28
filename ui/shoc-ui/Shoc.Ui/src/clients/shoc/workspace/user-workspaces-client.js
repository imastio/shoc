import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "workspace" service
 */
export default class UserWorkspacesClient extends BaseAxiosClient {
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
      api: `api/user-workspaces`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   getById(token, id){

    const url = this.urlify({
      api: `api/user-workspaces/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getPermissionsById(token, id){

    const url = this.urlify({
      api: `api/user-workspaces/${id}/permissions`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getByName(token, name){

    const url = this.urlify({
      api: `api/user-workspaces/by-name/${name}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getPermissionsByName(token, name){

    const url = this.urlify({
      api: `api/user-workspaces/by-name/${name}/permissions`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
   create(token, input){
    const url = this.urlify({
      api: `api/user-workspaces`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


   updateById(token, id, input) {

    const url = this.urlify({
      api: `api/user-workspaces/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   deleteById(token, id){

    const url = this.urlify({
      api: `api/user-workspaces/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
}