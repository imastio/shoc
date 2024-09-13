import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "registry" service
 */
export default class RegistryCredentialsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-registry", config);
  }
 
  getAll(token, registryId){

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getAllExtended(token, registryId){

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials/extended`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   getById(token, registryId, id){

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   create(token, registryId, input){

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


   updateById(token, registryId, id, input) {

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updatePasswordById(token, registryId, id, input) {

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials/${id}/password`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   deleteById(token, registryId, id){

    const url = this.urlify({
      api: `api/registries/${registryId}/credentials/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
}