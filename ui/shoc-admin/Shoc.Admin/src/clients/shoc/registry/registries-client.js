import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "registry" service
 */
export default class RegistriesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-registry", config);
  }
 
  getAll(token){

    const url = this.urlify({
      api: `api/registries`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getAllExtended(token){

    const url = this.urlify({
      api: `api/registries/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   getById(token, id){

    const url = this.urlify({
      api: `api/registries/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   create(token, input){

    const url = this.urlify({
      api: `api/registries`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


   updateById(token, id, input) {

    const url = this.urlify({
      api: `api/registries/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   deleteById(token, id){

    const url = this.urlify({
      api: `api/registries/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
}