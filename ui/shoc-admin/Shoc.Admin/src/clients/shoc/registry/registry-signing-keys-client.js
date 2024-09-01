import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "registry" service
 */
export default class RegistrySigningKeysClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-registry", config);
  }

  getAll(token, registryId) {

    const url = this.urlify({
      api: `api/registries/${registryId}/signing-keys`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, registryId, input) {

    const url = this.urlify({
      api: `api/registries/${registryId}/signing-keys`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


  deleteById(token, registryId, id) {

    const url = this.urlify({
      api: `api/registries/${registryId}/signing-keys/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

}